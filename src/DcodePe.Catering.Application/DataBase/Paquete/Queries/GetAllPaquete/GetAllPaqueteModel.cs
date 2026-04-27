using DcodePe.Catering.Application.DataBase.PaqueteProducto.Queries.GetAllPaqueteProducto;
using DcodePe.Catering.Application.DataBase.PaqueteServicio.Queries.GetAllPaqueteServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Paquete.Queries.GetAllPaquete
{
    public class GetAllPaqueteModel
    {
        public int PaqueteID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioTotal { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioEliminacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        public bool? Estado { get; set; }
        public virtual ICollection<GetAllPaqueteProductoModel> PaqueteProducto { get; set; }
        public virtual ICollection<GetAllPaqueteServicioModel> PaqueteServicio { get; set; }
    }
}
