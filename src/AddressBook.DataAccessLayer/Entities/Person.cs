using AddressBook.DataAccessLayer.Entities.Common;

namespace AddressBook.DataAccessLayer.Entities;

public class Person : BaseEntity
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateOnly BirthDate { get; set; }

    public string Gender { get; set; }

    public string City { get; set; }

    public string Province { get; set; }

    public string FiscalCode { get; set; }

    public string CellphoneNumber { get; set; }

    public string EmailAddress { get; set; }
}