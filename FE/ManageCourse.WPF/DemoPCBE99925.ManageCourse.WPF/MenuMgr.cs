using Arc4u.Dependency;
using Arc4u.Dependency.Attribute;
using Arc4u.Security.Principal;
using EG.DemoPCBE99925.ManageCourse.WPF.Common.Events;
using EG.DemoPCBE99925.ManageCourse.WPF.Common.Menu;
using EG.DemoPCBE99925.ManageCourse.WPF.Contracts;
using EG.DemoPCBE99925.ManageCourse.WPF.Views;
using Prism.Events;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Linq;

namespace EG.DemoPCBE99925.ManageCourse.WPF;

[Export(typeof(IMenuMgr)), Shared]
public class MenuMgr : IMenuMgr
{
    public MenuMgr(IContainerResolve container, IApplicationContext applicationContext)
    {
        _mainMenu = new MenuItemsCollection(null);
        _rightMenu = new MenuItemsCollection(null);
        _container = container;
        _applicationContext = applicationContext;
    }

    private MenuItemsCollection _mainMenu;
    private MenuItemsCollection _rightMenu;
    private readonly IContainerResolve _container;
    private readonly IApplicationContext _applicationContext;

    #region IMenuMgr Members

    public MenuItemsCollection RootMenu
    {
        get { return _mainMenu; }
        set { _mainMenu = value; }
    }

    public MenuItemsCollection RightMenu
    {
        get { return _rightMenu; }
        set { _rightMenu = value; }
    }

    public void LoadMenu()
    {
        MenuItem.DefaultActionCommand = o => MessageBox.Show("Not yet implemented.");

        _mainMenu = Load(new Uri("pack://application:,,,/DemoPCBE99925.ManageCourse.WPF;component/menu.xml", UriKind.Absolute));
        var exitItem = _mainMenu.FindItem("File.Exit");
        if (null != exitItem) exitItem.ActionCommand = o => Application.Current.Shutdown();


        _rightMenu = Load(new Uri("pack://application:,,,/DemoPCBE99925.ManageCourse.WPF;component/rightmenu.xml", UriKind.Absolute));
        var aboutMenu = _rightMenu.FindItem("About");
        if (null != aboutMenu) aboutMenu.ActionCommand = a => _container.Resolve<About>().ShowDialog();

        var frMenu = RightMenu.FindItem("Language.Fr");
        if (null != frMenu) frMenu.ActionCommand = a => SetLanguageTo(new CultureInfo("fr-BE"));

        var nlMenu = RightMenu.FindItem("Language.Nl");
        if (null != nlMenu) nlMenu.ActionCommand = a => SetLanguageTo(new CultureInfo("nl-BE"));

        var enMenu = RightMenu.FindItem("Language.En");
        if (null != enMenu) enMenu.ActionCommand = a => SetLanguageTo(new CultureInfo("en-US"));

        // Check the correct language following the current culture.
        switch (Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToUpper())
        {
            case "FR":
                if (frMenu != null) frMenu.IsChecked = true;
                break;
            case "NL":
                if (nlMenu != null) nlMenu.IsChecked = true;
                break;
            default:
                if (enMenu != null) enMenu.IsChecked = true;
                break;
        }
    }

    public MenuItemsCollection Load(Uri menu)
    {
        StreamResourceInfo info = Application.GetResourceStream(menu);

        var reader = new StreamReader(info.Stream);

        var root = XDocument.Load(reader);
        var result = new MenuItemsCollection();
        GetItems(root).ForEach(result.Add);

        return result;
    }

    private List<MenuItem> GetItems(XContainer item)
    {
        var items = item.Element("MenuItems");
        if (null == items)
            return new List<MenuItem>();

        return (from xElt in items.Elements() select GetMenu(xElt)).ToList();
    }

    private MenuItem GetMenu(XContainer item)
    {
        var menuItem = new MenuItem();

        var x = item.Element("Access"); menuItem.Access = GetString(x);
        x = item.Element("Text"); menuItem.Text = GetString(x);
        x = item.Element("Key"); menuItem.Key = GetString(x);
        x = item.Element("GroupName"); menuItem.GroupName = GetString(x);
        x = item.Element("ImageUrl"); menuItem.ImageUrl = GetString(x);
        x = item.Element("InputGestureText"); menuItem.InputGestureText = GetString(x);
        x = item.Element("ToolTip"); menuItem.ToolTip = GetString(x);
        x = item.Element("IsCheckable"); menuItem.IsCheckable = GetBoolean(x);
        x = item.Element("IsChecked"); menuItem.IsChecked = GetBoolean(x);
        x = item.Element("IsEnabled"); menuItem.IsEnabled = GetBoolean(x);
        x = item.Element("IsSeparator"); menuItem.IsSeparator = GetBoolean(x);
        x = item.Element("StaysOpenOnClick"); menuItem.StaysOpenOnClick = GetBoolean(x);
        x = item.Element("ResourceType"); menuItem.ResourceType = GetString(x);

        var isAuthorized = menuItem.Access == null
                                ? true
                                : _applicationContext.Principal.IsAuthorized(menuItem.Access.Split(new[] { ',' }));

        if (isAuthorized)
            GetItems(item).ForEach(subMenu => menuItem.Items.Add(subMenu));

        return menuItem;
    }

    private void SetLanguageTo(CultureInfo cultureInfo)
    {
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
        Thread.CurrentThread.CurrentCulture = cultureInfo;

        //Must set the Principal also.!

        var eventAggregator = DependencyContext.Current.Resolve<IEventAggregator>();
        eventAggregator.GetEvent<CultureInfoChangedEvent>().Publish(cultureInfo);
    }

    private static String GetString(XElement x)
    {
        if (null == x)
            return null;

        var s = x.Value.Trim();

        return String.IsNullOrEmpty(s) ? null : s;
    }

    private static bool GetBoolean(XElement x)
    {
        var b = false;
        if (null != x)
            bool.TryParse(x.Value, out b);
        return b;
    }

    #endregion
}