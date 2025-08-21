using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using ScriptHive.Application.Services.AuthServices;
using ScriptHive.Domain.Entities.User;
using ScriptHive.Domain.Helpers;
using ScriptHive.Domain.Interfaces.AuthInterfaces;
using ScriptHive.Domain.ValueObjects.UserRole;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptHive.Tests.Unit.Services;

public class AuthServiceTests
{
    private Mock<IAuthRepository> _mockRepo = null!;
    private Mock<IConfiguration> _mockConfig = null!;
    private AuthService _authService = null!;
    private const string Key = "12345678901234567890123456789012";
    private const string Issuer = "issuer_test";
    private const string Audience = "audience_test";

    [SetUp]
    public void SetUp()
    {
        _mockRepo = new Mock<IAuthRepository>();
        _mockConfig = new Mock<IConfiguration>();
        _mockConfig.Setup(c => c["Jwt:Key"]).Returns(Key);
        _mockConfig.Setup(c => c["Jwt:Issuer"]).Returns(Issuer);
        _mockConfig.Setup(c => c["Jwt:Audience"]).Returns(Audience);
        _authService = new AuthService(_mockRepo.Object, _mockConfig.Object);
    }

    [Test]
    public async Task AuthenticateAsync_ValidCredentials_ReturnsValidJwt()
    {
        var passwordHash = PasswordHelper.HashPassword("praise@sun");
        _mockRepo.Setup(r => r.GetByUsernameAsync("Solaire"))
                 .ReturnsAsync(new User { Id = Guid.NewGuid(), Name = "Solaire", Email = "solaire@email.com", Role = UserRole.Admin, PasswordHash = passwordHash });

        var token = await _authService.AuthenticateAsync("Solaire", "praise@sun");
        Assert.That(token, Is.Not.Null.And.Not.Empty);

        var handler = new JwtSecurityTokenHandler();
        var parameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key)),
            ValidateIssuer = true,
            ValidIssuer = Issuer,
            ValidateAudience = true,
            ValidAudience = Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        handler.ValidateToken(token!, parameters, out var validatedToken);
        Assert.That(validatedToken, Is.TypeOf<JwtSecurityToken>());

        _mockRepo.Verify(r => r.GetByUsernameAsync("Solaire"), Times.Once);
    }

    [Test]
    public void AuthenticateAsync_UserNotFound_ThrowsUnauthorizedAccessException()
    {
        _mockRepo.Setup(r => r.GetByUsernameAsync("inexistente"))
                 .ReturnsAsync((User?)null);

        var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await _authService.AuthenticateAsync("inexistente", "qualquer"));

        Assert.That(ex!.Message, Is.EqualTo("Usuário não encontrado."));
        _mockRepo.Verify(r => r.GetByUsernameAsync("inexistente"), Times.Once);
    }

    [Test]
    public void AuthenticateAsync_InvalidPassword_ThrowsUnauthorizedAccessException()
    {
        _mockRepo.Setup(r => r.GetByUsernameAsync("Solaire"))
                 .ReturnsAsync(new User { Id = Guid.NewGuid(), Name = "Solaire", Email = "solaire@email.com", Role = UserRole.User, PasswordHash = "praise@sun" });

        var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await _authService.AuthenticateAsync("Solaire", "senha_errada"));

        Assert.That(ex!.Message, Is.EqualTo("Senha incorreta."));
        _mockRepo.Verify(r => r.GetByUsernameAsync("Solaire"), Times.Once);
    }
}
