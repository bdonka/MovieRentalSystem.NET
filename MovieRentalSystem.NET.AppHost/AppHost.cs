var builder = DistributedApplication.CreateBuilder(args);

var dbPassword = builder.AddParameter("db-password", secret: true);

var sql = builder.AddSqlServer("sql", password: dbPassword, port: 14430).AddDatabase("sqldata");

var migrations = builder.AddProject<Projects.MovieRentalSystem_NET_MigrationService>("migrations")
    .WithReference(sql)
    .WaitFor(sql);

var api = builder.AddProject<Projects.MovieRentalSystem_NET_WebApi>("api")
    .WithUrlForEndpoint("http", url => url.Url += "/scalar")
    .WithUrlForEndpoint("https", url => url.Url += "/scalar")
    .WithReference(sql)
    .WithReference(migrations)
    .WaitForCompletion(migrations);

builder.Build().Run();
