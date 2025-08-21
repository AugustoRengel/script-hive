using Microsoft.EntityFrameworkCore.Metadata;
using ScriptHive.Domain.Entities.ScriptExecution;
using ScriptHive.Domain.ValueObjects.ExecutionStatus;

namespace ScriptHive.Tests.Unit.Infrastructure.Configurations.ScriptConfigurations;

public class ScriptExecutionConfigurationTests
{
    private TestDbContext _ctx = null!;
    private IEntityType _et = null!;

    [SetUp]
    public void Setup()
    {
        _ctx = TestDbContext.Create(nameof(ScriptConfigurationTests));
        _et = _ctx.Model.FindEntityType(typeof(ScriptExecution))!;
    }

    [TearDown]
    public void Teardown()
    {
        _ctx.Dispose();
    }

    [Test]
    public void Script_Execution_Should_Have_Key_Id()
    {
        var key = _et.FindPrimaryKey()!.Properties.Single().Name;
        Assert.That(key, Is.EqualTo(nameof(ScriptExecution.Id)));
    }

    [Test]
    public void Script_Execution_ScriptId_Should_Be_Required()
    {
        var prop = _et.FindProperty(nameof(ScriptExecution.ScriptId))!;
        Assert.That(prop.IsNullable, Is.False);
    }

    [Test]
    public void Script_Execution_Input_Should_Be_Required()
    {
        var prop = _et.FindProperty(nameof(ScriptExecution.Input))!;
        Assert.That(prop.IsNullable, Is.False);
    }

    [Test]
    public void Script_Execution_Status_Should_Have_ValueConverter_And_Be_Required()
    {
        var prop = _et.FindProperty(nameof(ScriptExecution.Status))!;
        var converter = prop.GetValueConverter()!;
        var toProvider = converter.ConvertToProviderExpression.Compile().DynamicInvoke(ExecutionStatus.Running);
        var fromProvider = converter.ConvertFromProviderExpression.Compile().DynamicInvoke("Running");

        Assert.That(prop.IsNullable, Is.False);
        Assert.That(converter, Is.Not.Null);
        Assert.That(toProvider, Is.EqualTo("Running"));
        Assert.That(fromProvider, Is.EqualTo(ExecutionStatus.Running));
    }

    [Test]
    public void Script_Execution_Result_Can_Be_Null()
    {
        var prop = _et.FindProperty(nameof(ScriptExecution.Result))!;
        Assert.That(prop.IsNullable, Is.True);
    }

    [Test]
    public void Script_Execution_CreatedAt_Should_Be_Required()
    {
        var prop = _et.FindProperty(nameof(ScriptExecution.CreatedAt))!;
        Assert.That(prop.IsNullable, Is.False);
    }

    [Test]
    public void Script_Execution_StartedAt_And_FinishedAt_Can_Be_Null()
    {
        var started = _et.FindProperty(nameof(ScriptExecution.StartedAt))!;
        var finished = _et.FindProperty(nameof(ScriptExecution.FinishedAt))!;

        Assert.That(started.IsNullable, Is.True);
        Assert.That(finished.IsNullable, Is.True);
    }
}
