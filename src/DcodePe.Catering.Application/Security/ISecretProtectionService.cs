namespace DcodePe.Catering.Application.Security
{
    public interface ISecretProtectionService
    {
        string Protect(string plainText);
        string Unprotect(string protectedText);
        bool TryUnprotect(string protectedText, out string plainText);
    }
}
