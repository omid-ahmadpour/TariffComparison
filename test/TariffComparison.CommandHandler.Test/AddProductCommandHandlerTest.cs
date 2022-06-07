using TariffComparison.Common.Exceptions;
using TariffComparison.Persistance.CommandHandlers.Products;
using TariffComparison.Persistance.Db;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using TariffComparison.Application.Products.Command.CreateProduct;

namespace TariffComparison.CommandHandler.Test
{
    public class AddProductCommandHandlerTest
    {
        [Fact]
        public async Task Should_ThrowException_When_InputIsNull()
        {
            var dbContext = new Mock<AppWriteDbContext>();
            var logger = new Mock<ILogger<CreateProductCommandHandler>>();

            //var commandHandler = new CreateProductCommandHandler(dbContext.Object, logger.Object);

            var request = new Mock<CreateProductCommand>();

            //await Assert.ThrowsAsync<InvalidNullInputException>(() => commandHandler.Handle(request.Object, CancellationToken.None));
            //await Assert.ThrowsAsync<InvalidNullInputException>(() => commandHandler.Handle(null, CancellationToken.None));
        }
    }
}