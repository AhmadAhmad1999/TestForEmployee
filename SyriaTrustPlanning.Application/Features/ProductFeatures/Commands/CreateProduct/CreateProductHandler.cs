using AutoMapper;
using MediatR;
using SharijhaAward.Application.Responses;
using SyriaTrustPlanning.Application.Contract.Infrastructure;
using SyriaTrustPlanning.Application.Contract.Persistence;
using SyriaTrustPlanning.Domain.Entities.CategoryProductModel;
using SyriaTrustPlanning.Domain.Entities.IdentityModels;
using SyriaTrustPlanning.Domain.Entities.ProductModel;
using System.Transactions;

namespace SyriaTrustPlanning.Application.Features.ProductFeatures.Commands.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, BaseResponse<object>>
    {
        private readonly IMapper _Mapper;
        private readonly IAsyncRepository<Product> _ProductRepository;
        private readonly IAsyncRepository<CategoryProduct> _CategoryProductRepository;
        private readonly IAsyncRepository<User> _UserRepository;
        private readonly IJwtProvider _JWTProvider;

        public CreateProductHandler(IMapper Mapper,
            IAsyncRepository<Product> ProductRepository,
            IAsyncRepository<CategoryProduct> CategoryProductRepository,
            IAsyncRepository<User> UserRepository,
            IJwtProvider JWTProvider)
        {
            _Mapper = Mapper;
            _ProductRepository = ProductRepository;
            _CategoryProductRepository = CategoryProductRepository;
            _UserRepository = UserRepository;
            _JWTProvider = JWTProvider;
        }

        public async Task<BaseResponse<object>> Handle(CreateProductCommand Request, CancellationToken cancellationToken)
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
                    Product NewProductEntity = _Mapper.Map<Product>(Request);

                    await _ProductRepository.AddAsync(NewProductEntity);

                    IEnumerable<CategoryProduct> NewCategoryProductEntities = Request.CategoriesIds
                        .Select(x => new CategoryProduct()
                        {
                            CategoryId = x,
                            ProductId = NewProductEntity.Id
                        });

                    await _CategoryProductRepository.AddRangeAsync(NewCategoryProductEntities);

                    string ResponseMessage = "Created successfully";

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