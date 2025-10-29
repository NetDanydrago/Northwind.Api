using DomainTransaction.Interfaces;
using Product.Dtos;

namespace Product.Interfaces;
public interface ICommandProductRepository : ITransactionHandler
{
    Task<int> CreateProductAsync(CreateProductDto createProduct);
}
