using Microsoft.EntityFrameworkCore;
using SharedLibrary.Entities;

namespace SharedLibrary.Contexts;

public class ChristoDbContext : DbContext
{
    public ChristoDbContext()
    {
        
    }
    public ChristoDbContext(DbContextOptions<ChristoDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite();

        try
        {
            Database.Migrate();
        }
        catch
        {

        }
    }

    public DbSet<AppSettings> Settings { get; set; }
}
