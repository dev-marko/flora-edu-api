using AutoMapper;
using FloraEdu.Domain.DataTransferObjects;
using FloraEdu.Domain.DataTransferObjects.Article;
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

        CreateMap<Plant, PlantCardDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

        CreateMap<PlantComment, PlantCommentDto>()
            .ForMember(dest => dest.PlantId, opt => opt.MapFrom(src => src.PlantId))
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.Likes.Count))
            .ReverseMap();

        // User
        CreateMap<User, AuthorDto>();
        CreateMap<User, CommentUserInfoDto>();
        CreateMap<User, UserInfo>();

        // Blog
        CreateMap<Article, ArticleDto>();
        CreateMap<ArticleComment, ArticleCommentDto>()
            .ForMember(dest => dest.ArticleId, opt => opt.MapFrom(src => src.ArticleId))
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.Likes.Count))
            .ReverseMap();
    }
}
