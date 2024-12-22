using Acme.Api.Middlewares;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Acme.Api;

public static class DependencyInjection
{
    public static void AddPresentation(this IServiceCollection services, IHostBuilder builder)
    {
        services
            .AddRouting(options => options.LowercaseUrls = true)
            .AddControllers();

        services.AddSwagger();
        services.AddMiddlewares();

        builder.UseSerilog((context, configuration) =>
        {
            configuration
                .Enrich.FromLogContext()
                .MinimumLevel.Information()
                .WriteTo.Console();
        });
    }

    private static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "Matt_Acme_Api",
                Version = "3.0",
                Description = "This is the settings for Acme API",
            });
            c.EnableAnnotations();
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description = "Enter 'bearer' [space] and your token",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });
    }

    private static void AddMiddlewares(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();
    }
}