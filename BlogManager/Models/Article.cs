namespace blogplatform.Models;

public class Article
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Guid AuthorId { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime LastUpdate { get; set; }
}