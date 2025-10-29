namespace Category.Controller;

internal class CreateCategoryController(ICreateCategoryInputPort inputPort) : ICreateCategoryController
{
    public async Task<HandlerRequestResult<CategoryDto>> CreateAsync(CreateCategoryDto createCategoryDto)
    {
        HandlerRequestResult<CategoryDto> result = default;
        try
        {
            var categoryResult = await inputPort.CreateAsync(createCategoryDto);
            result = new HandlerRequestResult<CategoryDto>(categoryResult);
        }
        catch (Exception ex)
        {
            result = new HandlerRequestResult<CategoryDto>(ex.Message);
        }
        return result;
    }
}