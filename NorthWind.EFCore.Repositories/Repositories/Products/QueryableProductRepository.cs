using NorthWind.EFCore.Repositories.DbContexts;
using Product.Dtos;
using Product.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace NorthWind.EFCore.Repositories.Repositories.Products
{
    internal class QueryableProductRepository(QueryDbContext context)
     : IQueryableProductRepository
    {
        public async Task<IEnumerable<ProductDto>> GetAllActiveAsync()
        {
            var query =
                from p in context.Products
                join c in context.Categories on p.CategoryId equals c.Id
                where p.IsActive
                select new ProductDto(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.CategoryId,
                    c.Name
                );

            return await query.ToListAsync();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var query =
                from p in context.Products
                join c in context.Categories on p.CategoryId equals c.Id
                where p.Id == id
                select new ProductDto(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.CategoryId,
                    c.Name
                );

            return await query.FirstOrDefaultAsync();
        }

        public async Task<ProductDto?> GetByNameAsync(string name)
        {
            var query =
                from p in context.Products
                join c in context.Categories on p.CategoryId equals c.Id
                where p.Name == name
                select new ProductDto(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.CategoryId,
                    c.Name
                );

            return await query.FirstOrDefaultAsync();
        }
    }
}
