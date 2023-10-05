using Arc4u.Dependency.Attribute;
using EG.DemoPCBE99925.ManageCourse.Web.Proxies;
using EG.DemoPCBE99925.ManageCourse.Web.Utils.Enums;
using EG.DemoPCBE99925.ManageCourseService.Facade.Sdk;

namespace EG.DemoPCBE99925.ManageCourse.Web.States;

[Export, Shared]
public class TeacherState: BaseState
{
    private readonly DemoPCBE99925ManageCourseServiceTeacherFacade _facade;

    public IList<TeacherDto> Teachers { get; set; }

    private bool _loadingRefresh = false;
    public bool LoadingRefresh
    {
        get => _loadingRefresh;
        set => SetProperty(ref _loadingRefresh, value);
    }

    public TeacherState(DemoPCBE99925ManageCourseServiceTeacherFacade facade)
    {
        _facade = facade;
    }

    public async Task RefreshList()
    {
        _loadingRefresh = true;
        Teachers = await _facade.Proxy.GetAllAsync().ConfigureAwait(false);
        _loadingRefresh = false;
    }
}
