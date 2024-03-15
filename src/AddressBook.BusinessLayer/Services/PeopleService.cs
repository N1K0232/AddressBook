using System.Linq.Dynamic.Core;
using AddressBook.BusinessLayer.Services.Interfaces;
using AddressBook.DataAccessLayer;
using AddressBook.Shared.Models;
using AddressBook.Shared.Models.Common;
using AddressBook.Shared.Models.Requests;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OperationResults;
using TinyHelpers.Extensions;
using Entities = AddressBook.DataAccessLayer.Entities;

namespace AddressBook.BusinessLayer.Services;

public class PeopleService : IPeopleService
{
    private readonly IDataContext dataContext;
    private readonly IMapper mapper;

    public PeopleService(IDataContext dataContext,
        IMapper mapper)
    {
        this.dataContext = dataContext;
        this.mapper = mapper;
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        try
        {
            var person = await dataContext.GetAsync<Entities.Person>(id);
            if (person is not null)
            {
                await dataContext.DeleteAsync(person);
                await dataContext.SaveAsync();

                return Result.Ok();
            }

            return Result.Fail(FailureReasons.ItemNotFound, "Person not found", $"{id} doesn't exists");
        }
        catch (DbUpdateException ex)
        {
            return Result.Fail(FailureReasons.DatabaseError, ex);
        }
    }

    public async Task<Result<Person>> GetAsync(Guid id)
    {
        var dbPerson = await dataContext.GetAsync<Entities.Person>(id);
        if (dbPerson is not null)
        {
            var person = mapper.Map<Person>(dbPerson);
            return person;
        }

        return Result.Fail(FailureReasons.ItemNotFound, "Person not found", $"{id} doesn't exists");
    }

    public async Task<Result<ListResult<Person>>> GetListAsync(string searchText, string orderBy, int pageIndex, int itemsPerPage)
    {
        var query = dataContext.Get<Entities.Person>();

        if (searchText.HasValue())
        {
            query = query.Where(p => p.FirstName.Contains(searchText) || p.LastName.Contains(searchText));
        }

        if (orderBy.HasValue())
        {
            query = query.OrderBy(orderBy);
        }

        var totalCount = await query.CountAsync();
        var people = await query.ProjectTo<Person>(mapper.ConfigurationProvider)
            .Skip(pageIndex * itemsPerPage).Take(itemsPerPage + 1)
            .ToListAsync();

        var result = new ListResult<Person>
        {
            Content = people.Take(itemsPerPage),
            TotalCount = totalCount,
            HasNextPage = people.Count > itemsPerPage
        };

        return result;
    }

    public async Task<Result<Person>> SaveAsync(SavePersonRequest person)
    {
        try
        {
            var exists = await ExistsAsync(person.FirstName, person.LastName, person.BirthDate);
            if (exists)
            {
                return Result.Fail(FailureReasons.Conflict, "Person already exists", $"{person.FirstName} {person.LastName} already exists");
            }

            var dbPerson = mapper.Map<Entities.Person>(person);
            await dataContext.InsertAsync(dbPerson);

            var affectedRows = await dataContext.SaveAsync();
            if (affectedRows > 0)
            {
                var savedPerson = mapper.Map<Person>(dbPerson);
                return savedPerson;
            }

            return Result.Fail(FailureReasons.DatabaseError, "No row updated", "No person was inserted");
        }
        catch (DbUpdateException ex)
        {
            return Result.Fail(FailureReasons.DatabaseError, ex);
        }
    }

    public async Task<Result<Person>> SaveAsync(Guid id, SavePersonRequest person)
    {
        try
        {
            var query = dataContext.Get<Entities.Person>(trackingChanges: true);
            var dbPerson = await query.FirstOrDefaultAsync(p => p.Id == id);

            if (dbPerson is null)
            {
                return Result.Fail(FailureReasons.ItemNotFound, "Person not found", $"{id} doesn't exists");
            }

            var exists = await ExistsAsync(person.FirstName, person.LastName, person.BirthDate);
            if (exists)
            {
                return Result.Fail(FailureReasons.Conflict, "Person already exists", $"{person.FirstName} {person.LastName} already exists");
            }

            mapper.Map(person, dbPerson);

            var affectedRows = await dataContext.SaveAsync();
            if (affectedRows > 0)
            {
                var savedPerson = mapper.Map<Person>(dbPerson);
                return savedPerson;
            }

            return Result.Fail(FailureReasons.DatabaseError, "No row updated", "No person was updated");
        }
        catch (DbUpdateException ex)
        {
            return Result.Fail(FailureReasons.DatabaseError, ex);
        }
    }

    private async Task<bool> ExistsAsync(string firstName, string lastName, DateOnly birthDate)
    {
        var query = dataContext.Get<Entities.Person>();
        return await query.AnyAsync(p => p.FirstName == firstName && p.LastName == lastName && p.BirthDate == birthDate);
    }
}