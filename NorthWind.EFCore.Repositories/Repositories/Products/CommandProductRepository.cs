using Category.Dtos;
using Microsoft.EntityFrameworkCore;
using NorthWind.EFCore.Repositories.DbContexts;
using Product.Dtos;
using Product.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.EFCore.Repositories.Repositories.Products
{
    internal class CommandProductRepository(CommandDbContext context)
            : TransactionHandlerBase(context.Database), ICommandProductRepository
    {
        public async Task<int> CreateProductAsync(CreateProductDto createProduct)
        {
            var entity = await context.Products.AddAsync(new Entities.Product
            {
                Name = createProduct.Name,
                Description = createProduct.Description,
                CategoryId = createProduct.CategoryId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            });

            await context.SaveChangesAsync();

            return entity.Entity.Id;
        }

        public async Task DeactivateProductAsync(int id)
        {
            var product = await context.Products.FirstOrDefaultAsync(c => c.Id == id);
            if (product != null)
            {
                product.UpdatedAt = DateTime.UtcNow;
                product.IsActive = false;
                context.Products.Update(product);
            }
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(UpdateProductDto upadateProductDto)
        {
            var product = await context.Products.FirstOrDefaultAsync(c => c.Id == upadateProductDto.Id);
            if (product != null)
            {
                product.Name = upadateProductDto.Name;
                product.Description = upadateProductDto.Description;
                product.UpdatedAt = DateTime.UtcNow;
                context.Products.Update(product);
            }
        }
    }
}
