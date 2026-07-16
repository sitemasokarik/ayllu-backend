using DcodePe.Catering.Application.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using DcodePe.Catering.Application.DataBase.Empresa.Helpers;

namespace DcodePe.Catering.Application.DataBase.Empresa.Commands.Update
{
    public class UpdateEmpresaCommand : IUpdateEmpresaCommand
    {
        private readonly IDataBaseService _databaseService;
        private readonly ISecretProtectionService _secretProtection;

        public UpdateEmpresaCommand(IDataBaseService databaseService, ISecretProtectionService secretProtection)
        {
            _databaseService = databaseService;
            _secretProtection = secretProtection;
        }

        public async Task<bool> Execute(UpdateEmpresaModel model)
        {
            var entity = await _databaseService.Empresa
                .FirstOrDefaultAsync(e => e.EmpresaID == model.EmpresaID);

            if (entity == null)
                return false;

            entity.RazonSocial = model.RazonSocial;
            entity.NombreComercial = model.NombreComercial;
            entity.RUC = model.RUC;
            entity.Email = model.Email;
            entity.Telefono = model.Telefono;
            entity.TelefonoSecundario = model.TelefonoSecundario;
            entity.WhatsApp = model.WhatsApp;
            entity.Direccion = model.Direccion;
            entity.Ciudad = model.Ciudad;
            entity.Pais = model.Pais;
            entity.Facebook = model.Facebook;
            entity.Instagram = model.Instagram;
            entity.LinkedIn = model.LinkedIn;
            entity.Twitter = model.Twitter;
            entity.HorarioAtencion = model.HorarioAtencion;
            entity.Logo = model.Logo;
            entity.BancoNombre = model.BancoNombre;
            entity.NumeroCuenta = model.NumeroCuenta;
            entity.Cci = model.Cci;
            entity.YapeNumero = model.YapeNumero;
            entity.PlinNumero = model.PlinNumero;
            entity.QrPagoUrl = model.QrPagoUrl;
            entity.InstruccionesPago = model.InstruccionesPago;
            entity.MontoAdelantoReserva = model.MontoAdelantoReserva > 0 ? model.MontoAdelantoReserva : 1000m;
            entity.GeneraFactElect = model.GeneraFactElect ?? false;
            entity.Ubigeo = model.Ubigeo;
            entity.RutaCertificadoServidor = "fe/certificado";
            entity.CertificadoFileName = model.CertificadoFileName ?? entity.CertificadoFileName;
            entity.UsuarioSol = model.UsuarioSol;
            entity.SunatModo = string.IsNullOrWhiteSpace(model.SunatModo) ? "DESARROLLO" : model.SunatModo.Trim().ToUpperInvariant();
            entity.SunatWsProduccion = null;

            if (!string.IsNullOrWhiteSpace(model.ClaveCertificado))
                entity.ClaveCertificado = _secretProtection.Protect(model.ClaveCertificado);

            if (!string.IsNullOrWhiteSpace(model.ClaveSol))
                entity.ClaveSol = _secretProtection.Protect(model.ClaveSol);

            if (!string.IsNullOrWhiteSpace(model.ApiPeruDevToken))
                entity.ApiPeruDevToken = _secretProtection.Protect(model.ApiPeruDevToken);

            if (model.CuentasPago?.Count > 0)
            {
                entity.CuentasPagoJson = EmpresaCuentaPagoHelper.Serialize(model.CuentasPago);
                var principal = model.CuentasPago.First();
                entity.BancoNombre = principal.BancoNombre;
                entity.NumeroCuenta = principal.NumeroCuenta;
                entity.Cci = principal.Cci;
                entity.YapeNumero = principal.YapeNumero;
                entity.PlinNumero = principal.PlinNumero;
            }
            else if (!string.IsNullOrWhiteSpace(model.CuentasPagoJson))
            {
                entity.CuentasPagoJson = model.CuentasPagoJson;
            }

            entity.UsuarioModificacion = model.UsuarioModificacion ?? "SYSTEM";
            entity.FechaModificacion = DateTime.Now;

            await _databaseService.SaveAsync();
            return true;
        }
    }
}
