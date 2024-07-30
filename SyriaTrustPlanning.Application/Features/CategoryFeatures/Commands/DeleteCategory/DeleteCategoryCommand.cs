using MediatR;
using SharijhaAward.Application.Responses;

namespace SyriaTrustPlanning.Application.Features.CategoryFeatures.Commands.DeleteCategory
{
    public class DeleteCategoryCommand : IRequest<BaseResponse<object>>
    {
        public int Id { get; set; }
    }
}
