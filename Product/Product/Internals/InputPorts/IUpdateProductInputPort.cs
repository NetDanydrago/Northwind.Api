using Product.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Internals.InputPorts;

internal interface IUpdateProductInputPort
{
    Task UpdateProductAsync(UpdateProductDto updateProductDto);
}
