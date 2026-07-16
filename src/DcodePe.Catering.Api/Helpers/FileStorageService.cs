using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace DcodePe.Catering.Api.Helpers
{
    public interface IFileStorageService
    {
        Task<string> SaveVoucherAsync(IFormFile file);
        Task<string> SaveQrPagoAsync(IFormFile file);
        Task<string> SaveImageAsync(IFormFile file, string virtualFolder);
        Task<string> SaveCertificadoAsync(IFormFile file, string password);
    }

    public class FileStorageService : IFileStorageService
    {
        private static readonly string[] ImageExtensions = [".jpg", ".jpeg", ".png", ".webp", ".gif"];

        public Task<string> SaveVoucherAsync(IFormFile file)
            => SaveRawAsync(file, "uploads/vouchers", new[] { ".jpg", ".jpeg", ".png", ".webp", ".pdf" }, 8 * 1024 * 1024);

        public Task<string> SaveQrPagoAsync(IFormFile file)
            => SaveImageAsync(file, "uploads/qr-pago");

        public async Task<string> SaveCertificadoAsync(IFormFile file, string password)
        {
            if (file == null || file.Length == 0)
                throw new InvalidOperationException("Debes adjuntar un certificado .pfx válido.");

            if (file.Length > 2 * 1024 * 1024)
                throw new InvalidOperationException("El certificado no puede superar 2 MB.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (extension != ".pfx")
                throw new InvalidOperationException("Solo se permiten certificados .pfx.");

            if (string.IsNullOrWhiteSpace(password))
                throw new InvalidOperationException("Ingresa la clave del certificado para validarlo.");

            await using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            var bytes = stream.ToArray();

            if (!System.Security.Cryptography.X509Certificates.X509Certificate2.GetCertContentType(bytes)
                    .Equals(System.Security.Cryptography.X509Certificates.X509ContentType.Pkcs12))
                throw new InvalidOperationException("El archivo no es un certificado PKCS#12 válido.");

            try
            {
                using var cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(
                    bytes, password, System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.Exportable);
                if (!cert.HasPrivateKey)
                    throw new InvalidOperationException("El certificado no contiene clave privada.");
            }
            catch (System.Security.Cryptography.CryptographicException)
            {
                throw new InvalidOperationException("No se pudo abrir el certificado. Verifica la clave.");
            }

            var targetDir = AppStoragePaths.CertificadoPhysicalDir;
            Directory.CreateDirectory(targetDir);

            var fileName = $"cert_{Guid.NewGuid():N}.pfx";
            var physicalPath = Path.Combine(targetDir, fileName);
            await File.WriteAllBytesAsync(physicalPath, bytes);

            return fileName;
        }

        public async Task<string> SaveImageAsync(IFormFile file, string virtualFolder)
        {
            ValidateImageInput(file, 5 * 1024 * 1024);

            var targetDir = AppStoragePaths.GetPhysicalDir(virtualFolder);
            Directory.CreateDirectory(targetDir);

            var fileName = $"{Guid.NewGuid():N}.jpg";
            var physicalPath = Path.Combine(targetDir, fileName);

            await using var input = file.OpenReadStream();
            using var image = await Image.LoadAsync(input);

            if (image.Width > 1920)
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(1920, 0),
                    Mode = ResizeMode.Max
                }));
            }

            await image.SaveAsJpegAsync(physicalPath, new JpegEncoder { Quality = 82 });
            return AppStoragePaths.ToVirtualUrl(virtualFolder, fileName);
        }

        private static async Task<string> SaveRawAsync(
            IFormFile file,
            string virtualFolder,
            string[] allowedExtensions,
            long maxBytes)
        {
            if (file == null || file.Length == 0)
                throw new InvalidOperationException("Debes adjuntar un archivo válido.");

            if (file.Length > maxBytes)
                throw new InvalidOperationException($"El archivo no puede superar {maxBytes / (1024 * 1024)} MB.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
                throw new InvalidOperationException("Formato de archivo no permitido.");

            var targetDir = AppStoragePaths.GetPhysicalDir(virtualFolder);
            Directory.CreateDirectory(targetDir);

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var physicalPath = Path.Combine(targetDir, fileName);

            await using var stream = new FileStream(physicalPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return AppStoragePaths.ToVirtualUrl(virtualFolder, fileName);
        }

        private static void ValidateImageInput(IFormFile file, long maxBytes)
        {
            if (file == null || file.Length == 0)
                throw new InvalidOperationException("Debes adjuntar una imagen válida.");

            if (file.Length > maxBytes)
                throw new InvalidOperationException("La imagen no puede superar 5 MB.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!ImageExtensions.Contains(extension))
                throw new InvalidOperationException("Formato de imagen no permitido. Usa JPG, PNG, WEBP o GIF.");
        }
    }
}
