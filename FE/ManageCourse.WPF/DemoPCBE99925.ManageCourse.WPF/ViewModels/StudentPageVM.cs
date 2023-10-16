using Arc4u.Dependency;
using Arc4u.Dependency.Attribute;
using EG.DemoPCBE99925.ManageCourse.WPF.Views.Resources;
using EG.DemoPCBE99925.ManageCourse.WPF.Common;
using Microsoft.Extensions.Logging;
using EG.DemoPCBE99925.ManageCourse.WPF.Proxies;
using System.Collections.ObjectModel;
using EG.DemoPCBE99925.ManageCourseService.Facade.Sdk;

namespace EG.DemoPCBE99925.ManageCourse.WPF.ViewModels;

[Export, Shared]
public class StudentPageVM : VmBase<StudentRes, StudentPageVM>
{
    private ObservableCollection<StudentDto> _students;

    public ObservableCollection<StudentDto> Students
    {
        get { return _students; }
        set { _students = value; }
    }

    public readonly DemoPCBE99925ManageCourseServiceStudentFacade _facade;
    public StudentPageVM(IContainerResolve container,
        ILogger<StudentPageVM> logger,
        DemoPCBE99925ManageCourseServiceStudentFacade facade) : base(new StudentRes(), container, logger)
    {
        _facade = facade;

        InvokeMethod( async() =>
        {
            var students = await _facade.Proxy.GetAllAsync().ConfigureAwait(false);
            Students = new ObservableCollection<StudentDto>(students);
        });
    }

}
