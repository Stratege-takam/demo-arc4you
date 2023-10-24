using Arc4u.Dependency.Attribute;
using EG.DemoPCBE99925.ManageCourse.WPF.Common.Events;
using EG.DemoPCBE99925.ManageCourse.WPF.Common.Menu;
using EG.DemoPCBE99925.ManageCourse.WPF.Contracts;
using Prism.Events;
using Prism.Mvvm;
using System.Globalization;

namespace EG.DemoPCBE99925.ManageCourse.WPF.ViewModels;

[Export]
public class MenuBarVM :  BindableBase
{
    public MenuBarVM(IMenuMgr menuMgr, IEventAggregator eventAggregator)
    {
        _menuMgr = menuMgr;

        //Subscribe to a change in the cultureInfo.
        eventAggregator.GetEvent<CultureInfoChangedEvent>().Subscribe(UpdateCulture, ThreadOption.UIThread, true);
        eventAggregator.GetEvent<LoadMenuEvent>().Subscribe(MenuLoaded, ThreadOption.UIThread, true, module => String.IsNullOrWhiteSpace(module));
			
		Menu = menuMgr.RootMenu;
        RightMenu = menuMgr.RightMenu;
    }

    IMenuMgr _menuMgr;

    private MenuItemsCollection _menu;
    public MenuItemsCollection Menu
    {
        get
        {
            return _menu;
        }
        set
        {
            SetProperty(ref _menu, value);
        }
    }

    private MenuItemsCollection _rightMenu;
    public MenuItemsCollection RightMenu
    {
        get
            { return _rightMenu; }
        set
        {
            SetProperty(ref _rightMenu, value);
        }
    }


    private void UpdateCulture(CultureInfo cultureInfo)
    {
        var rMenu = RightMenu;
        var lMenu = Menu;
        RightMenu = new MenuItemsCollection(null);
        Menu = new MenuItemsCollection(null);

        RightMenu = rMenu;
        Menu = lMenu;
    }

    private void MenuLoaded(String info)
    {
        if (!String.IsNullOrWhiteSpace(info)) return;

        RightMenu = _menuMgr.RightMenu;
        Menu = _menuMgr.RootMenu;
    }

}
