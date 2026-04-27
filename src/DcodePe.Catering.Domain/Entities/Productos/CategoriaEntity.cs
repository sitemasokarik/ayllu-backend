using DcodePe.Catering.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace DcodePe.Catering.Domain.Entities;

/// <summary>
/// Entidad que representa una categoría de productos con soporte jerárquico
/// </summary>
public partial class CategoriaEntity : BaseEntity
{
    /// <summary>
    /// Identificador único de la categoría
    /// </summary>
    public int CategoriaID { get; set; }

    /// <summary>
    /// ID de la categoría padre (null si es categoría raíz)
    /// </summary>
    public int? CategoriaPadreID { get; set; }

    /// <summary>
    /// Nombre de la categoría (ej: Entradas, Platos Principales, Postres, Bebidas)
    /// </summary>
    public string Nombre { get; set; }

    /// <summary>
    /// Descripción detallada de la categoría
    /// </summary>
    public string Descripcion { get; set; }

    /// <summary>
    /// Nivel de profundidad en la jerarquía (0 = raíz, 1 = hijo directo, 2 = nieto, etc.)
    /// Facilita consultas y previene jerarquías muy profundas
    /// </summary>
    public int Nivel { get; set; }

    /// <summary>
    /// Orden de visualización dentro del mismo nivel
    /// </summary>
    public int Orden { get; set; }

    /// <summary>
    /// Icono o ruta de imagen representativa de la categoría
    /// </summary>
    public string? Icono { get; set; }

    public int Limite { get; set; }

    // ===== NAVEGACIONES PARA JERARQUÍA =====

    /// <summary>
    /// Categoría padre (null si es categoría raíz)
    /// </summary>
    public virtual CategoriaEntity? CategoriaPadre { get; set; }

    /// <summary>
    /// Subcategorías hijas de esta categoría (permite recursividad)
    /// </summary>
    public virtual ICollection<CategoriaEntity> Subcategorias { get; set; } = new List<CategoriaEntity>();

    /// <summary>
    /// Colección de productos que pertenecen directamente a esta categoría
    /// Productos pueden estar en cualquier nivel de la jerarquía
    /// </summary>
    public virtual ICollection<ProductoEntity> Productos { get; set; } = new List<ProductoEntity>();

    /// <summary>
    /// Propiedad calculada que indica si esta categoría es una hoja (no tiene subcategorías)
    /// </summary>
    [NotMapped]
    public bool EsHoja => Subcategorias == null || !Subcategorias.Any();
}
