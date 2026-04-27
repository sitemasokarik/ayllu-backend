using DcodePe.Catering.Domain.Entities.Base;

namespace DcodePe.Catering.Domain.Entities.Clientes
{
    /// <summary>
    /// Entidad que representa un cliente del sistema de catering
    /// </summary>
    public partial class ClienteEntity : BaseEntity
    {
        /// <summary>
        /// Identificador único del cliente
        /// </summary>
        public int ClienteID { get; set; }

        /// <summary>
        /// Tipo de documento (DNI, RUC, Pasaporte, Carnet de Extranjería)
        /// </summary>
        public string TipoDocumento { get; set; }

        /// <summary>
        /// Número de documento de identidad
        /// </summary>
        public string NumeroDocumento { get; set; }

        /// <summary>
        /// Nombre completo o razón social del cliente
        /// </summary>
        public string NombreCompleto { get; set; }

        /// <summary>
        /// Correo electrónico del cliente
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Número de teléfono principal
        /// </summary>
        public string? Telefono { get; set; }

        /// <summary>
        /// Número de teléfono secundario o celular
        /// </summary>
        public string? TelefonoSecundario { get; set; }

        /// <summary>
        /// Dirección completa del cliente
        /// </summary>
        public string? Direccion { get; set; }

        /// <summary>
        /// Ciudad del cliente
        /// </summary>
        public string? Ciudad { get; set; }

        /// <summary>
        /// País del cliente
        /// </summary>
        public string? Pais { get; set; }

        /// <summary>
        /// Tipo de cliente (Particular, Empresa, Gobierno, etc.)
        /// </summary>
        public string TipoCliente { get; set; }

        /// <summary>
        /// Observaciones o notas adicionales sobre el cliente
        /// </summary>
        public string? Observaciones { get; set; }

        /// <summary>
        /// Indica si el cliente está en lista VIP
        /// </summary>
        public bool EsVIP { get; set; }

        /// <summary>
        /// Fecha de nacimiento o fecha de constitución (para empresas)
        /// </summary>
        public DateTime? FechaNacimiento { get; set; }

        // Navegación a Cotizaciones
        /// <summary>
        /// Cotizaciones realizadas por el cliente
        /// </summary>
        public virtual ICollection<CotizacionEntity> Cotizaciones { get; set; } = new List<CotizacionEntity>();
    }
}
