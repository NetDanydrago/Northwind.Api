namespace Product.Dtos;
public class ProductDto(int id, string name, string description, int categoryId , string categoryName)
{
    public int Id => id;
    public string Name => name;
    public string Description => description;
    public int CategoryId => categoryId;
    public string CategoryName => categoryName;
}
