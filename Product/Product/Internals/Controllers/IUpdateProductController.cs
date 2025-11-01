using Product.Dtos;
using Product.Dtos.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Internals.Controllers;

internal interface IUpdateProductController
{
    Task<HandlerRequestResult<ProductDto>> UpdateProductAsync(UpdateProductDto updateProductDto);
}
