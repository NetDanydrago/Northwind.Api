using Category.Interfaces;
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
        IQueryableCategoryRepository queryCategoryRepository,
        ILogger<CreateProductHandler> logger
    ) : ICreateProductInputPort
    {
        public async Task CreateProductAsync(CreateProductDto createProductDto)
        {
            if (createProductDto is null)
                throw new ArgumentNullException(nameof(createProductDto));

            var name = (createProductDto.Name ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del producto es requerido.", nameof(createProductDto.Name));

            logger.LogInformation("Attempting to create product: {Name}", name);

            var existing = await queryProductRepository.GetByNameAsync(name);
            if (existing is not null && string.Equals(existing.Name?.Trim(), name, StringComparison.OrdinalIgnoreCase))
            {
                logger.LogWarning("Duplicate product name: {Name}", name);
                throw new InvalidOperationException($"Ya existe un producto con el nombre '{name}'.");
            }

            var category = await queryCategoryRepository.GetByIdAsync(createProductDto.CategoryId);
            if (category is null)
            {
                logger.LogWarning("Category not found for product {Name}. CategoryId: {CategoryId}", name, createProductDto.CategoryId);
                throw new InvalidOperationException($"La categoría '{createProductDto.CategoryId}' no existe.");
            }

            if (category is { IsActive: false })
            {
                logger.LogWarning("Inactive category for product {Name}. CategoryId: {CategoryId}", name, createProductDto.CategoryId);
                throw new InvalidOperationException($"La categoría '{createProductDto.CategoryId}' está inactiva.");
            }

            await using var scope = new DomainTransactionScope();
            await scope.EnlistAsync(productRepository);
            try
            {
                var productId = await productRepository.CreateProductAsync(
                    new CreateProductDto(name, createProductDto.Description, createProductDto.CategoryId)
                );

                await productRepository.SaveChangesAsync();
                scope.Complete();

                logger.LogInformation("Successfully created product: {ProductId}", productId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating product: {Name}", createProductDto.Name);
                throw;
            }
        }
    }
}
