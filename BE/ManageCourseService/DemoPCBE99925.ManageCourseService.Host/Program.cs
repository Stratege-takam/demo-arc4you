using Arc4u.Authorization;
using Arc4u.Caching;
using Arc4u.Configuration;
using Arc4u.Configuration.Decryptor;
using Arc4u.Dependency;
using Arc4u.Dependency.ComponentModel;
using Arc4u.Diagnostics;
using Arc4u.gRPC.Interceptors;
using Arc4u.Security.Principal;
using Arc4u.Serializer;
using EG.DemoPCBE99925.Rights;
using EG.DemoPCBE99925.ManageCourseService.Host.HealthChecks;
using EG.DemoPCBE99925.ManageCourseService.Host.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Formatters;
using NSwag;
using NSwag.Generation.Processors.Security;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using System.Reflection;
using Arc4u.OAuth2.Extensions;
using Arc4u.OAuth2.AspNetCore.Filters;
using Arc4u.AspNetCore.Middleware;
using Arc4u.OAuth2.Middleware;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up DemoPCBE99925.ManageCourseService.");

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    // Remove sensitive information that can be exploit by hackers.
    builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

    builder.Host
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            foreach (var source in config.Sources.Where(s => s.GetType().Name.Contains("Json")).ToList())
            {
                config.Sources.Remove(source);
            }

            var env = hostingContext.HostingEnvironment.EnvironmentName;

            config.AddJsonFile("configs/appsettings.json", true, true);
            config.AddJsonFile($"configs/appsettings.{env}.json", true, true);
            config.AddCertificateDecryptorConfiguration();
        })
        .UseSerilog((ctx, lc) => lc
                    .ReadFrom.Configuration(ctx.Configuration))
      .UseWindowsService()
      .UseServiceProviderFactory(new DependencyFactory());

    var services = builder.Services;

    var Configuration = builder.Configuration;

    services.ConfigureSettings("AppSettings", Configuration, "AppSettings");

    services.AddApplicationConfig(Configuration);
    services.AddApplicationContext();
    services.AddScopedOperationsPolicy(Operations.Scopes, Operations.Values, options =>
    {
        // Add you custom policy here.
        //options.AddPolicy("Custom", policy =>
        //policy.Requirements.Add(new ScopedOperationsRequirement((int)Access.AccessApplication, (int)Access.CanSeeSwaggerFacadeApi)));
    });
    services.AddCacheContext(Configuration);

    services.AddJwtAuthentication(Configuration);

    if (Configuration.GetSection("OpenTelemetry").Exists())
    {
        var openTelemetrySettings = new OpenTelemetrySettings();
        Configuration.GetSection("OpenTelemetry").Bind(openTelemetrySettings);

        services.AddOpenTelemetry().WithTracing(
        builder =>
        {
            builder
            .SetResourceBuilder(ResourceBuilder.CreateEmpty()
              .AddAttributes(new List<KeyValuePair<string, object>> {
                            new KeyValuePair<string, object>("host.name", System.Environment.MachineName),
                            new KeyValuePair<string, object>("service.name", Configuration["Application.Configuration:Environment:loggingName"]),
                            new KeyValuePair<string, object>("service.version", Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString() ?? "0.0"),
                            new KeyValuePair<string, object>("deployment.environment", Configuration["Application.Configuration:Environment:Name"]),
                }).AddAttributes(openTelemetrySettings.Attributes.ToList()))
              .AddAspNetCoreInstrumentation()
               .AddSource(openTelemetrySettings.Sources.ToArray())
              .AddOtlpExporter(configure =>
              {
                  configure.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                  configure.Endpoint = new Uri(openTelemetrySettings.Address);
              })
;
        });
    }

    services.AddControllers(opt =>
    {
        // disable null to 204 => which will throw an exception.
        opt.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
        opt.Filters.Add<ManageExceptionsFilter>();
        opt.Filters.Add<SetCultureActionFilter>();
    }).AddDapr();

    services.AddGrpcReflection();

    services.AddSystemMonitoring();

    var useManSidekick = Configuration.GetSection("DaprSidekick").Exists();
    if (useManSidekick)
    {
        services.AddDaprSidekick(Configuration);
    }

    services
        .AddHealthChecks()
            .AddCheck<LogHealthCheck>("LogInfo")
            .AddSeqPublisher((options) => { options.Endpoint = Configuration["AppSettings:seq_ingest"]; });

    services.AddOpenApiDocument(option =>
      {
             option.DocumentName = "facade";
             option.ApiGroupNames = new[] { "facade" };
             option.PostProcess = postProcess =>
             {
                   postProcess.Info.Title = "Facade contracts are only used by Guis and no perinity is provided.";
             };
      });

    services.AddOpenApiDocument(option =>
      {
             option.DocumentName = "interface";
             option.ApiGroupNames = new[] { "interface", "deprecated" };
             option.PostProcess = postProcess =>
             {
                   postProcess.Info.Title = "Interface description.";
             };
      });

    services.AddOpenApiDocument(option =>
    {
             option.DocumentName = "interfaceSdk";
             option.ApiGroupNames = new[] { "interface" };
             option.PostProcess = postProcess =>
             {
                   postProcess.Info.Title = "Interface description.";
             };
            option.AddSecurity(JwtBearerDefaults.AuthenticationScheme, Enumerable.Empty<string>(),
                   new OpenApiSecurityScheme
                   {
                          Name = "Authorization",
                          Description = "Input your Bearer token to access this API",
                          In = OpenApiSecurityApiKeyLocation.Header,
                          Type = OpenApiSecuritySchemeType.Http,
                          Scheme = JwtBearerDefaults.AuthenticationScheme,
                          BearerFormat = "JWT",
                   });
             option.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor());
      });

    services.AddGrpc(options =>
    {
        options.Interceptors.Add<AuthorizationInterceptor>();
        options.MaxSendMessageSize = Int32.MaxValue;
        options.MaxReceiveMessageSize = Int32.MaxValue;
    });

    services.AddGrpcReflection();

    IContainerRegistry containerRegistry = new ComponentModelContainer(services).InitializeFromConfig(Configuration);
    containerRegistry.Register<IObjectSerialization, JsonSerialization>();
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName is not null && a.FullName.StartsWith("DemoPCBE99925.ManageCourseService")));

    var app = builder.Build();

    app.UseSecurityHeaderCSP(Configuration, builder.Environment);

    app.UseRouting();

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.AddMonitoringTimeElapsed();

    var container = app.Services.GetRequiredService<IContainerResolve>();

  	container.Resolve<AutoMapper.IConfigurationProvider>().AssertConfigurationIsValid();

    app.UseCors();

    app.AddGrpcAuthenticationControl();
    app.AddGrpcMonitoringTimeElapsed();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseResourcesRightValidationFor();
    app.UseAddContextToPrincipal();

    // Always replace the virtual path of this backend with the one from the yarp. Required for swagger and hangfire when hosted in IISHost!
    app.Use((context, next) =>
    {
        var pathBase = context.Request.Headers["X-Forwarded-Prefix"];
        if (!string.IsNullOrWhiteSpace(pathBase))
            context.Request.PathBase = new PathString(pathBase);
        return next();
    });

    app.UseOpenApi(configure =>
    {
        configure.DocumentName = "facade";
    })
    .UseOpenApi(configure =>
    {
        configure.DocumentName = "interface";
    })
    .UseSwaggerUi3(configure =>
    {
        configure.Path = "/swagger/facade";
        configure.DocumentPath = configure.Path + "/swagger.json";
        configure.TagsSorter = "alpha";
        configure.OperationsSorter = "alpha";
    })
    .UseSwaggerUi3(configure =>
    {
        configure.Path = "/swagger/interface";
        configure.DocumentPath = configure.Path + "/swagger.json";
        configure.TagsSorter = "alpha";
        configure.OperationsSorter = "alpha";
    });

    app.MapHealthChecks("/health/ready", new HealthCheckOptions()
    {
        Predicate = (check) => check.Tags.Contains("ready"),
    });
    app.MapHealthChecks("/health/live", new HealthCheckOptions()
    {
        Predicate = (check) => check.Tags.Contains("live")
    });
    app.MapHealthChecks("healthz", new HealthCheckOptions()
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    app.UseWelcomePage("/");
    app.MapControllers();
    app.MapGrpcReflectionService();
    app.MapSubscribeHandler();
    if (useManSidekick) app.MapDaprMetrics();
    
    var logger = container.Resolve<ILogger<Program>>();

    container.InitializeTimeZoneContext();

    logger.Technical().Information("Start the Api process.").Log();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shutdown complete");
    Log.CloseAndFlush();
}
