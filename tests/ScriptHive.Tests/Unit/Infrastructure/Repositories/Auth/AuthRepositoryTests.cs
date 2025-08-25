using ScriptHive.Domain.Entities.User;
using ScriptHive.Domain.ValueObjects.UserRole;
using ScriptHive.Infrastructure.Context;
using ScriptHive.Infrastructure.Repositories.AuthRepository;
using ScriptHive.Tests.Unit.Infrastructure.Common;

namespace ScriptHive.Tests.Unit.Infrastructure.Repositories.AuthRepositoryTest;

public class AuthRepositoryTests
{
    private AppDbContext _ctx = null!;

    [SetUp]
    public void Setup()
    {
        _ctx = DbTestUtils.CreateInMemory(nameof(AuthRepositoryTests));
    }

    [TearDown]
    public void Teardown()
    {
        _ctx.Dispose();
    }

    [Test]
    public async Task GetByUsernameAsync_Returns_User_When_Exists()
    {
        _ctx.Users.Add(new User { Id = Guid.NewGuid(), Name = "Isa", Email = "Isa@email.com", Role = UserRole.User, PasswordHash = "false@hash123" });
        await _ctx.SaveChangesAsync();

        var repo = new AuthRepository(_ctx);
        var user = await repo.GetByUsernameAsync("Isa");

        Assert.That(user, Is.Not.Null);
        Assert.That(user!.Name, Is.EqualTo("Isa"));
    }

    [Test]
    public async Task GetByUsernameAsync_Returns_Null_When_NotFound()
    {
        var repo = new AuthRepository(_ctx);
        var user = await repo.GetByUsernameAsync("ghost");
        Assert.That(user, Is.Null);
    }
}
