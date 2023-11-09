using AutoMapper;
using FloraEdu.Domain.DataTransferObjects.Plant;
using FloraEdu.Domain.Entities;

namespace FloraEdu.Web.AutoMapper;

public class FloraEduMappingProfile : Profile
{
    public FloraEduMappingProfile()
    {
        // Plant
        CreateMap<Plant, PlantDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ReverseMap();

        CreateMap<PlantComment, PlantCommentDto>()
            .ForMember(dest => dest.PlantId, opt => opt.MapFrom(src => src.PlantId))
            .ReverseMap();
    }
}
