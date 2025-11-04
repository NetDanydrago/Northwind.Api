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

internal class GetProductHandler(IQueryableProductRepository queryableProductRepository,
    ILogger<GetProductHandler> logger) : IGetProductInputPort
{
    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        try
        {
            logger.LogInformation("Attempting to get product by ID: {Id}", id);
            var product = await queryableProductRepository.GetByIdAsync(id);
            if (product == null)
            {
                throw new Exception("Product not find");
            }
            logger.LogInformation("Successfully retrieved product: {Id}", id);
            return product;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting product by ID: {Id}", id);
            throw;
        }
    }
}
