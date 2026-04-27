using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Evento.Commands.Create
{
    public interface ICreateEventoCommand
    {
        Task<CreateEventoModel> ExecuteSaveEvento(CreateEventoModel model);
    }
}
