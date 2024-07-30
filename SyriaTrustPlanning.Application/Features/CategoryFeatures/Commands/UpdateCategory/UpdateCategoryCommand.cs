using MediatR;
using SharijhaAward.Application.Responses;

namespace SyriaTrustPlanning.Application.Features.CategoryFeatures.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest<BaseResponse<object>>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public string? Token { get; set; }
    }
}
