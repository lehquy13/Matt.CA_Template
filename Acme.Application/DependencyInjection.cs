using System.Reflection;
using Acme.Domain;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Matt.SharedKernel.Application.Authorizations;
using Matt.SharedKernel.Application.Validations;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(DependencyInjection).Assembly;

        services
            .AddMediator(GetApplicationCoreAssemblies)
            .AddValidatorsFromAssembly(applicationAssembly) // Handle base validation of application layer
            .AddApplicationMappings(applicationAssembly)
            .AddLazyCache();

        return services;
    }

    private static IServiceCollection AddMediator(this IServiceCollection services, Assembly[] assemblies)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assemblies);

            cfg.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
        });

        return services;
    }

    private static IServiceCollection AddApplicationMappings(this IServiceCollection services, Assembly assembly)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        config.Scan(assembly);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }

    private static Assembly[] GetApplicationCoreAssemblies =>
    [
        typeof(DependencyInjection).Assembly,
        typeof(DomainDependencyInjection).Assembly
    ];
}