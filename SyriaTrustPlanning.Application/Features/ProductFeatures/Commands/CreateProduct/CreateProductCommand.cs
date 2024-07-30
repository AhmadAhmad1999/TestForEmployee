using MediatR;
using SharijhaAward.Application.Responses;

namespace SyriaTrustPlanning.Application.Features.ProductFeatures.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<BaseResponse<object>>
    {
        public List<int> CategoriesIds { get; set; } = new List<int>();
        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public float Price { get; set; }
        public string? Token { get; set; }
    }
}
