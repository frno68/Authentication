using BlazorApp2.Models.Requests;
using BlazorApp2.Models.Responses;
using BlazorApp2.Services.TokenValidators;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorApp2.Services.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly TokenValidator _tokenValidator;
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomAuthenticationStateProvider(
            ILocalStorageService localStorageService,
            IHttpClientFactory httpClientFactory,
            TokenValidator tokenValidator)
        {
            _localStorageService = localStorageService;
            _httpClientFactory = httpClientFactory;
            _tokenValidator = tokenValidator;
        }
        private readonly ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var authenticatedUserResponseText = _localStorageService.GetItem("authenticatedUserResponseText");
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity() { });
            if (string.IsNullOrEmpty(authenticatedUserResponseText))
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }
            AuthenticatedUserResponse authenticatedUserResponse = 
                JsonConvert.DeserializeObject<AuthenticatedUserResponse>(authenticatedUserResponseText);
            try
            {
                SecurityToken securityToken = _tokenValidator.Validate(authenticatedUserResponse.AccessToken);
            }
            catch (SecurityTokenExpiredException)
            {
                await Refresh();
                authenticatedUserResponseText = _localStorageService.GetItem("authenticatedUserResponseText");
            }
            catch (Exception)
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }

            var jwtSecurityToken = new JwtSecurityToken(authenticatedUserResponse.AccessToken);
            var claims = jwtSecurityToken.Claims;
            var claimsIdentity = new ClaimsIdentity(claims, "auth");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private async Task<bool> Refresh()
        {
            var authenticatedUserResponseText = _localStorageService.GetItem("authenticatedUserResponseText");
            AuthenticatedUserResponse authenticatedUserResponse = JsonConvert.DeserializeObject<AuthenticatedUserResponse>(authenticatedUserResponseText);
            RefreshRequest request = new RefreshRequest()
            {
                RefreshToken = authenticatedUserResponse.RefreshToken
            };
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "refresh")
            {
                Content = new StringContent(JsonConvert.SerializeObject(request))
            };
            requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var client = _httpClientFactory.CreateClient("AuthenticationService");
            var response = await client.SendAsync(requestMessage);
            var responseStatusCode = response.StatusCode;
            authenticatedUserResponseText = await response.Content.ReadAsStringAsync();

            if (responseStatusCode.ToString() != "OK")
            {
                return await Task.FromResult(false);
            }
            _localStorageService.SetItem("authenticatedUserResponseText", authenticatedUserResponseText);
            return await Task.FromResult(true);
        }

    }
}
