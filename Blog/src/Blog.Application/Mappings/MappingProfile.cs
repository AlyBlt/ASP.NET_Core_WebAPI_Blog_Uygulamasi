using Blog.Application.DTOs;
using Blog.Domain.Entities;
using AutoMapper;

namespace Blog.Application.Mappings
{
    public class MappingProfile : Profile
    {
        
        public MappingProfile()
        {
          // Article -> ArticleReadDto dönüşümü
           CreateMap<ArticleEntity, ArticleReadDto>();
           CreateMap<ArticleCreateDto, ArticleEntity>();
           CreateMap<ArticleUpdateDto, ArticleEntity>();
           CreateMap<UserEntity, UserReadDto>();
           CreateMap<ArticleEntity, ArticleReadDto>()
             .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
             .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username));


        }
    }
}


