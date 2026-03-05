using Blog.Domain.Entities;
using AutoMapper;
using Blog.Application.DTOs.Article;
using Blog.Application.DTOs.Category;
using Blog.Application.DTOs.Comment;
using Blog.Application.DTOs.Tag;
using Blog.Application.DTOs.User;

namespace Blog.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Article -> ArticleReadDto
            CreateMap<ArticleEntity, ArticleReadDto>()
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.UserName))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            // ArticleCreateDto -> ArticleEntity
            CreateMap<ArticleCreateDto, ArticleEntity>();

            // ArticleUpdateDto -> ArticleEntity
            CreateMap<ArticleUpdateDto, ArticleEntity>();

            // UserEntity -> UserReadDto
            CreateMap<UserEntity, UserReadDto>();

            // ------------------- Comment Mapping -------------------
            CreateMap<CommentEntity, CommentReadDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<CommentCreateDto, CommentEntity>();
            CreateMap<CommentUpdateDto, CommentEntity>();

            // ------------------- Category Mapping -------------------
            CreateMap<CategoryEntity, CategoryReadDto>();
            CreateMap<CategoryCreateDto, CategoryEntity>();
            CreateMap<CategoryUpdateDto, CategoryEntity>();

            // TagEntity <-> TagReadDto
            CreateMap<TagEntity, TagReadDto>();

            // TagCreateDto -> TagEntity
            CreateMap<TagCreateDto, TagEntity>();

            // TagUpdateDto -> TagEntity
            CreateMap<TagUpdateDto, TagEntity>();
        }
    }
}