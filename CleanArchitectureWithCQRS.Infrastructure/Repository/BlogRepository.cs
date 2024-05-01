using CleanArchitectureWithCQRS.Domain.Entity;
using CleanArchitectureWithCQRS.Domain.Repository;
using CleanArchitectureWithCQRS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureWithCQRS.Infrastructure.Repository
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDbContext _context;

        public BlogRepository(BlogDbContext context)
        {
            _context = context;
        }
        public async Task<Blog> CreateBlogAsync(Blog blog)
        {
            await _context.Blog.AddAsync(blog);
            await _context.SaveChangesAsync();
            return blog;
        }

        public async Task<int> DeleteBlogAsync(int id)
        {
            return await _context.Blog.Where(b => b.Id == id).ExecuteDeleteAsync();
        }

        public async Task<List<Blog>> GetAllBlogsAsync()
        {
            return await _context.Blog.ToListAsync();
        }

        public async Task<Blog> GetBlogByIdAsync(int id)
        {
            return await _context.Blog.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<int> UpdateBlogAsync(int id, Blog blog)
        {
            return await _context.Blog.Where(b => b.Id == id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(b => b.Id, blog.Id)
                .SetProperty(b => b.Name, blog.Name)
                .SetProperty(b => b.Description, blog.Description)
                .SetProperty(b => b.Author, blog.Author)
                );
        }
    }
}
