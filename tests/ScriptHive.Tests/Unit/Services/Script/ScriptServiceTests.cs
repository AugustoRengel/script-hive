using ScriptHive.Domain.Entities.Script;
using ScriptHive.Domain.Entities.ScriptExecution;
using ScriptHive.Domain.Interfaces.ScriptInterfaces;
using ScriptHive.Application.Commands.ScriptCommands;
using ScriptHive.Application.Services.ScriptServices;

using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace ScriptHive.Tests.Unit.Services.ScriptServiceTests;

public class ScriptServiceTests
{
    private Mock<IScriptRepository> _scriptRepositoryMock = null!;
    private Mock<IScriptExecutionRepository> _scriptExecutionRepositoryMock = null!;
    private Mock<IValidator<CreateScriptCommand>> _validatorMock = null!;
    private Mock<IScriptExecutionResultQueue> _queueMock = null!;
    private ScriptService _scriptService = null!;

    [SetUp]
    public void SetUp()
    {
        _scriptRepositoryMock = new Mock<IScriptRepository>();
        _scriptExecutionRepositoryMock = new Mock<IScriptExecutionRepository>();
        _validatorMock = new Mock<IValidator<CreateScriptCommand>>();
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateScriptCommand>(), default))
                  .ReturnsAsync(new ValidationResult());
        _queueMock = new Mock<IScriptExecutionResultQueue>();

        _scriptService = new ScriptService(_scriptRepositoryMock.Object, _scriptExecutionRepositoryMock.Object, _validatorMock.Object, _queueMock.Object);
    }

    [Test]
    public async Task CreateAsync_ValidDto_CallsValidatorAndRepository()
    {
        var dto = new CreateScriptCommand(
            Title: "Script Title", 
            Content: """
            function process(data) {
              return JSON.parse(JSON.stringify(data));
            }
            """, 
            InputTestData: "[{\"key\": \"value\"}]", 
            OutputTestData: "[{\"key\": \"value\"}]",
            OwnerId: Guid.NewGuid()
        );

        var validationContext = new ValidationContext<CreateScriptCommand>(dto);

        _validatorMock.Setup(v => v.ValidateAsync(It.Is<ValidationContext<CreateScriptCommand>>(c => c.InstanceToValidate == dto), default))
            .ReturnsAsync(new ValidationResult());
        _scriptRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Script>())).Returns(Task.CompletedTask);

        await _scriptService.CreateAsync(dto);

        _validatorMock.Verify(v => v.ValidateAsync(It.Is<ValidationContext<CreateScriptCommand>>(c => c.InstanceToValidate == dto), default), Times.Once);
        _scriptRepositoryMock.Verify(r => r.CreateAsync(It.Is<Script>(c => c.Id != Guid.Empty)), Times.Once);
    }

    [Test]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        var scripts = new List<Script>
        {
            new Script { Id = Guid.NewGuid(), Title = "Script 01", Content = "code", OwnerId = Guid.NewGuid()},
            new Script { Id = Guid.NewGuid(), Title = "Script 02", Content = "code", OwnerId = Guid.NewGuid()},
            new Script { Id = Guid.NewGuid(), Title = "Script 03", Content = "code", OwnerId = Guid.NewGuid()}
        };
        _scriptRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(scripts);

        var result = await _scriptService.GetAllAsync();

        Assert.That(result.Count(), Is.EqualTo(3));
        Assert.That(result.First().Title, Is.EqualTo("Script 01"));
        Assert.That(result.Last().Title, Is.EqualTo("Script 03"));
    }

    [Test]
    public async Task GetByIdAsync_ExistingId_ReturnsMappedDto()
    {
        var id = Guid.NewGuid();
        var script = new Script { Id = id, Title = "Script 01", Content = "code", OwnerId = Guid.NewGuid() };
        _scriptRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(script);

        var result = await _scriptService.GetByIdAsync(id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(id));
    }

    [Test]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        var id = Guid.NewGuid();
        _scriptRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Script?)null);

        var result = await _scriptService.GetByIdAsync(id);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task UpdateAsync_ValidDto_ExistingScript_Updates()
    {
        var id = Guid.NewGuid();
        var dto = new CreateScriptCommand(
            Title: "Script Title",
            Content: """
            function process(data) {
              return JSON.parse(JSON.stringify(data));
            }
            """,
            InputTestData: "[{\"key\": \"value\"}]",
            OutputTestData: "[{\"key\": \"value\"}]",
            OwnerId: Guid.NewGuid()
        );
        var existing = new Script { Id = id, Title = "Script 01", Content = "code", OwnerId = Guid.NewGuid() };
        var validationContext = new ValidationContext<CreateScriptCommand>(dto);

        _validatorMock.Setup(v => v.ValidateAsync(It.Is<ValidationContext<CreateScriptCommand>>(c => c.InstanceToValidate == dto), default))
            .ReturnsAsync(new ValidationResult());
        _scriptRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
        _scriptRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Script>())).Returns(Task.CompletedTask);

        await _scriptService.UpdateAsync(id, dto);

        _validatorMock.Verify(v => v.ValidateAsync(It.Is<ValidationContext<CreateScriptCommand>>(c => c.InstanceToValidate == dto), default), Times.Once);
        _scriptRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Script>(c => c.Id == id && c.CreatedAt == existing.CreatedAt)), Times.Once);
    }

    [Test]
    public void UpdateAsync_NonExistingScript_ThrowsKeyNotFoundException()
    {
        var id = Guid.NewGuid();
        var dto = new CreateScriptCommand(
            Title: "Script Title",
            Content: """
            function process(data) {
              return JSON.parse(JSON.stringify(data));
            }
            """,
            InputTestData: "[{\"key\": \"value\"}]",
            OutputTestData: "[{\"key\": \"value\"}]",
            OwnerId: Guid.NewGuid()
        );
        _validatorMock.Setup(v => v.ValidateAsync(dto, default)).ReturnsAsync(new ValidationResult());
        _scriptRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Script?)null);

        var ex = Assert.ThrowsAsync<KeyNotFoundException>(() => _scriptService.UpdateAsync(id, dto));
        Assert.That(ex!.Message, Is.EqualTo("Script não encontrado."));
    }

    [Test]
    public async Task DeleteAsync_ExistingScript_Deletes()
    {
        var id = Guid.NewGuid();
        var existing = new Script { Id = id };
        _scriptRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);
        _scriptRepositoryMock.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

        await _scriptService.DeleteAsync(id);

        _scriptRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
    }

    [Test]
    public void DeleteAsync_NonExistingScript_ThrowsKeyNotFoundException()
    {
        var id = Guid.NewGuid();
        _scriptRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Script?)null);

        var ex = Assert.ThrowsAsync<KeyNotFoundException>(() => _scriptService.DeleteAsync(id));
        Assert.That(ex!.Message, Is.EqualTo("Script não encontrado."));
    }
}
