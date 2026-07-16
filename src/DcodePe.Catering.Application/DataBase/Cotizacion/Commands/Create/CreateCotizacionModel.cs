using System;
using System.Collections.Generic;

namespace DcodePe.Catering.Application.DataBase.Cotizacion.Commands.Create
{
    public class CreateCotizacionModel
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
        public string? Observacion { get; set; }
        public string EstadoCotizacion { get; set; }
        public string OrigenCotizacion { get; set; } = "Admin";
        public string? BorradorJson { get; set; }
        public int? CreadoPorUsuarioID { get; set; }
        public string? CreadoPorNombre { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Estado { get; set; }
        public virtual List<CreateCotizacionProductoModel> CotizacionProducto { get; set; }
        public virtual List<CreateCotizacionServicioModel> CotizacionServicio { get; set; }
        //public virtual List<CreateCotizacionPaqueteModel> CotizacionPaquete { get; set; }
    }
}
