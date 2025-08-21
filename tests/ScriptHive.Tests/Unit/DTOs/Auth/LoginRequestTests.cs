using ScriptHive.Application.DTOs.AuthDTOs;

namespace ScriptHive.Tests.Unit.DTOs.AuthDTOs;

public class LoginRequestTests
{
    [Test]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var username = "usuario";
        var password = "senha123";

        var dto = new LoginRequest(username, password);

        Assert.That(dto.Username, Is.EqualTo(username));
        Assert.That(dto.Password, Is.EqualTo(password));
    }
}
