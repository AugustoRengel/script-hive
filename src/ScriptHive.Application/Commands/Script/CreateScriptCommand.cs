namespace ScriptHive.Application.Commands.ScriptCommands;

public record CreateScriptCommand
(
    string Title,
    string Content,
    string InputTestData,
    string OutputTestData,
    Guid OwnerId
);
