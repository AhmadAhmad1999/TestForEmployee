using AutoMapper;
using MediatR;
using SharijhaAward.Application.Responses;
using SyriaTrustPlanning.Application.Contract.Persistence;
using SyriaTrustPlanning.Domain.Entities.ProductModel;

namespace SyriaTrustPlanning.Application.Features.ProductFeatures.Commands.UpdateProduct
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, BaseResponse<object>>
    {
        private readonly IAsyncRepository<Product> _ProductRepository;
        private readonly IMapper _Mapper;

        public UpdateProductHandler(IMapper Mapper,
            IAsyncRepository<Product> ProductRepository)
        {
            _ProductRepository = ProductRepository;
            _Mapper = Mapper;
        }

        public async Task<BaseResponse<object>> Handle(UpdateProductCommand Request, CancellationToken cancellationToken)
        {
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
