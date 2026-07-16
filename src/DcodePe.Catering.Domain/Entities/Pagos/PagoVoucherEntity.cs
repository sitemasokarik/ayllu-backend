using DcodePe.Catering.Domain.Entities;
using DcodePe.Catering.Domain.Entities.Clientes;

namespace DcodePe.Catering.Domain.Entities.Pagos
{
    public class PagoVoucherEntity
    {
        public int PagoVoucherID { get; set; }
        public int CotizacionID { get; set; }
        public int ClienteID { get; set; }
        public string ArchivoUrl { get; set; } = string.Empty;
        public string NombreArchivo { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public DateTime? FechaReservadaElegida { get; set; }
        public string EstadoPago { get; set; } = "Pendiente";
        public string? ObservacionCliente { get; set; }
        public string? ObservacionAdmin { get; set; }
        public string? UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Estado { get; set; }

        public virtual CotizacionEntity Cotizacion { get; set; } = null!;
        public virtual ClienteEntity Cliente { get; set; } = null!;
    }
}
