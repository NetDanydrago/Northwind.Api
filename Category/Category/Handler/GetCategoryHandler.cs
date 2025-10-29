namespace Category.Handler;

internal class GetCategoryHandler(
    IQueryableCategoryRepository queryCategoryRepository,
    ILogger<GetCategoryHandler> logger) : IGetCategoryInputPort
{
    public async Task<CategoryDto> GetByIdAsync(int id)
    {
        try
        {
            logger.LogInformation("Attempting to get category by ID: {Id}", id);

            var category = await queryCategoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new Exception(CategoryMessages.CategoryNotFound);
            }

            logger.LogInformation("Successfully retrieved category: {Id}", id);
            return category;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting category by ID: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<CategoryDto>> GetAllActiveAsync()
    {
        try
        {
            logger.LogInformation("Attempting to get all active categories");

            var categories = await queryCategoryRepository.GetAllActiveAsync();

            logger.LogInformation("Successfully retrieved {Count} active categories", categories.Count());
            return categories;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting all active categories");
            throw;
        }
    }
}