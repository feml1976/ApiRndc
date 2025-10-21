# 🚀 Inicio Rápido - ApiRndc

## ✅ Estado del Proyecto

- ✅ **Estructura creada**: Todos los proyectos configurados
- ✅ **Compilación exitosa**: 0 errores, 0 advertencias
- ✅ **Migración creada**: Script SQL listo para aplicar
- ⏳ **Base de datos**: Pendiente de configuración
- ⏳ **Primera ejecución**: Pendiente

---

## 📋 Próximos Pasos

### Paso 1: Configurar Base de Datos ⚠️ IMPORTANTE

**Problema detectado**: La conexión a la base de datos remota `201.184.51.27:5401` no está disponible desde tu ubicación actual.

**Solución**: Tienes 3 opciones:

#### Opción A: Base de Datos Local (Más Rápido - Recomendado)

1. **Instalar PostgreSQL** (si no lo tienes):
   - Descarga: https://www.postgresql.org/download/windows/
   - Ejecuta el instalador
   - Usa puerto: 5432, contraseña: la que prefieras

2. **Crear base de datos**:
   ```bash
   # Abre PostgreSQL desde el menú inicio o ejecuta:
   psql -U postgres

   # En el prompt de PostgreSQL:
   CREATE DATABASE "RndcDb_Dev";
   \q
   ```

3. **Configurar conexión local**:

   Crea el archivo: `src/ApiRndc.Web/appsettings.Development.json`

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=RndcDb_Dev;Username=postgres;Password=TU_PASSWORD"
     }
   }
   ```

   ⚠️ Reemplaza `TU_PASSWORD` con tu contraseña de PostgreSQL

4. **Aplicar migraciones**:
   ```bash
   dotnet ef database update --project src/ApiRndc.Infrastructure --startup-project src/ApiRndc.Web
   ```

#### Opción B: Usar Script SQL Generado

Si prefieres aplicar el script manualmente:

```bash
# Conectarse a PostgreSQL
psql -U postgres -d RndcDb_Dev

# Ejecutar el script
\i src/ApiRndc.Infrastructure/Data/Migrations/InitialCreate.sql

# Salir
\q
```

#### Opción C: Base de Datos Remota (Requiere VPN/Acceso)

Si tienes acceso a la red donde está el servidor remoto:

1. Verifica conectividad:
   ```bash
   psql -h 201.184.51.27 -p 5401 -U fmontoya -d RndcDb
   ```

2. Si conecta, aplica migraciones:
   ```bash
   dotnet ef database update --project src/ApiRndc.Infrastructure --startup-project src/ApiRndc.Web
   ```

📖 **Documentación detallada**: Ver `docs/CONFIGURACION_BD.md`

---

### Paso 2: Ejecutar la Aplicación

Una vez configurada la base de datos:

```bash
# Desde la raíz del proyecto
dotnet run --project src/ApiRndc.Web

# O con hot reload (recomendado para desarrollo)
dotnet watch run --project src/ApiRndc.Web
```

Abre tu navegador en:
- **HTTPS**: https://localhost:7000
- **HTTP**: http://localhost:5000

---

### Paso 3: Crear Usuario Administrador

La aplicación crea automáticamente los roles al iniciar, pero necesitas crear el primer usuario:

#### Método 1: Interfaz Web

1. Navega a: https://localhost:7000/Account/Register
2. Crea un usuario con:
   - Email: admin@apirndc.com
   - Contraseña: Admin123! (o la que prefieras, mínimo 8 caracteres)

3. Asignar rol de Administrador (usando SQL):
   ```sql
   -- Conectar a la base de datos
   psql -U postgres -d RndcDb_Dev

   -- Ver usuarios y roles
   SELECT "Id", "Email" FROM public."AspNetUsers";
   SELECT "Id", "Name" FROM public."AspNetRoles";

   -- Asignar rol (reemplaza los IDs)
   INSERT INTO public."AspNetUserRoles" ("UserId", "RoleId")
   VALUES ('ID_DEL_USUARIO', 'ID_DEL_ROL_ADMINISTRADOR');
   ```

#### Método 2: Agregar código temporal al Program.cs

Descomentar y agregar después de la creación de roles en Program.cs:

```csharp
// Crear usuario administrador (solo desarrollo)
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
var adminEmail = "admin@apirndc.com";
var adminUser = await userManager.FindByEmailAsync(adminEmail);

if (adminUser == null)
{
    adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
    await userManager.CreateAsync(adminUser, "Admin123!");
    await userManager.AddToRoleAsync(adminUser, "Administrador");
}
```

---

### Paso 4: Probar Registro de Tercero

Una vez dentro del sistema:

1. **Login**: https://localhost:7000/Account/Login
2. **Crear un tercero de prueba**:

   Endpoint: `POST /api/terceros/registrar` (cuando esté implementado)

   O usar directamente MediatR en un endpoint de prueba:

```csharp
var terceroDto = new TerceroDto
{
    NumNitEmpresaTransporte = "860504882",
    CodTipoIdTercero = "N",
    NumIdTercero = "75145705",
    NomIdTercero = "Francisco",
    PrimerApellidoIdTercero = "Montoya",
    SegundoApellidoIdTercero = "Lopez",
    NumTelefonoContacto = "3166917374",
    NomenclaturaDireccion = "Carrera 59 # 70-125",
    CodMunicipioRndc = "5023112",
    Latitud = 4.563256m,
    Longitud = -74.14451m,
    RegimenSimple = "N"
};

var command = new RegistrarTerceroCommand { Tercero = terceroDto };
var result = await mediator.Send(command);

Console.WriteLine($"Success: {result.Success}");
Console.WriteLine($"IngresoId: {result.IngresoId}");
```

---

## 📁 Estructura del Proyecto

```
ApiRndc/
├── src/
│   ├── ApiRndc.Domain/              ✅ Entidades, enums, interfaces
│   ├── ApiRndc.Application/         ✅ Comandos, handlers (MediatR)
│   ├── ApiRndc.Infrastructure/      ✅ DbContext, Repositories, SOAP Client
│   │   └── Data/Migrations/         ✅ Migraciones de EF Core
│   ├── ApiRndc.Shared/              ✅ DTOs y constantes
│   └── ApiRndc.Web/                 ✅ Blazor Server (UI)
├── tests/
│   └── ApiRndc.UnitTests/           ✅ Pruebas unitarias
└── docs/
    ├── INSTALACION.md               ✅ Guía completa de instalación
    └── CONFIGURACION_BD.md          ✅ Guía de configuración de BD
```

---

## 🔧 Comandos Útiles

```bash
# Compilar solución
dotnet build ApiRndc.sln

# Ejecutar aplicación
dotnet run --project src/ApiRndc.Web

# Ejecutar con hot reload
dotnet watch run --project src/ApiRndc.Web

# Ver migraciones
dotnet ef migrations list --project src/ApiRndc.Infrastructure --startup-project src/ApiRndc.Web

# Crear nueva migración
dotnet ef migrations add NombreMigracion --project src/ApiRndc.Infrastructure --startup-project src/ApiRndc.Web

# Aplicar migraciones
dotnet ef database update --project src/ApiRndc.Infrastructure --startup-project src/ApiRndc.Web

# Revertir última migración
dotnet ef database update PreviousMigration --project src/ApiRndc.Infrastructure --startup-project src/ApiRndc.Web

# Generar script SQL
dotnet ef migrations script --project src/ApiRndc.Infrastructure --startup-project src/ApiRndc.Web --output migration.sql

# Ejecutar pruebas
dotnet test

# Limpiar solución
dotnet clean ApiRndc.sln
```

---

## 📝 Archivos de Configuración

### appsettings.json (Producción)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=201.184.51.27;Port=5401;Database=RndcDb;Username=fmontoya;Password=F935cjm9262"
  },
  "Rndc": {
    "Username": "TYSROBOT@0764",
    "Password": "@Tys860504882@",
    "ServiceUrl": "http://rndcws.mintransporte.gov.co:8080/soap/IBPMServices"
  }
}
```

### appsettings.Development.json (Desarrollo - Crear este archivo)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=RndcDb_Dev;Username=postgres;Password=TU_PASSWORD"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug"
    }
  }
}
```

---

## 🛠️ Tareas Pendientes de Implementación

El proyecto tiene la estructura base completa, pero faltan estos componentes:

### Alta Prioridad
- [ ] Páginas Blazor para la UI (Login, Dashboard, Formularios)
- [ ] Handlers para las demás transacciones (Vehículos, Remesas, Manifiestos, Cumplidos)
- [ ] Validadores con FluentValidation
- [ ] Endpoints de API (Controllers o Minimal API)

### Media Prioridad
- [ ] Servicio de reintentos automáticos en segundo plano
- [ ] Exportación de reportes (Excel con EPPlus, PDF con QuestPDF)
- [ ] Componentes MudBlazor personalizados

### Baja Prioridad
- [ ] Pruebas unitarias e integración
- [ ] Caché de consultas frecuentes
- [ ] Documentación de API (Swagger)

---

## 📚 Documentación

- **README.md**: Documentación general del proyecto
- **docs/INSTALACION.md**: Guía completa de instalación y despliegue
- **docs/CONFIGURACION_BD.md**: Guía detallada de configuración de base de datos

---

## ❓ Problemas Comunes

### "Connection refused" al ejecutar
➡️ Verifica que PostgreSQL esté corriendo y que la cadena de conexión sea correcta

### "Cannot find module"
➡️ Ejecuta `dotnet restore`

### "Migration already applied"
➡️ La migración ya fue aplicada, no necesitas hacer nada

### Logs no se guardan
➡️ Verifica que la carpeta `Logs/` tenga permisos de escritura

---

## 🆘 ¿Necesitas Ayuda?

1. Revisa los logs en: `Logs/log-[fecha].txt`
2. Consulta la documentación en `docs/`
3. Verifica que todos los servicios estén corriendo (PostgreSQL, etc.)

---

## ✅ Checklist de Inicio

- [ ] PostgreSQL instalado y corriendo
- [ ] Base de datos creada (`RndcDb_Dev`)
- [ ] Archivo `appsettings.Development.json` creado con cadena de conexión local
- [ ] Migraciones aplicadas exitosamente
- [ ] Aplicación ejecutándose en https://localhost:7000
- [ ] Usuario administrador creado
- [ ] Login exitoso

Una vez completado este checklist, estarás listo para comenzar a desarrollar las funcionalidades restantes.

---

**Fecha**: 20 de Octubre de 2025
**Versión del Proyecto**: 1.0 (Base)
**Estado**: Estructura completa, pendiente de implementación de UI y funcionalidades
