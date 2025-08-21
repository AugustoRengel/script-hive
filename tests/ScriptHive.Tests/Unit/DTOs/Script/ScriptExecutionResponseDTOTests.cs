using ScriptHive.Application.DTOs.ScriptExecutionDTOs;

namespace ScriptHive.Tests.Unit.DTOs.ScriptDTOs;

public class ScriptExecutionResponseDTOTests
{
    [Test]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var id = Guid.NewGuid();
        var result = "[]";
        var status = "Running";
        var startedAt = new DateTime(2020, 8, 1);
        var finishedAt = new DateTime(2020, 8, 1);

        var dto = new ScriptExecutionResponseDTO
        {
            Id = id,
            Result = result,
            Status = status,
            StartedAt = startedAt,
            FinishedAt = finishedAt
        };

        Assert.That(dto.Id, Is.EqualTo(id));
        Assert.That(dto.Result, Is.EqualTo(result));
        Assert.That(dto.Status, Is.EqualTo(status));
        Assert.That(dto.StartedAt, Is.EqualTo(startedAt));
        Assert.That(dto.FinishedAt, Is.EqualTo(finishedAt));
    }
}