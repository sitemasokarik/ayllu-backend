using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Empresa.Commands.Delete
{
    public class DeleteEmpresaCommand : IDeleteEmpresaCommand
    {
        private readonly IDataBaseService _databaseService;

        public DeleteEmpresaCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<bool> Execute(int empresaId, string usuarioEliminacion)
        {
            var entity = await _databaseService.Empresa
                .FirstOrDefaultAsync(e => e.EmpresaID == empresaId);

            if (entity == null)
                return false;

            entity.Estado = false;
            entity.UsuarioEliminacion = usuarioEliminacion ?? "SYSTEM";
            entity.FechaEliminacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
