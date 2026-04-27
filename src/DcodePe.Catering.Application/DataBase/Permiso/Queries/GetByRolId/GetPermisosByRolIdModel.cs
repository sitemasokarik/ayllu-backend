namespace DcodePe.Catering.Application.DataBase.Permiso.Queries.GetByRolId
{
    public class GetPermisosByRolIdModel
    {
        public int PermisoID { get; set; }
        public int RolID { get; set; }
        public int PaginaID { get; set; }
        public string PaginaNombre { get; set; }
        public string PaginaDescripcion { get; set; }
        public bool PuedeVer { get; set; }
        public bool PuedeCrear { get; set; }
        public bool PuedeEditar { get; set; }
        public bool PuedeEliminar { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Estado { get; set; }
    }
}
