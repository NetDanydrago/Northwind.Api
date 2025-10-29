using Category.Dtos;
using Category.Interfaces;
using Microsoft.EntityFrameworkCore;
using NorthWind.EFCore.Repositories.DbContexts;

namespace NorthWind.EFCore.Repositories.Repositories;
internal class QueryableCategoryRepository(QueryDbContext context) : IQueryableCategoryRepository
{
    public async Task<IEnumerable<CategoryDto>> GetAllActiveAsync()
    {
        var query = context.Categories.Select(s => new CategoryDto
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            IsActive = s.IsActive
        });
        return await query.ToListAsync();
    }

    public async Task<CategoryDto> GetByIdAsync(int id)
    {
        var query = context.Categories.Where(w => w.Id == id).Select(s => new CategoryDto
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            IsActive = s.IsActive
        });
        return await query.FirstOrDefaultAsync();
    }

    public async Task<CategoryDto> GetByNameAsync(string name)
    {
        var query = context.Categories.Where(w => w.Name == name).Select(s => new CategoryDto
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            IsActive = s.IsActive
        });
        return await query.FirstOrDefaultAsync();
    }
}
