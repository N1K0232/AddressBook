using AddressBook.DataAccessLayer.Entities.Common;

namespace AddressBook.DataAccessLayer.Entities;

public class Person : BaseEntity
{
    public Guid CityId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateOnly BirthDate { get; set; }

    public virtual City City { get; set; }
}