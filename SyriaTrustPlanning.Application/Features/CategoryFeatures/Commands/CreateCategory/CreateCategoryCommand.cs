using MediatR;
using SharijhaAward.Application.Responses;

namespace SyriaTrustPlanning.Application.Features.CategoryFeatures.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<BaseResponse<object>>
    {
        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public string? Token { get; set; }
    }
}
