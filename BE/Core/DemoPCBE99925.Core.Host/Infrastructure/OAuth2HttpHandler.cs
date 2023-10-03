using Arc4u.Configuration;
using Arc4u.Dependency.Attribute;
using Arc4u.OAuth2;
using Arc4u.OAuth2.Token;
using Microsoft.Extensions.Options;

namespace EG.DemoPCBE99925.Core.Host.Infrastructure;

[Export]
public class OAuth2HttpHandler : JwtHttpHandler
{
    public OAuth2HttpHandler(IScopedServiceProviderAccessor accessor, ILogger<OAuth2HttpHandler> logger, IOptionsMonitor<SimpleKeyValueSettings> settings) : base(accessor, logger, settings.Get("OAuth2"))
    {

    }
}