using System.Windows.Controls;
using Arc4u.Dependency.Attribute;
using EG.DemoPCBE99925.ManageCourse.WPF.Model;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Views.Teachers;
/// <summary>
/// Interaction logic for TeacherPage.xaml
/// </summary>
[Export(ChildRegionConst.TeacherPage, typeof(Object))]
public partial class TeacherPage : UserControl
{
    public TeacherPage()
    {
        InitializeComponent();
    }
}
