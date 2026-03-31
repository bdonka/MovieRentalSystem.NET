var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql");
var sqldata = sql.AddDatabase("sqldata");


var migrations = builder.AddProject<Projects.MovieRentalSystem_NET_MigrationService>("migrations")
    .WithReference(sql)
    .WaitFor(sql);

var api = builder.AddProject<Projects.MovieRentalSystem_NET_WebApi>("api")
    .WithReference(sqldata)
    .WithReference(migrations)
    .WaitForCompletion(migrations);

builder.Build().Run();
