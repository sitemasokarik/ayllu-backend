using DcodePe.Catering.Application.DataBase.Comprobante.Commands.Create;
using DcodePe.Catering.Application.DataBase.Cotizacion.Queries.GetCotizacionFacturacion;
using DcodePe.Catering.Application.External.Sunat;
using DcodePe.Catering.Domain.Entities.Facturacion;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Comprobante.Commands.Create
{
    public class CreateComprobanteCommand(
        IDataBaseService databaseService,
        ISunatEmisionService sunatEmisionService,
        IGetCotizacionFacturacionQuery getCotizacionFacturacionQuery) : ICreateComprobanteCommand
    {
        private readonly IDataBaseService _databaseService = databaseService;
        private readonly ISunatEmisionService _sunatEmisionService = sunatEmisionService;
        private readonly IGetCotizacionFacturacionQuery _getCotizacionFacturacionQuery = getCotizacionFacturacionQuery;

        public async Task<CreateComprobanteModel> Execute(CreateComprobanteModel model)
        {
            if (!model.CotizacionID.HasValue)
                throw new InvalidOperationException("Debe seleccionar una cotización en estado Evento.");

            var facturacion = await _getCotizacionFacturacionQuery.Execute(model.CotizacionID.Value);

            if (Math.Abs(model.Total - facturacion.MontoAdelanto) > 0.01m)
                throw new InvalidOperationException(
                    $"El total del comprobante (S/ {model.Total:F2}) debe coincidir con el adelanto aprobado (S/ {facturacion.MontoAdelanto:F2}).");

            var serieDefault = model.Tipo.Equals("factura", StringComparison.OrdinalIgnoreCase) ? "F001" : "B001";
            var serieCodigo = string.IsNullOrWhiteSpace(model.Serie) ? serieDefault : model.Serie;

            var serie = await _databaseService.ComprobanteSerie
                .FirstOrDefaultAsync(s =>
                    s.Tipo == model.Tipo &&
                    s.Serie == serieCodigo);

            if (serie == null)
            {
                serie = new ComprobanteSerieEntity
                {
                    Tipo = model.Tipo,
                    Serie = serieCodigo,
                    UltimoCorrelativo = 0
                };

                await _databaseService.ComprobanteSerie.AddAsync(serie);
                await _databaseService.SaveAsync();
            }

            serie.UltimoCorrelativo++;
            var correlativo = serie.UltimoCorrelativo.ToString("D8");
            var numeroCompleto = $"{serieCodigo}-{correlativo}";

            var cotizacionId = model.CotizacionID.Value;
            var descripcionAdelanto = $"Adelanto reserva evento - Cot. #{cotizacionId}";

            if (model.Items == null || model.Items.Count == 0)
            {
                var baseAmount = Math.Round(model.Total / 1.18m, 2);
                var igv = Math.Round(model.Total - baseAmount, 2);
                model.Items =
                [
                    new CreateComprobanteItemModel
                    {
                        Item = 1,
                        Codigo = $"COT-{cotizacionId}",
                        Descripcion = descripcionAdelanto,
                        IdTipoIgv = "10",
                        TipoIgv = "Gravado - Operación Onerosa",
                        UnidadMedida = "ZZ",
                        Valor = baseAmount,
                        Cantidad = 1,
                        Subtotal = baseAmount,
                        Igv = igv,
                        Importe = model.Total
                    }
                ];
            }
            else if (model.Items.Count == 1)
            {
                model.Items[0].Descripcion = descripcionAdelanto;
            }

            var entity = new ComprobanteElectronicoEntity
            {
                Tipo = model.Tipo,
                Serie = serieCodigo,
                Correlativo = correlativo,
                NumeroCompleto = numeroCompleto,
                CotizacionID = model.CotizacionID,
                PagoVoucherID = facturacion.PagoVoucherID,
                PagoMercadoPagoID = facturacion.PagoMercadoPagoID,
                MontoAdelantoFacturado = facturacion.MontoAdelanto,
                ClienteNombre = model.ClienteNombre,
                ClienteDocumento = model.ClienteDocumento,
                TipoDocumento = model.TipoDocumento,
                ClienteDireccion = model.ClienteDireccion,
                ClienteTelefono = model.ClienteTelefono,
                FechaEmision = model.FechaEmision == default ? DateTime.Now : model.FechaEmision,
                FormaPago = model.FormaPago,
                MedioPago = model.MedioPago,
                Moneda = model.Moneda,
                OpGravadas = model.OpGravadas,
                OpInafectas = model.OpInafectas,
                OpExoneradas = model.OpExoneradas,
                Subtotal = model.Subtotal,
                Igv = model.Igv,
                Total = model.Total,
                Recibido = model.Recibido,
                Vuelto = model.Vuelto,
                ModoEmision = model.ModoEmision,
                EstadoComprobante = "Registrado",
                UsuarioCreacion = model.UsuarioCreacion ?? "SYSTEM",
                FechaCreacion = DateTime.Now,
                Estado = true
            };

            await _databaseService.ComprobanteElectronico.AddAsync(entity);
            await _databaseService.SaveAsync();

            foreach (var item in model.Items)
            {
                var detalle = new ComprobanteDetalleEntity
                {
                    ComprobanteID = entity.ComprobanteID,
                    Item = item.Item,
                    Codigo = item.Codigo,
                    Descripcion = item.Descripcion,
                    IdTipoIgv = item.IdTipoIgv,
                    TipoIgv = item.TipoIgv,
                    UnidadMedida = item.UnidadMedida,
                    Valor = item.Valor,
                    Cantidad = item.Cantidad,
                    Subtotal = item.Subtotal,
                    Igv = item.Igv,
                    Importe = item.Importe,
                    UsuarioCreacion = model.UsuarioCreacion ?? "SYSTEM",
                    FechaCreacion = DateTime.Now,
                    Estado = true
                };

                await _databaseService.ComprobanteDetalle.AddAsync(detalle);
            }

            await _databaseService.SaveAsync();

            if (model.ModoEmision.Equals("sunat", StringComparison.OrdinalIgnoreCase))
            {
                entity.Detalles = await _databaseService.ComprobanteDetalle
                    .Where(d => d.ComprobanteID == entity.ComprobanteID)
                    .ToListAsync();

                var sunatResult = await _sunatEmisionService.EmitirComprobanteAsync(entity);

                entity.SunatTicket = sunatResult.Ticket;
                entity.SunatRespuesta = sunatResult.Respuesta;
                entity.SunatCdr = sunatResult.SunatCdr;
                entity.SunatHashCpe = sunatResult.SunatHashCpe;
                entity.RutaXml = sunatResult.RutaXml;
                entity.RutaCdr = sunatResult.RutaCdr;
                entity.SunatCodigoError = sunatResult.SunatCodigoError;
                entity.EstadoComprobante = sunatResult.EstadoComprobante;

                await _databaseService.SaveAsync();
            }

            model.ComprobanteID = entity.ComprobanteID;
            model.Serie = entity.Serie;
            model.Correlativo = entity.Correlativo;
            model.NumeroCompleto = entity.NumeroCompleto;
            model.EstadoComprobante = entity.EstadoComprobante;
            model.SunatTicket = entity.SunatTicket;
            model.SunatRespuesta = entity.SunatRespuesta;
            model.SunatCdr = entity.SunatCdr;

            return model;
        }
    }
}
