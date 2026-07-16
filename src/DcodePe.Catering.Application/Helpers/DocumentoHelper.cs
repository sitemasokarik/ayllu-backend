namespace DcodePe.Catering.Application.Helpers
{
    public static class DocumentoHelper
    {
        public static string NormalizarDni(string? documento)
        {
            if (string.IsNullOrWhiteSpace(documento))
                return string.Empty;

            return new string(documento.Where(char.IsDigit).ToArray());
        }
    }
}
