using Acme.Domain;
using Acme.Domain.DomainServices;
using Acme.Domain.DomainServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<IIdentityDomainServices, IdentityDomainServices>();

        return services;
    }
}