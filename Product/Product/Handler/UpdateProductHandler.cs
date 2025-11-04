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

internal class UpdateProductHandler(
    ICommandProductRepository productRepository,
    IQueryableProductRepository queryProductRepository,
    ILogger<CreateProductHandler> logger) : IUpdateProductInputPort
{
    public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
    {
		try
		{
			logger.LogInformation("Attempting to update category: {Id}", updateProductDto.Id);
            var existProduct = await queryProductRepository.GetByIdAsync(updateProductDto.Id);
            if(existProduct == null)
            {
                throw new Exception("Product Not Found");
            }

            var productWhitSameName = await queryProductRepository.GetByNameAsync(updateProductDto.Name);
            if(productWhitSameName != null && productWhitSameName.Id != updateProductDto.Id)
            {
                throw new Exception("Product Al Ready Exists");
            }

            await using var scope = new DomainTransactionScope();
            await scope.EnlistAsync(productRepository);
            await productRepository.UpdateProductAsync(updateProductDto);
            await productRepository.SaveChangesAsync();

            scope.Complete();

        }
		catch (Exception ex)
		{
            logger.LogError(ex, "Error updating category: {Id}", updateProductDto.Id);
            throw;
		}
    }
}
