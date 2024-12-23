using Acme.Api;
using Acme.Api.AspireIntegrations;
using Acme.Application;
using Acme.Aspire.ServiceDefaults;
using Acme.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddAspire();

// Add services to the container.
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPersistence(builder.Configuration)
    .AddPresentation(builder.Host);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"); });
}

app.UseExceptionHandler();

app.UseSerilogRequestLogging();

app.AddInfrastructureMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();