namespace Category.Internals.Controllers;
internal interface IGetCategoryController
{
    Task<HandlerRequestResult<CategoryDto>> GetByIdAsync(int id);
    Task<HandlerRequestResult<IEnumerable<CategoryDto>>> GetAllActiveAsync();
}