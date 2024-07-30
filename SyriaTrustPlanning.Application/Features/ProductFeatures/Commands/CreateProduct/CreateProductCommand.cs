using MediatR;
using SharijhaAward.Application.Responses;

namespace SyriaTrustPlanning.Application.Features.ProductFeatures.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<BaseResponse<object>>
    {
        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public float Price { get; set; }
    }
}
