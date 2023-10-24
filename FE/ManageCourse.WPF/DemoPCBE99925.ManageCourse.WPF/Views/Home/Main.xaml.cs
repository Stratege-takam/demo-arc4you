using Arc4u.Dependency.Attribute;
using EG.DemoPCBE99925.ManageCourse.WPF.Common;
using EG.DemoPCBE99925.ManageCourse.WPF.Model;
using Prism.Regions;
using System.Windows;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Views.Home;

/// <summary>
/// Interaction logic for Main.xaml
/// </summary>
[Export]
public partial class Main : Window
{
    public Main(IRegionManager regionManager)
    {
        InitializeComponent();

        Loaded += (s, e) => regionManager.RequestNavigate(Constants.MainRegion, ChildRegionConst.LoginPage);
    }
}
