using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Acme_Api>("AcmeApi");

builder.Build().Run();