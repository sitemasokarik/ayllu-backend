using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Usuario.Commands.Delete
{
    public class DeleteUsuarioCommand : IDeleteUsuarioCommand
    {
        private readonly IDataBaseService _databaseService;

        public DeleteUsuarioCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<bool> Execute(int usuarioId, string usuarioEliminacion)
        {
            var entity = await _databaseService.Usuario
                .FirstOrDefaultAsync(u => u.UsuarioID == usuarioId);

            if (entity == null)
                return false;

            // Soft delete
            entity.Estado = false;
            entity.UsuarioEliminacion = usuarioEliminacion ?? "SYSTEM";
            entity.FechaEliminacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
