#nullable disable
using System;

namespace DcodePe.Catering.Domain.Entities
{
    public partial class EmpresaEntity
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

        public string? BancoNombre { get; set; }
        public string? NumeroCuenta { get; set; }
        public string? Cci { get; set; }
        public string? YapeNumero { get; set; }
        public string? PlinNumero { get; set; }
        public string? QrPagoUrl { get; set; }
        public string? InstruccionesPago { get; set; }
        public string? CuentasPagoJson { get; set; }
        public decimal? MontoAdelantoReserva { get; set; }

        public bool? GeneraFactElect { get; set; }
        public string? Ubigeo { get; set; }
        public string? RutaCertificadoServidor { get; set; }
        public string? CertificadoFileName { get; set; }
        public string? ClaveCertificado { get; set; }
        public string? UsuarioSol { get; set; }
        public string? ClaveSol { get; set; }
        public string? SunatModo { get; set; }
        public string? SunatWsProduccion { get; set; }
        public string? ApiPeruDevToken { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string UsuarioEliminacion { get; set; }

        public DateTime? FechaEliminacion { get; set; }

        public bool? Estado { get; set; }
    }
}
