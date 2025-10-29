namespace Category.Internals.Controllers;
internal interface IDeactivateCategoryController
{
    Task<HandlerRequestResult> DeactivateAsync(DeactivateCategoryDto deactivateCategoryDto);
}