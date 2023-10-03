namespace EG.DemoPCBE99925.ManageCourseService.Host.Infrastructure;

public static class SecurityHeaderCSPExtension
{
    public static void UseSecurityHeaderCSP(this IApplicationBuilder app, IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        app.UseSecurityHeaders(policies =>
        {
            policies.AddDefaultSecurityHeaders();
            Action<CspBuilder> cspBuilder = builder =>
            {
                          builder.AddUpgradeInsecureRequests(); // upgrade-insecure-requests
                          builder.AddBlockAllMixedContent(); // block-all-mixed-content

                          builder.AddDefaultSrc() // default-src 
                              .None();

                          builder.AddConnectSrc()
                              .None();

                          builder.AddFontSrc() // font-src
                              .None();

                          builder.AddObjectSrc() // object-src 
                              .None();

                          builder.AddFormAction() // form-action
                              .None();

                          builder.AddImgSrc() // img-src
                              .None();

                          builder.AddScriptSrc()
                              .Self()
                              .WithHash256("6IIvUyrpNJsrV0PElO/SFu1ORPnryCprHLVIlaW4hDM=");


                          builder.AddStyleSrc() // style-src 
                              .Self()
                              .WithHash256("c5o8WnfLAUwx9Cfl/JgwrKsLPSZmgAj3Gujjp00yJuc=");

                          builder.AddMediaSrc() // media-src 
                              .None();

                          builder.AddFrameAncestors() // frame-ancestors 
                              .None();

                          builder.AddBaseUri() // base-ri 
                              .None();

                          builder.AddFrameSrc()
                             .None();
            };

        if (hostEnvironment.IsDevelopment())
            policies.AddContentSecurityPolicyReportOnly(cspBuilder); // to report on the browser console.
        else
            policies.AddContentSecurityPolicy(cspBuilder);
        });
    }
}
