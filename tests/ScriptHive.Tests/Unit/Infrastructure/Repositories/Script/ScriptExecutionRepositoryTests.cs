using ScriptHive.Domain.Entities.ScriptExecution;
using ScriptHive.Domain.ValueObjects.ExecutionStatus;
using ScriptHive.Infrastructure.Context;
using ScriptHive.Infrastructure.Repositories.ScriptExecutionRepository;
using ScriptHive.Tests.Unit.Infrastructure.Common;

namespace ScriptHive.Tests.Unit.Infrastructure.Repositories.ScriptExecutionRepositoryTests;

public class ScriptExecutionRepositoryTests
{
    private AppDbContext _ctx = null!;

    [SetUp]
    public void Setup()
    {
        _ctx = DbTestUtils.CreateInMemory(nameof(ScriptExecutionRepositoryTests));
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
        var repo = new ScriptExecutionRepository(ctx);

        await repo.CreateAsync(
            new ScriptExecution 
            { 
                Id = Guid.NewGuid(),
                ScriptId = Guid.NewGuid(),
                Input = "[{\"key\": \"value\"}]",
                Status = ExecutionStatus.Running,
                Result = null,
                CreatedAt = DateTime.UtcNow,
                StartedAt = null,
                FinishedAt = null
            }
        );

        var all = await repo.GetAllAsync();
        Assert.That(all.Count(), Is.EqualTo(1));
        Assert.That(all.First().Input, Is.EqualTo("[{\"key\": \"value\"}]"));
    }

    [Test]
    public async Task GetById_And_Exists_Works()
    {
        using var ctx = DbTestUtils.CreateInMemory(nameof(GetById_And_Exists_Works));
        var repo = new ScriptExecutionRepository(ctx);

        var c = new ScriptExecution
        {
            Id = Guid.NewGuid(),
            ScriptId = Guid.NewGuid(),
            Input = "[{\"key\": \"value\"}]",
            Status = ExecutionStatus.Running,
            Result = null,
            CreatedAt = DateTime.UtcNow,
            StartedAt = null,
            FinishedAt = null
        };

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
        var repo = new ScriptExecutionRepository(ctx);

        var c = new ScriptExecution 
        {
            Id = Guid.NewGuid(),
            ScriptId = Guid.NewGuid(),
            Input = "[{\"key\": \"value\"}]",
            Status = ExecutionStatus.Running,
            Result = null,
            CreatedAt = DateTime.UtcNow,
            StartedAt = null,
            FinishedAt = null
        };
        await repo.CreateAsync(c);

        c.Status = ExecutionStatus.Completed;
        await repo.UpdateAsync(c);

        var updated = await repo.GetByIdAsync(c.Id);
        Assert.That(updated!.Status.IsCompleted);

        await repo.DeleteAsync(c.Id);
        var afterDelete = await repo.GetByIdAsync(c.Id);
        Assert.That(afterDelete, Is.Null);
    }
}
