using Arc4u.Dependency.Attribute;
using EG.DemoPCBE99925.ManageCourseService.Facade.Sdk;

namespace EG.DemoPCBE99925.ManageCourse.Web.States;

[Export, Shared]
public class TeacherState: BaseState
{
    private readonly ITeacherClient _client;
    public IList<TeacherDto> Teachers { get; set; }


    public TeacherState(ITeacherClient teacherClient)
    {
        _client = teacherClient;
    }

    public void RefreshList()
    {

    }
}
