using Arc4u.Dependency.Attribute;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace EG.DemoPCBE99925.ManageCourseService.Database;

[Export, Shared]
public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContextFactory(DbContextOptions<DatabaseContext> dbContextOptions)
    {
        Options = dbContextOptions;
    }

    private readonly DbContextOptions<DatabaseContext> Options;

    public DatabaseContext CreateDbContext(string[] args)
    {
        return new DatabaseContext(Options);
    }
}
