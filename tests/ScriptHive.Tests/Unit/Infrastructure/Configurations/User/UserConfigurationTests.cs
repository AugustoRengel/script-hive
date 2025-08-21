using Microsoft.EntityFrameworkCore.Metadata;
using ScriptHive.Domain.Entities.User;
using ScriptHive.Domain.ValueObjects.UserRole;

namespace ScriptHive.Tests.Unit.Infrastructure.Configurations.UserConfigurations;

public class UserConfigurationTests
{
    private TestDbContext _ctx = null!;
    private IEntityType _et = null!;

    [SetUp]
    public void Setup()
    {
        _ctx = TestDbContext.Create(nameof(UserConfigurationTests));
        _et = _ctx.Model.FindEntityType(typeof(User))!;
    }

    [TearDown]
    public void Teardown()
    {
        _ctx.Dispose();
    }

    [Test]
    public void User_Should_Have_PrimaryKey_Id()
    {
        var key = _et.FindPrimaryKey()!.Properties.Single().Name;
        Assert.That(key, Is.EqualTo(nameof(User.Id)));
    }

    [Test]
    public void User_Name_Should_Be_Required_And_MaxLength_100()
    {
        var name = _et.FindProperty(nameof(User.Name))!;
        Assert.That(name.IsNullable, Is.False);
        Assert.That(name.GetMaxLength(), Is.EqualTo(100));
    }

    [Test]
    public void User_Email_Should_Have_MaxLength_100()
    {
        var email = _et.FindProperty(nameof(User.Email))!;
        Assert.That(email.GetMaxLength(), Is.EqualTo(100));
    }

    [Test]
    public void User_Role_Should_Have_ValueConverter()
    {
        var role = _et.FindProperty(nameof(User.Role))!;
        var converter = role.GetValueConverter()!;
        var toProvider = converter.ConvertToProviderExpression.Compile().DynamicInvoke(UserRole.Admin);
        var fromProvider = converter.ConvertFromProviderExpression.Compile().DynamicInvoke("Admin");

        Assert.That(role.IsNullable, Is.False);
        Assert.That(converter, Is.Not.Null);
        Assert.That(toProvider, Is.EqualTo("Admin"));
        Assert.That(fromProvider, Is.EqualTo(UserRole.Admin));
    }

    [Test]
    public void User_PasswordHash_Should_Be_Required()
    {
        var passwordHash = _et.FindProperty(nameof(User.PasswordHash))!;
        Assert.That(passwordHash.IsNullable, Is.False);
    }

    [Test]
    public void User_CreatedAt_Should_Be_Required()
    {
        var created = _et.FindProperty(nameof(User.CreatedAt))!;
        Assert.That(created.IsNullable, Is.False);
    }
}