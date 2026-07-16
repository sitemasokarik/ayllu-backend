namespace DcodePe.Catering.Application.DataBase.Pagina.Commands.Update
{
    public class UpdatePaginaModel
    {
        public int PaginaID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Url { get; set; }
        public string Icono { get; set; }
        public string? GrupoMenu { get; set; }
        public int? OrdenMenu { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
