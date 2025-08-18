using ScriptHive.Domain.Entities.User;
using ScriptHive.Domain.Entities.Script;

using Microsoft.EntityFrameworkCore;

namespace ScriptHive.Infrastructure.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Script> Scripts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
