using AutoMapper;
using BlogApp.Server.Domain.Models;
using BlogApp.Shared.DTOs;
using BlogApp.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Server.Infra.Repository
{
    public class BlogRepository
    {
        public readonly AppDbContext _context;
        public readonly IMapper _mapper;
        public BlogRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<BlogDto>>> GetAll()
        {
            Result<List<BlogDto>> result = new();

            try
            {
                List<Blog> blogs = await _context.Blog.ToListAsync();

                result.Success = true;

                if (blogs.Count > 0)
                    result.Data = _mapper.Map<List<BlogDto>>(blogs);
                else
                    result.Message = "No hay registros";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
