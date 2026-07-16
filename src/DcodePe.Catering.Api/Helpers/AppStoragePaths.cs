namespace DcodePe.Catering.Api.Helpers
{
    /// <summary>
    /// Rutas de archivos subidos alineadas con la base SQLite (AppContext.BaseDirectory),
    /// independientes de si la API se ejecuta con dotnet run o desde bin/Debug.
    /// </summary>
    public static class AppStoragePaths
    {
        public static string CertificadoPhysicalDir =>
            Path.Combine(AppContext.BaseDirectory, "fe", "certificado");

        public static string XmlPhysicalDir =>
            Path.Combine(AppContext.BaseDirectory, "fe", "xml");

        public static string CdrPhysicalDir =>
            Path.Combine(AppContext.BaseDirectory, "fe", "cdr");

        public static string UploadsRoot =>
            Path.Combine(AppContext.BaseDirectory, "Data", "uploads");

        public static string VouchersPhysicalDir =>
            Path.Combine(UploadsRoot, "vouchers");

        public static string QrPagoPhysicalDir =>
            Path.Combine(UploadsRoot, "qr-pago");

        public static string ToVirtualUrl(string virtualFolder, string fileName) =>
            $"/{virtualFolder}/{fileName}".Replace('\\', '/');

        public static string GetPhysicalDir(string virtualFolder)
        {
            var normalized = (virtualFolder ?? string.Empty).Trim().TrimStart('/').Replace('\\', '/');

            return normalized.ToLowerInvariant() switch
            {
                "uploads/vouchers" or "vouchers" => VouchersPhysicalDir,
                "uploads/qr-pago" or "qr-pago" => QrPagoPhysicalDir,
                _ when normalized.StartsWith("uploads/", StringComparison.OrdinalIgnoreCase)
                    => Path.Combine(UploadsRoot, normalized["uploads/".Length..].Replace('/', Path.DirectorySeparatorChar)),
                _ => Path.Combine(UploadsRoot, normalized.Replace('/', Path.DirectorySeparatorChar))
            };
        }

        public static string ResolvePhysicalPath(IWebHostEnvironment environment, string relativeUrl)
        {
            var normalized = (relativeUrl ?? string.Empty).Trim().Replace('\\', '/');
            if (normalized.StartsWith('/'))
                normalized = normalized[1..];

            var primary = MapVirtualUrlToPhysical(normalized);
            if (File.Exists(primary))
                return primary;

            foreach (var legacyRoot in GetLegacyWebRoots(environment))
            {
                var legacy = Path.Combine(legacyRoot, normalized.Replace('/', Path.DirectorySeparatorChar));
                if (File.Exists(legacy))
                    return legacy;
            }

            return primary;
        }

        public static void EnsureDirectoriesExist()
        {
            Directory.CreateDirectory(VouchersPhysicalDir);
            Directory.CreateDirectory(QrPagoPhysicalDir);
            Directory.CreateDirectory(Path.Combine(UploadsRoot, "productos"));
            Directory.CreateDirectory(Path.Combine(UploadsRoot, "locales"));
            Directory.CreateDirectory(Path.Combine(UploadsRoot, "servicios"));
            Directory.CreateDirectory(Path.Combine(UploadsRoot, "eventos"));
            Directory.CreateDirectory(Path.Combine(UploadsRoot, "blog"));
            Directory.CreateDirectory(Path.Combine(UploadsRoot, "empresa"));
            Directory.CreateDirectory(CertificadoPhysicalDir);
            Directory.CreateDirectory(XmlPhysicalDir);
            Directory.CreateDirectory(CdrPhysicalDir);
        }

        public static void MigrateLegacyUploads(ILogger logger)
        {
            EnsureDirectoriesExist();

            foreach (var webRoot in GetLegacyWebRoots(null))
            {
                MigrateFolder(Path.Combine(webRoot, "uploads", "vouchers"), VouchersPhysicalDir, logger);
                MigrateFolder(Path.Combine(webRoot, "uploads", "qr-pago"), QrPagoPhysicalDir, logger);
            }

            // Corregir guardado en uploads/uploads/* (bug de ruta duplicada)
            foreach (var folder in new[] { "productos", "locales", "servicios", "eventos", "blog", "empresa" })
            {
                MigrateFolder(
                    Path.Combine(UploadsRoot, "uploads", folder),
                    Path.Combine(UploadsRoot, folder),
                    logger);
            }
        }

        private static string MapVirtualUrlToPhysical(string normalized)
        {
            if (normalized.StartsWith("uploads/", StringComparison.OrdinalIgnoreCase))
            {
                var underUploads = normalized["uploads/".Length..];
                return Path.Combine(UploadsRoot, underUploads.Replace('/', Path.DirectorySeparatorChar));
            }

            return Path.Combine(AppContext.BaseDirectory, normalized.Replace('/', Path.DirectorySeparatorChar));
        }

        private static IEnumerable<string> GetLegacyWebRoots(IWebHostEnvironment? environment)
        {
            var roots = new List<string>();

            if (environment != null)
            {
                var webRoot = environment.WebRootPath;
                if (string.IsNullOrWhiteSpace(webRoot))
                    webRoot = Path.Combine(environment.ContentRootPath, "wwwroot");
                roots.Add(webRoot);
            }

            roots.Add(Path.Combine(AppContext.BaseDirectory, "wwwroot"));
            roots.Add(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "wwwroot")));

            return roots.Distinct(StringComparer.OrdinalIgnoreCase);
        }

        private static void MigrateFolder(string sourceDir, string destDir, ILogger logger)
        {
            if (!Directory.Exists(sourceDir))
                return;

            Directory.CreateDirectory(destDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                var dest = Path.Combine(destDir, Path.GetFileName(file));
                if (File.Exists(dest))
                    continue;

                File.Copy(file, dest);
                logger.LogInformation("Archivo migrado a almacenamiento estable: {File}", Path.GetFileName(file));
            }
        }
    }
}
