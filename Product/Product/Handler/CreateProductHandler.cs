using DomainTransaction;
using Microsoft.Extensions.Logging;
using Product.Dtos;
using Product.Interfaces;
using Product.Internals.InputPorts;

namespace Product.Handler
{
    internal class CreateProductHandler(
        ICommandProductRepository productRepository,
        IQueryableProductRepository queryProductRepository,
        ILogger<CreateProductHandler> logger
    ) : ICreateProductInputPort
    {
        public async Task CreateProductAsync(CreateProductDto createProductDto)
        {
            try
            {
                logger.LogInformation("Attempting to create product: {Name}", createProductDto.Name);

                var existingProduct = await queryProductRepository.GetByNameAsync(createProductDto.Name);
                if (existingProduct != null)
                {
                    throw new Exception("Product name already exists.");
                }
                await using var scope = new DomainTransactionScope();
                await scope.EnlistAsync(productRepository);
                int productId = await productRepository.CreateProductAsync(createProductDto);
                await productRepository.SaveChangesAsync();

                scope.Complete();

                logger.LogInformation("Successfully created product: {productId}", productId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating product: {Name}", createProductDto.Name);
                throw;
            }
        }
    }
}
