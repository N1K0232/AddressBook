namespace AddressBook.Shared.Models.Common;

public class ListResult<T>
{
    public IEnumerable<T>? Content { get; init; }

    public int TotalCount { get; init; }

    public bool HasNextPage { get; init; }
}