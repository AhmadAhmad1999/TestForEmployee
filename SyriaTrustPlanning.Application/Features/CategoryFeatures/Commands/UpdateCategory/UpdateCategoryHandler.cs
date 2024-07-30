using AutoMapper;
using MediatR;
using SharijhaAward.Application.Responses;
using SyriaTrustPlanning.Application.Contract.Infrastructure;
using SyriaTrustPlanning.Application.Contract.Persistence;
using SyriaTrustPlanning.Domain.Entities.CategoryModel;
using SyriaTrustPlanning.Domain.Entities.IdentityModels;

namespace SyriaTrustPlanning.Application.Features.CategoryFeatures.Commands.UpdateCategory
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, BaseResponse<object>>
    {
        private readonly IAsyncRepository<Category> _CategoryRepository;
        private readonly IMapper _Mapper;
        private readonly IAsyncRepository<User> _UserRepository;
        private readonly IJwtProvider _JWTProvider;

        public UpdateCategoryHandler(IMapper Mapper,
            IAsyncRepository<Category> CategoryRepository,
            IAsyncRepository<User> UserRepository,
            IJwtProvider JWTProvider)
        {
            _CategoryRepository = CategoryRepository;
            _Mapper = Mapper;
            _UserRepository = UserRepository;
            _JWTProvider = JWTProvider;
        }

        public async Task<BaseResponse<object>> Handle(UpdateCategoryCommand Request, CancellationToken cancellationToken)
        {
            int UserId = _JWTProvider.GetUserIdFromToken(Request.Token!);

            User? CheckUserId = await _UserRepository.FirstOrDefaultAsync(x => x.Id == UserId);

            if (CheckUserId is null)
                return new BaseResponse<object>("Unauthorized user", true, 401);

            string ResponseMessage = string.Empty;

            Category? CategoryEntityToUpdate = await _CategoryRepository
                .FirstOrDefaultAsync(x => x.Id == Request.Id);

            if (CategoryEntityToUpdate == null)
            {
                ResponseMessage = "Category is not found";

                return new BaseResponse<object>(ResponseMessage, false, 404);
            }

            _Mapper.Map(Request, CategoryEntityToUpdate, typeof(UpdateCategoryCommand), typeof(Category));

            await _CategoryRepository.UpdateAsync(CategoryEntityToUpdate);

            ResponseMessage = "Category has been updated successfully";

            return new BaseResponse<object>(ResponseMessage, true, 200);
        }
    }
}
