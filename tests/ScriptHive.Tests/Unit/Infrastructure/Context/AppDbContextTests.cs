using ScriptHive.Domain.Entities.User;
using ScriptHive.Domain.Entities.Script;
using ScriptHive.Domain.Entities.ScriptExecution;
using ScriptHive.Tests.Unit.Infrastructure.Common;
using ScriptHive.Domain.ValueObjects.UserRole;

namespace ScriptHive.Tests.Unit.Infrastructure.Context;

public class AppDbContextTests
{
    [Test]
    public void Model_Contains_All_Entities_And_Configurations_Applied()
    {
        var (ctx, conn) = DbTestUtils.CreateSqlite(nameof(Model_Contains_All_Entities_And_Configurations_Applied));
        using (conn)
        using (ctx)
        {
            var model = ctx.Model;

            Assert.That(model.FindEntityType(typeof(User)), Is.Not.Null);
            Assert.That(model.FindEntityType(typeof(Script)), Is.Not.Null);
            Assert.That(model.FindEntityType(typeof(ScriptExecution)), Is.Not.Null);
        }
    }

    [Test]
    public async Task DbSets_Works_With_Sqlite()
    {
        var (ctx, conn) = DbTestUtils.CreateSqlite(nameof(DbSets_Works_With_Sqlite));
        using (conn)
        using (ctx)
        {
            ctx.Users.Add(new User { Id= Guid.NewGuid(), Name = "John", Email = "john@email.com", Role = UserRole.Admin, PasswordHash = "false@hash" });
            await ctx.SaveChangesAsync();

            var all = ctx.Users.ToList();
            Assert.That(all.Count, Is.EqualTo(1));
            Assert.That(all[0].Name, Is.EqualTo("John"));
        }
    }
}
