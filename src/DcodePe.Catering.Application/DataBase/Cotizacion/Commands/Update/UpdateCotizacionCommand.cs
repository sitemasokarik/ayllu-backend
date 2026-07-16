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
            entity.BorradorJson = model.BorradorJson;
            entity.UsuarioModificacion = model.UsuarioModificacion ?? "SYSTEM";
            entity.FechaModificacion = DateTime.Now;

            // Actualizar productos
            if (model.CotizacionProducto != null)
            {
                var productosExistentes = entity.CotizacionProducto.ToList();
                if (productosExistentes.Count > 0)
                {
                    _databaseService.CotizacionProducto.RemoveRange(productosExistentes);
                    entity.CotizacionProducto.Clear();
                    await _databaseService.SaveAsync();
                }

                var productosDistintos = model.CotizacionProducto
                    .Where(p => p.ProductoID > 0)
                    .GroupBy(p => p.ProductoID)
                    .Select(g => g.First())
                    .ToList();

                if (productosDistintos.Count > 0)
                {
                    var nuevosProductos = productosDistintos.Select(productoModel => new CotizacionProductoEntity
                    {
                        CotizacionID = entity.CotizacionID,
                        ProductoID = productoModel.ProductoID,
                        Cantidad = productoModel.Cantidad > 0 ? productoModel.Cantidad : 1,
                        UsuarioCreacion = model.UsuarioModificacion ?? "SYSTEM",
                        FechaCreacion = DateTime.Now,
                        Estado = true
                    }).ToList();

                    await _databaseService.CotizacionProducto.AddRangeAsync(nuevosProductos);
                }
            }

            // Actualizar servicios
            if (model.CotizacionServicio != null)
            {
                var serviciosExistentes = entity.CotizacionServicio.ToList();
                if (serviciosExistentes.Count > 0)
                {
                    _databaseService.CotizacionServicio.RemoveRange(serviciosExistentes);
                    entity.CotizacionServicio.Clear();
                    await _databaseService.SaveAsync();
                }

                var serviciosDistintos = model.CotizacionServicio
                    .Where(s => s.ServicioID > 0)
                    .GroupBy(s => s.ServicioID)
                    .Select(g => g.First())
                    .ToList();

                if (serviciosDistintos.Count > 0)
                {
                    var nuevosServicios = serviciosDistintos.Select(servicioModel => new CotizacionServicioEntity
                    {
                        CotizacionID = entity.CotizacionID,
                        ServicioID = servicioModel.ServicioID,
                        Cantidad = servicioModel.Cantidad > 0 ? servicioModel.Cantidad : 1,
                        UsuarioCreacion = model.UsuarioModificacion ?? "SYSTEM",
                        FechaCreacion = DateTime.Now,
                        Estado = true
                    }).ToList();

                    await _databaseService.CotizacionServicio.AddRangeAsync(nuevosServicios);
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
