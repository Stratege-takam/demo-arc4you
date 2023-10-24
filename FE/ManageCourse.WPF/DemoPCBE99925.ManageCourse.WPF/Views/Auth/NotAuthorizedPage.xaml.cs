using Arc4u.Dependency.Attribute;
using EG.DemoPCBE99925.ManageCourse.WPF.Model;
using System.Windows.Controls;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Views.Auth;

/// <summary>
/// Interaction logic for NotAuthorizedPage.xaml
/// </summary>
[Export(ChildRegionConst.NotAuthorizedPage, typeof(Object))]
public partial class NotAuthorizedPage : UserControl
{
    public NotAuthorizedPage()
    {
        InitializeComponent();
    }
}
