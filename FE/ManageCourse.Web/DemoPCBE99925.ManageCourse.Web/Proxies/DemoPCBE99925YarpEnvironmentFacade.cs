using Arc4u;
using Arc4u.Configuration;
using Arc4u.Dependency.Attribute;
using EG.DemoPCBE99925.Yarp.Facade.Sdk;
using System.Net.Http;

namespace EG.DemoPCBE99925.ManageCourse.Web.Proxies;

[Export, Scoped]
public class DemoPCBE99925YarpEnvironmentFacade
{
    public DemoPCBE99925YarpEnvironmentFacade(IAppSettings appSettings, IHttpClientFactory httpClientFactory)
    {
        EnvironmentClient.PreProcessException = (response) =>
        {
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest && response.ReasonPhrase.Equals("AppException"))
            {
                AppException.ProcessAppException(response);
            }

            // Add any other specific HttpStatusCode specific to your client.
        };

        var client = httpClientFactory.CreateClient("OAuth2");

        Proxy = new EnvironmentClient(client)
        {
            BaseUrl = appSettings.Values["GatewayServiceUrl"]
        };
    }

    public EnvironmentClient Proxy { get; private set; }
}
