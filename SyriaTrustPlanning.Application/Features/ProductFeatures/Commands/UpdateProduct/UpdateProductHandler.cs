using AutoMapper;
using MediatR;
using SharijhaAward.Application.Responses;
using SyriaTrustPlanning.Application.Contract.Infrastructure;
using SyriaTrustPlanning.Application.Contract.Persistence;
using SyriaTrustPlanning.Domain.Entities.IdentityModels;
using SyriaTrustPlanning.Domain.Entities.ProductModel;

namespace SyriaTrustPlanning.Application.Features.ProductFeatures.Commands.UpdateProduct
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, BaseResponse<object>>
    {
        private readonly IAsyncRepository<Product> _ProductRepository;
        private readonly IMapper _Mapper;
        private readonly IAsyncRepository<User> _UserRepository;
        private readonly IJwtProvider _JWTProvider;

        public UpdateProductHandler(IMapper Mapper,
            IAsyncRepository<Product> ProductRepository,
            IAsyncRepository<User> UserRepository,
            IJwtProvider JWTProvider)
        {
            _ProductRepository = ProductRepository;
            _Mapper = Mapper;
            _UserRepository = UserRepository;
            _JWTProvider = JWTProvider;
        }

        public async Task<BaseResponse<object>> Handle(UpdateProductCommand Request, CancellationToken cancellationToken)
        {
            int UserId = _JWTProvider.GetUserIdFromToken(Request.Token!);

            User? CheckUserId = await _UserRepository.FirstOrDefaultAsync(x => x.Id == UserId);

            if (CheckUserId is null)
                return new BaseResponse<object>("Unauthorized user", true, 401);

            string ResponseMessage = string.Empty;

            Product? ProductEntityToUpdate = await _ProductRepository
                .FirstOrDefaultAsync(x => x.Id == Request.Id);

            if (ProductEntityToUpdate == null)
            {
                ResponseMessage = "Product is not found";

                return new BaseResponse<object>(ResponseMessage, false, 404);
            }

            _Mapper.Map(Request, ProductEntityToUpdate, typeof(UpdateProductCommand), typeof(Product));

            await _ProductRepository.UpdateAsync(ProductEntityToUpdate);

            ResponseMessage = "Product has been updated successfully";

            return new BaseResponse<object>(ResponseMessage, true, 200);
        }
    }
}
