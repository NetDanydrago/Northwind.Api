namespace Category.Interfaces;

public interface IQueryableCategoryRepository
{
    Task<CategoryDto> GetByIdAsync(int id);
    Task<IEnumerable<CategoryDto>> GetAllActiveAsync();
    Task<CategoryDto> GetByNameAsync(string name);
}