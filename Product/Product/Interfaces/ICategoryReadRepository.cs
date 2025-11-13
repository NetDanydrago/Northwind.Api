using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Interfaces;

public interface ICategoryReadRepository
{
    Task<bool> CategoryExistAsync(int categoryId);
    Task<bool> CategoryIsActivateAsync(int categoryId);
}
