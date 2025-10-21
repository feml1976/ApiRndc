# ApiRndc - Sistema de Integración RNDC Colombia

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-Server-512BD4)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Latest-336791)](https://www.postgresql.org/)
[![MudBlazor](https://img.shields.io/badge/MudBlazor-Latest-594AE2)](https://mudblazor.com/)

Sistema completo de gestión e integración con el **Registro Nacional de Despacho de Carga (RNDC)** del Ministerio de Transporte de Colombia, desarrollado con .NET 9.0 y Blazor Server.

## 📋 Tabla de Contenidos

- [Características](#-características)
- [Arquitectura](#-arquitectura)
- [Tecnologías](#-tecnologías)
- [Requisitos Previos](#-requisitos-previos)
- [Instalación](#-instalación)
- [Configuración](#-configuración)
- [Uso](#-uso)
- [Procesos RNDC Implementados](#-procesos-rndc-implementados)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Documentación](#-documentación)
- [Seguridad](#-seguridad)
- [Contribuciones](#-contribuciones)
- [Licencia](#-licencia)

## 🚀 Características

### Funcionalidades Principales

- ✅ **Registro de Terceros** (Proceso 11) - Conductores, propietarios, tenedores
- ✅ **Registro de Vehículos** (Proceso 12) - Vehículos de carga terrestre
- ✅ **Expedición de Remesas** (Proceso 3) - Remesas terrestres de carga
- ✅ **Expedición de Manifiestos** (Proceso 4) - Manifiestos de carga con remesas
- ✅ **Cumplido de Remesas** (Proceso 5) - Registro de entregas de remesas
- ✅ **Cumplido de Manifiestos** (Proceso 6) - Registro de entregas de manifiestos

### Características Técnicas

- 🔐 **Autenticación y Autorización** - Sistema de roles (Administrador, Operador, Consulta)
- 📊 **Dashboard Interactivo** - Estadísticas y métricas en tiempo real
- 📝 **Historial Completo** - Registro de todas las transacciones con XML de request/response
- 🎨 **Interfaz Moderna** - Material Design con MudBlazor
- 📱 **Responsive Design** - Compatible con dispositivos móviles y tablets
- 🔍 **Búsqueda y Filtros** - Filtrado avanzado de transacciones
- 📋 **Validación de Datos** - Validación client-side y server-side
- 🔄 **Manejo de Errores** - Sistema robusto de manejo de errores y reintentos
- 📈 **Logging Estructurado** - Serilog con almacenamiento en archivos y consola
- 🏗️ **Clean Architecture** - Separación de responsabilidades por capas

## 🏛️ Arquitectura

El proyecto sigue los principios de **Clean Architecture** y está organizado en las siguientes capas:

```
ApiRndc/
├── src/
│   ├── ApiRndc.Domain/          # Entidades, Enums, Interfaces (Core del negocio)
│   ├── ApiRndc.Application/     # Casos de uso, Commands, Handlers (CQRS)
│   ├── ApiRndc.Infrastructure/  # Implementaciones, DbContext, Servicios externos
│   ├── ApiRndc.Shared/          # DTOs, Constantes compartidas
│   └── ApiRndc.Web/             # Blazor Server UI, Componentes
├── tests/
│   └── ApiRndc.UnitTests/       # Pruebas unitarias
└── docs/                        # Documentación
```

### Patrones Implementados

- **CQRS** (Command Query Responsibility Segregation) con MediatR
- **Repository Pattern** - Abstracción de acceso a datos
- **Unit of Work** - Gestión de transacciones
- **Dependency Injection** - Inyección de dependencias nativa de .NET
- **SOAP Client Pattern** - Cliente especializado para consumo de servicios SOAP

## 🛠️ Tecnologías

### Backend
- **.NET 9.0** - Framework principal
- **C# 13** - Lenguaje de programación
- **Entity Framework Core 9.0** - ORM
- **MediatR** - Patrón mediador para CQRS
- **FluentValidation** - Validación de modelos
- **Serilog** - Logging estructurado

### Frontend
- **Blazor Server** - Framework UI interactivo
- **MudBlazor** - Biblioteca de componentes Material Design
- **Razor Components** - Componentes reutilizables

### Base de Datos
- **PostgreSQL** - Base de datos relacional
- **Npgsql** - Provider de PostgreSQL para .NET

### Integración
- **SOAP/XML** - Comunicación con servicio RNDC del Ministerio de Transporte

## 📦 Requisitos Previos

Antes de instalar el proyecto, asegúrese de tener:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) o superior
- [PostgreSQL 14+](https://www.postgresql.org/download/) (local o remoto)
- [Git](https://git-scm.com/downloads) (para clonar el repositorio)
- Un IDE compatible:
  - [Visual Studio 2022](https://visualstudio.microsoft.com/) (Recomendado)
  - [Visual Studio Code](https://code.visualstudio.com/) con extensión C#
  - [JetBrains Rider](https://www.jetbrains.com/rider/)

### Opcional
- [IIS 10.0+](https://www.iis.net/) (para deployment en producción)
- [pgAdmin](https://www.pgadmin.org/) (para administración de PostgreSQL)

## 📥 Instalación

### 1. Clonar el Repositorio

```bash
git clone https://github.com/SU-USUARIO/ApiRndc.git
cd ApiRndc
```

### 2. Restaurar Dependencias

```bash
dotnet restore
```

### 3. Configurar Base de Datos

Copie el archivo de configuración de ejemplo:

```bash
copy src\ApiRndc.Web\appsettings.Example.json src\ApiRndc.Web\appsettings.json
```

Edite `appsettings.json` con sus credenciales:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=RndcDb;Username=postgres;Password=SU_PASSWORD"
  },
  "RndcConfig": {
    "ServiceUrl": "http://rndcws.mintransporte.gov.co:8080/soap/IBPMServices",
    "Username": "SU_USUARIO_RNDC",
    "Password": "SU_PASSWORD_RNDC"
  }
}
```

### 4. Crear Base de Datos

**Opción A - Automática (en desarrollo):**

El sistema creará la base de datos automáticamente al ejecutar.

**Opción B - Manual:**

```bash
cd src/ApiRndc.Infrastructure
dotnet ef migrations script --output init.sql
```

Ejecute el script `init.sql` en PostgreSQL.

### 5. Compilar el Proyecto

```bash
dotnet build
```

### 6. Ejecutar la Aplicación

```bash
cd src/ApiRndc.Web
dotnet run
```

La aplicación estará disponible en:
- **HTTP:** http://localhost:5000
- **HTTPS:** https://localhost:5001

## ⚙️ Configuración

### Variables de Configuración

Todas las configuraciones se encuentran en `appsettings.json`:

#### Conexión a Base de Datos
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=HOST;Port=5432;Database=RndcDb;Username=USER;Password=PASS"
}
```

#### Configuración RNDC
```json
"RndcConfig": {
  "ServiceUrl": "http://rndcws.mintransporte.gov.co:8080/soap/IBPMServices",
  "Username": "USUARIO_RNDC",
  "Password": "PASSWORD_RNDC",
  "RequestType": "request"
}
```

#### Logging
```json
"Serilog": {
  "MinimumLevel": {
    "Default": "Information",
    "Override": {
      "Microsoft": "Warning"
    }
  }
}
```

## 🎯 Uso

### Primer Inicio de Sesión

1. Ejecute la aplicación
2. Navegue a `/Account/Register`
3. Cree el primer usuario (se asignará automáticamente el rol "Consulta")
4. Un administrador debe cambiar el rol en `/usuarios`

### Usuarios y Roles

El sistema tiene 3 roles:

- **👑 Administrador**
  - Gestión de usuarios
  - Acceso a configuración
  - Todas las operaciones

- **⚙️ Operador**
  - Registro de terceros y vehículos
  - Expedición de remesas y manifiestos
  - Registro de cumplidos
  - Consulta de historial

- **👁️ Consulta**
  - Solo visualización del historial de transacciones

### Flujo de Trabajo Típico

1. **Registrar Terceros** (`/terceros`)
   - Conductores
   - Propietarios
   - Tenedores

2. **Registrar Vehículos** (`/vehiculos`)
   - Vehículos de carga
   - Remolques

3. **Expedir Remesas** (`/remesas`)
   - Crear remesa de carga
   - Asociar remitente y destinatario

4. **Expedir Manifiestos** (`/manifiestos`)
   - Crear manifiesto
   - Asociar remesas al manifiesto

5. **Registrar Cumplidos** (`/cumplidos`)
   - Cumplido de remesas
   - Cumplido de manifiestos

6. **Consultar Historial** (`/transacciones`)
   - Ver todas las transacciones
   - Filtrar por tipo, estado, fecha
   - Revisar XML de request/response

## 📋 Procesos RNDC Implementados

| Proceso | Nombre | Descripción | Estado |
|---------|--------|-------------|--------|
| 11 | Registro de Terceros | Registro de conductores, propietarios, tenedores | ✅ |
| 12 | Registro de Vehículos | Registro de vehículos de carga terrestre | ✅ |
| 3 | Expedición de Remesa | Expedición de remesa terrestre de carga | ✅ |
| 4 | Expedición de Manifiesto | Expedición de manifiesto de carga | ✅ |
| 5 | Cumplido de Remesa | Registro de cumplido de remesa | ✅ |
| 6 | Cumplido de Manifiesto | Registro de cumplido de manifiesto | ✅ |

## 📂 Estructura del Proyecto

```
ApiRndc/
│
├── src/
│   ├── ApiRndc.Domain/
│   │   ├── Entities/           # Entidades del dominio
│   │   ├── Enums/              # Enumeraciones
│   │   └── Interfaces/         # Interfaces del dominio
│   │
│   ├── ApiRndc.Application/
│   │   └── Commands/           # Commands y Handlers (CQRS)
│   │
│   ├── ApiRndc.Infrastructure/
│   │   ├── Data/               # DbContext y Migraciones
│   │   ├── Repositories/       # Implementación de repositorios
│   │   └── Services/           # Servicios (SOAP Client)
│   │
│   ├── ApiRndc.Shared/
│   │   ├── DTOs/               # Data Transfer Objects
│   │   └── Constants/          # Constantes compartidas
│   │
│   └── ApiRndc.Web/
│       ├── Components/
│       │   ├── Layout/         # Layouts (MainLayout, NavMenu)
│       │   └── Pages/          # Páginas Blazor
│       ├── wwwroot/            # Archivos estáticos
│       └── Program.cs          # Punto de entrada
│
├── tests/
│   └── ApiRndc.UnitTests/      # Pruebas unitarias
│
├── docs/                       # Documentación
│   ├── INSTALACION.md
│   ├── CONFIGURACION_BD.md
│   ├── HANDLERS_GUIA.md
│   └── GITHUB_SETUP.md
│
├── .gitignore
├── README.md
└── ApiRndc.sln
```

## 📚 Documentación

Documentación detallada disponible en la carpeta `/docs`:

- [📖 Guía de Instalación](docs/INSTALACION.md)
- [🗄️ Configuración de Base de Datos](docs/CONFIGURACION_BD.md)
- [⚡ Guía de Handlers](docs/HANDLERS_GUIA.md)
- [📤 Configuración de GitHub](docs/GITHUB_SETUP.md)

## 🔐 Seguridad

### Mejores Prácticas Implementadas

- ✅ Autenticación con ASP.NET Core Identity
- ✅ Autorización basada en roles
- ✅ Validación de entrada de datos
- ✅ Protección contra SQL Injection (Entity Framework)
- ✅ HTTPS obligatorio en producción
- ✅ Cookies seguras (HttpOnly, Secure)
- ✅ Logging de todas las operaciones

### Recomendaciones de Seguridad

1. **Nunca** commitear archivos `appsettings.json` con credenciales reales
2. Usar **variables de entorno** para credenciales en producción
3. Cambiar las **contraseñas por defecto** de PostgreSQL
4. Habilitar **SSL** en la conexión a PostgreSQL
5. Mantener **.NET y dependencias actualizadas**
6. Revisar **logs regularmente** para detectar actividad sospechosa

## 🤝 Contribuciones

Este es un proyecto privado. Para contribuir:

1. Contacte al administrador del proyecto
2. Fork el repositorio (si tiene acceso)
3. Cree una rama feature (`git checkout -b feature/AmazingFeature`)
4. Commit sus cambios (`git commit -m 'Add some AmazingFeature'`)
5. Push a la rama (`git push origin feature/AmazingFeature`)
6. Abra un Pull Request

## 📄 Licencia

Este proyecto es de **uso privado**. Todos los derechos reservados.

Copyright © 2025 - Proyecto ApiRndc

---

## 📞 Soporte

Para reportar problemas o solicitar soporte:

- 📧 Email: [contacto@ejemplo.com](mailto:contacto@ejemplo.com)
- 📝 Issues: [GitHub Issues](https://github.com/SU-USUARIO/ApiRndc/issues)

---

## 🙏 Agradecimientos

- Ministerio de Transporte de Colombia - Por la documentación del RNDC
- Comunidad .NET - Por las excelentes herramientas y frameworks
- MudBlazor Team - Por la increíble biblioteca de componentes

---

**Desarrollado con ❤️ usando .NET 9.0 y Blazor Server**

**Última actualización:** 2025-01-20
