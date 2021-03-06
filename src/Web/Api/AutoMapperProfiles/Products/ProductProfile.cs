using AutoMapper;
using TariffComparison.Api.Controllers.v1.Products.Requests;
using TariffComparison.Application.Products.Command.CreateProduct;
using TariffComparison.Application.Products.Query.GetProducts;

namespace TariffComparison.Api.AutoMapperProfiles.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductRequest, CreateProductCommand>();

            CreateMap<GetProductsRequest, GetProductsQuery>();
        }
    }
}
