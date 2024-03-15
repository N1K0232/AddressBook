using AddressBook.Shared.Models;
using AddressBook.Shared.Models.Common;
using AddressBook.Shared.Models.Requests;
using OperationResults;

namespace AddressBook.BusinessLayer.Services.Interfaces;

public interface IPeopleService
{
    Task<Result> DeleteAsync(Guid id);

    Task<Result<Person>> GetAsync(Guid id);

    Task<Result<ListResult<Person>>> GetListAsync(string searchText, string orderBy, int pageIndex, int itemsPerPage);

    Task<Result<Person>> SaveAsync(SavePersonRequest person);

    Task<Result<Person>> SaveAsync(Guid id, SavePersonRequest person);
}