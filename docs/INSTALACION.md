# Guía de Instalación y Configuración - ApiRndc

## Tabla de Contenidos
1. [Requisitos Previos](#requisitos-previos)
2. [Instalación Paso a Paso](#instalación-paso-a-paso)
3. [Configuración de Base de Datos](#configuración-de-base-de-datos)
4. [Configuración de Credenciales](#configuración-de-credenciales)
5. [Primera Ejecución](#primera-ejecución)
6. [Crear Usuario Administrador](#crear-usuario-administrador)
7. [Despliegue en IIS](#despliegue-en-iis)
8. [Solución de Problemas Comunes](#solución-de-problemas-comunes)

---

## Requisitos Previos

### Software Necesario

1. **.NET 9.0 SDK**
   - Descarga: https://dotnet.microsoft.com/download/dotnet/9.0
   - Verificar instalación: `dotnet --version`

2. **PostgreSQL 12 o superior**
   - Descarga: https://www.postgresql.org/download/
   - Versión recomendada: PostgreSQL 16

3. **Visual Studio 2022 (Opcional pero recomendado)**
   - Edición Community (gratuita): https://visualstudio.microsoft.com/es/
   - Workloads requeridos:
     - ASP.NET y desarrollo web
     - Desarrollo multiplataforma de .NET

4. **Git (Opcional)**
   - Para control de versiones
   - Descarga: https://git-scm.com/downloads

### Verificar Instalaciones

Abre una terminal/CMD y ejecuta:

```bash
# Verificar .NET
dotnet --version
# Debe mostrar: 9.0.306 o superior

# Verificar PostgreSQL
psql --version
# Debe mostrar: psql (PostgreSQL) 12.x o superior
```

---

## Instalación Paso a Paso

### 1. Obtener el Código Fuente

```bash
# Si tienes Git instalado
git clone [URL_DEL_REPOSITORIO] C:\ApiRndc

# O simplemente copia la carpeta del proyecto a C:\ApiRndc
```

### 2. Restaurar Paquetes NuGet

```bash
cd C:\Dirsop\Proyectos\Net\Pruebas\ApiRndc
dotnet restore ApiRndc.sln
```

Este comando descargará todos los paquetes necesarios:
- Entity Framework Core
- PostgreSQL provider (Npgsql)
- Serilog
- MudBlazor
- MediatR
- FluentValidation
- Y sus dependencias

### 3. Compilar la Solución

```bash
dotnet build ApiRndc.sln --configuration Release
```

Deberías ver:
```
Compilación correcta.
    0 Advertencia(s)
    0 Errores
```

---

## Configuración de Base de Datos

### Opción A: Desarrollo Local

#### 1. Crear Base de Datos en PostgreSQL

Abre pgAdmin o usa la terminal:

```sql
-- Conectarse a PostgreSQL
psql -U postgres

-- Crear base de datos
CREATE DATABASE "RndcDb_Dev"
    WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'Spanish_Colombia.1252'
    LC_CTYPE = 'Spanish_Colombia.1252'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

-- Crear usuario (opcional)
CREATE USER rndc_user WITH PASSWORD 'tu_password_seguro';
GRANT ALL PRIVILEGES ON DATABASE "RndcDb_Dev" TO rndc_user;
```

#### 2. Configurar Cadena de Conexión

Edita `src/ApiRndc.Web/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=RndcDb_Dev;Username=postgres;Password=tu_password"
  }
}
```

### Opción B: Producción (Base de Datos Remota)

La cadena de conexión ya está configurada en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=201.184.51.27;Port=5401;Database=RndcDb;Username=fmontoya;Password=F935cjm9262"
  }
}
```

### 3. Aplicar Migraciones de Entity Framework

```bash
# Instalar herramienta de EF Core (si no la tienes)
dotnet tool install --global dotnet-ef

# Crear migración inicial
dotnet ef migrations add InitialCreate \
  --project src/ApiRndc.Infrastructure \
  --startup-project src/ApiRndc.Web \
  --context ApplicationDbContext \
  --output-dir Data/Migrations

# Aplicar migración a la base de datos
dotnet ef database update \
  --project src/ApiRndc.Infrastructure \
  --startup-project src/ApiRndc.Web \
  --context ApplicationDbContext
```

Esto creará todas las tablas necesarias:
- `rndc_transactions`
- `terceros`
- `vehiculos`
- `remesas`
- `manifiestos`
- `manifiesto_remesas`
- Tablas de Identity (usuarios, roles, etc.)

---

## Configuración de Credenciales

### Opción 1: User Secrets (Recomendado para Desarrollo)

```bash
cd src/ApiRndc.Web

# Inicializar secrets
dotnet user-secrets init

# Configurar credenciales del RNDC
dotnet user-secrets set "Rndc:Username" "TYSROBOT@0764"
dotnet user-secrets set "Rndc:Password" "@Tys860504882@"

# Configurar cadena de conexión
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=RndcDb_Dev;Username=postgres;Password=tu_password"
```

### Opción 2: Variables de Entorno (Producción)

En Windows (IIS):
1. Abre **IIS Manager**
2. Selecciona el sitio ApiRndc
3. Haz doble clic en **Configuration Editor**
4. En **Section**, selecciona `system.webServer/aspNetCore`
5. Click en **environmentVariables**
6. Agrega:
   ```
   ASPNETCORE_ENVIRONMENT = Production
   Rndc__Username = TYSROBOT@0764
   Rndc__Password = @Tys860504882@
   ConnectionStrings__DefaultConnection = [tu_cadena_de_conexión]
   ```

### Opción 3: appsettings.json (Solo para pruebas)

⚠️ **NO recomendado para producción** - Las credenciales quedan expuestas

El archivo ya está configurado con las credenciales de prueba.

---

## Primera Ejecución

### 1. Ejecutar la Aplicación

```bash
cd C:\Dirsop\Proyectos\Net\Pruebas\ApiRndc
dotnet run --project src/ApiRndc.Web
```

O con hot reload (recomendado para desarrollo):

```bash
dotnet watch run --project src/ApiRndc.Web
```

Verás una salida similar a:

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7000
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

### 2. Acceder a la Aplicación

Abre un navegador y navega a:
- **HTTPS**: `https://localhost:7000`
- **HTTP**: `http://localhost:5000`

---

## Crear Usuario Administrador

### Método 1: Interfaz Web (Recomendado)

1. En el navegador, navega a `/register`
2. Completa el formulario:
   - **Email**: admin@apirndc.com
   - **Contraseña**: Minimo8Caracteres! (debe cumplir requisitos)
   - **Confirmar Contraseña**
3. Click en **Registrar**

4. Asignar rol Administrador mediante SQL:

```sql
-- Conectarse a la base de datos
psql -U postgres -d RndcDb_Dev

-- Obtener IDs
SELECT "Id", "UserName", "Email" FROM "AspNetUsers";
SELECT "Id", "Name" FROM "AspNetRoles" WHERE "Name" = 'Administrador';

-- Asignar rol (reemplaza los IDs)
INSERT INTO "AspNetUserRoles" ("UserId", "RoleId")
VALUES
  ('id_del_usuario', 'id_del_rol_administrador');
```

### Método 2: Mediante Código (Avanzado)

Agrega este código temporal al `Program.cs` después de crear los roles:

```csharp
// Crear usuario administrador por defecto (solo desarrollo)
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var adminEmail = "admin@apirndc.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Administrador");
                Log.Information("Usuario administrador creado: {Email}", adminEmail);
            }
        }
    }
}
```

---

## Despliegue en IIS

### 1. Publicar la Aplicación

```bash
# Crear carpeta de publicación
mkdir C:\inetpub\ApiRndc

# Publicar en modo Release
dotnet publish src/ApiRndc.Web/ApiRndc.Web.csproj \
  -c Release \
  -o C:\inetpub\ApiRndc \
  --self-contained false \
  --runtime win-x64
```

### 2. Instalar ASP.NET Core Hosting Bundle

1. Descarga el **Hosting Bundle**: https://dotnet.microsoft.com/download/dotnet/9.0
2. Ejecuta el instalador
3. Reinicia IIS:
   ```bash
   net stop was /y
   net start w3svc
   ```

### 3. Configurar Sitio en IIS

#### A. Abrir IIS Manager

```bash
# Desde CMD o PowerShell
inetmgr
```

#### B. Crear Application Pool

1. Click derecho en **Application Pools** > **Add Application Pool**
2. Configuración:
   - **Name**: ApiRndcPool
   - **.NET CLR Version**: No Managed Code
   - **Managed Pipeline Mode**: Integrated
3. Click **OK**
4. Click derecho en **ApiRndcPool** > **Advanced Settings**
   - **Start Mode**: AlwaysRunning
   - **Identity**: ApplicationPoolIdentity

#### C. Crear Sitio Web

1. Click derecho en **Sites** > **Add Website**
2. Configuración:
   - **Site name**: ApiRndc
   - **Application pool**: ApiRndcPool
   - **Physical path**: C:\inetpub\ApiRndc
   - **Binding**:
     - Type: http
     - Port: 80
     - Host name: (dejar en blanco o especificar dominio)
3. Click **OK**

### 4. Configurar Permisos

```powershell
# Dar permisos de lectura al Application Pool
icacls "C:\inetpub\ApiRndc" /grant "IIS AppPool\ApiRndcPool:(OI)(CI)R" /T

# Dar permisos de escritura a la carpeta Logs
mkdir C:\inetpub\ApiRndc\Logs
icacls "C:\inetpub\ApiRndc\Logs" /grant "IIS AppPool\ApiRndcPool:(OI)(CI)M" /T
```

### 5. Configurar HTTPS (Opcional pero Recomendado)

#### Opción A: Certificado Autofirmado (Solo para pruebas)

```powershell
# Crear certificado
$cert = New-SelfSignedCertificate -DnsName "apirndc.local" -CertStoreLocation "cert:\LocalMachine\My"

# Obtener thumbprint
$cert.Thumbprint
```

En IIS Manager:
1. Selecciona el sitio **ApiRndc**
2. Click derecho > **Edit Bindings**
3. Click **Add**
   - Type: https
   - Port: 443
   - SSL certificate: [Selecciona el certificado creado]

#### Opción B: Let's Encrypt (Gratuito para producción)

1. Instala **win-acme**: https://www.win-acme.com/
2. Ejecuta `wacs.exe` y sigue el asistente

### 6. Verificar Instalación

1. Abre navegador
2. Navega a `http://localhost` o `https://localhost`
3. Deberías ver la página principal de ApiRndc

---

## Solución de Problemas Comunes

### Error: "Failed to start application"

**Causa**: Hosting Bundle no instalado o versión incorrecta

**Solución**:
```bash
# Verificar módulo en IIS
%systemroot%\system32\inetsrv\appcmd.exe list modules

# Reinstalar Hosting Bundle
# Descargar de https://dotnet.microsoft.com/download/dotnet/9.0
# Ejecutar instalador
# Reiniciar IIS
net stop was /y
net start w3svc
```

### Error: "Connection refused" (PostgreSQL)

**Causa**: PostgreSQL no está corriendo o firewall bloquea conexión

**Solución**:
```bash
# Verificar estado de PostgreSQL
sc query postgresql-x64-16

# Iniciar servicio
sc start postgresql-x64-16

# Verificar firewall
netsh advfirewall firewall add rule name="PostgreSQL" dir=in action=allow protocol=TCP localport=5432
```

### Error: "Cannot create shadow copy"

**Causa**: Permisos insuficientes en carpeta temporal

**Solución**:
```powershell
# Crear carpeta temporal
mkdir C:\inetpub\temp\ApiRndc

# Dar permisos
icacls "C:\inetpub\temp\ApiRndc" /grant "IIS AppPool\ApiRndcPool:(OI)(CI)F" /T

# Modificar web.config para usar esta carpeta
# Agregar dentro de <system.webServer><aspNetCore>:
# <aspNetCore shadowCopyDirectory="C:\inetpub\temp\ApiRndc" />
```

### La aplicación funciona pero no guarda logs

**Causa**: Sin permisos de escritura en carpeta Logs

**Solución**:
```powershell
icacls "C:\inetpub\ApiRndc\Logs" /grant "IIS AppPool\ApiRndcPool:(OI)(CI)M" /T
```

### Error: "Unable to connect to RNDC service"

**Causa**: Credenciales incorrectas o servicio RNDC caído

**Solución**:
1. Verificar credenciales en appsettings.json
2. Probar conectividad:
   ```bash
   curl http://rndcws.mintransporte.gov.co:8080/soap/IBPMServices
   ```
3. Revisar logs en `Logs/log-[fecha].txt`

### Error: "Violación de restricción de clave foránea"

**Causa**: Intentando eliminar un registro referenciado

**Solución**:
- El sistema usa "soft delete" (IsActive = false)
- No se eliminan registros físicamente
- Verificar la lógica de eliminación en el código

### La página se carga muy lento

**Posibles causas y soluciones**:

1. **Consultas lentas a BD**:
   ```sql
   -- Crear índices
   CREATE INDEX idx_transactions_status ON rndc_transactions(status);
   CREATE INDEX idx_transactions_created ON rndc_transactions(created_at);
   ```

2. **Logs excesivos**:
   - Cambiar nivel de log en appsettings.json:
   ```json
   {
     "Serilog": {
       "MinimumLevel": {
         "Default": "Warning"
       }
     }
   }
   ```

3. **Sin caché**:
   - Verificar que IMemoryCache esté configurado
   - Implementar caché para consultas frecuentes

---

## Próximos Pasos

Una vez instalado y configurado:

1. ✅ **Iniciar sesión** con usuario administrador
2. ✅ **Crear usuarios** para operadores y consultores
3. ✅ **Registrar terceros** (conductores, propietarios)
4. ✅ **Registrar vehículos** de la flota
5. ✅ **Crear remesas** de carga
6. ✅ **Generar manifiestos** con remesas asociadas
7. ✅ **Registrar cumplidos** de entregas
8. ✅ **Exportar reportes** en Excel/PDF
9. ✅ **Monitorear logs** para detectar problemas

---

## Recursos Adicionales

- **Documentación completa**: Ver `README.md`
- **Arquitectura del sistema**: Ver `docs/ARQUITECTURA.md` (crear si es necesario)
- **API del RNDC**: http://rndcws.mintransporte.gov.co:8080/soap/IBPMServices?wsdl
- **Soporte .NET**: https://learn.microsoft.com/es-es/aspnet/core/

---

## Contacto

Para problemas o consultas:
- **Email técnico**: soporte@apirndc.com
- **Documentación**: C:\Dirsop\Proyectos\Net\Pruebas\ApiRndc\README.md

---

**Versión del documento**: 1.0
**Última actualización**: 20 de Octubre de 2025
