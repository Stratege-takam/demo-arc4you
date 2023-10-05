using Arc4u.Dependency.Attribute;
using EG.DemoPCBE99925.ManageCourse.Web.Proxies;
using EG.DemoPCBE99925.ManageCourseService.Facade.Sdk;

namespace EG.DemoPCBE99925.ManageCourse.Web.States;

[Export, Shared]
public class TeacherState: BaseState
{
    public IList<TeacherDto> Teachers { get; set; }


    public void RefreshList()
    {

    }
}
