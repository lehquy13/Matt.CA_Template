using System.Reflection;
using Acme.Application;
using Acme.Infrastructure;
using Acme.Persistence;
using Matt.AutoDI;
using Matt.SharedKernel.DependencyInjections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.Api.Host;

public static class DependencyInjection
{
    public static IServiceCollection AddHost(this IServiceCollection services, ConfigurationManager configuration)
    {
        Assembly[] assemblies =
        {
            typeof(Acme.Application.DependencyInjection).Assembly,
            typeof(Acme.Infrastructure.DependencyInjection).Assembly,
            typeof(Acme.Persistence.DependencyInjection).Assembly,
        };

        services.AddServiced(assemblies);
        
        services
            .AddSharedKernel(typeof(Acme.Application.DependencyInjection).Assembly)
            .AddApplication()
            .AddPersistence(configuration)
            .AddInfrastructure(configuration);

        return services;
    }
}