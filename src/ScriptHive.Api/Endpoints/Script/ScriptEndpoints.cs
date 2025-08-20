using FluentValidation;
using Jint.Native;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScriptHive.Api.DTOs.ScriptDTOs;
using ScriptHive.Api.Helpers;
using ScriptHive.Application.Commands.ScriptCommands;
using ScriptHive.Application.Interfaces.ScriptInterfaces;
using System.Security.Claims;
using System.Text.Json;

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

        group.MapPost("/", 
            async (
                [FromForm] ScriptRequestDTO dto, 
                IScriptService service,
                IValidator<ScriptRequestDTO> _validator,
                HttpContext httpContext) =>
        {
            await _validator.ValidateAndThrowAsync(dto);

            var ownerId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(ownerId))
                return Results.Unauthorized();

            var content = await FormFileHelper.ReadFormFileAsync(dto.ContentFile);
            if (content is null)
                return Results.BadRequest("O arquivo do script é obrigatório.");

            var inputTestData = await FormFileHelper.ReadFormFileAsync(dto.InputTestData);
            if (inputTestData is null)
                return Results.BadRequest("O arquivo do inputTestData é obrigatório.");

            var outputTestData = await FormFileHelper.ReadFormFileAsync(dto.OutputTestData);
            if (outputTestData is null)
                return Results.BadRequest("O arquivo do outputTestData é obrigatório.");

            var command = new CreateScriptCommand(
                Title: dto.Title,
                Content: content,
                InputTestData: inputTestData,
                OutputTestData: outputTestData,
                OwnerId: new Guid(ownerId)
            );

            await service.CreateAsync(command);
            return Results.Created($"/scripts", null);
        })
        .DisableAntiforgery();

        group.MapPost("/{id:guid}/execute",
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

                var scriptOutput = await service.ExecuteByIdAsync(id, inputData);
                return Results.Text(scriptOutput, "application/json");
            })
        .DisableAntiforgery();

        group.MapPut("/{id:guid}", 
            async (
                Guid id, 
                [FromForm] ScriptRequestDTO dto, 
                IScriptService service,
                IValidator<ScriptRequestDTO> _validator,
                HttpContext httpContext) =>
        {
            await _validator.ValidateAndThrowAsync(dto);

            var ownerId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(ownerId))
                return Results.Unauthorized();

            var content = await FormFileHelper.ReadFormFileAsync(dto.ContentFile);
            if (content is null)
                return Results.BadRequest("O arquivo do script é obrigatório.");

            var inputTestData = await FormFileHelper.ReadFormFileAsync(dto.InputTestData);
            if (inputTestData is null)
                return Results.BadRequest("O arquivo do inputTestData é obrigatório.");

            var outputTestData = await FormFileHelper.ReadFormFileAsync(dto.OutputTestData);
            if (outputTestData is null)
                return Results.BadRequest("O arquivo do outputTestData é obrigatório.");

            var command = new CreateScriptCommand(
                Title: dto.Title,
                Content: content,
                InputTestData: inputTestData,
                OutputTestData: outputTestData,
                OwnerId: new Guid(ownerId)
            );

            await service.UpdateAsync(id, command);
            return Results.NoContent();
        })
        .DisableAntiforgery();

        group.MapDelete("/{id:guid}", async (Guid id, IScriptService service) =>
        {
            await service.DeleteAsync(id);
            return Results.NoContent();
        });
    }
}
