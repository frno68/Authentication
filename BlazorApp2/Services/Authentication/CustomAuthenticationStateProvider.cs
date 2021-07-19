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
        private readonly TokenValidator _tokenValidator;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenInformationStorage _tokenInformationStorage;

        public CustomAuthenticationStateProvider(
            IHttpClientFactory httpClientFactory,
            TokenValidator tokenValidator,
            ITokenInformationStorage tokenInformationStorage)
        {
            _httpClientFactory = httpClientFactory;
            _tokenValidator = tokenValidator;
            _tokenInformationStorage = tokenInformationStorage;
        }
        private readonly ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            TokenInformation tokenInformation = await _tokenInformationStorage.GetAsync();
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity() { });
            if (tokenInformation == null)
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }
            try
            {
                SecurityToken securityToken = _tokenValidator.Validate(tokenInformation.AccessToken);
            }
            catch (SecurityTokenExpiredException)
            {
                await Refresh();
                tokenInformation = await _tokenInformationStorage.GetAsync();
            }
            catch (Exception)
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }

            var jwtSecurityToken = new JwtSecurityToken(tokenInformation.AccessToken);
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
            TokenInformation tokenInformation = await _tokenInformationStorage.GetAsync();
            await _tokenInformationStorage.RemoveAsync();
            RefreshRequest request = new RefreshRequest()
            {
                RefreshToken = tokenInformation.RefreshToken
            };
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "refresh")
            {
                Content = new StringContent(JsonConvert.SerializeObject(request))
            };
            requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var client = _httpClientFactory.CreateClient("AuthenticationService");
            var response = await client.SendAsync(requestMessage);
            var responseStatusCode = response.StatusCode;
            var tokenInformationJson = await response.Content.ReadAsStringAsync();
            if (responseStatusCode.ToString() != "OK")
            {
                return await Task.FromResult(false);
            }
            await _tokenInformationStorage.SetAsync(tokenInformationJson);
            return await Task.FromResult(true);
        }

    }
}
