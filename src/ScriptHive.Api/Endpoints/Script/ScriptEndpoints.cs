using ScriptHive.Application.DTOs.ScriptDTOs;
using ScriptHive.Application.Interfaces.ScriptInterfaces;

namespace ScriptHive.Api.Endpoints.ScriptEndpoints;

public static class ScriptEndpoints
{
    public static void MapScriptEndpoints(this RouteGroupBuilder group)
    {
        group.WithTags("Scripts");

        group.MapGet("/", async (IScriptService service) =>
        {
            var scripts = await service.GetAllAsync();
            return Results.Ok(scripts);
        });

        group.MapGet("/{id:guid}", async (Guid id, IScriptService service) =>
        {
            var script = await service.GetByIdAsync(id);
            return script is null ? Results.NotFound() : Results.Ok(script);
        });

        group.MapPost("/", async (ScriptRequestDTO dto, IScriptService service) =>
        {
            await service.CreateAsync(dto);
            return Results.Created($"/scripts", null);
        });

        group.MapPut("/{id:guid}", async (Guid id, ScriptRequestDTO dto, IScriptService service) =>
        {
            await service.UpdateAsync(id, dto);
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IScriptService service) =>
        {
            await service.DeleteAsync(id);
            return Results.NoContent();
        });
    }
}
