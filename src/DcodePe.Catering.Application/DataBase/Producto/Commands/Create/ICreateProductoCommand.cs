using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Producto.Commands.Create
{
    public interface ICreateProductoCommand
    {
        Task<CreateProductoModel> ExecuteSaveProducto(CreateProductoModel model);
    }
}
