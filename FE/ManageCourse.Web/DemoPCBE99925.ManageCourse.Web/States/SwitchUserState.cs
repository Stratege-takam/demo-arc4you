using Arc4u.Dependency.Attribute;
using EG.DemoPCBE99925.ManageCourse.Web.Utils.Enums;
using Microsoft.AspNetCore.Components;

namespace EG.DemoPCBE99925.ManageCourse.Web.States;

[Export, Shared]
public class SwitchUserState: BaseState
{
    private NavigationManager _navigationManager;

    private UserTypeEnum _currentRole = UserTypeEnum.None;
    public UserTypeEnum CurrentRole
    {
        get => _currentRole;
        set => SetProperty(ref _currentRole, value);
    }


    public SwitchUserState(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    public void Switch(UserTypeEnum role,  string route)
    {
        CurrentRole = role;

        _navigationManager.NavigateTo(route);
    }
}
