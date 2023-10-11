using Arc4u.Configuration;
using Arc4u.Dependency;
using Arc4u.Dependency.Attribute;
using Arc4u.gRPC.Interceptors;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EG.DemoPCBE99925.ManageCourse.WPF.Infrastructure;

[Export]
public class OAuth2GrpcInterceptor : OAuth2Interceptor
{
    public OAuth2GrpcInterceptor(IContainerResolve container, ILogger<OAuth2GrpcInterceptor> logger, IOptionsMonitor<SimpleKeyValueSettings> settings) : base(container, logger, settings.Get("OAuth2"))
    {
    }
}
