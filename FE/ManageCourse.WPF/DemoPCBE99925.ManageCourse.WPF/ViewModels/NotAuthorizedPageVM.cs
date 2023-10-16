using Arc4u.Dependency;
using Arc4u.Dependency.Attribute;
using EG.DemoPCBE99925.ManageCourse.WPF.Common;
using EG.DemoPCBE99925.ManageCourse.WPF.Views.Resources;
using Microsoft.Extensions.Logging;

namespace EG.DemoPCBE99925.ManageCourse.WPF.ViewModels;

[Export, Shared]
class NotAuthorizedPageVM : VmBase<LoginRes, NotAuthorizedPageVM>
{
    public NotAuthorizedPageVM(IContainerResolve container, ILogger<NotAuthorizedPageVM> logger) : base(new LoginRes(), container, logger)
    {
    }
}
