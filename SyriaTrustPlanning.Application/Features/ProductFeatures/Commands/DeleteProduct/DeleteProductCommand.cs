using MediatR;
using SharijhaAward.Application.Responses;

namespace SyriaTrustPlanning.Application.Features.ProductFeatures.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<BaseResponse<object>>
    {
        public int Id { get; set; }
    }
}
