using MediatR;
using SharijhaAward.Application.Responses;
using SyriaTrustPlanning.Application.Contract.Persistence;
using SyriaTrustPlanning.Domain.Entities.IdentityModels;
using SyriaTrustPlanning.Application.Contract.Infrastructure;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace SyriaTrustPlanning.Application.Features.UserFeatures.Commands.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, BaseResponse<AuthenticationResponse>>
    {
        private readonly IJwtProvider _JwtProvider;
        private readonly IAsyncRepository<User> _UserRepository;
        public LoginHandler(IJwtProvider JwtProvider,
            IAsyncRepository<User> UserRepository)
        {
            _JwtProvider = JwtProvider;
            _UserRepository = UserRepository;
        }

        public async Task<BaseResponse<AuthenticationResponse>> Handle(LoginCommand Request, CancellationToken cancellationToken)
        {
            string ResponseMessage = string.Empty;

            User? UserToLogin = await _UserRepository
                .FirstOrDefaultAsync(x => x.Email.ToLower() == Request.Email.ToLower());

            if (UserToLogin is null)
            {
                ResponseMessage = "Invalid email or password";

                return new BaseResponse<AuthenticationResponse>(ResponseMessage, false, 404);
            }

            byte[] salt = new byte[16] { 41, 214, 78, 222, 28, 87, 170, 211, 217, 125, 200, 214, 185, 144, 44, 34 };

            string CheckPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: Request.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            if (UserToLogin == null)
                return new BaseResponse<AuthenticationResponse>("Invalid email or password", false, 400);

            if (CheckPassword == UserToLogin.Password)
            {
                string Token = _JwtProvider.Generate(UserToLogin);

                AuthenticationResponse Data = new AuthenticationResponse()
                {
                    token = Token,
                    message = "Login succeed"
                };

                return new BaseResponse<AuthenticationResponse>("Login succeed", true, 200, Data);
            }

            return new BaseResponse<AuthenticationResponse>("Invalid email or password", false, 400);
        }
    }
}
