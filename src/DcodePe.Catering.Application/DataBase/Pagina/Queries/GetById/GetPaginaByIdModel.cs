namespace DcodePe.Catering.Application.DataBase.Pagina.Queries.GetById
{
    public class GetPaginaByIdModel
    {
        public int PaginaID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Url { get; set; }
        public string Icono { get; set; }
        public string? GrupoMenu { get; set; }
        public int? OrdenMenu { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Estado { get; set; }
    }
}
