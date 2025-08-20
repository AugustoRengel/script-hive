namespace ScriptHive.Api.DTOs.ScriptDTOs;

public record ScriptRequestDTO
(
    string Title,
    IFormFile ContentFile,
    IFormFile InputTestData,
    IFormFile OutputTestData
);
