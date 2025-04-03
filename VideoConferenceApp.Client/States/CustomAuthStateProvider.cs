using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using NetcodeHub.Packages.Extensions.LocalStorage;

namespace VideoConferenceApp.Client.States
{
    public class CustomAuthStateProvide
        (ILocalStorageService localStorageService, IConfiguration configuration) : AuthenticationStateProvider
    {
        ClaimsPrincipal User = new(new ClaimsIdentity());

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Get token key
            string key = configuration["Token:Key"]!;
            if (key == null)
            {
                return await Task.FromResult(new AuthenticationState(User));
            }

            // Get token from local storage
            string token = await localStorageService.GetItemAsStringAsync(key);
            if (token == null)
            {
                return await Task.FromResult(new AuthenticationState(User));
            }

            User = SetClaim(token);

            return await Task.FromResult(new AuthenticationState(User));
        }

        public async Task SetUserAuthenticated(string token)
        {
            if (configuration["Token:Key"] == null) return;

            string key = configuration["Token:Key"]!;
            await localStorageService.SaveAsStringAsync(key, token);
            User = SetClaim(key);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(User)));
        }

        public async Task SetUserLoggedOut()
        {
            string key = configuration["Token:Key"]!;
            if (key == null) return;

            await localStorageService.DeleteItemAsync(key);
            User = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(User)));
        }

        private static ClaimsPrincipal SetClaim(string token)
        {
            try 
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var claims = jwtToken.Claims;
                return new ClaimsPrincipal(new ClaimsIdentity(claims, "JwtAuth"));
            }
            catch
            {
                return new(new ClaimsIdentity());
            }
        }
    }
}
