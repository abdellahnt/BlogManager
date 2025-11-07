using Microsoft.AspNetCore.Mvc;
using blogplatform.Infrastructure;
using blogplatform.Models;
using Microsoft.EntityFrameworkCore;


namespace blogplatform.Controllers;

[ApiController]
[Route("api/author-controller")]
public class AuthorController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public AuthorController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    [HttpGet("{authorId}")]
    public async Task<IActionResult> GetArticleById(Guid authorId)
    {
        Author? author = await _appDbContext.Authors.FindAsync(authorId);

        if (author == null)
        {
            return NotFound();
        }
        return Ok(author);
    }
    
    [HttpGet("all-authors-in-blog")]
    public async Task<IActionResult> GetAllAuthors()
    {
        List<Author>? authors = await _appDbContext.Authors.ToListAsync();
        return Ok(authors);
    }

    [HttpPost("add-new-author")]
    public async Task<IActionResult> AddNewAuthor(String authorName, String authorEmail)
    {
        Author author = new Author
        {
            Id = Guid.NewGuid(),
            Name = authorName,
            Email = authorEmail
        };
        await _appDbContext.Authors.AddAsync(author);
        await _appDbContext.SaveChangesAsync();
        return Ok(author);
    }

    [HttpDelete("{authorId}")]
    public async Task<IActionResult> DeleteAuthor(Guid authorId)
    {
        if (await _appDbContext.Authors.AnyAsync(author => author.Id == authorId))
        {
            await _appDbContext.Authors.Where(author => author.Id == authorId).ExecuteDeleteAsync();
            await _appDbContext.Articles.Where(article => article.AuthorId == authorId).ExecuteDeleteAsync();

            await _appDbContext.SaveChangesAsync();
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}