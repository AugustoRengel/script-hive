using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ScriptHive.Api.Endpoints.AuthEndpoints;
using ScriptHive.Api.Endpoints.ScriptEndpoints;
using ScriptHive.Api.Endpoints.UserEndpoints;
using ScriptHive.Api.Middlewares;
using ScriptHive.Api.OpenApi;
using ScriptHive.Application;
using ScriptHive.Infrastructure;
using ScriptHive.Infrastructure.Context;
using ScriptHive.Infrastructure.Persistence;
using System.Text;

namespace ScriptHive.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        // JWT Configuration
        var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer");
        var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience");
        var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key");

        builder.Services.AddOpenApi(options =>
        {
            // Adiciona um transformer que publica o esquema "Bearer" no documento
            options.AddDocumentTransformer<JwtBearerSecuritySchemeTransformer>();

            // Marca operações que exigem autorização com o requirement "Bearer"
            options.AddOperationTransformer((operation, context, ct) =>
            {
                var needsAuth = context.Description.ActionDescriptor
                    .EndpointMetadata.OfType<IAuthorizeData>().Any();
                if (needsAuth)
                {
                    operation.Security ??= new List<OpenApiSecurityRequirement>();
                    operation.Security.Add(new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        }] = Array.Empty<string>()
                    });
                }
                return Task.CompletedTask;
            });
        });

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
            options.AddPolicy("UserOrAdmin", p => p.RequireRole("User", "Admin"));
        });
        // End of JWT Configuration

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure();

        var app = builder.Build();

        app.UseMiddleware<GlobalExceptionMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/openapi/v1.json", "v1");
                c.RoutePrefix = "swagger"; // UI em /swagger
            });
        }

        if (!app.Environment.IsEnvironment("Testing"))
        {
            using var scope = app.Services.CreateScope();
            //var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            DbInitializer.Seed(dbContext);
        }

        if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") != "true")
        {
            app.UseHttpsRedirection();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGroup("/auth").MapAuthEndpoints();
        app.MapGroup("/users").RequireAuthorization("AdminOnly").MapUserEndpoints();
        app.MapGroup("/scripts").RequireAuthorization("UserOrAdmin").MapScriptEndpoints();

        app.MapControllers();

        app.Run();
    }
}
