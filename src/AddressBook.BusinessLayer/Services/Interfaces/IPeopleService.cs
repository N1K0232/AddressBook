using AddressBook.Shared.Models;
using AddressBook.Shared.Models.Requests;
using OperationResults;

namespace AddressBook.BusinessLayer.Services.Interfaces;

public interface IPeopleService
{
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<Result<Person>> GetAsync(Guid id, CancellationToken cancellationToken);

    Task<Result<PaginatedList<Person>>> GetListAsync(string searchText, int pageIndex, int itemsPerPage, string orderBy, CancellationToken cancellationToken);

    Task<Result<Person>> InsertAsync(SavePersonRequest request, CancellationToken cancellationToken);

    Task<Result> UpdateAsync(Guid id, SavePersonRequest request, CancellationToken cancellationToken);
}