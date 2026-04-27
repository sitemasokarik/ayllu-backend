using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Cotizacion.Queries.GetAllCotizacion
{
    public class GetAllCotizacionModel
    {

        public int CotizacionID { get; set; }

        public int ClienteID { get; set; }

        public int LocalID { get; set; }

        public int? EventoID { get; set; }

        public DateTime? FechaTentativa { get; set; }

        public DateTime? FechaTentativaOpcional { get; set; }

        public DateTime? FechaContacto { get; set; }

        public TimeOnly? HoraContacto { get; set; }

        public string LocalNombre { get; set; }

        public string LocalDireccion { get; set; }

        public int LocalCapacidad { get; set; }

        public decimal LocalPrecioAlquiler { get; set; }

        public decimal LocalHorasEvento { get; set; }

        public string ClienteNombre { get; set; }

        public string ClienteDocumento { get; set; }

        public string ClienteTelefono { get; set; }

        public string ClienteEmail { get; set; }

        public string EventoNombre { get; set; }

        public string EventoDescripcion { get; set; }

        public int NumeroInvitados { get; set; }

        public decimal CostoDePersonal { get; set; }

        public decimal Garantia { get; set; }

        public decimal TarifaMenuPorInvitado { get; set; }

        public decimal SubtotalMenu { get; set; }

        public decimal TotalEvento { get; set; }

        public decimal PrecioPorCubierto { get; set; }

        public decimal PrecioPorCubiertoConDescuento { get; set; }

        public decimal TotalCotizacion { get; set; }

        public string Observacion { get; set; }

        public string EstadoCotizacion { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string UsuarioEliminacion { get; set; }

        public DateTime? FechaEliminacion { get; set; }

        public bool? Estado { get; set; }

        public virtual List<GetAllCotizacionProductoModel> CotizacionProducto { get; set; }
        public virtual List<GetAllCotizacionServicioModel> CotizacionServicio { get; set; }
        //public virtual List<GetAllCotizacionPaqueteModel> CotizacionPaquete { get; set; }
    }
}
