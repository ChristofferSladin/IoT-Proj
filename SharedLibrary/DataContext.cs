using Microsoft.EntityFrameworkCore;
using SharedLibrary.Entities;

namespace SharedLibrary;

public class DataContext : DbContext
{
    //public DataContext()
    //{
        
    //}
    //public DataContext(DbContextOptions<DataContext> options) : base(options)
    //{
    //    Database.Migrate();
    //}

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlite();
    //}

    public DbSet<SettingsEntity> Settings { get; set; }
}
