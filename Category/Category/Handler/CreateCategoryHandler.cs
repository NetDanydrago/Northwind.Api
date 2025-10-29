namespace Category.Handler;

internal class CreateCategoryHandler(
    ICommandCategoryRepository categoryRepository,
    IQueryableCategoryRepository queryCategoryRepository,
    ILogger<CreateCategoryHandler> logger) : ICreateCategoryInputPort
{
    public async Task<CategoryDto> CreateAsync(CreateCategoryDto createCategoryDto)
    {
        try
        {
            logger.LogInformation("Attempting to create category: {Name}", createCategoryDto.Name);

            // Validate if category name already exists
            var existingCategory = await queryCategoryRepository.GetByNameAsync(createCategoryDto.Name);
            if (existingCategory != null)
            {
                throw new Exception(CategoryMessages.CategoryNameAlreadyExists);
            }

            await using var scope = new DomainTransactionScope();
            await scope.EnlistAsync(categoryRepository);
            
            var newCategory = await categoryRepository.CreateAsync(createCategoryDto);
            await categoryRepository.SaveChangesAsync();
            
            scope.Complete();

            logger.LogInformation("Successfully created category: {Name} with ID: {Id}", newCategory.Name, newCategory.Id);
            return newCategory;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating category: {Name}", createCategoryDto.Name);
            throw;
        }
    }
}