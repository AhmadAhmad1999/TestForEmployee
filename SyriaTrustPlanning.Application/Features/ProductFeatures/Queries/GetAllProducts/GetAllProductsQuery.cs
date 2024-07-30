using MediatR;
using SharijhaAward.Application.Responses;

namespace SyriaTrustPlanning.Application.Features.ProductFeatures.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<BaseResponse<List<GetAllProductsListVM>>>
    {
        public int? CategoryId { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
    }
}
