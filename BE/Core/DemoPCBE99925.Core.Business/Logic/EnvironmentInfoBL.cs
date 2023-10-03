using Arc4u;
using Arc4u.Dependency.Attribute;
using Arc4u.Configuration;
using EG.DemoPCBE99925.Core.Domain;
using EG.DemoPCBE99925.Core.IBusiness;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EG.DemoPCBE99925.Core.Business;

[Export(typeof(IEnvironmentInfoBL)), Scoped]
public class EnvironmentInfoBL : IEnvironmentInfoBL
{
    public EnvironmentInfoBL(IOptions<ApplicationConfig> config, IAppSettings settings, IConfiguration configuration, ILogger<EnvironmentInfoBL> logger)
    {
        _config = config.Value;
        _appSettings = settings;
        _configuration = configuration;
        _logger = logger;
    }

    private readonly ApplicationConfig _config;
    private readonly IAppSettings _appSettings;
    private readonly IConfiguration _configuration;
    private readonly ILogger<EnvironmentInfoBL> _logger;

    public Task<EnvironmentInfo> GetEnvironmentInfoAsync()
    {
        if (null == _config)
        {
            throw new AppException("Invalid application.configuration config.");
        }

        var info = new EnvironmentInfo
        {
            Name = _config.Environment.Name,
            Server = System.Net.Dns.GetHostName(),
            DatabaseInfo = GetDatabaseInfo()
        };

        return Task.FromResult(info);
    }

    private static List<DatabaseInfo> GetDatabaseInfo()
    {
        var result = new List<DatabaseInfo>();

        // build your own code to display database information!
        //_configuration.GetConnectionString("{key used for the connection string}");

        return result;
    }
}
