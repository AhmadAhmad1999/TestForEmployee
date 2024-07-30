using SyriaTrustPlanning.Domain.Common;
using SyriaTrustPlanning.Domain.Entities.CategoryModel;
using SyriaTrustPlanning.Domain.Entities.ProductModel;

namespace SyriaTrustPlanning.Domain.Entities.CategoryProductModel
{
    public class CategoryProduct : AuditableEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
