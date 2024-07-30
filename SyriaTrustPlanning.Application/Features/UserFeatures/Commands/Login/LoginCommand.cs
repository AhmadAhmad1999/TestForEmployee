using MediatR;
using SharijhaAward.Application.Responses;

namespace SyriaTrustPlanning.Application.Features.UserFeatures.Commands.Login
{
    public class LoginCommand : IRequest<BaseResponse<AuthenticationResponse>>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
