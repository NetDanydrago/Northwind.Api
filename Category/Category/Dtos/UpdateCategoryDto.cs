namespace Category.Dtos;

public class UpdateCategoryDto(int id, string name, string description)
{
    public int Id => id;
    public string Name => name;
    public string Description => description;
}