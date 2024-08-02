var builder = DistributedApplication.CreateBuilder(args);

var apiapp = builder.AddProject<Projects.MSBuildForContainers_ApiApp>("apiapp");

builder.AddProject<Projects.MSBuildForContainers_WebApp>("webapp")
       .WithReference(apiapp);

builder.Build().Run();
