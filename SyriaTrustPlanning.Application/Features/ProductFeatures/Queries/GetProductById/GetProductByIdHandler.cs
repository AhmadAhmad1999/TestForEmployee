using AutoMapper;
using MediatR;
using SharijhaAward.Application.Responses;
using SyriaTrustPlanning.Application.Contract.Persistence;
using SyriaTrustPlanning.Domain.Entities.ProductModel;

namespace SyriaTrustPlanning.Application.Features.ProductFeatures.Queries.GetProductById
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, BaseResponse<GetProductByIdDto>>
    {
        private readonly IAsyncRepository<Product> _ProductRepository;
        private readonly IMapper _Mapper;
        public GetProductByIdHandler(IAsyncRepository<Product> ProductRepository,
            IMapper Mapper)
        {
            _ProductRepository = ProductRepository;
            _Mapper = Mapper;
        }

        public async Task<BaseResponse<GetProductByIdDto>> Handle(GetProductByIdQuery Request, CancellationToken cancellationToken)
        {
            string ResponseMessage = string.Empty;

            Product? ProductEntity = await _ProductRepository
                .FirstOrDefaultAsync(x => x.Id == Request.Id);

            if (ProductEntity == null)
            {
                ResponseMessage = "Product is not found";

                return new BaseResponse<GetProductByIdDto>(ResponseMessage, false, 404);
            }

            GetProductByIdDto GetProductByIdDto = _Mapper.Map<GetProductByIdDto>(ProductEntity);

            return new BaseResponse<GetProductByIdDto>(ResponseMessage, true, 200, GetProductByIdDto);
        }
    }
}
