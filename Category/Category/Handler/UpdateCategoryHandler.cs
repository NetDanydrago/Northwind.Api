namespace Category.Handler;

internal class UpdateCategoryHandler(
    ICommandCategoryRepository categoryRepository,
    IQueryableCategoryRepository queryCategoryRepository,
    ILogger<UpdateCategoryHandler> logger) : IUpdateCategoryInputPort
{
    public async Task<CategoryDto> UpdateAsync(UpdateCategoryDto updateCategoryDto)
    {
        try
        {
            logger.LogInformation("Attempting to update category: {Id}", updateCategoryDto.Id);

            // Validate if category exists
            var existingCategory = await queryCategoryRepository.GetByIdAsync(updateCategoryDto.Id);
            if (existingCategory == null)
            {
                throw new Exception(CategoryMessages.CategoryNotFound);
            }

            if (!existingCategory.IsActive)
            {
                throw new Exception(CategoryMessages.CategoryNotActive);
            }

            // Validate if new name already exists (excluding current category)
            var categoryWithSameName = await queryCategoryRepository.GetByNameAsync(updateCategoryDto.Name);
            if (categoryWithSameName != null && categoryWithSameName.Id != updateCategoryDto.Id)
            {
                throw new Exception(CategoryMessages.CategoryNameAlreadyExists);
            }

            await using var scope = new DomainTransactionScope();
            await scope.EnlistAsync(categoryRepository);
            
            await categoryRepository.UpdateAsync(updateCategoryDto);
            await categoryRepository.SaveChangesAsync();
            
            scope.Complete();

            // Get updated category
            var updatedCategory = await queryCategoryRepository.GetByIdAsync(updateCategoryDto.Id);

            logger.LogInformation("Successfully updated category: {Id}", updateCategoryDto.Id);
            return updatedCategory;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating category: {Id}", updateCategoryDto.Id);
            throw;
        }
    }
}