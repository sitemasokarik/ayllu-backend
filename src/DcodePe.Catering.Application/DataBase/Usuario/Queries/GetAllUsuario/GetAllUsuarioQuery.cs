using DcodePe.Catering.Application.DataBase.Local.Queries.GetAllLocal;

namespace DcodePe.Catering.Application.DataBase.Usuario.Queries.GetAllUsuario
{
    public class GetAllUsuarioQuery(IDataBaseService databaseService) : IGetAllUsuarioQuery
    {
        private readonly IDataBaseService _databaseService = databaseService;
        public async Task<List<GetAllUsuarioModel>> ExecuteListUsuario()
        {
            var result = await _databaseService.Usuario.Where(x => x.Estado == true)
                .Select(Usuario => new GetAllUsuarioModel()
                {
                    UsuarioID = Usuario.UsuarioID,
                    Nombre = Usuario.Nombre,
                    Email = Usuario.Email,
                    Password = Usuario.Password,
                    RolID = Usuario.RolID,
                    UsuarioCreacion = Usuario.UsuarioCreacion,
                    FechaCreacion = Usuario.FechaCreacion,
                    UsuarioModificacion = Usuario.UsuarioModificacion,
                    FechaModificacion = Usuario.FechaModificacion,
                    UsuarioEliminacion = Usuario.UsuarioEliminacion,
                    FechaEliminacion = Usuario.FechaEliminacion,
                    Estado = Usuario.Estado,
                    //Evento = Usuario.Evento.Select(Evento => new GetAllEventoModel()
                    //{
                    //    EventoID = Evento.EventoID,
                    //    Nombre = Evento.Nombre,
                    //    Descripcion = Evento.Descripcion,
                    //    Fecha = Evento.Fecha,
                    //    LocalID = Evento.LocalID,
                    //    UsuarioID = Evento.UsuarioID,
                    //    EstadoEvento = Evento.EstadoEvento,
                    //    UsuarioCreacion = Evento.UsuarioCreacion,
                    //    FechaCreacion = Evento.FechaCreacion,
                    //    UsuarioModificacion = Evento.UsuarioModificacion,
                    //    FechaModificacion = Evento.FechaModificacion,
                    //    UsuarioEliminacion = Evento.UsuarioEliminacion,
                    //    FechaEliminacion = Evento.FechaEliminacion,
                    //    Estado = Evento.Estado,
                    //    Local = new GetAllLocalModel()
                    //    {
                    //        LocalID = Evento.Local.LocalID,
                    //        Nombre = Evento.Local.Nombre,
                    //        Direccion = Evento.Local.Direccion,
                    //        Capacidad = Evento.Local.Capacidad,
                    //        Fotos = Evento.Local.Fotos,
                    //        TerminosCondiciones = Evento.Local.TerminosCondiciones,
                    //        UsuarioCreacion = Evento.Local.UsuarioCreacion,
                    //        FechaCreacion = Evento.Local.FechaCreacion,
                    //        UsuarioModificacion = Evento.Local.UsuarioModificacion,
                    //        FechaModificacion = Evento.Local.FechaModificacion,
                    //        UsuarioEliminacion = Evento.Local.UsuarioEliminacion,
                    //        FechaEliminacion = Evento.Local.FechaEliminacion,
                    //        Estado = Evento.Local.Estado
                    //    }
                    //}).ToList()
                })
                .AsNoTracking()
                .ToListAsync();

            return result;
        }
    }
}
