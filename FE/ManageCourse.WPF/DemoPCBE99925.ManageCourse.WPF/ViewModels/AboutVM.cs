using Arc4u.Dependency;
using Arc4u.Dependency.Attribute;
using Arc4u.Security.Principal;
using EG.DemoPCBE99925.Yarp.Facade.Sdk;
using EG.DemoPCBE99925.ManageCourse.WPF.Common;
using EG.DemoPCBE99925.ManageCourse.WPF.Proxies;
using EG.DemoPCBE99925.ManageCourse.WPF.Views.Resources;
using Microsoft.Extensions.Logging;
using Prism.Regions;

namespace EG.DemoPCBE99925.ManageCourse.WPF;

[Export, Shared]
public class AboutVM : VmBase<AboutRes, AboutVM>, INavigationAware
{
    public AboutVM(IContainerResolve container, DemoPCBE99925YarpEnvironmentFacade facade, ILogger<AboutVM> logger, IApplicationContext applicationContext) : base(new AboutRes(), container, logger)
    {
        _facade = facade;
        _applicationContext = applicationContext;
    }

    private EnvironmentInfo _environmentInfo = new EnvironmentInfo();
    private readonly DemoPCBE99925YarpEnvironmentFacade _facade;
    private readonly IApplicationContext _applicationContext;
    public EnvironmentInfo EnvironmentInfo
    {
        get { return _environmentInfo; }
        set
        {
            SetProperty(ref _environmentInfo, value);
        }
    }

    public UserProfile UserProfile => _applicationContext.Principal.Profile;

    public Authorization Authorization => _applicationContext.Principal.Authorization;


    public async void OnNavigatedTo(NavigationContext navigationContext)
    {
        if (null != _facade.Proxy)
        {
            EnvironmentInfo = await _facade.Proxy.GetAsync().ConfigureAwait(true) ?? new EnvironmentInfo();
        }
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return false;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
    }
}