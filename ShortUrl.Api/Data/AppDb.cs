using Microsoft.EntityFrameworkCore;
using ShortUrl.Api.Models;


namespace ShortUrl.Api.Data;


public class AppDb(DbContextOptions<AppDb> options) : DbContext(options)
{
    public DbSet<Link> Links => Set<Link>();


    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Link>(e =>
        {
            e.HasIndex(x => x.Code).IsUnique();
            e.Property(x => x.Code).IsRequired();
            e.Property(x => x.Url).IsRequired();
        });
    }
}