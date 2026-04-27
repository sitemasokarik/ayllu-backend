using DcodePe.Catering.Api;
using DcodePe.Catering.Application;
using DcodePe.Catering.Common;
using DcodePe.Catering.External;
using DcodePe.Catering.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configuración de CORS para permitir todos los orígenes
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()      // Permite cualquier origen
              .AllowAnyMethod()      // Permite cualquier método HTTP (GET, POST, PUT, DELETE, etc.)
              .AllowAnyHeader();     // Permite cualquier encabezado
    });
});

builder.Services
 .AddWebApi()
 .AddCommon()
 .AddApplication()
 .AddExternal(builder.Configuration)
 .AddPersistence(builder.Configuration);

var app = builder.Build();

// Habilitar Swagger solo en entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

// Habilitar CORS (debe estar antes de UseAuthentication y UseAuthorization)
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
