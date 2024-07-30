namespace SyriaTrustPlanning.Application.Features.ProductFeatures.Queries.GetProductById
{
    public class GetProductByIdDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public float Price { get; set; }
    }
}
