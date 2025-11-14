using Product.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Interfaces;

public interface IQueryableCategoryReadRepository
{
    Task<CategoryReadDto?> GetCategoryAsync(int categoryId);
}
