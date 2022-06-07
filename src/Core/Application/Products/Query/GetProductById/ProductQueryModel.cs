namespace TariffComparison.Application.Products.Query.GetProductById
{
    public class ProductQueryModel
    {
        public long Id { get; set; }

        public string TariffName { get; set; }

        public int AnnualCosts { get; set; }
    }
}
