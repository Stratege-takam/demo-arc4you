using Arc4u.Dependency.Attribute;
using System.Windows.Controls;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Views;

/// <summary>
/// Interaction logic for LoginPage.xaml
/// </summary>
[Export("LoginPage", typeof(Object))]
public partial class LoginPage : UserControl
{
    public LoginPage()
    {
        InitializeComponent();
    }
}