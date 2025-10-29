namespace Category.Internals.InputPorts;
internal interface IDeactivateCategoryInputPort
{
    Task DeactivateAsync(DeactivateCategoryDto deactivateCategoryDto);
}