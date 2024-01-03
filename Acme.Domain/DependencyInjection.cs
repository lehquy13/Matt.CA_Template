using Acme.Domain.DomainServices;
using Acme.Domain.DomainServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {

        return services;
    }
}