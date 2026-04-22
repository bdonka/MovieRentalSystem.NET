using MovieRentalSystem.NET.Infrastructure.Data;
using MovieRentalSystem.NET.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddSqlServerDbContext<ApplicationDbContext>("sqldata");

var host = builder.Build();
host.Run();
