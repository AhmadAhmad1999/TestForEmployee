using MediatR;
using Microsoft.EntityFrameworkCore;
using SharijhaAward.Application.Responses;
using SyriaTrustPlanning.Application.Contract.Infrastructure;
using SyriaTrustPlanning.Application.Contract.Persistence;
using SyriaTrustPlanning.Domain.Entities.CategoryModel;
using SyriaTrustPlanning.Domain.Entities.CategoryProductModel;
using SyriaTrustPlanning.Domain.Entities.IdentityModels;
using System.Transactions;

namespace SyriaTrustPlanning.Application.Features.CategoryFeatures.Commands.DeleteCategory
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, BaseResponse<object>>
    {
        private readonly IAsyncRepository<Category> _CategoryRepository;
        private readonly IAsyncRepository<CategoryProduct> _CategoryProductRepository;
        private readonly IAsyncRepository<User> _UserRepository;
        private readonly IJwtProvider _JWTProvider;

        public DeleteCategoryHandler(IAsyncRepository<Category> CategoryRepository,
            IAsyncRepository<CategoryProduct> CategoryProductRepository,
            IAsyncRepository<User> UserRepository,
            IJwtProvider JWTProvider)
        {
            _CategoryRepository = CategoryRepository;
            _CategoryProductRepository = CategoryProductRepository;
            _UserRepository = UserRepository;
            _JWTProvider = JWTProvider;
        }

        public async Task<BaseResponse<object>> Handle(DeleteCategoryCommand Request, CancellationToken cancellationToken)
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

                    Category? CategoryEntityToDelete = await _CategoryRepository
                        .FirstOrDefaultAsync(x => x.Id == Request.Id);

                    if (CategoryEntityToDelete == null)
                    {
                        ResponseMessage = "Category is not found";

                        return new BaseResponse<object>(ResponseMessage, false, 404);
                    }

                    await _CategoryRepository.DeleteAsync(CategoryEntityToDelete);

                    List<CategoryProduct> CategoryProductEntitiesToDelete = await _CategoryProductRepository
                        .Where(x => x.CategoryId == Request.Id)
                        .ToListAsync();

                    if (CategoryProductEntitiesToDelete.Any())
                        await _CategoryProductRepository.DeleteListAsync(CategoryProductEntitiesToDelete);

                    ResponseMessage = "Category has been deleted successfully";

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
