# Prueba Tech Lead - Sistema de Pedidos

Este proyecto es parte de una prueba técnica para el cargo de **Tech Lead**.

## 🚀 Tecnologías usadas
- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server
- JWT para autenticación

## 📂 Estructura del proyecto
- `Controllers/` → Controladores de la API.
- `Models/` → Modelos de datos.
- `Services/` → Servicios de negocio.
- `appsettings.json` → Configuración de conexión y JWT.

## 🔐 Endpoint de autenticación
`POST /api/Auth/login`  
Body:
```json
{
  "username": "admin",
  "password": "12345"
}
