using SyriaTrustPlanning.Domain.Common;

namespace SyriaTrustPlanning.Domain.Entities.ProductModel
{
    public class Product : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public float Price { get; set; }
    }
}
