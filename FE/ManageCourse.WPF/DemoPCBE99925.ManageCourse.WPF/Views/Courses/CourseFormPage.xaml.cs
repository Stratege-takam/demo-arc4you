using System.Windows.Controls;
using Arc4u.Dependency.Attribute;
using EG.DemoPCBE99925.ManageCourse.WPF.Model;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Views.Courses;
/// <summary>
/// Interaction logic for CourseFormPage.xaml
/// </summary>

[Export(ChildRegionConst.CourseFormPage, typeof(Object))]
public partial class CourseFormPage : UserControl
{
    public CourseFormPage()
    {
        InitializeComponent();
    }
}
