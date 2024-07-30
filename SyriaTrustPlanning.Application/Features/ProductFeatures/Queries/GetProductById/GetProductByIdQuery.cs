using MediatR;
using SharijhaAward.Application.Responses;

namespace SyriaTrustPlanning.Application.Features.ProductFeatures.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<BaseResponse<GetProductByIdDto>>
    {
        public int Id { get; set; }
    }
}
