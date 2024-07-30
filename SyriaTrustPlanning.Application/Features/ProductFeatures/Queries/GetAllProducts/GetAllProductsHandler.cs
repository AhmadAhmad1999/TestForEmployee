using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharijhaAward.Application.Responses;
using SyriaTrustPlanning.Application.Contract.Persistence;
using SyriaTrustPlanning.Domain.Entities.CategoryProductModel;

namespace SyriaTrustPlanning.Application.Features.ProductFeatures.Queries.GetAllProducts
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, BaseResponse<List<GetAllProductsListVM>>>
    {
        private readonly IAsyncRepository<CategoryProduct> _CategoryProductRepository;
        private readonly IMapper _Mapper;
        public GetAllProductsHandler(IAsyncRepository<CategoryProduct> CategoryProductRepository,
            IMapper Mapper)
        {
            _CategoryProductRepository = CategoryProductRepository;
            _Mapper = Mapper;
        }

        public async Task<BaseResponse<List<GetAllProductsListVM>>>
            Handle(GetAllProductsQuery Request, CancellationToken cancellationToken)
        {
            string ResponseMessage = string.Empty;

            List<GetAllProductsListVM> Products = _Mapper.Map<List<GetAllProductsListVM>>(_CategoryProductRepository
                .Where(x => x.CategoryId == Request.CategoryId)
                .Include(x => x.Product!)
                .Select(x => x.Product!)
                .AsEnumerable()
                .OrderByDescending(x => x.CreatedAt)
                .Skip((Request.Page - 1) * Request.PerPage)
                .Take(Request.PerPage)
                .ToList());

            int TotalCount = await _CategoryProductRepository.GetCountAsync(null);

            Pagination PaginationParameter = new Pagination(Request.Page,
                Request.PerPage, TotalCount);

            return new BaseResponse<List<GetAllProductsListVM>>(ResponseMessage, true, 200, Products, PaginationParameter);
        }
    }
}
