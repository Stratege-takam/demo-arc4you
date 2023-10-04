using Arc4u.Authorization;
using Arc4u.Blazor;
using Arc4u.Caching;
using Arc4u.Configuration;
using Arc4u.Dependency;
using Arc4u.Dependency.ComponentModel;
using Arc4u.Diagnostics;
using Arc4u.Security.Principal;
using Blazored.LocalStorage;
using EG.DemoPCBE99925.Rights;
using EG.DemoPCBE99925.ManageCourse.Web;
using EG.DemoPCBE99925.ManageCourse.Web.Infrastructure;
using Nova.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Polly;
using Serilog;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var logger = new LoggerConfiguration()
				.ReadFrom.Configuration(builder.Configuration)
				.MinimumLevel.Verbose()
				.CreateLogger();

var services = builder.Services;

services.AddHttpClient("OAuth2")
			.AddHttpMessageHandler<OAuth2HttpHandler>()
			.AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(200)));

services.AddILogger();
services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(logger: logger, dispose: true));

services.AddBlazoredLocalStorage();

services.AddNovaDesignSystem();

services.AddLocalization();

services.ConfigureSettings("OAuth2", builder.Configuration, "Blazor.OAuth2.ManageCourse.Web.Settings");
services.ConfigureSettings("AppSettings", builder.Configuration, "AppSettings");

services.AddMsalAuthentication<RemoteAuthenticationState, RemoteUserAccount>(options =>
{
	builder.Configuration.Bind("Blazor.OAuth2.ManageCourse.Web.Settings", options.ProviderOptions.Authentication);
	foreach (var scope in builder.Configuration["Blazor.OAuth2.ManageCourse.Web.Settings:Scopes"].Split(new[] { ',' }))
		options.ProviderOptions.AdditionalScopesToConsent.Add(scope);
	// uncomment this for redirect-based login instead of popup based login.
	//options.ProviderOptions.LoginMode = "redirect";
}).AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, RemoteUserAccount, ClaimsPrincipalFactory>();

services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

services.AddScopedOperationsPolicy(Operations.Scopes, Operations.Values, options =>
{
	// Add you custom policy here.
	//options.AddPolicy("Custom", policy =>
	//policy.Requirements.Add(new ScopedOperationsRequirement((int)Access.AccessApplication, (int)Access.CanSeeSwaggerFacadeApi)));
});

services.AddApplicationConfig(builder.Configuration);

var container = new ComponentModelContainer(services).InitializeFromConfig(builder.Configuration);

// Add extra Ioc registration types.
container.RegisterInstance<IConfiguration>(builder.Configuration);
container.RegisterSingleton<IAddPropertiesToLog, DefaultLoggingProperties>();
container.RegisterSingleton<IApplicationContext, ApplicationInstanceContext>();

var host = builder.Build();
await host.SetDefaultCulture().ConfigureAwait(false);

// initialize secure cache used to store sensitive data (in memory).
var cache = host.Services.GetService<ISecureCache>();
cache?.Initialize(string.Empty);

var logging = host.Services.GetService<ILogger<Program>>();

logging.Technical().LogInformation("App is started.");

await host.RunAsync().ConfigureAwait(false);
