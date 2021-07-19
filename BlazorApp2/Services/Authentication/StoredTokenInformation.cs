using BlazorApp2.Models.Responses;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BlazorApp2.Services.Authentication
{
    public interface ITokenInformationStorage
    {
        public Task<TokenInformation> GetAsync();
        public ValueTask SetAsync(string value);
        public ValueTask RemoveAsync();
    }

    public class TokenInformationStorage: ITokenInformationStorage
    {
        private readonly string _key; 
        private readonly ISessionStorageService _sessionStorage;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenInformationStorage(ISessionStorageService sessionStorage, 
            IHttpContextAccessor httpContextAccessor)
        {
            _sessionStorage = sessionStorage;
            _httpContextAccessor = httpContextAccessor;
            _key = httpContextAccessor.HttpContext.User.Identity.Name + "_storedToken";
        }
        public async Task<TokenInformation> GetAsync()
        {
            var tokenInformationJson = await _sessionStorage.GetItemAsync<string>(_key);
            TokenInformation tokenInformation = null;
            try
            {
                tokenInformation = JsonConvert.DeserializeObject<TokenInformation>(tokenInformationJson);
            }
            catch (Exception) { }
            return tokenInformation;
        }
        public ValueTask SetAsync(string value)
        {
            return _sessionStorage.SetItemAsync(_key, value);
        }
        public ValueTask RemoveAsync()
        {
            return _sessionStorage.RemoveItemAsync(_key);
        }

    }
    
}
