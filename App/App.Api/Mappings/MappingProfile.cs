using AutoMapper;
using App.Api.Models;
using App.Api.DTOs;

namespace App.Api.Mappings
{
    public class MappingProfile : Profile
    {
        
        public MappingProfile()
        {
          // Article -> ArticleReadDto dönüşümü
           CreateMap<Article, ArticleReadDto>();
           CreateMap<ArticleCreateDto, Article>();
           CreateMap<ArticleUpdateDto, Article>();
           CreateMap<Article, ArticleUpdateDto>();
           CreateMap<ArticleReadDto, Article>();


        }
    }
}


