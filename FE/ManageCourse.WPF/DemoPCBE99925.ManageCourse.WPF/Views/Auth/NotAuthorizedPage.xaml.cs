using Arc4u.Dependency.Attribute;
using System.Windows.Controls;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Views.Auth;

/// <summary>
/// Interaction logic for NotAuthorizedPage.xaml
/// </summary>
[Export("NotAuthorizedPage", typeof(Object))]
public partial class NotAuthorizedPage : UserControl
{
    public NotAuthorizedPage()
    {
        InitializeComponent();
    }
}
