namespace Product.Dtos;
public class CreateProductDto(string name, string description, int categoryId)
{
    public string Name => name;
    public string Description => description;
    public int CategoryId => categoryId;
}
