namespace DcodePe.Catering.Application.DataBase.Rol.Queries.GetById
{
    public class GetRolByIdModel
    {
        public int RolID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Estado { get; set; }
        public List<PermisoResumenModel> Permisos { get; set; } = new List<PermisoResumenModel>();
    }

    public class PermisoResumenModel
    {
        public int PermisoID { get; set; }
        public int PaginaID { get; set; }
        public string PaginaNombre { get; set; }
        public string Url { get; set; }
        public string Icono { get; set; }
        public string? GrupoMenu { get; set; }
        public int? OrdenMenu { get; set; }
        public bool PuedeVer { get; set; }
        public bool PuedeCrear { get; set; }
        public bool PuedeEditar { get; set; }
        public bool PuedeEliminar { get; set; }
    }
}
