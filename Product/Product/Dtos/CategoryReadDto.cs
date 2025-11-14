namespace Product.Dtos;
public class CategoryReadDto(int id, string name, string description, bool isActive)
{
    public int Id => id;
    public string Name => name;
    public string Description => description;
    public bool IsActive => isActive;
}
