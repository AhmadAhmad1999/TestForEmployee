using AutoMapper;
using SyriaTrustPlanning.Application.Features.CategoryFeatures.Commands.CreateCategory;
using SyriaTrustPlanning.Application.Features.CategoryFeatures.Commands.UpdateCategory;
using SyriaTrustPlanning.Application.Features.CategoryFeatures.Queries.GetAllCategories;
using SyriaTrustPlanning.Application.Features.CategoryFeatures.Queries.GetCategoryById;
using SyriaTrustPlanning.Application.Features.ProductFeatures.Commands.CreateProduct;
using SyriaTrustPlanning.Application.Features.ProductFeatures.Commands.UpdateProduct;
using SyriaTrustPlanning.Application.Features.ProductFeatures.Queries.GetAllProducts;
using SyriaTrustPlanning.Application.Features.ProductFeatures.Queries.GetProductById;
using SyriaTrustPlanning.Domain.Entities.CategoryModel;
using SyriaTrustPlanning.Domain.Entities.ProductModel;

namespace SyriaTrustPlanning.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CreateCategoryCommand>().ReverseMap();
            CreateMap<Category, UpdateCategoryCommand>().ReverseMap();
            CreateMap<Category, GetAllCategoriesListVM>().ReverseMap();
            CreateMap<Category, GetCategoryByIdDto>().ReverseMap();

            CreateMap<Product, CreateProductCommand>().ReverseMap();
            CreateMap<Product, UpdateProductCommand>().ReverseMap();
            CreateMap<Product, GetAllProductsListVM>().ReverseMap();
            CreateMap<Product, GetProductByIdDto>().ReverseMap();
        }
    }
}
