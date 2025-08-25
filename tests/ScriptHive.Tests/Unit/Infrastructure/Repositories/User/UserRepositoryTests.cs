using ScriptHive.Domain.Entities.User;
using ScriptHive.Domain.ValueObjects.UserRole;
using ScriptHive.Infrastructure.Context;
using ScriptHive.Infrastructure.Repositories.UserRepository;
using ScriptHive.Tests.Unit.Infrastructure.Common;

namespace ScriptHive.Tests.Unit.Infrastructure.Repositories.UserRepositoryTest;

public class UserRepositoryTests
{
    private AppDbContext _ctx = null!;

    [SetUp]
    public void Setup()
    {
        _ctx = DbTestUtils.CreateInMemory(nameof(UserRepositoryTests));
    }

    [TearDown]
    public void Teardown()
    {
        _ctx.Dispose();
    }

    [Test]
    public async Task Create_And_GetAll_Works()
    {
        var repo = new UserRepository(_ctx);

        await repo.CreateAsync(new User { Id = Guid.NewGuid(), Name = "John", Email = "john@email.com", Role = UserRole.Admin, PasswordHash = "false@hash" });

        var all = await repo.GetAllAsync();
        Assert.That(all.Count(), Is.EqualTo(1));
        Assert.That(all.First().Name, Is.EqualTo("John"));
    }

    [Test]
    public async Task GetById_And_Exists_Works()
    {
        var repo = new UserRepository(_ctx);

        var c = new User { Id = Guid.NewGuid(), Name = "Junior", Email = "junior@email.com", Role = UserRole.User, PasswordHash = "false@hash123" };
        await repo.CreateAsync(c);

        var got = await repo.GetByIdAsync(c.Id);
        var exists = await repo.ExistsAsync(c.Id);

        Assert.That(got, Is.Not.Null);
        Assert.That(exists, Is.True);
    }

    [Test]
    public async Task Update_And_Delete_Works()
    {
        var repo = new UserRepository(_ctx);

        var c = new User { Id = Guid.NewGuid(), Name = "Solaire", Email = "solaire@email.com", Role = UserRole.User, PasswordHash = "praise@sun" };
        await repo.CreateAsync(c);

        c.Name = "sunBro";
        await repo.UpdateAsync(c);

        var updated = await repo.GetByIdAsync(c.Id);
        Assert.That(updated!.Name, Is.EqualTo("sunBro"));

        await repo.DeleteAsync(c.Id);
        var afterDelete = await repo.GetByIdAsync(c.Id);
        Assert.That(afterDelete, Is.Null);
    }
}
