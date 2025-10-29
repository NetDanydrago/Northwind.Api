namespace Category.Internals.InputPorts;
internal interface IGetCategoryInputPort
{
    Task<CategoryDto> GetByIdAsync(int id);
    Task<IEnumerable<CategoryDto>> GetAllActiveAsync();
}