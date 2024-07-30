using AutoMapper;
using MediatR;
using SharijhaAward.Application.Responses;
using SyriaTrustPlanning.Application.Contract.Persistence;
using SyriaTrustPlanning.Domain.Entities.CategoryModel;

namespace SyriaTrustPlanning.Application.Features.CategoryFeatures.Commands.CreateCategory
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, BaseResponse<object>>
    {
        private readonly IMapper _Mapper;
        private readonly IAsyncRepository<Category> _CategoryRepository;

        public CreateCategoryHandler(IMapper Mapper,
            IAsyncRepository<Category> CategoryRepository)
        {
            _Mapper = Mapper;
            _CategoryRepository = CategoryRepository;
        }

        public async Task<BaseResponse<object>> Handle(CreateCategoryCommand Request, CancellationToken cancellationToken)
        {
            Category NewCategoryEntity = _Mapper.Map<Category>(Request);

            await _CategoryRepository.AddAsync(NewCategoryEntity);

            string ResponseMessage = "Created successfully";

            return new BaseResponse<object>(ResponseMessage, true, 200);
        }
    }
}