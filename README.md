# DcodePe.Catering. Api

Api para servicio de Catering - Sistema integral de gestión de eventos y cotizaciones

## 📋 Tabla de Contenidos
- [Arquitectura](#arquitectura)
- [Entidades del Sistema](#entidades-del-sistema)
- [Endpoints API](#endpoints-api)
- [Tecnologías](#tecnologías)
- [Configuración](#configuración)

## 🏗️ Arquitectura

El proyecto implementa una **arquitectura limpia (Clean Architecture)** con separación de responsabilidades en capas:

```
DcodePe.Catering. Api/
├── src/
│   ├── DcodePe.Catering. Api/          # Capa de presentación (Controllers, Middleware)
│   ├── DcodePe.Catering.Application/  # Lógica de aplicación (Commands, Queries)
│   ├── DcodePe. Catering.Domain/       # Entidades de dominio y modelos
│   ├── DcodePe. Catering.Persistence/  # Acceso a datos (EF Core, Migrations)
│   └── DcodePe.Catering.Common/       # Utilidades y constantes comunes
```

### Principios Aplicados
- **CQRS Pattern**: Separación de Commands (escritura) y Queries (lectura)
- **Dependency Injection**: Inyección de dependencias con ASP.NET Core
- **Repository Pattern**: Abstracción del acceso a datos
- **Clean Architecture**: Separación de capas con dependencias unidireccionales


## ⚙️ Configuración

### Requisitos Previos
- . NET 8.0 SDK
- SQL Server 2019+
- Visual Studio 2022 o VS Code

### Cadena de Conexión
Configurar en `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=. ;Database=CateringIntranetDB;Trusted_Connection=True;"
  }
}


## 📝 Modelos de Respuesta

Todas las respuestas siguen el formato estándar:
```json
{
  "statusCode": 200,
  "success": true,
  "message": "Consulta exitosa",
  "data": { }
}
```

## 📄 Licencia

Proyecto privado - Todos los derechos reservados