using DomainTransaction;
using Microsoft.Extensions.Logging;
using DomainTransaction;
using Microsoft.Extensions.Logging;
using Product.Dtos;
using Product.Interfaces;
using Product.Internals.InputPorts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Handler;

internal class DesactivateProductHandler(
    ICommandProductRepository productRepository,
    IQueryableProductRepository queryableProduct,
    ILogger<DesactivateProductDto> logger) : IDesactivateProductInputPort
{
    public async Task DesactivateAsync(DesactivateProductDto desactivateProductDto)
    {
        try
        {
            logger.LogInformation("Attempting to deactivate product: {Id}", desactivateProductDto.Id);

            var existingProduct = await queryableProduct.GetByIdAsync(desactivateProductDto.Id);
            if (existingProduct == null)
            {
                throw new Exception("Product not found");
            }

            await using var scope = new DomainTransactionScope();
            await scope.EnlistAsync(productRepository);

            await productRepository.DeactivateProductAsync(desactivateProductDto.Id);
            await productRepository.SaveChangesAsync();

            scope.Complete();

            logger.LogInformation("Successfully deactivated product: {Id}", desactivateProductDto.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deactivating product: {Id}", desactivateProductDto.Id);
            throw;
        }
    }
}