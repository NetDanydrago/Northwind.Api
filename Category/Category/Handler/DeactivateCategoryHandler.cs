namespace Category.Handler;

internal class DeactivateCategoryHandler(
    ICommandCategoryRepository categoryRepository,
    IQueryableCategoryRepository queryCategoryRepository,
    ILogger<DeactivateCategoryHandler> logger) : IDeactivateCategoryInputPort
{
    public async Task DeactivateAsync(DeactivateCategoryDto deactivateCategoryDto)
    {
        try
        {
            logger.LogInformation("Attempting to deactivate category: {Id}", deactivateCategoryDto.Id);

            // Validate if category exists
            var existingCategory = await queryCategoryRepository.GetByIdAsync(deactivateCategoryDto.Id);
            if (existingCategory == null)
            {
                throw new Exception(CategoryMessages.CategoryNotFound);
            }

            if (!existingCategory.IsActive)
            {
                throw new Exception(CategoryMessages.CategoryAlreadyInactive);
            }

            await using var scope = new DomainTransactionScope();
            await scope.EnlistAsync(categoryRepository);
            
            await categoryRepository.DeactivateAsync(deactivateCategoryDto.Id);
            await categoryRepository.SaveChangesAsync();
            
            scope.Complete();

            logger.LogInformation("Successfully deactivated category: {Id}", deactivateCategoryDto.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deactivating category: {Id}", deactivateCategoryDto.Id);
            throw;
        }
    }
}