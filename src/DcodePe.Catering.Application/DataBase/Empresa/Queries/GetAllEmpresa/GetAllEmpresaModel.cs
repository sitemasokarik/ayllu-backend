using System;

namespace DcodePe.Catering.Application.DataBase.Empresa.Queries.GetAllEmpresa
{
    public class GetAllEmpresaModel
    {
        public int EmpresaID { get; set; }
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public string RUC { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string TelefonoSecundario { get; set; }
        public string WhatsApp { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Pais { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string LinkedIn { get; set; }
        public string Twitter { get; set; }
        public string HorarioAtencion { get; set; }
        public string Logo { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioEliminacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        public bool? Estado { get; set; }
    }
}
