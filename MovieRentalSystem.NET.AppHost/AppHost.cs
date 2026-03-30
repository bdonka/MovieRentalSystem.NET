var builder = DistributedApplication.CreateBuilder(args);
var api = builder.AddProject<Projects.MovieRentalSystem_NET_WebApi>("api");

builder.Build().Run();
