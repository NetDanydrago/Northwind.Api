namespace Category.Internals.Controllers;
internal interface ICreateCategoryController
{
    Task<HandlerRequestResult<CategoryDto>> CreateAsync(CreateCategoryDto createCategoryDto);
}