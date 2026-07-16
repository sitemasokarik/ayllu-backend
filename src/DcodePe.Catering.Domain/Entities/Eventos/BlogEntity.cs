#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;

namespace DcodePe.Catering.Domain.Entities
{
    public partial class BlogEntity
    {
        public int BlogID { get; set; }

        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public string Resumen { get; set; }

        public string MisionTitulo { get; set; }

        public string MisionTexto { get; set; }

        public string VisionTitulo { get; set; }

        public string VisionTexto { get; set; }

        public string ValoresJson { get; set; }

        public string Imagenes { get; set; }

        public string LandingConfigJson { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string UsuarioEliminacion { get; set; }

        public DateTime? FechaEliminacion { get; set; }

        public bool? Estado { get; set; }

        [NotMapped]
        public List<ValorEmpresarial> Valores
        {
            get
            {
                if (string.IsNullOrEmpty(ValoresJson))
                    return new List<ValorEmpresarial>();
                
                try
                {
                    return JsonSerializer.Deserialize<List<ValorEmpresarial>>(ValoresJson);
                }
                catch
                {
                    return new List<ValorEmpresarial>();
                }
            }
            set
            {
                ValoresJson = value != null && value.Count > 0
                    ? JsonSerializer.Serialize(value)
                    : null;
            }
        }

        [NotMapped]
        public List<string> ImagenesUrls
        {
            get => string.IsNullOrEmpty(Imagenes)
                ? new List<string>()
                : Imagenes.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList();
            set => Imagenes = value != null && value.Any()
                ? string.Join(";", value)
                : null;
        }
    }

    public class ValorEmpresarial
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}