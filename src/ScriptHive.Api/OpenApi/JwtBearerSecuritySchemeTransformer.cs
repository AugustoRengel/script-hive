using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace ScriptHive.Api.OpenApi;

/// <summary>
/// Transformer que injeta o esquema "Bearer" no topo do documento OpenAPI
/// com base nos esquemas de autenticação registrados.
/// </summary>
sealed class JwtBearerSecuritySchemeTransformer(
    Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider schemes
) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document,
        OpenApiDocumentTransformerContext context, CancellationToken ct)
    {
        var all = await schemes.GetAllSchemesAsync();
        if (!all.Any(s => s.Name == JwtBearerDefaults.AuthenticationScheme || s.Name == "Bearer"))
            return;

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, OpenApiSecurityScheme>();

        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Insira apenas o token (sem 'Bearer ')."
        };
    }
}
