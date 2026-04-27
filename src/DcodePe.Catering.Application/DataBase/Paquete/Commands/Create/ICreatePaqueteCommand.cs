using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Paquete.Commands.Create
{
    public interface ICreatePaqueteCommand
    {
        Task<CreatePaqueteModel> ExecuteSavePaquete(CreatePaqueteModel model);
    }
}
