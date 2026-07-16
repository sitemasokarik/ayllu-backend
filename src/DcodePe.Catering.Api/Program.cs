using DcodePe.Catering.Api;
using DcodePe.Catering.Api.Helpers;
using DcodePe.Catering.Application;
using DcodePe.Catering.Common;
using DcodePe.Catering.External;
using DcodePe.Catering.Persistence;
using DcodePe.Catering.Persistence.DataBase;
using DcodePe.Catering.Application.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

if (builder.Configuration.GetValue<bool>("Database:UseSqlite"))
{
    var sqlitePath = builder.Configuration["Database:SqlitePath"] ?? "Data/ayllu-dev.db";
    if (!Path.IsPathRooted(sqlitePath))
    {
        builder.Configuration["Database:SqlitePath"] =
            Path.Combine(builder.Environment.ContentRootPath, sqlitePath);
    }
}

builder.Services.AddControllers();

// Configuración de CORS para permitir todos los orígenes
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services
 .AddWebApi()
 .AddCommon()
 .AddApplication()
 .AddExternal(builder.Configuration)
 .AddPersistence(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<DataBaseService>();
    var useSqlite = builder.Configuration.GetValue<bool>("Database:UseSqlite");

    try
    {
        if (useSqlite)
        {
            db.Database.EnsureCreated();
            SqliteSchemaPatcher.Apply(db);
            DevPaginaSeeder.EnsurePages(db);

            var importDump = builder.Configuration.GetValue<bool>("Database:ImportProductionDump");
            if (importDump)
            {
                var dumpPath = ProductionDumpImporter.ResolveDumpPath(
                    app.Environment.ContentRootPath,
                    builder.Configuration["Database:ProductionDumpPath"]);
                ProductionDumpImporter.ImportIfNeeded(db, dumpPath, app.Logger);
            }
            else
            {
                DevDatabaseSeeder.Seed(db);
            }

            DevBlogSeeder.EnsureBlog(db);
            DevEventoTarifasSeeder.EnsureTarifas(db, app.Logger);

            ApplyDevPasswordResetIfConfigured(db, builder.Configuration, scope.ServiceProvider, app.Logger);

            if (builder.Configuration.GetValue<bool>("DevDatabase:ClearCotizacionesOnStartup"))
            {
                var marker = Path.Combine(app.Environment.ContentRootPath, "Data", ".cotizaciones-cleared-v1");
                if (!File.Exists(marker))
                {
                    DevCotizacionCleanup.ClearAllCotizaciones(db, app.Logger);
                    Directory.CreateDirectory(Path.GetDirectoryName(marker)!);
                    File.WriteAllText(marker, DateTime.UtcNow.ToString("O"));
                }
            }

            app.Logger.LogInformation("Base SQLite de desarrollo lista.");
        }
        else
        {
            db.Database.Migrate();
            app.Logger.LogInformation("Migraciones SQL Server aplicadas correctamente.");
        }
    }
    catch (Exception ex)
    {
        app.Logger.LogWarning(
            ex,
            "No se pudo preparar la base de datos. Revisa scripts/dev-setup.ps1 o activa Database:UseSqlite.");
    }

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}
else
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<DataBaseService>();

    try
    {
        if (builder.Configuration.GetValue<bool>("Database:ApplyMigrationsOnStartup"))
        {
            db.Database.Migrate();
            app.Logger.LogInformation("Migraciones SQL Server aplicadas en producción.");
        }

        ProductionSchemaPatcher.Apply(db, app.Logger);

        if (builder.Configuration.GetValue<bool>("Database:ClearTransactionalDataOnStartup"))
        {
            var marker = Path.Combine(app.Environment.ContentRootPath, "Data", ".transactional-data-cleared");
            if (!File.Exists(marker))
            {
                ProductionDataCleanup.ClearTransactionalData(db, app.Logger);
                Directory.CreateDirectory(Path.GetDirectoryName(marker)!);
                File.WriteAllText(marker, DateTime.UtcNow.ToString("O"));
                app.Logger.LogWarning(
                    "Limpieza transaccional ejecutada. Desactiva Database:ClearTransactionalDataOnStartup en appsettings.");
            }
        }
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "No se pudo preparar la base de datos de producción.");
    }
}

app.UseCors("AllowAll");

AppStoragePaths.EnsureDirectoriesExist();
AppStoragePaths.MigrateLegacyUploads(app.Logger);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(AppStoragePaths.UploadsRoot),
    RequestPath = "/uploads"
});
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

static void ApplyDevPasswordResetIfConfigured(
    DataBaseService db,
    IConfiguration configuration,
    IServiceProvider services,
    ILogger logger)
{
    var userName = configuration["DevAuth:ResetPasswordFor"];
    var newPassword = configuration["DevAuth:NewPassword"];

    if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(newPassword))
        return;

    var usuario = db.Usuario.FirstOrDefault(u => u.UserName == userName);
    if (usuario == null)
        return;

    var hasher = services.GetRequiredService<IPasswordHashService>();
    usuario.Password = hasher.HashPassword(newPassword);
    db.SaveChanges();
    logger.LogInformation("Contraseña local de desarrollo aplicada para el usuario {UserName}.", userName);
}
