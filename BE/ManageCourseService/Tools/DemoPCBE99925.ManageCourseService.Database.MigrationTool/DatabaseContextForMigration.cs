using EG.DemoPCBE99925.ManageCourseService.Database;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace EG.DemoPCBE99925.ManageCourseService.Database.MigrationTool;

public class DatabaseContextForMigrationFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        return Program.Container().GetRequiredService<DatabaseContext>();
    }
}