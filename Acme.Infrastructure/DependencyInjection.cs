using System.Text;
using Acme.Application.Contracts.Interfaces.Infrastructures;
using Acme.Infrastructure.AppLogger;
using Acme.Infrastructure.Authentication;
using Acme.Infrastructure.BackgroundJobs.Configs;
using Acme.Infrastructure.CloudServices;
using Acme.Infrastructure.EmailServices;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Quartz;

namespace Acme.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        IdentityModelEventSource.ShowPII = true; //Add this line
        
        services.AddHealthChecks();

        // Authentication configuration using jwt bearer
        services.AddAuth(configuration)
            .AddEmailConfiguration(configuration)
            .AddCloudinaryConfiguration(configuration)
            .AddQuartzConfiguration(configuration);

        //Other services
        // Should register this one in another scope
        services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ICurrentTenantService, CurrentTenantService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        //configure BackgroundService
        //services.AddHostedService<InfrastructureBackgroundService>();
        return services;
    }

    private static IServiceCollection AddCloudinaryConfiguration(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        // set configuration settings to cloudinarySettings and turn it into Singleton
        var cloudinary = new CloudinarySetting();

        configuration.Bind(CloudinarySetting.SectionName, cloudinary);
        services.AddSingleton(Options.Create(cloudinary));
        services.AddScoped<ICloudService, CloudinaryService>();

        return services;
    }

    private static IServiceCollection AddQuartzConfiguration(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        // We gonna split these config into many smaller parts
        services.AddQuartz();

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true; // wait for jobs to complete before disposing of scheduler
        }); //Create an instance while background service is triggered

        services.ConfigureOptions<LoggingBackgroundJobSetup>();
        services.ConfigureOptions<ReportBackgroundJobSetup>();
        return services;
    }


    private static IServiceCollection AddEmailConfiguration(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        // set configuration settings to emailSettingName and turn it into Singleton
        var emailSettingNames = new EmailSettings();

        configuration.Bind(EmailSettings.SectionName, emailSettingNames);
        services.AddSingleton(Options.Create(emailSettingNames));
        services.AddScoped<IEmailSender, EmailSender>();

        return services;
    }

    private static IServiceCollection AddAuth(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        // set configuration settings to jwtSettings and turn it into Singleton
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddScoped<IJwtTokenGenerator, IdentityJwtTokenClaimService>();

        services.AddAuthentication(scheme =>
            {
                scheme.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                scheme.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });
        // services.AddAuthorization(options =>
        // {
        //     options.AddPolicy("RequireAdministratorRole", policy =>
        //     {
        //         policy.RequireRole(adminRole);
        //     });
        // });
        return services;
    }
}