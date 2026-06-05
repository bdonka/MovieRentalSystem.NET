using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Infrastructure.Data;

namespace MovieRentalSystem.NET.Infrastructure.Tests.Common;

public static class DbContextFactory
{
    public static ApplicationDbContext CreateSqlite()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        return context;
    }
}
