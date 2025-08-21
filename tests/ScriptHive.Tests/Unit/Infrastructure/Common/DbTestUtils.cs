using ScriptHive.Infrastructure.Context;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ScriptHive.Tests.Unit.Infrastructure.Common;

public static class DbTestUtils
{
    public static AppDbContext CreateInMemory(string name)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(name)
            .EnableSensitiveDataLogging()
            .Options;

        var ctx = new AppDbContext(options);
        ctx.Database.EnsureDeleted();
        ctx.Database.EnsureCreated();
        return ctx;
    }

    public static (AppDbContext ctx, SqliteConnection conn) CreateSqlite(string name)
    {
        var conn = new SqliteConnection("DataSource=:memory:");
        conn.Open();

        conn.CreateFunction("GETUTCDATE", () => DateTime.UtcNow);

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(conn)
            .EnableSensitiveDataLogging()
            .Options;

        var ctx = new AppDbContext(options);
        ctx.Database.EnsureCreated();

        return (ctx, conn);
    }
}
