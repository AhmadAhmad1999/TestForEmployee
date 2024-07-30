using AutoMapper;
using MediatR;
using SharijhaAward.Application.Responses;
using SyriaTrustPlanning.Application.Contract.Persistence;
using SyriaTrustPlanning.Domain.Entities.CategoryModel;

namespace SyriaTrustPlanning.Application.Features.CategoryFeatures.Commands.UpdateCategory
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, BaseResponse<object>>
    {
        private readonly IAsyncRepository<Category> _CategoryRepository;
        private readonly IMapper _Mapper;

        public UpdateCategoryHandler(IMapper Mapper,
            IAsyncRepository<Category> CategoryRepository)
        {
            _CategoryRepository = CategoryRepository;
            _Mapper = Mapper;
        }

        public async Task<BaseResponse<object>> Handle(UpdateCategoryCommand Request, CancellationToken cancellationToken)
        {
            string ResponseMessage = string.Empty;

            Category? CategoryEntityToUpdate = await _CategoryRepository
                .FirstOrDefaultAsync(x => x.Id == Request.Id);

            if (CategoryEntityToUpdate == null)
            {
                ResponseMessage = "Category is not found";

                return new BaseResponse<object>(ResponseMessage, false, 404);
            }

            _Mapper.Map(Request, CategoryEntityToUpdate, typeof(UpdateCategoryCommand), typeof(Category));

            await _CategoryRepository.UpdateAsync(CategoryEntityToUpdate);

            ResponseMessage = "Category has been updated successfully";

            return new BaseResponse<object>(ResponseMessage, true, 200);
        }
    }
}
