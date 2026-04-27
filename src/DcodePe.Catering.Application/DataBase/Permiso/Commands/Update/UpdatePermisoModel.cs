namespace DcodePe.Catering.Application.DataBase.Permiso.Commands.Update
{
    public class UpdatePermisoModel
    {
        public int PermisoID { get; set; }
        public int RolID { get; set; }
        public int PaginaID { get; set; }
        public bool PuedeVer { get; set; }
        public bool PuedeCrear { get; set; }
        public bool PuedeEditar { get; set; }
        public bool PuedeEliminar { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
