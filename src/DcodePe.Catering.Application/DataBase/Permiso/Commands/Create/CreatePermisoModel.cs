namespace DcodePe.Catering.Application.DataBase.Permiso.Commands.Create
{
    public class CreatePermisoModel
    {
        public int RolID { get; set; }
        public int PaginaID { get; set; }
        public bool PuedeVer { get; set; }
        public bool PuedeCrear { get; set; }
        public bool PuedeEditar { get; set; }
        public bool PuedeEliminar { get; set; }
        public string UsuarioCreacion { get; set; }
    }
}
