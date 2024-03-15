using AddressBook.Shared.Models;
using AddressBook.Shared.Models.Requests;
using AutoMapper;
using Entities = AddressBook.DataAccessLayer.Entities;

namespace AddressBook.BusinessLayer.MapperProfiles;

public class PersonMapperProfile : Profile
{
    public PersonMapperProfile()
    {
        CreateMap<Entities.Person, Person>();
        CreateMap<SavePersonRequest, Entities.Person>();
    }
}