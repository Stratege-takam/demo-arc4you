using Arc4u.Dependency.Attribute;
using EG.DemoPCBE99925.ManageCourse.Web.Utils.Enums;

namespace EG.DemoPCBE99925.ManageCourse.Web.States;

[Export, Shared]
public class SwitchUserState: BaseState
{

    private UserTypeEnum _currentRole = UserTypeEnum.Teacher;
    public UserTypeEnum CurrentRole
    {
        get => _currentRole;
        set => SetProperty(ref _currentRole, value);
    }
}
