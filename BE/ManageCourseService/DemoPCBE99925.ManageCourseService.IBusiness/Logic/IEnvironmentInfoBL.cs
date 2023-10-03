using EG.DemoPCBE99925.ManageCourseService.Domain;

namespace EG.DemoPCBE99925.ManageCourseService.IBusiness;

public interface IEnvironmentInfoBL
{
    Task<EnvironmentInfo> GetEnvironmentInfoAsync();
}