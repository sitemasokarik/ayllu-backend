using DcodePe.Catering.Application.DataBase.Comprobante.Queries.GetAllComprobante;
using Microsoft.EntityFrameworkCore;

namespace DcodePe.Catering.Application.DataBase.Comprobante.Queries.GetComprobanteById
{
    public class GetComprobanteByIdQuery(IDataBaseService databaseService) : IGetComprobanteByIdQuery
    {
        private readonly IDataBaseService _databaseService = databaseService;

        public async Task<GetAllComprobanteModel?> Execute(int comprobanteId)
        {
            return await _databaseService.ComprobanteElectronico
                .Where(c => c.ComprobanteID == comprobanteId && c.Estado == true)
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
                .FirstOrDefaultAsync();
        }
    }
}
