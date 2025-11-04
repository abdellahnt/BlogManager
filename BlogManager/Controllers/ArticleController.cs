using blogplatform.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using blogplatform.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;

namespace blogplatform.Controllers;

[ApiController]
[Route("api/article-controller")]
public class ArticleController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public ArticleController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetArticleById(Guid articleId)
    {
        Article? article = await _appDbContext.Articles.FindAsync(articleId);

        if (article == null)
        {
            return NotFound();
        }
        return Ok(article);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllArticlesByAuthorId(Guid authorId)
    {
        List<Article>? articles = await _appDbContext.Articles.Where(x => x.AuthorId == authorId).ToListAsync();
        
        return Ok(articles);
    }

    [HttpPost]
    public async Task<IActionResult> AddArticle(Guid authorId, String articleTitle, String articleText)
    {
        Article article = new Article
        {
            Id = Guid.NewGuid(),
            AuthorId = authorId,
            Title = articleTitle,
            Content = articleText,
            PublishDate = DateTime.Now
        };
        await _appDbContext.Articles.AddAsync(article);
        await _appDbContext.SaveChangesAsync();
        
        return Ok(article);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateFullArticle(Guid articleId, Guid authorId, String articleTitle, String articleText)
    {
        Article article = new Article
        {
            Id = articleId,
            AuthorId = authorId,
            Title = articleTitle,
            Content = articleText,
            PublishDate = DateTime.Now
        };
        await _appDbContext.Articles.AddAsync(article);
        await _appDbContext.SaveChangesAsync();
        
        return Ok(article);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateArticleTitle( Guid articleId,
        [FromBody] JsonPatchDocument<Article>? patchArticle)
    {
        if (patchArticle == null)
            return  BadRequest();
        
        Article? article = await _appDbContext.Articles.FindAsync(articleId);

        if (article == null)
            return NotFound();
        
        patchArticle.ApplyTo(article);
        
        await _appDbContext.SaveChangesAsync();
        return Ok(article);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteArticle(Guid articleId)
    {
        if (await _appDbContext.Articles.AnyAsync(article => article.Id == articleId))
        {
            await _appDbContext.Articles.Where(article => article.Id == articleId).ExecuteDeleteAsync();
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}