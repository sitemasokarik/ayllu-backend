using Microsoft.AspNetCore.DataProtection;

namespace DcodePe.Catering.Api.Helpers
{
    public class DataProtectionSecretService(IDataProtectionProvider provider) : DcodePe.Catering.Application.Security.ISecretProtectionService
    {
        private readonly IDataProtector _protector = provider.CreateProtector("Ayllu.FE.Secrets.v1");

        public string Protect(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            return _protector.Protect(plainText);
        }

        public string Unprotect(string protectedText)
        {
            if (string.IsNullOrEmpty(protectedText))
                return string.Empty;

            return _protector.Unprotect(protectedText);
        }

        public bool TryUnprotect(string protectedText, out string plainText)
        {
            plainText = string.Empty;
            if (string.IsNullOrEmpty(protectedText))
                return false;

            try
            {
                plainText = _protector.Unprotect(protectedText);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
