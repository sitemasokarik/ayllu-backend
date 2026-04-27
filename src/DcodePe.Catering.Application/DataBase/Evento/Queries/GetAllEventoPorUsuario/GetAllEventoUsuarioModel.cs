using DcodePe.Catering.Application.DataBase.Local.Queries.GetAllLocal;
using DcodePe.Catering.Application.DataBase.Usuario.Queries.GetAllUsuario;
using DcodePe.Catering.Domain.Entities;

namespace DcodePe.Catering.Application.DataBase.Evento.Queries.GetAllEventoPorUsuario
{
    public class GetAllEventoUsuarioModel
    {
        public int UsuarioID { get; set; }

        public string Nombre { get; set; }

        public string Email { get; set; }

        public virtual List<GetAllEventoModel> Evento { get; set; }

    }
}
