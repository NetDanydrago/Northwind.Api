namespace Category.Controller;

internal class UpdateCategoryController(IUpdateCategoryInputPort inputPort) : IUpdateCategoryController
{
    public async Task<HandlerRequestResult<CategoryDto>> UpdateAsync(UpdateCategoryDto updateCategoryDto)
    {
        HandlerRequestResult<CategoryDto> result = default;
        try
        {
            var categoryResult = await inputPort.UpdateAsync(updateCategoryDto);
            result = new HandlerRequestResult<CategoryDto>(categoryResult);
        }
        catch (Exception ex)
        {
            result = new HandlerRequestResult<CategoryDto>(ex.Message);
        }
        return result;
    }
}