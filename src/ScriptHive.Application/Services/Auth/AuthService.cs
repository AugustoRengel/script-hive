using ScriptHive.Application.Interfaces.AuthInterfaces;
using ScriptHive.Domain.Interfaces.AuthInterfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ScriptHive.Domain.Helpers;

namespace ScriptHive.Application.Services.AuthServices;

public class AuthService(IAuthRepository repository, IConfiguration config) : IAuthService
{
    private readonly IAuthRepository _repository = repository;
    private readonly IConfiguration _config = config;

    public async Task<string> AuthenticateAsync(string username, string password)
    {
        var user = await _repository.GetByUsernameAsync(username);

        if (user == null)
            throw new UnauthorizedAccessException("Usuário não encontrado.");

        if (!PasswordHelper.VerifyPassword(password, user.PasswordHash))
            throw new UnauthorizedAccessException("Senha incorreta.");

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
