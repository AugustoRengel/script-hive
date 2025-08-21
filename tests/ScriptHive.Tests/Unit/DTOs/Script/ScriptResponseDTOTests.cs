using ScriptHive.Application.DTOs.ScriptDTOs;

namespace ScriptHive.Tests.Unit.DTOs.ScriptDTOs;

public class ScriptResponseDTOTests
{
    [Test]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var id = Guid.NewGuid();
        var title = "Script Title";
        var content = "Script content string";
        var ownerId = Guid.NewGuid();
        var createdAt = new DateTime(2020, 8, 1);

        var dto = new ScriptResponseDTO
        {
            Id = id,
            Title = title,
            Content = content,
            OwnerId = ownerId,
            CreatedAt = createdAt
        };

        Assert.That(dto.Id, Is.EqualTo(id));
        Assert.That(dto.Title, Is.EqualTo(title));
        Assert.That(dto.Content, Is.EqualTo(content));
        Assert.That(dto.OwnerId, Is.EqualTo(ownerId));
        Assert.That(dto.CreatedAt, Is.EqualTo(createdAt));
    }
}
