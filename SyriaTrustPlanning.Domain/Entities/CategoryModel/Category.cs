using SyriaTrustPlanning.Domain.Common;

namespace SyriaTrustPlanning.Domain.Entities.CategoryModel
{
    public class Category : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
    }
}
