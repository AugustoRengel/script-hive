namespace ScriptHive.Application.Interfaces.AuthInterfaces;

public interface IAuthService
{
    Task<string> AuthenticateAsync(string username, string password);
}
