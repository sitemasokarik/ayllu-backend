using DcodePe.Catering.Domain.Entities;
using DcodePe.Catering.Domain.Entities.Clientes;

namespace DcodePe.Catering.Domain.Entities.Pagos
{
    public class PagoMercadoPagoEntity
    {
        public int PagoMercadoPagoID { get; set; }
        public int CotizacionID { get; set; }
        public int ClienteID { get; set; }
        public decimal Monto { get; set; }
        public DateTime? FechaReservadaElegida { get; set; }
        public string MpPaymentId { get; set; } = string.Empty;
        public string MpPreferenceId { get; set; } = string.Empty;
        public string EstadoPago { get; set; } = "Pendiente";
        public string? MpStatusDetail { get; set; }
        public DateTime? FechaPago { get; set; }
        public string? UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Estado { get; set; }

        public virtual CotizacionEntity Cotizacion { get; set; } = null!;
        public virtual ClienteEntity Cliente { get; set; } = null!;
    }
}
