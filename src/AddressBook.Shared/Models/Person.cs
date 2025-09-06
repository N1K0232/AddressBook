namespace AddressBook.Shared.Models;

public record class Person(Guid Id, City City, string FirstName, string LastName, DateOnly BirthDate);