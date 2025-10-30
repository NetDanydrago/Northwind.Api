using Product.Dtos;

namespace Product.Interfaces
{
    public interface IQueryableProductRepository
    {
        Task<ProductDto> GetByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllActiveAsync();
        Task<ProductDto> GetByNameAsync(string name);
    }
}
