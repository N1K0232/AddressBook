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

public class CityService(IDataContext dataContext, IMapper mapper) : ICityService
{
    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var deletedRows = await dataContext.GetData<Entities.City>()
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedRows > 0)
        {
            return Result.Ok();
        }

        return Result.Fail(FailureReasons.ItemNotFound, "No city found", $"No city found with id {id}");
    }

    public async Task<Result<City>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var dbCity = await dataContext.GetAsync<Entities.City>(id, cancellationToken);
        if (dbCity is null)
        {
            return Result.Fail(FailureReasons.ItemNotFound, "No city found", $"No city found with id {id}");
        }

        var city = mapper.Map<City>(dbCity);
        return city;
    }

    public async Task<Result<IEnumerable<City>>> GetListAsync(string name, CancellationToken cancellationToken)
    {
        var cities = await dataContext.GetData<Entities.City>()
            .WhereIf(name.HasValue(), c => c.Name.Contains(name))
            .ProjectTo<City>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return cities;
    }

    public async Task<Result<City>> InsertAsync(SaveCityRequest request, CancellationToken cancellationToken)
    {
        var cityExists = await dataContext.GetData<Entities.City>()
            .AnyAsync(c => c.Name == request.Name, cancellationToken);

        if (cityExists)
        {
            return Result.Fail(FailureReasons.Conflict, "City already exists", "City already exists");
        }

        var dbCity = mapper.Map<Entities.City>(request);
        await dataContext.InsertAsync(dbCity, cancellationToken);

        var city = mapper.Map<City>(dbCity);
        return city;
    }

    public async Task<Result> UpdateAsync(Guid id, SaveCityRequest request, CancellationToken cancellationToken)
    {
        var dbCity = await dataContext.GetData<Entities.City>()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (dbCity is null)
        {
            return Result.Fail(FailureReasons.ItemNotFound, "No city found", $"No city found with id {id}");
        }

        mapper.Map(request, dbCity);
        await dataContext.UpdateAsync(dbCity, cancellationToken);

        return Result.Ok();
    }
}