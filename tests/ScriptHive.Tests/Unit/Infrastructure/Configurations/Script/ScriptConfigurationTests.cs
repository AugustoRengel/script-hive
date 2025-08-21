using Microsoft.EntityFrameworkCore.Metadata;
using ScriptHive.Domain.Entities.Script;

namespace ScriptHive.Tests.Unit.Infrastructure.Configurations.ScriptConfigurations;

public class ScriptConfigurationTests
{
    private TestDbContext _ctx = null!;
    private IEntityType _et = null!;

    [SetUp]
    public void Setup()
    {
        _ctx = TestDbContext.Create(nameof(ScriptConfigurationTests));
        _et = _ctx.Model.FindEntityType(typeof(Script))!;
    }

    [TearDown]
    public void Teardown()
    {
        _ctx.Dispose();
    }

    [Test]
    public void Script_Should_Have_Key_Id()
    {
        var key = _et.FindPrimaryKey()!.Properties.Single().Name;
        Assert.That(key, Is.EqualTo(nameof(Script.Id)));
    }

    [Test]
    public void Script_Title_Should_Be_Required_And_MaxLength_100()
    {
        var title = _et.FindProperty(nameof(Script.Title))!;
        Assert.That(title.IsNullable, Is.False);
        Assert.That(title.GetMaxLength(), Is.EqualTo(100));
    }

    [Test]
    public void Script_Content_Should_Be_Required()
    {
        var content = _et.FindProperty(nameof(Script.Content))!;
        Assert.That(content.IsNullable, Is.False);
    }

    [Test]
    public void Script_OwnerId_Should_Be_Required()
    {
        var ownerId = _et.FindProperty(nameof(Script.OwnerId))!;
        Assert.That(ownerId.IsNullable, Is.False);
    }

    [Test]
    public void Script_CreatedAt_Should_Be_Required()
    {
        var createdAt = _et.FindProperty(nameof(Script.CreatedAt))!;
        Assert.That(createdAt.IsNullable, Is.False);
    }
}
