namespace EG.DemoPCBE99925.Yarp.Host.Infrastructure;

public static class SecurityHeaderCSPExtension
{
    public static void UseSecurityHeaderCSP(this IApplicationBuilder app, IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        app.UseSecurityHeaders(policies =>
        {
            if (!hostEnvironment.IsDevelopment())
            {
                policies.AddDefaultSecurityHeaders();
            }

            Action<CspBuilder> cspBuilder = builder =>
            {
                builder.AddUpgradeInsecureRequests(); // upgrade-insecure-requests
                builder.AddBlockAllMixedContent(); // block-all-mixed-content

                builder.AddDefaultSrc() // default-src 'self'
                    .Self()
                    .From(configuration.GetValue<String>("Csp:DefaultSrc").Split(','));

                builder.AddConnectSrc() // connect-src 'self' 
                    .Self()
                    .From(configuration.GetValue<String>("Csp:ConnectSrc").Split(','));

                builder.AddFontSrc() // font-src 'self'
                    .Self()
                    .Data();

                builder.AddObjectSrc() // object-src 'none'
                    .None();

                builder.AddFormAction() // form-action 'self'
                    .Self()
                    .From(configuration.GetValue<String>("Authentication:DefaultAuthority:Url") + "/oauth2/v2.0/authorize")
                    ;

                builder.AddImgSrc() // img-src https:
                    .Self()
                    .Data()
                    .From("https://avatars3.githubusercontent.com/u/25212406");

                builder.AddScriptSrc()
                    .Self()
                    .WithHash256("6IIvUyrpNJsrV0PElO/SFu1ORPnryCprHLVIlaW4hDM=")
                    // Swagger
                    //.WithHash256("???")
                    // HealthChecks
                    .WithHash256("Pog3qaswXe7/IWv8apyiJjOx0FUoFHU464msTqEeZNc=");


                builder.AddStyleSrc() // style-src 'self' 'strict-dynamic'
                    .Self()
                    .WithHash256("c5o8WnfLAUwx9Cfl/JgwrKsLPSZmgAj3Gujjp00yJuc=")
                    .WithHash256("mvh00ICjr8vT7eqgfC3oP7+ei01u7qsWXH+GltiWn4k=")
                    .WithHash256("c5o8WnfLAUwx9Cfl/JgwrKsLPSZmgAj3Gujjp00yJuc=")
                    // swagger
                    .WithHash256("pyVPiLlnqL9OWVoJPs/E6VVF5hBecRzM2gBiarnaqAo=")
                    .WithHash256("tVFibyLEbUGj+pO/ZSi96c01jJCvzWilvI5Th+wLeGE=")
                    .WithHash256("47DEQpj8HBSa+/TImW+5JCeuQeRkm5NMpJWZG3hSuFU=")
                    // HealthChecks
                    .WithHash256("YMtUc29HaYBAAS/bFkyPYn8pD3FtgczsgqvWSSK7weQ=")
                    .WithHash256("lFM3E0OrK9yvMbQsE+cBXQZxKgBNnlF1NKBim8i51sI=")
                    .WithHash256("IHdzGAJ/dJwX6+L9U2MXqkGDEkza1pn7T8lsJlG1zCQ=")
                    // Hangfire
                    .WithHash256("z7zcnw/4WalZqx+PrNaRnoeLz/G9WXuFqV1WCJ129sg=");

                builder.AddMediaSrc() // media-src https:
                    .None();

                builder.AddFrameAncestors() // frame-ancestors 'none'
                    .None();

                builder.AddBaseUri() // base-uri 'self'
                    .Self();

                builder.AddFrameSrc()
                   .None();
            };

            if (hostEnvironment.IsDevelopment())
            {
                //policies.AddContentSecurityPolicy(cspBuilder);
                policies.AddContentSecurityPolicyReportOnly(cspBuilder); // to report on the browser console.
            }
            else
            {
                            policies.AddContentSecurityPolicyReportOnly(cspBuilder); // to report on the browser console.
                //policies.AddContentSecurityPolicy(cspBuilder);
            }
        });
    }
}