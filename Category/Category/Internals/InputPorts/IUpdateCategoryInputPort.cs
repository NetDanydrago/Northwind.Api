namespace Category.Internals.InputPorts;
internal interface IUpdateCategoryInputPort
{
    Task<CategoryDto> UpdateAsync(UpdateCategoryDto updateCategoryDto);
}