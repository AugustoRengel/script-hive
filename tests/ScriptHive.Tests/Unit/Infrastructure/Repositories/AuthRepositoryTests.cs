using ScriptHive.Domain.Entities.User;
using ScriptHive.Domain.ValueObjects.UserRole;
using ScriptHive.Infrastructure.Repositories.AuthRepository;
using ScriptHive.Tests.Unit.Infrastructure.Configurations;
using ScriptHive.Tests.Unit.Infrastructure.Configurations.ScriptConfigurations;

namespace ScriptHive.Tests.Unit.Infrastructure.Repositories;

public class AuthRepositoryTests
{
    private TestDbContext _ctx = null!;

    [SetUp]
    public void Setup()
    {
        _ctx = TestDbContext.Create(nameof(AuthRepositoryTests));
    }

    [TearDown]
    public void Teardown()
    {
        _ctx.Dispose();
    }

    [Test]
    public async Task GetByUsernameAsync_Returns_User_When_Exists()
    {
        _ctx.Users.Add(new User { Id = Guid.NewGuid(), Name = "daniel", Email = "daniel@email.com", Role = UserRole.User, PasswordHash = "false@hash123" });
        await _ctx.SaveChangesAsync();

        var repo = new AuthRepository(_ctx);
        var user = await repo.GetByUsernameAsync("daniel");

        Assert.That(user, Is.Not.Null);
        Assert.That(user!.Name, Is.EqualTo("daniel"));
    }

    [Test]
    public async Task GetByUsernameAsync_Returns_Null_When_NotFound()
    {
        var repo = new AuthRepository(_ctx);
        var user = await repo.GetByUsernameAsync("ghost");
        Assert.That(user, Is.Null);
    }
}
