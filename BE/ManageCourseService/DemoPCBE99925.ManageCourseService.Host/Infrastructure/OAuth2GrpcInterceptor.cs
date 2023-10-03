using Arc4u.Configuration;
using Arc4u.Dependency.Attribute;
using Arc4u.gRPC.Interceptors;
using Arc4u.OAuth2;
using Microsoft.Extensions.Options;

namespace EG.DemoPCBE99925.ManageCourseService.Host.Infrastructure;

[Export]
public class OAuth2GrpcInterceptor : OAuth2Interceptor
{
    public OAuth2GrpcInterceptor(IScopedServiceProviderAccessor accessor, ILogger<OAuth2GrpcInterceptor> logger, IOptionsMonitor<SimpleKeyValueSettings> settings) : base(accessor, logger, settings.Get("OAuth2"))
    {
    }
}
