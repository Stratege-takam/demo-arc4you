using Arc4u.Dependency.Attribute;
using EG.DemoPCBE99925.ManageCourse.WPF.Views.Resources;
using Prism.Regions;
using System.Windows;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Views;

/// <summary>
/// Interaction logic for About.xaml
/// </summary>
[Export]
public partial class About : Window
{
    public About()
    {
        InitializeComponent();
    }

    public About(AboutVM viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;

        Loaded += (s, e) => ((INavigationAware)DataContext).OnNavigatedTo(null);
        Unloaded += (s, e) => ((INavigationAware)DataContext).OnNavigatedFrom(null);

    }

    private AboutRes _resource;
    public AboutRes Resource
    {
        get { return _resource ?? (_resource = new AboutRes()); }
        set
        {
            _resource = value;
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}