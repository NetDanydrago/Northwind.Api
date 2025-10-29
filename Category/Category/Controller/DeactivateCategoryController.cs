namespace Category.Controller;

internal class DeactivateCategoryController(IDeactivateCategoryInputPort inputPort) : IDeactivateCategoryController
{
    public async Task<HandlerRequestResult> DeactivateAsync(DeactivateCategoryDto deactivateCategoryDto)
    {
        HandlerRequestResult result = default;
        try
        {
            await inputPort.DeactivateAsync(deactivateCategoryDto);
            result = new HandlerRequestResult();
        }
        catch (Exception ex)
        {
            result = new HandlerRequestResult(ex.Message);
        }
        return result;
    }
}