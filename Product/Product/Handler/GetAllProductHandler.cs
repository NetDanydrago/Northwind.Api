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

internal class GetAllProductHandler(IQueryableProductRepository queryableProductRepository,
    ILogger<GetAllProductHandler> logger) : IGetAllProductsInputPort
{
    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        try
        {
            logger.LogInformation("Attempting to get all active Products");
            var products = await queryableProductRepository.GetAllActiveAsync();
            logger.LogInformation("Successfully retrieved {Count} active categories", products.Count());
            return products;
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex, "Error getting all active products");
            throw;
        }
    }
}
