using Acme.Aspire.ServiceDefaults;

namespace Acme.Api.AspireIntegrations;

public static class AspireExtensions
{
    public static WebApplicationBuilder AddAspire(this WebApplicationBuilder builder)
    {
        builder.AddSqlServerClient(connectionName: "DefaultConnection");

        builder.AddServiceDefaults();

        return builder;
    }
}