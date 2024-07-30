using MediatR;
using SharijhaAward.Application.Responses;
using SyriaTrustPlanning.Domain.Constants;

namespace SyriaTrustPlanning.Application.Features.UserFeatures.Commands.SignUp
{
    public class SignUpCommand : IRequest<BaseResponse<AuthenticationResponse>>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; } = null!;
    }
}
