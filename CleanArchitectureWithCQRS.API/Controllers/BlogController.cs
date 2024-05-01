using CleanArchitectureWithCQRS.Application.Blogs.Commands.CreateBlog;
using CleanArchitectureWithCQRS.Application.Blogs.Commands.DeleteBlog;
using CleanArchitectureWithCQRS.Application.Blogs.Commands.UpdateBlog;
using CleanArchitectureWithCQRS.Application.Blogs.Queries.GetBlogById;
using CleanArchitectureWithCQRS.Application.Blogs.Queries.GetBlogs;
using CleanArchitectureWithCQRS.Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureWithCQRS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var blos = await Mediator.Send(new GetBlogQuery());
            return Ok(blos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var blog = await Mediator.Send(new GetBlogByIdQuery() { BlogId = id });
            if (blog == null)
            {
                return NotFound();
            }
            return Ok(blog);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogAsync(CreateBlogCommand command)
        {
            var createBlog = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new {id = createBlog.Id}, createBlog);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateBlogCommand command)
        {
            if(id != command.Id)
            {
                return BadRequest();
            }
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteAsync(int id)
        {
            var blog = Mediator.Send(new DeleteBlogCommand() { BlogId = id });
            if(blog == null)
            {
                return Task.FromResult<IActionResult>(NotFound(id));
            }
            return Task.FromResult<IActionResult>(Ok(blog));
        }
    }
}
