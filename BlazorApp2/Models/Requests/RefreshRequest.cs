using System.ComponentModel.DataAnnotations;

namespace BlazorApp2.Models.Requests
{
    public class RefreshRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
