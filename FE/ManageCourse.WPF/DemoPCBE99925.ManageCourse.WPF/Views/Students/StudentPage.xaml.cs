
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

    private void gridViewStudent_CellValidated(object sender, Telerik.Windows.Controls.GridViewCellValidatedEventArgs e)
    {

    }

    private void gridViewStudent_CellValidated_1(object sender, Telerik.Windows.Controls.GridViewCellValidatedEventArgs e)
    {

    }

    private void gridViewStudent_CellValidating(object sender, Telerik.Windows.Controls.GridViewCellValidatingEventArgs e)
    {

    }
}
