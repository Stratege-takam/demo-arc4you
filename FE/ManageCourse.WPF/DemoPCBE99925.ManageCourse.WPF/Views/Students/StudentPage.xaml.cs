
using System.Windows.Controls;
using Arc4u.Dependency.Attribute;
using EG.DemoPCBE99925.ManageCourse.WPF.Model;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Views.Students;
/// <summary>
/// Interaction logic for StudentPage.xaml
/// </summary>
[Export(ChildRegionConst.StudentPage, typeof(Object))]
public partial class StudentPage : UserControl
{
    public StudentPage()
    {
        InitializeComponent();
    }
}
