
namespace SharijhaAward.Application.Responses
{
    public class AuthenticationResponse
    {
        public string token { get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;
        public bool isSucceed { get; set; } = false;
    }
}
