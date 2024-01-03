using Acme.Domain.Acme;
using Acme.Domain.Acme.Users;
using Acme.Domain.Acme.Users.Identities;
using Acme.Persistence.EntityFrameworkCore;
using Acme.Persistence.Repository;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        // set configuration settings to emailSettingName and turn it into Singleton

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")
            )
        );

        var dbContext = services
            .BuildServiceProvider()
            .GetRequiredService<AppDbContext>();

        dbContext.Database.EnsureCreated();

        //Seed data using DataSeed
        DataSeeding.SeedData(dbContext);

        // Dependency Injection for repository
        //services.AddLazyCache();

        //services.AddScoped(typeof(IRepository<,>), typeof(RepositoryImpl<,>));
        //services.AddScoped(typeof(IReadOnlyRepository<,>), typeof(ReadOnlyRepositoryImpl<,>));
        //services.AddScoped<IUnitOfWork, UnitOfWork>();

        //services.AddScoped<IIdentityRepository, IdentityRepository>();
        //services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}