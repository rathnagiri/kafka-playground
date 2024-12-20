using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<BlazorApp>("webapp");
builder.AddProject<EmployeeApplicationApi>("EmployeeApplication");

builder.Build().Run();
