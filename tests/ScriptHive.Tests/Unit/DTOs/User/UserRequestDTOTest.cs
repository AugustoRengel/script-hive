using ScriptHive.Application.DTOs.UserDTOs;

namespace ScriptHive.Tests.Unit.DTOs.UserDTOs;

public class UserRequestDTOTest
{
    [Test]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var name = "John C Sharpinson";
        var email = "John@gmail.com";
        var role = "Admin";
        var password = "see#Sharp";

        var dto = new UserRequestDTO
        (
            Name: name,
            Email: email,
            Role: role,
            Password: password
        );

        Assert.That(dto.Name, Is.EqualTo(name));
        Assert.That(dto.Email, Is.EqualTo(email));
        Assert.That(dto.Role, Is.EqualTo(role));
        Assert.That(dto.Password, Is.EqualTo(password));
    }
}
