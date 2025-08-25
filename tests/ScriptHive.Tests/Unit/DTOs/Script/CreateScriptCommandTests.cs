using ScriptHive.Application.Commands.ScriptCommands;

namespace ScriptHive.Tests.Unit.DTOs.ScriptDTOs;

public class CreateScriptCommandTests
{
    [Test]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var title = "Script Title";
        var content = "Script content";
        var inputTestData = "script input";
        var outputTestData = "expected output";
        var ownerId = Guid.NewGuid();

        var dto = new CreateScriptCommand(
            Title: title,
            Content: content,
            InputTestData: inputTestData,
            OutputTestData: outputTestData,
            OwnerId: ownerId
        );

        Assert.That(dto.Title, Is.EqualTo(title));
        Assert.That(dto.Content, Is.EqualTo(content));
        Assert.That(dto.InputTestData, Is.EqualTo(inputTestData));
        Assert.That(dto.OutputTestData, Is.EqualTo(outputTestData));
        Assert.That(dto.OwnerId, Is.EqualTo(ownerId));
    }
}
