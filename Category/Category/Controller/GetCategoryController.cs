namespace Category.Controller;

internal class GetCategoryController(IGetCategoryInputPort inputPort) : IGetCategoryController
{
    public async Task<HandlerRequestResult<CategoryDto>> GetByIdAsync(int id)
    {
        HandlerRequestResult<CategoryDto> result = default;
        try
        {
            var categoryResult = await inputPort.GetByIdAsync(id);
            result = new HandlerRequestResult<CategoryDto>(categoryResult);
        }
        catch (Exception ex)
        {
            result = new HandlerRequestResult<CategoryDto>(ex.Message);
        }
        return result;
    }

    public async Task<HandlerRequestResult<IEnumerable<CategoryDto>>> GetAllActiveAsync()
    {
        HandlerRequestResult<IEnumerable<CategoryDto>> result = default;
        try
        {
            var categoriesResult = await inputPort.GetAllActiveAsync();
            result = new HandlerRequestResult<IEnumerable<CategoryDto>>(categoriesResult);
        }
        catch (Exception ex)
        {
            result = new HandlerRequestResult<IEnumerable<CategoryDto>>(ex.Message);
        }
        return result;
    }
}