using ScriptHive.Application.DTOs.AuthDTOs;
using ScriptHive.Application.Interfaces.AuthInterfaces;

using FluentValidation;

namespace ScriptHive.Api.Endpoints.AuthEndpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this RouteGroupBuilder group)
    {
        group.WithTags("Auth");

        group.MapPost("/login", async (
            LoginRequest request,
            IValidator<LoginRequest> validator,
            IAuthService authService) =>
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors.Select(e => e.ErrorMessage));

            var token = await authService.AuthenticateAsync(request.Username, request.Password);
            return Results.Ok(new { token });
        })
        .Produces(200)
        .Produces(400)
        .Produces(401);
    }
}
