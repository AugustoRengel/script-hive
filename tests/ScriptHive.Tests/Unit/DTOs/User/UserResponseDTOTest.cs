using ScriptHive.Application.DTOs.UserDTOs;

namespace ScriptHive.Tests.Unit.DTOs.UserDTOs;

public class UserResponseDTOTest
{
    [Test]
    public void Constructor_SetsPropertiesCorrectly()
    {
        var id = Guid.NewGuid();
        var name = "John C Sharpinson";
        var email = "John@gmail.com";
        var role = "Admin";
        var createdAt = new DateTime(2020, 8, 1);

        var dto = new UserResponseDTO
        {
            Id = id,
            Name = name,
            Email = email,
            Role = role,
            CreatedAt = createdAt
        };

        Assert.That(dto.Id, Is.EqualTo(id));
        Assert.That(dto.Name, Is.EqualTo(name));
        Assert.That(dto.Email, Is.EqualTo(email));
        Assert.That(dto.Role, Is.EqualTo(role));
        Assert.That(dto.CreatedAt, Is.EqualTo(createdAt));
    }
}