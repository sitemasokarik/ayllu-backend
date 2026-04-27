using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Local.Commands.CreateLocal
{
    public interface ICreateLocalCommand
    {
        Task<CreateLocalModel> ExecuteSaveLocal(CreateLocalModel model);
    }
}
