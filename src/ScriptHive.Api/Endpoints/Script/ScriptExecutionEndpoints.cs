using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ScriptHive.Api.DTOs.ScriptDTOs;
using ScriptHive.Api.Helpers;
using ScriptHive.Application.Interfaces.ScriptInterfaces;
using System.Security.Claims;

namespace ScriptHive.Api.Endpoints.ScriptExecutionEndpoints;

public static class ScriptExecutionEndpoints
{
    public static void MapScriptExecutionEndpoints(this RouteGroupBuilder group)
    {
        group.WithTags("Executions");

        group.MapGet("/", async (IScriptService service) =>
        {
            var executions = await service.GetAllExecutionsAsync();
            return Results.Ok(executions);
        });

        group.MapGet("/{id:guid}", async (Guid id, IScriptService service) =>
        {
            var script = await service.GetExecutionByIdAsync(id);
            return script is null ? Results.NotFound() : Results.Ok(script);
        });

        group.MapPost("/{id:guid}",
            async (
                Guid id,
                [FromForm] ScriptExecutionDTO dto,
                IScriptService service,
                IValidator<ScriptExecutionDTO> _validator,
                HttpContext httpContext) =>
            {
                await _validator.ValidateAndThrowAsync(dto);

                var ownerId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(ownerId))
                    return Results.Unauthorized();

                var inputData = await FormFileHelper.ReadFormFileAsync(dto.InputData);
                if (inputData is null)
                    return Results.BadRequest("O arquivo json com dados de input é obrigatório.");

                var executionId = await service.ExecuteByIdAsync(id, inputData);
                return Results.Accepted($"/{id}", new { executionId });
            })
        .DisableAntiforgery();

        group.MapPost("/{id:guid}/cancel", (Guid id, IScriptService service) =>
        {
            service.CancelExecution(id);
            return Results.NoContent();
        });
    }
}
