using AutoMapper;

namespace Infrastracture.Mapper;

/// <summary>
/// MappingProfile klasa nasleđuje AutoMapper Profile klasu i koristi se za definisanje
/// mapiranja između različitih objekata.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Dtos.Address, CustomerRewards.Catalog.Entities.Address>();
    }
}
