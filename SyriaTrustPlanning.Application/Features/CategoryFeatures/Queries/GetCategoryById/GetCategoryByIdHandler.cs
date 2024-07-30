using AutoMapper;
using MediatR;
using SharijhaAward.Application.Responses;
using SyriaTrustPlanning.Application.Contract.Persistence;
using SyriaTrustPlanning.Domain.Entities.CategoryModel;

namespace SyriaTrustPlanning.Application.Features.CategoryFeatures.Queries.GetCategoryById
{
    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, BaseResponse<GetCategoryByIdDto>>
    {
        private readonly IAsyncRepository<Category> _CategoryRepository;
        private readonly IMapper _Mapper;
        public GetCategoryByIdHandler(IAsyncRepository<Category> CategoryRepository,
            IMapper Mapper)
        {
            _CategoryRepository = CategoryRepository;
            _Mapper = Mapper;
        }

        public async Task<BaseResponse<GetCategoryByIdDto>> Handle(GetCategoryByIdQuery Request, CancellationToken cancellationToken)
        {
            string ResponseMessage = string.Empty;

            Category? CategoryEntity = await _CategoryRepository
                .FirstOrDefaultAsync(x => x.Id == Request.Id);

            if (CategoryEntity == null)
            {
                ResponseMessage = "Category is not found";

                return new BaseResponse<GetCategoryByIdDto>(ResponseMessage, false, 404);
            }

            GetCategoryByIdDto GetCategoryByIdDto = _Mapper.Map<GetCategoryByIdDto>(CategoryEntity);

            return new BaseResponse<GetCategoryByIdDto>(ResponseMessage, true, 200, GetCategoryByIdDto);
        }
    }
}
