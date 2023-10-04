using Arc4u.Configuration;
using Arc4u.Diagnostics;
using Arc4u.Security.Principal;
using Arc4u.ServiceModel;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace EG.DemoPCBE99925.ManageCourse.Web.Infrastructure;

public class ClaimsPrincipalFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
{
    public ClaimsPrincipalFactory(ILocalStorageService localStorage, IAppPrincipalFactory appPrincipalFactory, NavigationManager navigation, IOptionsMonitor<SimpleKeyValueSettings> optionsSettings, IAccessTokenProviderAccessor accessor, ILogger<ClaimsPrincipalFactory> logger) : base(accessor)
    {
        _appPrincipalFactory  = appPrincipalFactory;
        _logger = logger;
        _localStorage = localStorage;
        _navigation = navigation;
        _optionsSettings = optionsSettings;
    }

    private readonly IAppPrincipalFactory _appPrincipalFactory;
    private readonly ILogger<ClaimsPrincipalFactory> _logger;
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navigation;
    private readonly IOptionsMonitor<SimpleKeyValueSettings> _optionsSettings;

    public override async ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
    {
        var initialUser = await base.CreateUserAsync(account, options).ConfigureAwait(false);

        if (initialUser.Identity is not null && initialUser.Identity.IsAuthenticated)
        {
            try
            {
                var messages = new Messages();
                var principal = await _appPrincipalFactory.CreatePrincipal(_optionsSettings.Get("OAuth2"), messages).ConfigureAwait(false);

                var cultureLs = await _localStorage.GetItemAsStringAsync("uiculture").ConfigureAwait(false);

                if (String.IsNullOrWhiteSpace(cultureLs))
                {

                    // set the current culture on the principal!
                    // default is en-GB in case the culture doesn't exist in the claims.

                    cultureLs = string.IsNullOrWhiteSpace(principal.Profile.Culture.Name) ? "en-GB" : principal.Profile.Culture.Name;

                    try
                    {
                        _logger.Technical().Debug($"No culture is set in the local storage. Reload with the new culture: {cultureLs}").Log();
                        await _localStorage.SetItemAsStringAsync("uiculture", cultureLs).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        _logger.Technical().Exception(ex).Log();
                    }
                }

                principal.Profile.CurrentCulture = new System.Globalization.CultureInfo(cultureLs);

                return principal;
            }
            catch (Exception e)
            {
                _logger.Technical().Exception(e).Log();
            }
        }

        return initialUser;
    }
}