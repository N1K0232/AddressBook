using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Exceptions;
using AddressBook.BusinessLayer.Services.Interfaces;
using AddressBook.DataAccessLayer;
using AddressBook.Shared.Models;
using AddressBook.Shared.Models.Requests;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OperationResults;
using TinyHelpers.Extensions;
using Entities = AddressBook.DataAccessLayer.Entities;

namespace AddressBook.BusinessLayer.Services;

public class PeopleService(IDataContext dataContext, IMapper mapper) : IPeopleService
{
    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var deletedRows = await dataContext.GetData<Entities.Person>()
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedRows > 0)
        {
            return Result.Ok();
        }

        return Result.Fail(FailureReasons.ItemNotFound, "No person found", $"No person found with id {id}");
    }

    public async Task<Result<Person>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var dbPerson = await dataContext.GetAsync<Entities.Person>(id, cancellationToken);
        if (dbPerson is null)
        {
            return Result.Fail(FailureReasons.ItemNotFound, "No person found", $"No person found with id {id}");
        }

        var person = mapper.Map<Person>(dbPerson);
        return person;
    }

    public async Task<Result<PaginatedList<Person>>> GetListAsync(string searchText, int pageIndex, int itemsPerPage, string orderBy, CancellationToken cancellationToken)
    {
        var query = dataContext.GetData<Entities.Person>()
            .Include(p => p.City)
            .WhereIf(searchText.HasValue(), p => p.FirstName.Contains(searchText) || p.LastName.Contains(searchText));

        var totalCount = await query.CountAsync(cancellationToken);

        try
        {
            query = query.OrderBy(orderBy);
        }
        catch (ParseException ex)
        {
            return Result.Fail(FailureReasons.ClientError, ex);
        }

        var dbPeople = await query.Skip(pageIndex * itemsPerPage).Take(itemsPerPage + 1)
            .ProjectTo<Person>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var people = mapper.Map<IEnumerable<Person>>(dbPeople).Take(itemsPerPage);
        return new PaginatedList<Person>(people, totalCount, dbPeople.Count > itemsPerPage);
    }

    public async Task<Result<Person>> InsertAsync(SavePersonRequest request, CancellationToken cancellationToken)
    {
        var personExists = await dataContext.GetData<Entities.Person>()
            .Where(p => p.FirstName == request.FirstName && p.LastName == request.LastName && p.BirthDate == request.BirthDate)
            .AnyAsync(cancellationToken);

        if (personExists)
        {
            return Result.Fail(FailureReasons.Conflict, "Person already exists", "This person already exists");
        }

        var dbPerson = mapper.Map<Entities.Person>(request);
        await dataContext.InsertAsync(dbPerson, cancellationToken);

        var person = mapper.Map<Person>(dbPerson);
        return person;
    }

    public async Task<Result> UpdateAsync(Guid id, SavePersonRequest request, CancellationToken cancellationToken)
    {
        var dbPerson = await dataContext.GetData<Entities.Person>(true)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (dbPerson is null)
        {
            return Result.Fail(FailureReasons.ItemNotFound, "No person found", $"No person found with id {id}");
        }

        mapper.Map(request, dbPerson);
        await dataContext.UpdateAsync(dbPerson, cancellationToken);

        return Result.Ok();
    }
}