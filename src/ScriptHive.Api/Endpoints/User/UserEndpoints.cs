using ScriptHive.Application.DTOs.UserDTOs;
using ScriptHive.Application.Interfaces.UserInterfaces;

namespace ScriptHive.Api.Endpoints.UserEndpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this RouteGroupBuilder group)
    {
        group.WithTags("Users");

        group.MapGet("/", async (IUserService service) =>
        {
            var users = await service.GetAllAsync();
            return Results.Ok(users);
        });

        group.MapGet("/{id:guid}", async (Guid id, IUserService service) =>
        {
            var user = await service.GetByIdAsync(id);
            return user is null ? Results.NotFound() : Results.Ok(user);
        });

        group.MapPost("/", async (UserRequestDTO dto, IUserService service) =>
        {
            await service.CreateAsync(dto);
            return Results.Created($"/users", null);
        });

        group.MapPut("/{id:guid}", async (Guid id, UserRequestDTO dto, IUserService service) =>
        {
            await service.UpdateAsync(id, dto);
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IUserService service) =>
        {
            await service.DeleteAsync(id);
            return Results.NoContent();
        });
    }
}
