namespace Category.Interfaces;
public interface ICommandCategoryRepository : ITransactionHandler
{
    Task<CategoryDto> CreateAsync(CreateCategoryDto createCategoryDto);
    Task UpdateAsync(UpdateCategoryDto updateCategoryDto);
    Task DeactivateAsync(int id);
    Task SaveChangesAsync();
}