using Arc4u.Configuration;
using Arc4u.Dependency;
using Arc4u.Dependency.Attribute;
using Arc4u.OAuth2.Msal.Token;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Infrastructure;

[Export]
public class OAuth2HttpHandler : JwtHttpHandler
{
    public OAuth2HttpHandler(IContainerResolve container, ILogger<OAuth2HttpHandler> logger, IOptionsMonitor<SimpleKeyValueSettings> optionsSettings) : base(container, logger, optionsSettings.Get("OAuth2"))
    {

    }
}
