namespace AddressBook.Shared.Models.Requests;

public record class SavePersonRequest(Guid CityId, string FirstName, string LastName, DateOnly BirthDate);