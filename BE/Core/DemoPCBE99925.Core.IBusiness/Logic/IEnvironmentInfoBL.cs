using EG.DemoPCBE99925.Core.Domain;

namespace EG.DemoPCBE99925.Core.IBusiness;

public interface IEnvironmentInfoBL
{
    Task<EnvironmentInfo> GetEnvironmentInfoAsync();
}