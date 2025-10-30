using Product.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Interfaces
{
    public interface IQueryableProductRepository
    {
        Task<ProductDto?> GetByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllActiveAsync();
        Task<ProductDto?> GetByNameAsync(string name);
    }
}
