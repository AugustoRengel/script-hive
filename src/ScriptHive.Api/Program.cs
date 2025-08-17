
using Microsoft.EntityFrameworkCore;
using ScriptHive.Api.Endpoints.ScriptEndpoints;
using ScriptHive.Application;
using ScriptHive.Infrastructure;
using ScriptHive.Infrastructure.Context;

namespace ScriptHive.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddOpenApi();

        builder.Services.AddAuthorization();


        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure();

        var app = builder.Build();

        if (Environment.GetEnvironmentVariable("ENABLE_SWAGGER") == "true")
        {
            app.MapOpenApi();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "v1");
            });
        }

        if (!app.Environment.IsEnvironment("Testing"))
        {
            using var scope = app.Services.CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.Database.Migrate();

        }

        if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") != "true")
        {
            app.UseHttpsRedirection();
        }

        app.UseAuthorization();

        app.MapGroup("/scripts").MapScriptEndpoints();

        app.MapControllers();

        app.Run();
    }
}
