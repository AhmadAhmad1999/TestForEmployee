namespace SyriaTrustPlanning.Application.Features.CategoryFeatures.Queries.GetCategoryById
{
    public class GetCategoryByIdDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
    }
}
