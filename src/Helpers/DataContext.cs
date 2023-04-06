namespace src.Helpers;

using Microsoft.EntityFrameworkCore;
using src.Entities;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to postgres with connection string from app settings
        options.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
    }

    public DbSet<User>? Users { get; set; }
    public DbSet<Project>? Projects { get; set; }
    public DbSet<Todo>? Todos { get; set; }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<Project>()
    //        .HasOne(p => p.User)
    //        .WithMany(u => u.Projects)
    //        .HasForeignKey(p => p.Id);
    //}
}