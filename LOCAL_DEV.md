# Desarrollo local — Ayllu Backend

## Arranque rápido (recomendado)

Desde la raíz del repo:

```powershell
.\scripts\dev-setup.ps1      # primera vez
.\scripts\start-backend.ps1  # API → http://localhost:5236
.\scripts\start-frontends.ps1 # Landing :4200, Admin :4201
```

## Base de datos en desarrollo

Por defecto (`appsettings.Development.json`):

```json
"Database": {
  "UseSqlite": true,
  "SqlitePath": "Data/ayllu-dev.db"
}
```

- No requiere instalar SQL Server
- La API crea el archivo SQLite al iniciar e **importa automáticamente** `DB/db.sql` (datos de producción)
- Siembra series B001/F001 para facturación
- Archivo ignorado por git

### Credenciales admin en local

Tras importar el dump, en Development se aplica una contraseña local (solo en tu máquina):

| Usuario | Contraseña |
|---------|------------|
| `admin` | `Admin123*` |

Configurable en `appsettings.Development.json` → `DevAuth`. Para usar la contraseña de producción, deja `ResetPasswordFor` vacío.

### Reimportar datos desde cero

```powershell
.\scripts\reset-local-db.ps1
```

### Usar SQL Server (LocalDB / Express)

1. Instala [SQL Server Express](https://www.microsoft.com/es-es/sql-server/sql-server-downloads) con LocalDB
2. En `appsettings.Development.json` → `"UseSqlite": false`
3. Aplica migraciones:

```powershell
cd src/DcodePe.Catering.Api
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet ef database update --project ..\DcodePe.Catering.Persistence --startup-project .
```

En Development con SQL Server, las migraciones también se aplican **automáticamente** al iniciar la API.

## URLs

| Servicio | URL |
|----------|-----|
| API + Swagger | http://localhost:5236 |
| Landing | http://localhost:4200 |
| Admin | http://localhost:4201 |

## Endpoints de integración

| Módulo | Ruta |
|--------|------|
| Comprobantes | `/api/v1/comprobante` |
| Tickets | `/api/v1/ticket` |
| Consulta DNI/RUC | `/api/v1/consulta-documento` |
| Portal cliente | `/api/v1/Cliente/portal/register`, `/portal/login` |

## Producción

Monster ASP.NET — `Database:UseSqlite` no aplica; usa SQL Server de `appsettings.json` / `appsettings.Production.json`.

### Publicar API

```powershell
.\scripts\build-backend-prod.ps1
```

### Limpiar datos transaccionales (cotizaciones, mensajes, clientes portal, etc.)

Conserva **Pagina**, **Permiso**, **Rol**, **Usuario**, **Empresa**, catálogo y CMS.

- SQL: `DB/production-reset-transactional-data.sql` en SSMS
- O al primer arranque: `ClearTransactionalDataOnStartup: true` en `appsettings.Production.json`

Ver [INTEGRACIONES.md](../INTEGRACIONES.md) para el checklist completo de deploy.
