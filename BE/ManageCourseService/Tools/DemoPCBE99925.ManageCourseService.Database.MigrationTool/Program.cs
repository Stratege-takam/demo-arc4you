using McMaster.Extensions.CommandLineUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EG.DemoPCBE99925.ManageCourseService.Database.MigrationTool;

class Program
{
    static void Main(string[] args)
    {
        var databaseCtx = Container().GetService<DatabaseContext>();

        if (null== databaseCtx)
        {
            throw new NullReferenceException(nameof(databaseCtx));
        }

        var app = new CommandLineApplication
        {
            AllowArgumentSeparator = true,
            UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.StopParsingAndCollect,
        };
        
        app.Command("apply-all", config => {
            config.Description = "Do the migration to the latest version.";
            config.OnExecute(() =>
            {
               try
                {
                    var currentColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Migrate the database {databaseCtx.Database.GetDbConnection().Database}.");
                    var pendings = databaseCtx.Database.GetPendingMigrations();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Pendings migration that will be applied.");
                    if (pendings.Any())
                    {
                        foreach(var pending in pendings)
                            Console.WriteLine($"    - {pending}.");
                    }
                    databaseCtx.Database.Migrate();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Migration is finished.");
                    Console.ForegroundColor = currentColor;
                    return 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }
            });
        });

        app.Command("apply", config => {
            config.Description = "Do the migration to the specific migration step.";
            var migrationStep = config.Argument("migration", "name of the migration.", false);
            config.OnExecute(() =>
            {
               try
                {
                     var currentColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Migrate the database {databaseCtx.Database.GetDbConnection().Database} up to {migrationStep.Value}.");
                    var pendings = databaseCtx.Database.GetPendingMigrations();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    if (pendings.Where(p => p.Equals(migrationStep.Value, StringComparison.InvariantCultureIgnoreCase)).Any())
                    {
                        var migrator = databaseCtx.Database.GetService<IMigrator>();
                        migrator.Migrate(migrationStep.Value);
                    }
                    else
                    {
                        Console.WriteLine($"No pending migration found with name equal to {migrationStep.Value}.");
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Migration is finished.");
                    Console.ForegroundColor = currentColor;
                    return 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }

            });
        });

        app.Command("list-pendings", config => {
            config.Description = "Show the pending migrations.";
            config.OnExecute(() =>
            {
               try
                {
                     var currentColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"List the pending migration(s) for database {databaseCtx.Database.GetDbConnection().Database}.");
                    var pendings = databaseCtx.Database.GetPendingMigrations();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    if (pendings.Any())
                    {
                        foreach(var pending in pendings)
                            Console.WriteLine($"    - {pending}.");
                    }
                    else
                    {
                        Console.WriteLine("No migration is pending.");
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Finished.");
                    Console.ForegroundColor = currentColor;
                    return 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }
            });
        });

        app.Command("list-applied", config => {
            config.Description = "Show the migrations applied.";
            config.OnExecute(() =>
            {
               try
                {
                    var currentColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"List the migration(s) applied to database {databaseCtx.Database.GetDbConnection().Database}.");
                    var applieds = databaseCtx.Database.GetAppliedMigrations();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    if (applieds.Any())
                    {
                        foreach (var applied in applieds)
                            Console.WriteLine($"    - {applied}.");
                    }
                    else
                        Console.WriteLine("No migration has been applied.");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Finished.");
                    Console.ForegroundColor = currentColor;
                    return 0; 
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }

            });
        });

        app.Command("list-all", config => {
            config.Description = "Show the migrations defined applied and not.";
            config.OnExecute(() =>
            {
               try
                {
                    var currentColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"List the migration(s) defined to database {databaseCtx.Database.GetDbConnection().Database}.");
                    var applieds = databaseCtx.Database.GetMigrations();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    if (applieds.Any())
                    {
                        foreach (var applied in applieds)
                            Console.WriteLine($"    - {applied}.");
                    }
                    else
                        Console.WriteLine("No migration has been applied.");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Finished.");
                    Console.ForegroundColor = currentColor;
                    return 0; 
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return -1;
                }

            });
        });

        app.HelpOption("-? | -h | --help");
        app.Execute(args);
    }

    private static IServiceProvider? _container;

    public static IServiceProvider Container()
    {
        if (null != _container) return _container;

        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .Build();

        IServiceCollection services = new ServiceCollection();

        services.AddDbContext<DatabaseContext>(optionsBuilder =>
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DemoPCBE99925_ManageCourse_Db"),
                                        (options) =>
                                        {
                                            options.MigrationsHistoryTable("Database__MigrationsHistory", "app");
                                            options.MigrationsAssembly("DemoPCBE99925.ManageCourseService.Database.MigrationTool");
                                            options.CommandTimeout(30);
                                        })
                            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        _container = services.BuildServiceProvider();

        return _container;
    }
}