using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Usuario.Queries.GetAllUsuario
{
    public interface IGetAllUsuarioQuery
    {
        Task<List<GetAllUsuarioModel>> ExecuteListUsuario();
    }
}
