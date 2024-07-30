using AutoMapper;
using MediatR;
using SharijhaAward.Application.Responses;
using SyriaTrustPlanning.Application.Contract.Infrastructure;
using SyriaTrustPlanning.Application.Contract.Persistence;
using SyriaTrustPlanning.Domain.Entities.IdentityModels;

namespace SyriaTrustPlanning.Application.Features.UserFeatures.Commands.SignUp
{
    public class SignUpHandler : IRequestHandler<SignUpCommand, BaseResponse<AuthenticationResponse>>
    {
        private readonly IMapper _Mapper;
        private readonly IJwtProvider _JwtProvider;
        private readonly IAsyncRepository<User> _UserRepository;
        public SignUpHandler(IMapper Mapper,
            IJwtProvider JwtProvider,
            IAsyncRepository<User> UserRepository)
        {
            _Mapper = Mapper;
            _JwtProvider = JwtProvider;
            _UserRepository = UserRepository;
        }
        public async Task<BaseResponse<AuthenticationResponse>> Handle(SignUpCommand Request, CancellationToken cancellationToken)
        {
            User? CheckEmail = await _UserRepository
                .FirstOrDefaultAsync(x => x.Email.ToLower() == Request.Email.ToLower());

            if (CheckEmail is not null)
                return new BaseResponse<AuthenticationResponse>("This email is already used", false, 400);

            User NewUserEntity = _Mapper.Map<User>(Request);

            byte[] salt = new byte[16] { 41, 214, 78, 222, 28, 87, 170, 211, 217, 125, 200, 214, 185, 144, 44, 34 };

            NewUserEntity.Password = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: Request.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            await _UserRepository.AddAsync(NewUserEntity);

            string Token = _JwtProvider.Generate(NewUserEntity);

            AuthenticationResponse Data = new AuthenticationResponse()
            {
                token = Token,
                message = "SignUp succeed"
            };

            return new BaseResponse<AuthenticationResponse>("SignUp succeed", true, 200, Data);
        }
    }
}
