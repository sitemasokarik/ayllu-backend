namespace DcodePe.Catering.Domain.Entities;

public partial class PaginaEntity
{
    /// <summary>Agrupa ítems del menú lateral (ej. Catálogo, Seguridad).</summary>
    public string? GrupoMenu { get; set; }

    /// <summary>Orden dentro del grupo o del menú.</summary>
    public int? OrdenMenu { get; set; }
}
