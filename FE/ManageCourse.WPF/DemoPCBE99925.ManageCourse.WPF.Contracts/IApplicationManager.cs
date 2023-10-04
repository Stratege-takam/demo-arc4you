using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arc4u.Security.Principal;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Contracts;

public interface IApplicationManager
{
    AppPrincipal Principal { get; }
}