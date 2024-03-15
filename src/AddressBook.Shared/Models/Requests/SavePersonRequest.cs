namespace AddressBook.Shared.Models.Requests;

public class SavePersonRequest
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public string Gender { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Province { get; set; } = null!;

    public string? CellphoneNumber { get; set; }

    public string? EmailAddress { get; set; }
}