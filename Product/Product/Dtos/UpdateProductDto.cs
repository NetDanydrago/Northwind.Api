using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Dtos;

public class UpdateProductDto(int id, string name, string description, int categoryId)
{
    public int Id => id;
    public string Name => name;
    public string Description => description;
    public int CategoryId => categoryId;

}
