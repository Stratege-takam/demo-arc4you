using EG.DemoPCBE99925.Yarp.Domain;

namespace EG.DemoPCBE99925.Yarp.IBusiness;

public interface IEnvironmentInfoBL
{
    Task<EnvironmentInfo> GetEnvironmentInfoAsync();
}