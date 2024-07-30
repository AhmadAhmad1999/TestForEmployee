using AutoMapper;
using MediatR;
using SharijhaAward.Application.Responses;
using SyriaTrustPlanning.Application.Contract.Persistence;
using SyriaTrustPlanning.Domain.Entities.ProductModel;

namespace SyriaTrustPlanning.Application.Features.ProductFeatures.Commands.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, BaseResponse<object>>
    {
        private readonly IMapper _Mapper;
        private readonly IAsyncRepository<Product> _ProductRepository;

        public CreateProductHandler(IMapper Mapper,
            IAsyncRepository<Product> ProductRepository)
        {
            _Mapper = Mapper;
            _ProductRepository = ProductRepository;
        }

        public async Task<BaseResponse<object>> Handle(CreateProductCommand Request, CancellationToken cancellationToken)
        {
            Product NewProductEntity = _Mapper.Map<Product>(Request);

            await _ProductRepository.AddAsync(NewProductEntity);

            string ResponseMessage = "Created successfully";

            return new BaseResponse<object>(ResponseMessage, true, 200);
        }
    }
}