using AddressBook.Shared.Models;
using AddressBook.Shared.Models.Requests;
using OperationResults;

namespace AddressBook.BusinessLayer.Services.Interfaces;

public interface ICityService
{
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<Result<City>> GetAsync(Guid id, CancellationToken cancellationToken);

    Task<Result<IEnumerable<City>>> GetListAsync(string name, CancellationToken cancellationToken);

    Task<Result<City>> InsertAsync(SaveCityRequest request, CancellationToken cancellationToken);

    Task<Result> UpdateAsync(Guid id, SaveCityRequest request, CancellationToken cancellationToken);
}