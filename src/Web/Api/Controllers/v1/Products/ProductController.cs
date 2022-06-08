using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TariffComparison.Api.Controllers.v1.Products.Requests;
using TariffComparison.ApiFramework.Tools;
using TariffComparison.Application.Products.Command.CreateProduct;
using TariffComparison.Application.Products.Query.GetProducts;

namespace TariffComparison.Api.Controllers.v1.Products
{
    [ApiVersion("1")]
    public class ProductController : BaseControllerV1
    {
        [HttpPost("create")]
        [SwaggerOperation("create products")]
        public async Task<ApiResult<int>> CreateAsync(CreateProductRequest request)
        {
            var command = Mapper.Map<CreateProductCommand>(request);

            var result = await Mediator.Send(command);
            return new ApiResult<int>(result);
        }

        [HttpGet("list")]
        [SwaggerOperation("get all products")]
        public async Task<ApiResult<IEnumerable<ProductQueryModel>>> GetAllsync([FromQuery] GetProductsRequest request)
        {
            var query = Mapper.Map<GetProductsQuery>(request);

            var result = await Mediator.Send(query);
            return new ApiResult<IEnumerable<ProductQueryModel>>(result);
        }
    }
}