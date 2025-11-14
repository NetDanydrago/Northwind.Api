using Microsoft.EntityFrameworkCore;
using NorthWind.EFCore.Repositories.DbContexts;
using Product.Dtos;
using Product.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.EFCore.Repositories.Repositories.Products;

class QueryableCategoryReadRepository(QueryDbContext context) : IQueryableCategoryReadRepository
{
    public async Task<CategoryReadDto?> GetCategoryAsync(int categoryId)
    {
        return await context.Categories
            .Where(c => c.Id == categoryId)
            .Select(c => new CategoryReadDto(
                c.Id,
                c.Name,
                c.Description,
                c.IsActive
            ))
            .FirstOrDefaultAsync();
    }

}
