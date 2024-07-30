using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharijhaAward.Application.Responses;
using SyriaTrustPlanning.Application.Contract.Persistence;
using SyriaTrustPlanning.Domain.Entities.CategoryModel;

namespace SyriaTrustPlanning.Application.Features.CategoryFeatures.Queries.GetAllCategories
{
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, BaseResponse<List<GetAllCategoriesListVM>>>
    {
        private readonly IAsyncRepository<Category> _CategoryRepository;
        private readonly IMapper _Mapper;
        public GetAllCategoriesHandler(IAsyncRepository<Category> CategoryRepository,
            IMapper Mapper)
        {
            _CategoryRepository = CategoryRepository;
            _Mapper = Mapper;
        }

        public async Task<BaseResponse<List<GetAllCategoriesListVM>>> 
            Handle(GetAllCategoriesQuery Request, CancellationToken cancellationToken)
        {
            string ResponseMessage = string.Empty;

            List<GetAllCategoriesListVM> Categories = _Mapper.Map<List<GetAllCategoriesListVM>>(await _CategoryRepository
                .OrderByDescending(x => x.CreatedAt, Request.Page, Request.PerPage).ToListAsync());

            int TotalCount = await _CategoryRepository.GetCountAsync(null);

            Pagination PaginationParameter = new Pagination(Request.Page,
                Request.PerPage, TotalCount);

            return new BaseResponse<List<GetAllCategoriesListVM>>(ResponseMessage, true, 200, Categories, PaginationParameter);
        }
    }
}
