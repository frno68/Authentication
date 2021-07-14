using BlazorApp2.Models.Requests;
using BlazorApp2.Models.Responses;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BlazorApp2.Services.Authentication
{
    public interface IAccountService
	{
        public Task<bool> RegisterAsync(
            string username,
            string password,
            string confirmPassword,
            string email
        );
		public Task<bool> LoginAsync(
            string userName, 
            string password
        );
        public Task<bool> LogoutAsync();
    }

    public class AccountService : IAccountService
    {
        private readonly AuthenticationStateProvider _customAuthenticationStateProvider;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocalStorageService _localStorageService;
        public AccountService(
			ILocalStorageService localStorageService,
			AuthenticationStateProvider customAuthenticationStateProvider,
			IHttpClientFactory httpClientFactory
		)
        {
            _localStorageService = localStorageService;
            _customAuthenticationStateProvider = customAuthenticationStateProvider;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<bool> RegisterAsync(
            string username,
            string password,
            string confirmPassword,
            string email
        ){
            RegisterRequest request = new RegisterRequest()
            {
                Username = username,
                Password = password,
                ConfirmPassword = confirmPassword,
                Email = email
            };
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "register")
            {
                Content = new StringContent(JsonConvert.SerializeObject(request))
            };
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var client = _httpClientFactory.CreateClient("AuthenticationService");
            var response = await client.SendAsync(requestMessage);
            var responseStatusCode = response.StatusCode;
            if (responseStatusCode.ToString() != "OK")
            {
                return await Task.FromResult(false);
            }
            //var authenticatedUserResponseText = await response.Content.ReadAsStringAsync(); 
            //_localStorageService.SetItem("authenticatedUserResponseText", authenticatedUserResponseText);
            //(_customAuthenticationStateProvider as CustomAuthenticationStateProvider).NotifyAuthenticationStateChanged();
            return await Task.FromResult(true);
        }
        public async Task<bool> LoginAsync(string userName, string password)
        {
            LoginRequest request = new LoginRequest()
            {
                Username = userName,
                Password = password
            };
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "login")
            {
                Content = new StringContent(JsonConvert.SerializeObject(request))
            };
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");


            var client = _httpClientFactory.CreateClient("AuthenticationService");
            var response = await client.SendAsync(requestMessage);
			var responseStatusCode = response.StatusCode;
			if(responseStatusCode.ToString() != "OK")
            {
                return await Task.FromResult(false);
            }
            var authenticatedUserResponseText = await response.Content.ReadAsStringAsync();
            _localStorageService.SetItem("authenticatedUserResponseText", authenticatedUserResponseText);
            (_customAuthenticationStateProvider as CustomAuthenticationStateProvider).NotifyAuthenticationStateChanged();
            return await Task.FromResult(true);
        }
        public async Task<bool> LogoutAsync()
		{
            _localStorageService.RemoveItem("authenticatedUserResponseText");
            (_customAuthenticationStateProvider as CustomAuthenticationStateProvider).NotifyAuthenticationStateChanged();
            return await Task.FromResult(true);
		}
    }

	public class LoginInfo
    {
		public string UserName { get; set; } = "frno";
		public string Password { get; set; } = "frno";
    }

}
