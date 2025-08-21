using ScriptHive.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ScriptHive.Tests.Unit.Infrastructure.Context;


public class DesignTimeDbContextFactoryPostgresTests
{
    [Test]
    public void CreateDbContext_Loads_ConnectionString_And_Uses_Postgres()
    {
        // Cria estrutura temporária simulando a solução
        var tempRoot = Path.Combine(Path.GetTempPath(), "sh-" + Guid.NewGuid());
        var solutionDir = Path.Combine(tempRoot, "Solution");

        var apiDir = Path.Combine(solutionDir, "src/ScriptHive.Api");
        var testsDir = Path.Combine(solutionDir, "tests/ScriptHive.Tests");

        Directory.CreateDirectory(apiDir);
        Directory.CreateDirectory(testsDir);

        // Cria appsettings.json com connection string PostgreSQL
        var appsettingsPath = Path.Combine(apiDir, "appsettings.json");
        var json = JsonSerializer.Serialize(new
        {
            ConnectionStrings = new
            {
                DefaultConnection = "Host=localhost;Database=dummy;Username=postgres;Password=postgres"
            }
        });
        File.WriteAllText(appsettingsPath, json);

        var prevCwd = Directory.GetCurrentDirectory();
        try
        {
            Directory.SetCurrentDirectory(testsDir);

            var factory = new DesignTimeDbContextFactory();
            using var ctx = factory.CreateDbContext(Array.Empty<string>());

            // Validações
            Assert.That(ctx, Is.Not.Null);
            Assert.That(ctx, Is.InstanceOf<AppDbContext>());
            Assert.That(ctx.Database.ProviderName, Does.Contain("Npgsql"));
        }
        finally
        {
            Directory.SetCurrentDirectory(prevCwd);
            try { Directory.Delete(tempRoot, recursive: true); } catch { /* ignore */ }
        }
    }
}
