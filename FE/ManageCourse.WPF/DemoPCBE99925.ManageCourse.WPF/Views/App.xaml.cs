using System.Globalization;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Arc4u.Configuration;
using Arc4u.Dependency;
using Arc4u.Dependency.ComponentModel;
using Arc4u.Diagnostics;
using Arc4u.Security.Principal;
using EG.DemoPCBE99925.ManageCourse.WPF.Infrastructure;
using EG.DemoPCBE99925.ManageCourse.WPF.Views.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Prism.DI;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Serilog;

namespace EG.DemoPCBE99925.ManageCourse.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : PrismApplication
{
    static Func<Type, Type> applicationViewTypeToViewModelTypeResolver =
            viewType =>
            {
                var viewName = viewType.FullName;
                viewName = viewName.Replace(".Views.UserControls.", ".ViewModels.");
                viewName = viewName.Replace(".Views.Home.", ".ViewModels.");
                viewName = viewName.Replace(".Views.Courses.", ".ViewModels.");
                viewName = viewName.Replace(".Views.Auth.", ".ViewModels.");
                viewName = viewName.Replace(".Views.Students.", ".ViewModels.");
                viewName = viewName.Replace(".Views.Teachers.", ".ViewModels.");
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                var suffix = viewName.EndsWith("View") ? "Model" : "VM";
                var viewModelName = String.Format(CultureInfo.InvariantCulture, "{0}{1}, {2}", viewName, suffix, viewAssemblyName);
                return Type.GetType(viewModelName);
            };


   
    private IConfiguration _configuration;

    protected override Window CreateShell()
    {
        //consider Network
        if (!NetworkInterface.GetIsNetworkAvailable())
        {
            Common.Controls.MessageBox.Show(GlobalResources.NetworkUnavailableOnStart, "Application", MessageBoxButton.OK, MessageBoxImage.Information);
            System.Environment.Exit(-1);
            return null;
        }

        var shell = Container.Resolve<Views.Home.Main>();
        shell.WindowState = WindowState.Maximized;
        shell.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        return shell;
    }

    protected override void InitializeShell(Window shell)
    {
        IApplicationContext applicationContext = Container.Resolve<IApplicationContext>();

        if (null != applicationContext.Principal)
        {
            Thread.CurrentThread.CurrentCulture = applicationContext.Principal.Profile.CurrentCulture;
            Thread.CurrentThread.CurrentUICulture = applicationContext.Principal.Profile.CurrentCulture;
        }

        Application.Current.MainWindow = shell;
        Application.Current.MainWindow.Show();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        // Pay attention to the format. Assembly, full namespace file name.
        // As the logger is not yet initialized, an exception is thrown and the application will be stopped!
        _configuration = ConfigurationHelper.GetConfigurationFromResourceStream("DemoPCBE99925.ManageCourse.WPF, EG.DemoPCBE99925.ManageCourse.WPF.appsettings.json");

        Dispatcher.UnhandledException += Dispatcher_UnhandledException;

        // Must initialize the Logger => So we can send this to splunk!

        ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(applicationViewTypeToViewModelTypeResolver);

        base.OnStartup(e);
    }

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
    }

    protected override IContainerExtension CreateContainerExtension()
    {
        var logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(_configuration)
                        .MinimumLevel.Verbose()
                        .CreateLogger();

        var services = new ServiceCollection();

        services.AddILogger();
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(logger: logger, dispose: true));

        services.AddHttpClient("OAuth2")
            .AddHttpMessageHandler<OAuth2HttpHandler>()
            .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(200)));

        services.ConfigureSettings("OAuth2", _configuration, "WPF.OAuth2.ManageCourse.WPF.Settings");
        services.ConfigureSettings("AppSettings", _configuration, "AppSettings");
        services.AddApplicationConfig(_configuration);

        var container = new ComponentModelContainer(services).InitializeFromConfig(_configuration);
        container.RegisterInstance<IConfiguration>(_configuration);
        container.RegisterSingleton<IAddPropertiesToLog, DefaultLoggingProperties>();
        container.RegisterSingleton<IApplicationContext, ApplicationInstanceContext>();

      // var serviceProvider =  services.BuildServiceProvider();

        return new PrismContainerExtension(container);
    }

    protected override void RegisterTypes(Prism.Ioc.IContainerRegistry containerRegistry)
    {

    }


    void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
		string errorMessage = $"An unhandled exception occurred: {e.Exception.Message}";

        MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        e.Handled = true;
    }

}
