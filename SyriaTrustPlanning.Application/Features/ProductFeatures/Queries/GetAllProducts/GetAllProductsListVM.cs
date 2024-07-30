namespace SyriaTrustPlanning.Application.Features.ProductFeatures.Queries.GetAllProducts
{
    public class GetAllProductsListVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
        public float Price { get; set; }
    }
}
