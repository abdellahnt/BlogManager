using Microsoft.EntityFrameworkCore;
using blogplatform.Models;

namespace blogplatform.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<Author> Authors { get; set; }
}