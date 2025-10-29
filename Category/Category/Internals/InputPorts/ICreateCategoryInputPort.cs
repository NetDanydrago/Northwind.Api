namespace Category.Internals.InputPorts;
internal interface ICreateCategoryInputPort
{
    Task<CategoryDto> CreateAsync(CreateCategoryDto createCategoryDto);
}