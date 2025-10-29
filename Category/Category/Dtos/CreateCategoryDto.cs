namespace Category.Dtos;

public class CreateCategoryDto(string name, string description)
{
    public string Name => name;
    public string Description => description;
}