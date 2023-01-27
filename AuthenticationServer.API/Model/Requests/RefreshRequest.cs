using System.ComponentModel.DataAnnotations;

namespace AuthenticationServer.API.Model.Requests
{
    public class RefreshRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
