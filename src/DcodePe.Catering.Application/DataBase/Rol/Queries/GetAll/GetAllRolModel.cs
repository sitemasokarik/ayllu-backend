namespace DcodePe.Catering.Application.DataBase.Rol.Queries.GetAll
{
    public class GetAllRolModel
    {
        public int RolID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int TotalUsuarios { get; set; }
        public int TotalPermisos { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Estado { get; set; }
    }
}
