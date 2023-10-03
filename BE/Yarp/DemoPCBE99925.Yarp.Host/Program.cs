using System.Reflection;
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
using Arc4u.AspNetCore.Middleware;
using Arc4u.OAuth2.Extensions;
using Arc4u.OAuth2.Middleware;
using EG.DemoPCBE99925.Rights;
using EG.DemoPCBE99925.Yarp.Host.HealthChecks;
using EG.DemoPCBE99925.Yarp.Host.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Formatters;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Arc4u.OAuth2.AspNetCore.Filters;
using OpenTelemetry.Metrics;

Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()
	.CreateBootstrapLogger();

Log.Information("Starting up DemoPCBE99925.Yarp.");

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
			config.AddJsonFile("configs/reverseproxy.json", true, true);
			config.AddJsonFile($"configs/reverseproxy.{env}.json", true, true);
			config.AddCertificateDecryptorConfiguration();
		})
		.UseSerilog((ctx, lc) => lc
					.ReadFrom.Configuration(ctx.Configuration))
	  .UseWindowsService()
	  .UseServiceProviderFactory(new DependencyFactory());

	//Add services to the container.
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

	services.AddOidcAuthentication(Configuration);

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

	services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

	services.AddControllers(opt =>
	{
		// disable null to 204 => which will throw an exception.
		opt.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
		opt.Filters.Add<ManageExceptionsFilter>();
		opt.Filters.Add<SetCultureActionFilter>();
	});

	services.AddSystemMonitoring();

	services
		.AddHealthChecksUI()
		.AddInMemoryStorage()
			.Services
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

	services.AddGrpc(options =>
	{
		options.Interceptors.Add<AuthorizationInterceptor>();
		options.MaxSendMessageSize = Int32.MaxValue;
		options.MaxReceiveMessageSize = Int32.MaxValue;
	});

	services.AddGrpcReflection();

	services.AddReverseProxy().LoadFromConfig(Configuration.GetSection("ReverseProxy"));

	IContainerRegistry containerRegistry = new ComponentModelContainer(services).InitializeFromConfig(Configuration);
	containerRegistry.Register<IObjectSerialization, JsonSerialization>();
	services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName is not null && a.FullName.StartsWith("DemoPCBE99925.Yarp")));

	var app = builder.Build();

	app.UseSecurityHeaderCSP(Configuration, builder.Environment);

	app.UseRouting();

	app.UseSerilogRequestLogging();

	if (app.Environment.IsDevelopment())
	{
		app.UseDeveloperExceptionPage();
	}

	app.AddMonitoringTimeElapsed();

	var container = app.Services.GetRequiredService<IContainerResolve>();

	container.Resolve<AutoMapper.IConfigurationProvider>().AssertConfigurationIsValid();

	app.UseCors();

	app.UseBasicAuthentication();

	app.AddGrpcAuthenticationControl();
	app.AddGrpcMonitoringTimeElapsed();

	app.UseAuthentication();

	app.UseForceOfOpenId();
	app.UseAuthorization();
	app.UseResourcesRightValidationFor();

	app.UseAddContextToPrincipal();
	app.UseOpenIdBearerInjector();

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
		configure.SwaggerRoutes.Add(new NSwag.AspNetCore.SwaggerUi3Route("Gateway", "/swagger/facade/swagger.json"));
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
	if (!IsInvokedByNSwag())
	{
		app.MapGrpcReflectionService();
		app.MapHealthChecksUI();
		app.MapReverseProxy();
	}

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

static bool IsInvokedByNSwag()
{
	var env = System.Environment.GetEnvironmentVariable("NSwag");
	return env is not null && bool.TryParse(env, out var nswag) && nswag;
}
