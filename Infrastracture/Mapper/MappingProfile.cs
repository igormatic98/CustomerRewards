using AutoMapper;

namespace Infrastracture.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Dtos.Address, CustomerRewards.Catalog.Entities.Address>();
    }
}
