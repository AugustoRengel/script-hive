using ScriptHive.Domain.Entities.Script;
using ScriptHive.Infrastructure.Context;
using ScriptHive.Infrastructure.Repositories.ScriptRepository;
using ScriptHive.Tests.Unit.Infrastructure.Common;

namespace ScriptHive.Tests.Unit.Infrastructure.Repositories.ScriptRepositoryTests;

public class ScriptRepositoryTests
{
    private AppDbContext _ctx = null!;

    [SetUp]
    public void Setup()
    {
        _ctx = DbTestUtils.CreateInMemory(nameof(ScriptRepositoryTests));
    }

    [TearDown]
    public void Teardown()
    {
        _ctx.Dispose();
    }

    [Test]
    public async Task Create_And_GetAll_Works()
    {
        using var ctx = DbTestUtils.CreateInMemory(nameof(Create_And_GetAll_Works));
        var repo = new ScriptRepository(ctx);

        await repo.CreateAsync(new Script { Id = Guid.NewGuid(), Title = "Script 01", Content = "code", OwnerId = Guid.NewGuid()});

        var all = await repo.GetAllAsync();
        Assert.That(all.Count(), Is.EqualTo(1));
        Assert.That(all.First().Title, Is.EqualTo("Script 01"));
    }

    [Test]
    public async Task GetById_And_Exists_Works()
    {
        using var ctx = DbTestUtils.CreateInMemory(nameof(GetById_And_Exists_Works));
        var repo = new ScriptRepository(ctx);

        var c = new Script { Id = Guid.NewGuid(), Title = "Script 01", Content = "code", OwnerId = Guid.NewGuid() };
        await repo.CreateAsync(c);

        var got = await repo.GetByIdAsync(c.Id);
        var exists = await repo.ExistsAsync(c.Id);

        Assert.That(got, Is.Not.Null);
        Assert.That(exists, Is.True);
    }

    [Test]
    public async Task Update_And_Delete_Works()
    {
        using var ctx = DbTestUtils.CreateInMemory(nameof(Update_And_Delete_Works));
        var repo = new ScriptRepository(ctx);

        var c = new Script { Id = Guid.NewGuid(), Title = "Script 01", Content = "code", OwnerId = Guid.NewGuid() };
        await repo.CreateAsync(c);

        c.Title = "New title";
        await repo.UpdateAsync(c);

        var updated = await repo.GetByIdAsync(c.Id);
        Assert.That(updated!.Title, Is.EqualTo("New title"));

        await repo.DeleteAsync(c.Id);
        var afterDelete = await repo.GetByIdAsync(c.Id);
        Assert.That(afterDelete, Is.Null);
    }
}
