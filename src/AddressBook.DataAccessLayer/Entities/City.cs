using AddressBook.DataAccessLayer.Entities.Common;

namespace AddressBook.DataAccessLayer.Entities;

public class City : BaseEntity
{
    public string Name { get; set; }

    public virtual ICollection<Person> People { get; set; }
}