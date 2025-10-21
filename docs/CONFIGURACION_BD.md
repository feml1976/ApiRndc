# Configuración de Base de Datos - ApiRndc

## Opciones de Configuración

Tienes dos opciones para configurar la base de datos:

### Opción 1: Base de Datos Local (Recomendado para Desarrollo)
### Opción 2: Base de Datos Remota (Producción)

---

## Opción 1: Configuración Local con PostgreSQL

### 1. Instalar PostgreSQL

Si aún no tienes PostgreSQL instalado:

1. Descarga PostgreSQL desde: https://www.postgresql.org/download/windows/
2. Ejecuta el instalador
3. Durante la instalación:
   - Puerto: **5432** (predeterminado)
   - Contraseña de postgres: Ingresa una contraseña segura (recuérdala)
   - Locale: Spanish, Colombia

### 2. Crear Base de Datos Local

Opción A: Usando pgAdmin (Interfaz Gráfica)

1. Abre **pgAdmin 4**
2. Expande **Servers > PostgreSQL 16**
3. Click derecho en **Databases** > **Create > Database**
4. Configuración:
   - Database: `RndcDb_Dev`
   - Owner: `postgres`
   - Encoding: `UTF8`
5. Click **Save**

Opción B: Usando línea de comandos

```bash
# Conectarse a PostgreSQL
psql -U postgres

# Crear base de datos
CREATE DATABASE "RndcDb_Dev"
    WITH
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'Spanish_Colombia.1252'
    LC_CTYPE = 'Spanish_Colombia.1252'
    CONNECTION LIMIT = -1;

# Salir
\q
```

### 3. Configurar Cadena de Conexión Local

Edita el archivo: `src/ApiRndc.Web/appsettings.Development.json`

Crea el archivo si no existe:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=RndcDb_Dev;Username=postgres;Password=TU_PASSWORD_POSTGRES"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug"
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information"
    }
  }
}
```

**IMPORTANTE**: Reemplaza `TU_PASSWORD_POSTGRES` con la contraseña que configuraste durante la instalación.

### 4. Aplicar Migraciones

Ahora aplica las migraciones a tu base de datos local:

```bash
# Opción A: Usando dotnet ef (Recomendado)
dotnet ef database update --project src/ApiRndc.Infrastructure --startup-project src/ApiRndc.Web --context ApplicationDbContext

# Opción B: Usando el script SQL generado
psql -U postgres -d RndcDb_Dev -f src/ApiRndc.Infrastructure/Data/Migrations/InitialCreate.sql
```

### 5. Verificar Tablas Creadas

```bash
# Conectarse a la base de datos
psql -U postgres -d RndcDb_Dev

# Listar tablas
\dt

# Deberías ver:
# public | AspNetRoles
# public | AspNetRoleClaims
# public | AspNetUsers
# public | AspNetUserClaims
# public | AspNetUserLogins
# public | AspNetUserRoles
# public | AspNetUserTokens
# public | manifiesto_remesas
# public | manifiestos
# public | remesas
# public | rndc_transactions
# public | terceros
# public | vehiculos
# public | __EFMigrationsHistory

# Salir
\q
```

---

## Opción 2: Configuración de Base de Datos Remota

### Conexión Actual (Producción)

La cadena de conexión está configurada en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=201.184.51.27;Port=5401;Database=RndcDb;Username=fmontoya;Password=F935cjm9262"
  }
}
```

### Problemas de Conexión Remota

Si no puedes conectar a la base de datos remota, verifica:

1. **Firewall**: El puerto 5401 debe estar abierto
2. **PostgreSQL**: Debe permitir conexiones remotas
3. **VPN**: Puede que necesites estar conectado a una VPN corporativa
4. **pg_hba.conf**: Debe permitir tu IP

### Configurar PostgreSQL para Conexiones Remotas

Si tienes acceso al servidor PostgreSQL remoto:

#### 1. Editar postgresql.conf

```bash
# Ubicación típica: C:\Program Files\PostgreSQL\16\data\postgresql.conf
# O en Linux: /etc/postgresql/16/main/postgresql.conf

# Buscar y modificar:
listen_addresses = '*'  # Escuchar en todas las interfaces
port = 5401             # Puerto personalizado
```

#### 2. Editar pg_hba.conf

```bash
# Agregar al final del archivo pg_hba.conf:

# TYPE  DATABASE        USER            ADDRESS                 METHOD
host    RndcDb          fmontoya        0.0.0.0/0              md5
host    all             all             0.0.0.0/0              md5
```

#### 3. Reiniciar PostgreSQL

```bash
# Windows
net stop postgresql-x64-16
net start postgresql-x64-16

# Linux
sudo systemctl restart postgresql
```

#### 4. Abrir Puerto en Firewall

Windows:
```powershell
netsh advfirewall firewall add rule name="PostgreSQL Remote" dir=in action=allow protocol=TCP localport=5401
```

Linux:
```bash
sudo ufw allow 5401/tcp
```

### Aplicar Migraciones a Base de Datos Remota

Una vez que la conexión funcione:

```bash
dotnet ef database update --project src/ApiRndc.Infrastructure --startup-project src/ApiRndc.Web --context ApplicationDbContext
```

O ejecuta manualmente el script SQL:

```bash
psql -h 201.184.51.27 -p 5401 -U fmontoya -d RndcDb -f src/ApiRndc.Infrastructure/Data/Migrations/InitialCreate.sql
```

---

## Solución de Problemas

### Error: "Timeout during connection attempt"

**Causa**: No se puede conectar al servidor PostgreSQL

**Soluciones**:
1. Verifica que PostgreSQL esté corriendo:
   ```bash
   # Windows
   sc query postgresql-x64-16

   # Linux
   sudo systemctl status postgresql
   ```

2. Verifica la cadena de conexión en appsettings
3. Verifica firewall local y del servidor
4. Prueba conectividad:
   ```bash
   # Probar conexión local
   psql -U postgres -d RndcDb_Dev

   # Probar conexión remota
   psql -h 201.184.51.27 -p 5401 -U fmontoya -d RndcDb
   ```

### Error: "password authentication failed"

**Causa**: Contraseña incorrecta

**Solución**:
1. Verifica la contraseña en appsettings.json
2. Restablece la contraseña si es necesario:
   ```sql
   ALTER USER postgres WITH PASSWORD 'nueva_password';
   ```

### Error: "database does not exist"

**Causa**: La base de datos no fue creada

**Solución**:
```bash
createdb -U postgres RndcDb_Dev
```

### Error: "relation already exists"

**Causa**: Intentando aplicar migraciones cuando las tablas ya existen

**Solución**:
```bash
# Eliminar todas las tablas y empezar de nuevo
psql -U postgres -d RndcDb_Dev

DROP SCHEMA public CASCADE;
CREATE SCHEMA public;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO public;

\q

# Aplicar migraciones nuevamente
dotnet ef database update --project src/ApiRndc.Infrastructure --startup-project src/ApiRndc.Web
```

---

## Migrar de Desarrollo a Producción

Cuando estés listo para migrar de tu BD local a producción:

### 1. Generar Script SQL

```bash
dotnet ef migrations script --project src/ApiRndc.Infrastructure --startup-project src/ApiRndc.Web --context ApplicationDbContext --output migration.sql --idempotent
```

### 2. Revisar Script

Abre `migration.sql` y revisa que todo esté correcto

### 3. Ejecutar en Producción

```bash
psql -h 201.184.51.27 -p 5401 -U fmontoya -d RndcDb -f migration.sql
```

### 4. Actualizar appsettings

En producción, usa variables de entorno en lugar de appsettings.json

---

## Respaldar Base de Datos

### Crear Respaldo

```bash
# Local
pg_dump -U postgres -d RndcDb_Dev -F c -f backup_$(date +%Y%m%d).backup

# Remoto
pg_dump -h 201.184.51.27 -p 5401 -U fmontoya -d RndcDb -F c -f backup_prod_$(date +%Y%m%d).backup
```

### Restaurar desde Respaldo

```bash
pg_restore -U postgres -d RndcDb_Dev backup_20251020.backup
```

---

## Verificar Estado de Migraciones

```bash
# Ver migraciones aplicadas
dotnet ef migrations list --project src/ApiRndc.Infrastructure --startup-project src/ApiRndc.Web

# Ver SQL que se ejecutaría
dotnet ef migrations script --project src/ApiRndc.Infrastructure --startup-project src/ApiRndc.Web
```

---

## Próximos Pasos

Una vez que la base de datos esté configurada:

✅ Continúa con el paso 2: Ejecutar la aplicación
✅ Ver: `docs/INSTALACION.md` para instrucciones completas

---

**Última actualización**: 20 de Octubre de 2025
