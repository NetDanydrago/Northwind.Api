using Category.Dtos;
using Category.Interfaces;
using Microsoft.EntityFrameworkCore;
using NorthWind.EFCore.Repositories.DbContexts;

namespace NorthWind.EFCore.Repositories.Repositories.Categories;
internal class CommandCategoryRepository(CommandDbContext context) : TransactionHandlerBase(context.Database), ICommandCategoryRepository
{
    public async Task<CategoryDto> CreateAsync(CreateCategoryDto createCategoryDto)
    {
       var entity = await context.Categories.AddAsync(new Entities.Category
        {
            Name = createCategoryDto.Name,
            Description = createCategoryDto.Description,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });
        context.SaveChanges();
        return new CategoryDto
        {
            Id = entity.Entity.Id,
            Name = entity.Entity.Name,
            Description = entity.Entity.Description,
            IsActive = entity.Entity.IsActive,
            CreatedAt = entity.Entity.CreatedAt,
            UpdatedAt = entity.Entity.UpdatedAt
        };
    }

    public async Task DeactivateAsync(int id)
    {
        var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if (category != null)
        {
            category.IsActive = false;
            category.UpdatedAt = DateTime.UtcNow;
            context.Categories.Update(category);
        }
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdateCategoryDto updateCategoryDto)
    {
        //Update Category
        var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == updateCategoryDto.Id);
        if (category != null)
        {
            category.Name = updateCategoryDto.Name;
            category.Description = updateCategoryDto.Description;
            category.UpdatedAt = DateTime.UtcNow;
            context.Categories.Update(category);
        }
    }
}
