using JobHunting.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace JobHunting.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Resume> Resumes { get; set; }

    public ApplicationDbContext(DbContextOptions options) 
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
