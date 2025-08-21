using Microsoft.EntityFrameworkCore;
using ScriptHive.Infrastructure.Context;

namespace ScriptHive.Tests.Unit.Infrastructure.Configurations;

public class TestDbContext(DbContextOptions<AppDbContext> options) : AppDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public static TestDbContext Create(string name)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(name)
            .Options;
        return new TestDbContext(options);
    }
}
