using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Comprobante.Queries.GetAllComprobante
{
    public class GetAllComprobanteQuery(IDataBaseService databaseService) : IGetAllComprobanteQuery
    {
        private readonly IDataBaseService _databaseService = databaseService;

        public async Task<List<GetAllComprobanteModel>> Execute(string? tipo = null)
        {
            var query = _databaseService.ComprobanteElectronico
                .Where(c => c.Estado == true);

            if (!string.IsNullOrWhiteSpace(tipo))
            {
                query = query.Where(c => c.Tipo == tipo);
            }

            return await query
                .OrderByDescending(c => c.ComprobanteID)
                .Select(c => new GetAllComprobanteModel
                {
                    ComprobanteID = c.ComprobanteID,
                    Tipo = c.Tipo,
                    Serie = c.Serie,
                    Correlativo = c.Correlativo,
                    NumeroCompleto = c.NumeroCompleto,
                    CotizacionID = c.CotizacionID,
                    ClienteNombre = c.ClienteNombre,
                    ClienteDocumento = c.ClienteDocumento,
                    TipoDocumento = c.TipoDocumento,
                    ClienteDireccion = c.ClienteDireccion,
                    ClienteTelefono = c.ClienteTelefono,
                    FechaEmision = c.FechaEmision,
                    FormaPago = c.FormaPago,
                    MedioPago = c.MedioPago,
                    Moneda = c.Moneda,
                    OpGravadas = c.OpGravadas,
                    OpInafectas = c.OpInafectas,
                    OpExoneradas = c.OpExoneradas,
                    Subtotal = c.Subtotal,
                    Igv = c.Igv,
                    Total = c.Total,
                    Recibido = c.Recibido,
                    Vuelto = c.Vuelto,
                    ModoEmision = c.ModoEmision,
                    EstadoComprobante = c.EstadoComprobante,
                    SunatTicket = c.SunatTicket,
                    SunatCdr = c.SunatCdr,
                    SunatRespuesta = c.SunatRespuesta,
                    SunatCodigoError = c.SunatCodigoError,
                    RutaXml = c.RutaXml,
                    RutaCdr = c.RutaCdr,
                    MontoAdelantoFacturado = c.MontoAdelantoFacturado,
                    FechaCreacion = c.FechaCreacion,
                    Items = c.Detalles
                        .Where(d => d.Estado == true)
                        .Select(d => new GetAllComprobanteItemModel
                        {
                            ComprobanteDetalleID = d.ComprobanteDetalleID,
                            Item = d.Item,
                            Codigo = d.Codigo,
                            Descripcion = d.Descripcion,
                            IdTipoIgv = d.IdTipoIgv,
                            TipoIgv = d.TipoIgv,
                            UnidadMedida = d.UnidadMedida,
                            Valor = d.Valor,
                            Cantidad = d.Cantidad,
                            Subtotal = d.Subtotal,
                            Igv = d.Igv,
                            Importe = d.Importe
                        }).ToList()
                })
                .ToListAsync();
        }
    }
}
