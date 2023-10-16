using Arc4u.Configuration;
using Arc4u.Dependency;
using Arc4u.Dependency.Attribute;
using Arc4u.Diagnostics;
using Arc4u.OAuth2.Msal.TokenProvider.Client;
using Arc4u.OAuth2.Token;
using Arc4u.Security.Principal;
using Arc4u.ServiceModel;
using EG.DemoPCBE99925.Rights;
using EG.DemoPCBE99925.ManageCourse.WPF.Common;
using EG.DemoPCBE99925.ManageCourse.WPF.Common.Events;
using EG.DemoPCBE99925.ManageCourse.WPF.Contracts;
using EG.DemoPCBE99925.ManageCourse.WPF.Infrastructure;
using EG.DemoPCBE99925.ManageCourse.WPF.Model;
using EG.DemoPCBE99925.ManageCourse.WPF.Views.Resources;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Windows;
using System.Windows.Threading;
using Constants = EG.DemoPCBE99925.ManageCourse.WPF.Common.Constants;
using Environment = System.Environment;

namespace EG.DemoPCBE99925.ManageCourse.WPF.ViewModels;

[Export]
public class LoginPageVM : VmBase<LoginRes, LoginPageVM>, INavigationAware
{
	public const String AuthenticateUser = "AuthenticateUser";
	public const String UserIsAuthenticated = "UserIsAuthenticated";

	public LoginPageVM(IRegionManager regionManager,
        IAppPrincipalFactory appFactory,
        PublicClientApp publicClientApp,
        IOptionsMonitor<SimpleKeyValueSettings> optionSettings,
        IEventAggregator aggregator, IModuleCatalog moduleCatalog,
        IContainerResolve container, ILogger<LoginPageVM> logger,
        IApplicationContext applicationContext) : base(new LoginRes(), aggregator, container, logger)
	{
		_regionManager = regionManager;
		_appFactory = appFactory;
		_moduleCatalog = moduleCatalog;
		_applicationContext = applicationContext;
		_publicClientApp = publicClientApp;
		_securitySettings = optionSettings.Get("OAuth2");

		Messages = new ObservableCollection<LoadedMessageStatus>
						{
							new LoadedMessageStatus {Key = AuthenticateUser, Message = LoginRes.AuthenticateUser, IsDone = false},
							new LoadedMessageStatus{Key = UserIsAuthenticated, Message = LoginRes.UserIsAuthenticated, IsDone=false},
		};
	}

	private readonly IRegionManager _regionManager;
	private readonly IAppPrincipalFactory _appFactory;
	private readonly IModuleCatalog _moduleCatalog;
	private readonly IApplicationContext _applicationContext;
	private readonly PublicClientApp _publicClientApp;
	private readonly SimpleKeyValueSettings _securitySettings;

	public bool IsNavigationTarget(NavigationContext navigationContext)
	{
		return false;
	}

	public void OnNavigatedFrom(NavigationContext navigationContext)
	{
	}

	public async void OnNavigatedTo(NavigationContext navigationContext)
	{
		if (null != _securitySettings)
		{
			var timer = new Stopwatch();
			timer.Start();

			SetMessage(AuthenticateUser);

			Logger.Technical().Debug($"Create a principal").Log();

			try
			{
				Messages messages = new Messages();

				_publicClientApp.SetCustomWebUi(() => new EmbeddedBrowserWebUi(App.Current.MainWindow));

				_publicClientApp.PublicClient = PublicClientApplicationBuilder.Create(_securitySettings.Values[TokenKeys.ClientIdKey])
													.WithAdfsAuthority(_securitySettings.Values[TokenKeys.AuthorityKey], false)
                                                    //.WithDefaultRedirectUri()
                                                    .WithRedirectUri(_securitySettings.Values[TokenKeys.RedirectUrl])
                                                    .Build();

				await _appFactory.CreatePrincipal(_securitySettings, messages, null).ConfigureAwait(true);
			}
			catch (HttpRequestException)
			{
				Dispatcher.CurrentDispatcher.Invoke(() => MessageBox.Show(LoginRes.NetworkIssue,
							GlobalResources.ProgramStartup,
							MessageBoxButton.OK,
							MessageBoxImage.Information)
				);

				Environment.Exit(-1);
			}
			catch (Exception ex)
			{
				Logger.Technical().Exception(ex).Log();

				Dispatcher.CurrentDispatcher.Invoke(() => MessageBox.Show(ex.ToString(),
							GlobalResources.ProgramStartup,
							MessageBoxButton.OK,
							MessageBoxImage.Information)
					);
				Environment.Exit(-1);
			}

			if (!_applicationContext.Principal.IsAuthorized((int)Access.AccessApplication))
			{
				await _appFactory.SignOutUserAsync(_securitySettings, CancellationToken.None).ConfigureAwait(true);
				_regionManager.RequestNavigate(Constants.MainRegion, "NotAuthorizedPage");
				return;
			}

			SetMessage(UserIsAuthenticated);

			LoadMenu();

			while (timer.ElapsedMilliseconds < 1000)
			{
				await Task.Delay(100);
			}

			// The module are loaded but we have to update the menus.
			foreach (var module in _moduleCatalog.Modules)
				_eventAggregator.GetEvent<LoadMenuEvent>().Publish(module.ModuleName);

			_eventAggregator.GetEvent<LoadMenuEvent>().Publish(String.Empty);

			// Needed only if no navigation to another page is done.
			_regionManager.Regions[Constants.MainRegion].RemoveAll();

			// In case you have a HomePage navigate to this one.
			//_regionManager.RequestNavigate(Constants.MainRegion, "The Page to Navigate");
		}

	}

	private void LoadMenu()
	{
		var menuMgr = Container.Resolve<IMenuMgr>();

		menuMgr.LoadMenu();
	}

	public ObservableCollection<LoadedMessageStatus> Messages { get; set; }

	private void SetMessage(String key)
	{
		if (!Dispatcher.CurrentDispatcher.CheckAccess())
		{
			Dispatcher.CurrentDispatcher.BeginInvoke(new Action<String>(SetMessage), key);
			return;
		}

		var message = Messages.Where(status => status.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
		if (null != message)
		{
			message.IsDone = true;
		}
	}
}
