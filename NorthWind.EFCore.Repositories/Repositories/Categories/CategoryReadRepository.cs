using Microsoft.EntityFrameworkCore;
using NorthWind.EFCore.Repositories.DbContexts;
using Product.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.EFCore.Repositories.Repositories.Categories;

class CategoryReadRepository(QueryDbContext context) : ICategoryReadRepository
{
    public async Task<bool> CategoryExistAsync(int categoryId)
    {
        return await context.Categories.AnyAsync(c => c.Id == categoryId);
    }

    public async Task<bool> CategoryIsActivateAsync(int categoryId)
    {
        var cat = await context.Categories
                    .Where(c => c.Id == categoryId)
                    .Select(c => new { c.IsActive })
                    .FirstOrDefaultAsync();

        return cat is not null && cat.IsActive;
    }
}
