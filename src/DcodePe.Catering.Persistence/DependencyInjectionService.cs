namespace DcodePe.Catering.Persistence
{
    public static class DependencyInjectionService
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services,
            IConfiguration configuration)
        {
            var useSqlite = configuration.GetValue<bool>("Database:UseSqlite");
            DataBaseService.UseSqliteProvider = useSqlite;

            services.AddDbContext<DataBaseService>(options =>
            {
                if (useSqlite)
                {
                    var sqlitePath = configuration["Database:SqlitePath"] ?? "Data/ayllu-dev.db";
                    var fullPath = Path.IsPathRooted(sqlitePath)
                        ? sqlitePath
                        : Path.Combine(AppContext.BaseDirectory, sqlitePath);

                    var directory = Path.GetDirectoryName(fullPath);
                    if (!string.IsNullOrEmpty(directory))
                        Directory.CreateDirectory(directory);

                    options.UseSqlite($"Data Source={fullPath}");
                }
                else
                {
                    options.UseSqlServer(configuration.GetConnectionString("SQLConnectionString"));
                }
            });

            services.AddScoped<IDataBaseService, DataBaseService>();

            //if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "local")
            //{
            //    string tenantId = Environment.GetEnvironmentVariable("tenantId") ?? string.Empty;
            //    string clientId = Environment.GetEnvironmentVariable("clientId") ?? string.Empty;
            //    string clientSecret = Environment.GetEnvironmentVariable("clientSecret") ?? string.Empty;

            //    var tokenCredentials = new ClientSecretCredential(tenantId, clientId, clientSecret);
               
            //    var azureKeyVaultProvider = new SqlColumnEncryptionAzureKeyVaultProvider(tokenCredentials);

            //    SqlConnection.RegisterColumnEncryptionKeyStoreProviders(new Dictionary<string, 
            //        SqlColumnEncryptionKeyStoreProvider>(1, StringComparer.OrdinalIgnoreCase)
            //    {
            //        {
            //            SqlColumnEncryptionAzureKeyVaultProvider.ProviderName, azureKeyVaultProvider
            //        }
            //    });
            //}
            //else
            //{
            //    var azureKeyVaultProvider = new SqlColumnEncryptionAzureKeyVaultProvider(new ManagedIdentityCredential());

            //    SqlConnection.RegisterColumnEncryptionKeyStoreProviders(new Dictionary<string,
            //        SqlColumnEncryptionKeyStoreProvider>(1, StringComparer.OrdinalIgnoreCase)
            //    {
            //        {
            //            SqlColumnEncryptionAzureKeyVaultProvider.ProviderName, azureKeyVaultProvider
            //        }
            //    });
            //}


            return services;
        }
    }
}
