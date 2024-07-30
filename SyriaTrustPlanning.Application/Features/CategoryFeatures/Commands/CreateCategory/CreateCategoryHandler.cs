using AutoMapper;
using MediatR;
using SharijhaAward.Application.Responses;
using SyriaTrustPlanning.Application.Contract.Infrastructure;
using SyriaTrustPlanning.Application.Contract.Persistence;
using SyriaTrustPlanning.Domain.Entities.CategoryModel;
using SyriaTrustPlanning.Domain.Entities.IdentityModels;

namespace SyriaTrustPlanning.Application.Features.CategoryFeatures.Commands.CreateCategory
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, BaseResponse<object>>
    {
        private readonly IMapper _Mapper;
        private readonly IAsyncRepository<Category> _CategoryRepository;
        private readonly IAsyncRepository<User> _UserRepository;
        private readonly IJwtProvider _JWTProvider;

        public CreateCategoryHandler(IMapper Mapper,
            IAsyncRepository<Category> CategoryRepository,
            IAsyncRepository<User> UserRepository,
            IJwtProvider JWTProvider)
        {
            _Mapper = Mapper;
            _CategoryRepository = CategoryRepository;
            _UserRepository = UserRepository;
            _JWTProvider = JWTProvider;
        }

        public async Task<BaseResponse<object>> Handle(CreateCategoryCommand Request, CancellationToken cancellationToken)
        {
            int UserId = _JWTProvider.GetUserIdFromToken(Request.Token!);

            User? CheckUserId = await _UserRepository.FirstOrDefaultAsync(x => x.Id == UserId);

            if (CheckUserId is null)
                return new BaseResponse<object>("Unauthorized user", true, 401);

            Category NewCategoryEntity = _Mapper.Map<Category>(Request);

            await _CategoryRepository.AddAsync(NewCategoryEntity);

            string ResponseMessage = "Created successfully";

            return new BaseResponse<object>(ResponseMessage, true, 200);
        }
    }
}