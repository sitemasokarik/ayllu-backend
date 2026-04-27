namespace DcodePe.Catering.Domain.Entities.Base
{
    /// <summary>
    /// Entidad base con campos de auditoría comunes para todas las entidades del sistema
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Usuario que creó el registro
        /// </summary>
        public string UsuarioCreacion { get; set; }

        /// <summary>
        /// Fecha y hora de creación del registro
        /// </summary>
        public DateTime? FechaCreacion { get; set; }

        /// <summary>
        /// Usuario que modificó el registro por última vez
        /// </summary>
        public string? UsuarioModificacion { get; set; }

        /// <summary>
        /// Fecha y hora de la última modificación
        /// </summary>
        public DateTime? FechaModificacion { get; set; }

        /// <summary>
        /// Usuario que eliminó (soft delete) el registro
        /// </summary>
        public string? UsuarioEliminacion { get; set; }

        /// <summary>
        /// Fecha y hora de eliminación (soft delete)
        /// </summary>
        public DateTime? FechaEliminacion { get; set; }

        /// <summary>
        /// Estado del registro (true = activo, false = eliminado/inactivo)
        /// </summary>
        public bool? Estado { get; set; }
    }
}
