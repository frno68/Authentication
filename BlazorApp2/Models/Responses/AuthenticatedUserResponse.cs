namespace BlazorApp2.Models.Responses
{
    public class AuthenticatedUserResponse
    {
        public AuthenticatedUserResponse()
        {

        }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
