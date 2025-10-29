namespace Category.Internals.Controllers;
internal interface IUpdateCategoryController
{
    Task<HandlerRequestResult<CategoryDto>> UpdateAsync(UpdateCategoryDto updateCategoryDto);
}