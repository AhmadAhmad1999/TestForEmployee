using MediatR;
using SharijhaAward.Application.Responses;

namespace SyriaTrustPlanning.Application.Features.CategoryFeatures.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery : IRequest<BaseResponse<GetCategoryByIdDto>>
    {
        public int Id { get; set; }
    }
}
