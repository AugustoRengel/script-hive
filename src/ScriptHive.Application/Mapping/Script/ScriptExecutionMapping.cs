using Microsoft.FSharp.Core;
using ScriptHive.Application.DTOs.ScriptExecutionDTOs;
using ScriptHive.Domain.Entities.ScriptExecution;
using System.Text.Json;
using System.Xml;

namespace ScriptHive.Application.Mapping.ScriptExecutionMapping;

public static class ScriptExecutionMapping
{
    public static ScriptExecutionResponseDTO ToResponse(this ScriptExecution entity)
    {
        object? parsedResult = null;

        if (!string.IsNullOrEmpty(entity.Result))
        {
            parsedResult = JsonSerializer.Deserialize<JsonElement>(entity.Result);
        }

        return new ScriptExecutionResponseDTO
        {
            Id = entity.Id,
            Result = parsedResult,
            Status = entity.Status.ToString(),
            StartedAt = entity.StartedAt.HasValue ? entity.StartedAt.Value : null,
            FinishedAt = entity.FinishedAt.HasValue ? entity.FinishedAt.Value : null
        };
    }
}
