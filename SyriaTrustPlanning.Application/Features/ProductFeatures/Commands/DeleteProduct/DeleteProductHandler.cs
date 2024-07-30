using MediatR;
using Microsoft.EntityFrameworkCore;
using SharijhaAward.Application.Responses;
using SyriaTrustPlanning.Application.Contract.Infrastructure;
using SyriaTrustPlanning.Application.Contract.Persistence;
using SyriaTrustPlanning.Application.Features.ProductFeatures.Commands.DeleteProduct;
using SyriaTrustPlanning.Domain.Entities.CategoryProductModel;
using SyriaTrustPlanning.Domain.Entities.IdentityModels;
using SyriaTrustPlanning.Domain.Entities.ProductModel;
using System.Transactions;

namespace SyriaTrustPlanning.Application.Features.ProductFeatures.Commands.DeleteProduct
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, BaseResponse<object>>
    {
        private readonly IAsyncRepository<Product> _ProductRepository;
        private readonly IAsyncRepository<CategoryProduct> _CategoryProductRepository;
        private readonly IAsyncRepository<User> _UserRepository;
        private readonly IJwtProvider _JWTProvider;

        public DeleteProductHandler(IAsyncRepository<Product> ProductRepository,
            IAsyncRepository<CategoryProduct> CategoryProductRepository,
            IAsyncRepository<User> UserRepository,
            IJwtProvider JWTProvider)
        {
            _ProductRepository = ProductRepository;
            _CategoryProductRepository = CategoryProductRepository;
            _UserRepository = UserRepository;
            _JWTProvider = JWTProvider;
        }

        public async Task<BaseResponse<object>> Handle(DeleteProductCommand Request, CancellationToken cancellationToken)
        {
            int UserId = _JWTProvider.GetUserIdFromToken(Request.Token!);

            User? CheckUserId = await _UserRepository.FirstOrDefaultAsync(x => x.Id == UserId);

            if (CheckUserId is null)
                return new BaseResponse<object>("Unauthorized user", true, 401);

            TransactionOptions TransactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromMinutes(5)
            };

            using (TransactionScope Transaction = new TransactionScope(TransactionScopeOption.Required,
                TransactionOptions, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string ResponseMessage = string.Empty;

                    Product? ProductEntityToDelete = await _ProductRepository
                        .FirstOrDefaultAsync(x => x.Id == Request.Id);

                    if (ProductEntityToDelete == null)
                    {
                        ResponseMessage = "Product is not found";

                        return new BaseResponse<object>(ResponseMessage, false, 404);
                    }

                    await _ProductRepository.DeleteAsync(ProductEntityToDelete);

                    List<CategoryProduct> CategoryProductEntitiesToDelete = await _CategoryProductRepository
                        .Where(x => x.ProductId == Request.Id)
                        .ToListAsync();

                    if (CategoryProductEntitiesToDelete.Any())
                        await _CategoryProductRepository.DeleteListAsync(CategoryProductEntitiesToDelete);

                    ResponseMessage = "Product has been deleted successfully";

                    Transaction.Complete();

                    return new BaseResponse<object>(ResponseMessage, true, 200);
                }
                catch (Exception)
                {
                    Transaction.Dispose();
                    throw;
                }
            }
        }
    }
}
