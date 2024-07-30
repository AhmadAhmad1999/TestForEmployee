using MediatR;
using SharijhaAward.Application.Responses;

namespace SyriaTrustPlanning.Application.Features.CategoryFeatures.Queries.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<BaseResponse<List<GetAllCategoriesListVM>>>
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
    }
}
