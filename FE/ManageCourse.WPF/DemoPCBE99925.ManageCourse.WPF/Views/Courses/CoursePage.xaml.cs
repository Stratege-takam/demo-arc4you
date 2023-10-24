using System.Windows.Controls;
using Arc4u.Dependency.Attribute;
using EG.DemoPCBE99925.ManageCourse.WPF.Model;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Views.Courses;
/// <summary>
/// Interaction logic for CoursePage.xaml
/// </summary>
[Export(ChildRegionConst.CoursePage, typeof(Object))]
public partial class CoursePage : UserControl
{
    public CoursePage()
    {
        InitializeComponent();
    }
}
