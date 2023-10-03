using System;
using System.Collections.Generic;
using System.Net.Http;

namespace EG.DemoPCBE99925.Yarp.Facade.Sdk
{
    public partial class EnvironmentClient
    {
        public static Action<HttpResponseMessage> PreProcessException { get; set; }

        partial void ProcessResponse(HttpClient client, HttpResponseMessage response)
        {
            // if we have an implementation, we will return an exception!
            PreProcessException?.Invoke(response);
        }

#if NET5_0_OR_GREATER
        partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
        {
            request.Version = new Version(2, 0);
            request.VersionPolicy = HttpVersionPolicy.RequestVersionExact;
        }
#endif
    }
}

