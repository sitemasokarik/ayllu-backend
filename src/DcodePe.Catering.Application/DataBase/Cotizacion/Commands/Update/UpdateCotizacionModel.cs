using System;
using System.Collections.Generic;

namespace DcodePe.Catering.Application.DataBase.Cotizacion.Commands.Update
{
    public class UpdateCotizacionModel
    {
        public int CotizacionID { get; set; }
        public int ClienteID { get; set; }
        public int LocalID { get; set; }
        public int? EventoID { get; set; }
        public DateTime? FechaTentativa { get; set; }
        public DateTime? FechaTentativaOpcional { get; set; }
        public DateTime? FechaContacto { get; set; }
        public TimeOnly? HoraContacto { get; set; }
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
        public string UsuarioModificacion { get; set; }
        public virtual List<UpdateCotizacionProductoModel> CotizacionProducto { get; set; }
        public virtual List<UpdateCotizacionServicioModel> CotizacionServicio { get; set; }
        //public virtual List<UpdateCotizacionPaqueteModel> CotizacionPaquete { get; set; }
    }

    public class UpdateCotizacionProductoModel
    {
        public int ProductoID { get; set; }
        public int Cantidad { get; set; }
    }

    public class UpdateCotizacionServicioModel
    {
        public int ServicioID { get; set; }
        public int Cantidad { get; set; }
    }

    //public class UpdateCotizacionPaqueteModel
    //{
    //    public int PaqueteID { get; set; }
    //    public int Cantidad { get; set; }
    //}
}
