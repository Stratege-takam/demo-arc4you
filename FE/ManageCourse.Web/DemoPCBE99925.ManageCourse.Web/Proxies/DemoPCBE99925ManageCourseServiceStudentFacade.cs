using Arc4u;
using Arc4u.Configuration;
using Arc4u.Dependency.Attribute;
using EG.DemoPCBE99925.ManageCourseService.Facade.Sdk;
using Microsoft.Extensions.Options;

namespace EG.DemoPCBE99925.ManageCourse.Web.Proxies;

[Export, Scoped]
public class DemoPCBE99925ManageCourseServiceStudentFacade
{
    public DemoPCBE99925ManageCourseServiceStudentFacade(IOptionsMonitor<SimpleKeyValueSettings> optionsSettings, IHttpClientFactory httpClientFactory)
    {
        StudentClient.PreProcessException = (response) =>
        {
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest && response.ReasonPhrase.Equals("AppException"))
            {
                AppException.ProcessAppException(response);
            }

            // Add any other specific HttpStatusCode specific to your client.
        };

        var tokenSettings = optionsSettings.Get("OAuth2");
        if (tokenSettings is not null)
        {
            var client = httpClientFactory.CreateClient("OAuth2");

            Proxy = new StudentClient(client)
            {
                BaseUrl = tokenSettings.Values["RootServiceUrl"]
            };
        }
    }

    public StudentClient? Proxy { get; private set; }
}
