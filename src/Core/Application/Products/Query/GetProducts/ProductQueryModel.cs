namespace TariffComparison.Application.Products.Query.GetProducts
{
    public class ProductQueryModel
    {
        public long Id { get; set; }

        public string TariffName { get; set; }

        public int AnnualCosts { get; set; }
    }
}
