using AutoMapper;
using BlogApp.Server.Domain.Models;
using BlogApp.Shared.DTOs;

namespace BlogApp.Server.Domain.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Blog, BlogDto>().ReverseMap();
        }
    }
}
