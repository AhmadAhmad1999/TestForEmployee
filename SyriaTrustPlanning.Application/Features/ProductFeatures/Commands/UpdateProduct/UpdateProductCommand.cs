using MediatR;
using SharijhaAward.Application.Responses;

namespace SyriaTrustPlanning.Application.Features.ProductFeatures.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<BaseResponse<object>>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public float Price { get; set; }
    }
}
