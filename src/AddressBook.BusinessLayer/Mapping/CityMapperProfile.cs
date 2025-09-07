using AddressBook.Shared.Models;
using AddressBook.Shared.Models.Requests;
using AutoMapper;
using Entities = AddressBook.DataAccessLayer.Entities;

namespace AddressBook.BusinessLayer.Mapping;

public class CityMapperProfile : Profile
{
    public CityMapperProfile()
    {
        CreateMap<Entities.City, City>();
        CreateMap<SaveCityRequest, Entities.City>();
    }
}