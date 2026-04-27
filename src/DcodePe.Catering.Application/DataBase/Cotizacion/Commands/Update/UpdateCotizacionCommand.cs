using DcodePe.Catering.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DcodePe.Catering.Application.DataBase.Cotizacion.Commands.Update
{
    public class UpdateCotizacionCommand : IUpdateCotizacionCommand
    {
        private readonly IDataBaseService _databaseService;

        public UpdateCotizacionCommand(IDataBaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<bool> Execute(UpdateCotizacionModel model)
        {
            var entity = await _databaseService.Cotizacion
                .Include(c => c.CotizacionProducto)
                .Include(c => c.CotizacionServicio)
                .Include(c => c.CotizacionPaquete)
                .FirstOrDefaultAsync(c => c.CotizacionID == model.CotizacionID);

            if (entity == null)
                return false;

            // Actualizar datos principales
            entity.ClienteID = model.ClienteID;
            entity.LocalID = model.LocalID;
            entity.EventoID = model.EventoID;
            entity.FechaTentativa = model.FechaTentativa;
            entity.FechaTentativaOpcional = model.FechaTentativaOpcional;
            entity.FechaContacto = model.FechaContacto;
            entity.HoraContacto = model.HoraContacto;
            entity.NumeroInvitados = model.NumeroInvitados;
            entity.CostoDePersonal = model.CostoDePersonal;
            entity.Garantia = model.Garantia;
            entity.TarifaMenuPorInvitado = model.TarifaMenuPorInvitado;
            entity.SubtotalMenu = model.SubtotalMenu;
            entity.TotalEvento = model.TotalEvento;
            entity.PrecioPorCubierto = model.PrecioPorCubierto;
            entity.PrecioPorCubiertoConDescuento = model.PrecioPorCubiertoConDescuento;
            entity.TotalCotizacion = model.TotalCotizacion;
            entity.Observacion = model.Observacion;
            entity.EstadoCotizacion = model.EstadoCotizacion;
            entity.UsuarioModificacion = model.UsuarioModificacion ?? "SYSTEM";
            entity.FechaModificacion = DateTime.Now;

            // Actualizar productos
            if (model.CotizacionProducto != null)
            {
                // Eliminar productos existentes
                _databaseService.CotizacionProducto.RemoveRange(entity.CotizacionProducto);

                // Agregar nuevos productos
                foreach (var productoModel in model.CotizacionProducto)
                {
                    var cotizacionProducto = new CotizacionProductoEntity
                    {
                        CotizacionID = entity.CotizacionID,
                        ProductoID = productoModel.ProductoID,
                        Cantidad = productoModel.Cantidad,
                        UsuarioCreacion = model.UsuarioModificacion ?? "SYSTEM",
                        FechaCreacion = DateTime.Now,
                        Estado = true
                    };
                    await _databaseService.CotizacionProducto.AddAsync(cotizacionProducto);
                }
            }

            // Actualizar servicios
            if (model.CotizacionServicio != null)
            {
                // Eliminar servicios existentes
                _databaseService.CotizacionServicio.RemoveRange(entity.CotizacionServicio);

                // Agregar nuevos servicios
                foreach (var servicioModel in model.CotizacionServicio)
                {
                    var cotizacionServicio = new CotizacionServicioEntity
                    {
                        CotizacionID = entity.CotizacionID,
                        ServicioID = servicioModel.ServicioID,
                        Cantidad = servicioModel.Cantidad,
                        UsuarioCreacion = model.UsuarioModificacion ?? "SYSTEM",
                        FechaCreacion = DateTime.Now,
                        Estado = true
                    };
                    await _databaseService.CotizacionServicio.AddAsync(cotizacionServicio);
                }
            }

            // Actualizar paquetes
            //if (model.CotizacionPaquete != null)
            //{
            //    // Eliminar paquetes existentes
            //    _databaseService.CotizacionPaquete.RemoveRange(entity.CotizacionPaquete);

            //    // Agregar nuevos paquetes
            //    foreach (var paqueteModel in model.CotizacionPaquete)
            //    {
            //        var cotizacionPaquete = new CotizacionPaqueteEntity
            //        {
            //            CotizacionID = entity.CotizacionID,
            //            PaqueteID = paqueteModel.PaqueteID,
            //            Cantidad = paqueteModel.Cantidad,
            //            UsuarioCreacion = model.UsuarioModificacion ?? "SYSTEM",
            //            FechaCreacion = DateTime.Now,
            //            Estado = true
            //        };
            //        await _databaseService.CotizacionPaquete.AddAsync(cotizacionPaquete);
            //    }
            //}

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
