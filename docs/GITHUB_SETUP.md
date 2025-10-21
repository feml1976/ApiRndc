# Guía para Respaldar el Proyecto ApiRndc en GitHub

Esta guía detalla el proceso paso a paso para subir y respaldar el proyecto ApiRndc en GitHub.

## 📋 Requisitos Previos

Antes de comenzar, asegúrese de tener:

1. **Git instalado** en su sistema
   - Descargar desde: https://git-scm.com/downloads
   - Verificar instalación: Abrir CMD y ejecutar `git --version`

2. **Cuenta de GitHub**
   - Si no tiene una, créela en: https://github.com/signup

3. **Git configurado** con su información
   ```bash
   git config --global user.name "Su Nombre"
   git config --global user.email "su-email@ejemplo.com"
   ```

---

## 🚀 Paso 1: Crear Repositorio en GitHub

### 1.1 Iniciar sesión en GitHub
1. Vaya a https://github.com
2. Inicie sesión con sus credenciales

### 1.2 Crear nuevo repositorio
1. Haga clic en el botón **"+"** en la esquina superior derecha
2. Seleccione **"New repository"**

### 1.3 Configurar el repositorio
- **Repository name:** `ApiRndc` (o el nombre que prefiera)
- **Description:** `Sistema de integración con RNDC Colombia - Gestión de transporte de carga`
- **Visibility:**
  - ✅ **Private** (recomendado para código empresarial)
  - ⚠️ **Public** (solo si desea que sea público)
- **Initialize repository:**
  - ❌ **NO** marcar "Add a README file"
  - ❌ **NO** marcar "Add .gitignore"
  - ❌ **NO** marcar "Choose a license"

  > **Importante:** No inicialice con ningún archivo, ya que subiremos el proyecto existente

4. Haga clic en **"Create repository"**

### 1.4 Copiar la URL del repositorio
Después de crear el repositorio, GitHub mostrará la URL. Cópiela, será algo como:
```
https://github.com/su-usuario/ApiRndc.git
```

---

## 📝 Paso 2: Preparar el Proyecto Local

### 2.1 Crear archivo .gitignore

El archivo `.gitignore` evita que archivos innecesarios o sensibles se suban a GitHub.

**Opción A - Crear .gitignore manualmente:**

1. Abra el CMD o PowerShell
2. Navegue a la carpeta del proyecto:
   ```cmd
   cd C:\Dirsop\Proyectos\Net\Pruebas\ApiRndc
   ```

3. Cree el archivo .gitignore:
   ```cmd
   type nul > .gitignore
   ```

4. Abra el archivo con un editor de texto (Notepad, VS Code, etc.)

5. Pegue el siguiente contenido:

```gitignore
# Build results
[Dd]ebug/
[Dd]ebugPublic/
[Rr]elease/
[Rr]eleases/
x64/
x86/
[Ww][Ii][Nn]32/
[Aa][Rr][Mm]/
[Aa][Rr][Mm]64/
bld/
[Bb]in/
[Oo]bj/
[Ll]og/
[Ll]ogs/

# Visual Studio cache/options
.vs/
.vscode/
*.rsuser
*.suo
*.user
*.userosscache
*.sln.docstates

# User-specific files
*.suo
*.user
*.sln.docstates
*.userprefs

# Build Results
*_i.c
*_p.c
*_h.h
*.ilk
*.meta
*.obj
*.iobj
*.pch
*.pdb
*.ipdb
*.pgc
*.pgd
*.rsp
*.sbr
*.tlb
*.tli
*.tlh
*.tmp
*.tmp_proj
*_wpftmp.csproj
*.log
*.tlog
*.vspscc
*.vssscc
.builds
*.pidb
*.svclog
*.scc

# NuGet Packages
*.nupkg
*.snupkg
**/packages/*
!**/packages/build/
*.nuget.props
*.nuget.targets
project.lock.json
project.fragment.lock.json
artifacts/

# .NET Core
project.lock.json
project.fragment.lock.json
artifacts/

# ASP.NET Scaffolding
ScaffoldingReadMe.txt

# Files built by Visual Studio
*_i.c
*_p.c
*_h.h
*.ilk
*.obj
*.iobj
*.pch
*.pdb
*.ipdb
*.pgc
*.pgd
*.rsp
*.sbr
*.tlb
*.tli
*.tlh
*.tmp
*.tmp_proj
*_wpftmp.csproj
*.log
*.tlog
*.vspscc
*.vssscc
.builds
*.pidb
*.svclog
*.scc

# Click-Once directory
publish/

# Publish Web Output
*.[Pp]ublish.xml
*.azurePubxml
*.pubxml
*.publishproj

# Microsoft Azure Web App publish settings
PublishScripts/

# NuGet Packages
*.nupkg
*.snupkg
**/[Pp]ackages/*
!**/[Pp]ackages/build/

# Azure Functions localsettings file
local.settings.json

# Python Tools for Visual Studio (PTVS)
__pycache__/
*.pyc

# Node modules
node_modules/

# JetBrains Rider
.idea/
*.sln.iml

# Database files
*.db
*.db-shm
*.db-wal

# Sensitive configuration files
appsettings.Development.json
appsettings.Production.json
appsettings.*.json
!appsettings.json

# Environment files
.env
.env.local
.env.production

# Logs
logs/
*.log
npm-debug.log*
yarn-debug.log*
yarn-error.log*

# OS files
.DS_Store
Thumbs.db
Desktop.ini

# Backup files
*.bak
*.backup
*~
```

6. **Guarde el archivo**

**Opción B - Descargar .gitignore oficial de .NET:**

Puede descargar el `.gitignore` oficial desde:
https://github.com/github/gitignore/blob/main/VisualStudio.gitignore

### 2.2 Crear archivo README.md (Opcional pero recomendado)

Cree un archivo `README.md` en la raíz del proyecto con información básica:

```markdown
# ApiRndc - Sistema de Integración RNDC Colombia

Sistema de gestión para integración con el Registro Nacional de Despacho de Carga (RNDC) de Colombia.

## 🚀 Tecnologías

- .NET 9.0
- Blazor Server
- PostgreSQL
- MudBlazor
- Entity Framework Core
- MediatR (CQRS)

## 📋 Características

- Registro de Terceros (Proceso 11)
- Registro de Vehículos (Proceso 12)
- Expedición de Remesas (Proceso 3)
- Expedición de Manifiestos (Proceso 4)
- Cumplidos de Remesas (Proceso 5)
- Cumplidos de Manifiestos (Proceso 6)

## 🔧 Instalación

Ver documentación completa en `/docs/INSTALACION.md`

## 📝 Licencia

Uso privado - Todos los derechos reservados
```

### 2.3 Proteger información sensible

**IMPORTANTE:** Antes de subir a GitHub, asegúrese de:

1. **Revisar appsettings.json** - Eliminar o reemplazar datos sensibles:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=TU_HOST;Port=5401;Database=RndcDb;Username=TU_USUARIO;Password=TU_PASSWORD"
     },
     "RndcConfig": {
       "ServiceUrl": "http://rndcws.mintransporte.gov.co:8080/soap/IBPMServices",
       "Username": "TU_USUARIO_RNDC",
       "Password": "TU_PASSWORD_RNDC"
     }
   }
   ```

2. **Crear appsettings.Example.json** con valores de ejemplo:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=RndcDb;Username=postgres;Password=YOUR_PASSWORD"
     },
     "RndcConfig": {
       "ServiceUrl": "http://rndcws.mintransporte.gov.co:8080/soap/IBPMServices",
       "Username": "YOUR_RNDC_USERNAME",
       "Password": "YOUR_RNDC_PASSWORD"
     }
   }
   ```

---

## 💻 Paso 3: Inicializar Git en el Proyecto

### 3.1 Abrir terminal en la carpeta del proyecto

**Opción A - Usando CMD:**
```cmd
cd C:\Dirsop\Proyectos\Net\Pruebas\ApiRndc
```

**Opción B - Usando PowerShell:**
```powershell
cd C:\Dirsop\Proyectos\Net\Pruebas\ApiRndc
```

**Opción C - Desde el Explorador de Windows:**
- Navegue a la carpeta `C:\Dirsop\Proyectos\Net\Pruebas\ApiRndc`
- Escriba `cmd` en la barra de direcciones y presione Enter

### 3.2 Inicializar repositorio Git

```bash
git init
```

Verá el mensaje:
```
Initialized empty Git repository in C:/Dirsop/Proyectos/Net/Pruebas/ApiRndc/.git/
```

### 3.3 Configurar rama principal como "main"

```bash
git branch -M main
```

---

## 📤 Paso 4: Agregar Archivos y Realizar Primer Commit

### 4.1 Verificar archivos a incluir

Primero, verifique qué archivos serán agregados:

```bash
git status
```

Verá una lista de archivos en rojo (archivos sin seguimiento).

### 4.2 Agregar todos los archivos al staging

```bash
git add .
```

> **Nota:** El punto (`.`) significa "todos los archivos". Git respetará el `.gitignore` y no incluirá archivos listados allí.

### 4.3 Verificar archivos agregados

```bash
git status
```

Ahora los archivos deberían aparecer en verde (listos para commit).

### 4.4 Realizar el primer commit

```bash
git commit -m "Initial commit - ApiRndc Sistema de Integración RNDC"
```

Verá un resumen de los archivos agregados:
```
[main (root-commit) xxxxxxx] Initial commit - ApiRndc Sistema de Integración RNDC
 XXX files changed, XXXX insertions(+)
```

---

## 🌐 Paso 5: Conectar con GitHub y Subir el Código

### 5.1 Agregar el repositorio remoto

Reemplace `SU-USUARIO` con su nombre de usuario de GitHub:

```bash
git remote add origin https://github.com/SU-USUARIO/ApiRndc.git
```

**Ejemplo:**
```bash
git remote add origin https://github.com/fmontoya/ApiRndc.git
```

### 5.2 Verificar el remoto configurado

```bash
git remote -v
```

Debería ver:
```
origin  https://github.com/SU-USUARIO/ApiRndc.git (fetch)
origin  https://github.com/SU-USUARIO/ApiRndc.git (push)
```

### 5.3 Subir el código a GitHub

```bash
git push -u origin main
```

Se le pedirá autenticación:

**Opción 1 - Usando Token de Acceso Personal (Recomendado):**

1. Vaya a GitHub → Settings → Developer settings → Personal access tokens → Tokens (classic)
2. Click en "Generate new token (classic)"
3. Nombre: `ApiRndc-Token`
4. Seleccione scope: `repo` (full control of private repositories)
5. Click en "Generate token"
6. **COPIE EL TOKEN** (solo se muestra una vez)
7. Cuando Git solicite contraseña, **pegue el token** (no su contraseña de GitHub)

**Opción 2 - Usando GitHub CLI:**
```bash
gh auth login
```

**Opción 3 - Usando Credential Manager (Windows):**
Git abrirá una ventana de autenticación de GitHub. Inicie sesión normalmente.

Después de autenticarse, verá:
```
Enumerating objects: XXX, done.
Counting objects: 100% (XXX/XXX), done.
Delta compression using up to X threads
Compressing objects: 100% (XXX/XXX), done.
Writing objects: 100% (XXX/XXX), XXX KiB | XXX MiB/s, done.
Total XXX (delta XX), reused 0 (delta 0), pack-reused 0
To https://github.com/SU-USUARIO/ApiRndc.git
 * [new branch]      main -> main
Branch 'main' set up to track remote branch 'main' from 'origin'.
```

---

## ✅ Paso 6: Verificar el Repositorio en GitHub

1. Vaya a su navegador
2. Navegue a `https://github.com/SU-USUARIO/ApiRndc`
3. Verifique que todos los archivos estén presentes
4. Revise que el README.md se muestre correctamente

---

## 🔄 Comandos Git para Futuras Actualizaciones

Una vez que el proyecto está en GitHub, use estos comandos para mantenerlo actualizado:

### Verificar estado de cambios
```bash
git status
```

### Agregar archivos modificados
```bash
git add .
# o agregar archivos específicos
git add ruta/al/archivo.cs
```

### Hacer commit de cambios
```bash
git commit -m "Descripción de los cambios realizados"
```

### Subir cambios a GitHub
```bash
git push
```

### Descargar cambios del repositorio remoto
```bash
git pull
```

### Ver historial de commits
```bash
git log
```

### Ver diferencias en archivos
```bash
git diff
```

---

## 🌿 Trabajar con Ramas (Branch Strategy)

### Crear una nueva rama para desarrollo
```bash
git checkout -b develop
```

### Cambiar entre ramas
```bash
git checkout main
git checkout develop
```

### Subir una nueva rama a GitHub
```bash
git push -u origin develop
```

### Fusionar cambios de develop a main
```bash
git checkout main
git merge develop
git push
```

---

## 🔐 Mejores Prácticas de Seguridad

### ✅ HACER:

1. **Usar .gitignore** para excluir archivos sensibles
2. **Usar variables de entorno** para credenciales
3. **Revisar archivos** antes de cada commit con `git status`
4. **Usar tokens de acceso** en lugar de contraseñas
5. **Mantener appsettings.json genérico** y crear appsettings.Example.json
6. **Revisar el repositorio** después del primer push

### ❌ NUNCA:

1. Subir contraseñas o credenciales en archivos
2. Subir archivos de configuración con datos reales
3. Ignorar advertencias de Git sobre archivos grandes
4. Compartir tokens de acceso personal
5. Hacer commit de carpetas bin/ o obj/

---

## 🆘 Solución de Problemas Comunes

### Error: "failed to push some refs"

**Solución 1 - Hacer pull primero:**
```bash
git pull origin main --rebase
git push
```

**Solución 2 - Forzar push (¡CUIDADO! Solo si está seguro):**
```bash
git push -f origin main
```

### Error: "Support for password authentication was removed"

**Solución:** Usar token de acceso personal en lugar de contraseña.
Ver Paso 5.3 - Opción 1

### Error: "Permission denied"

**Solución:** Verificar que tiene permisos en el repositorio y que la URL es correcta:
```bash
git remote -v
```

### Deshacer el último commit (antes de push)

```bash
git reset --soft HEAD~1
```

### Remover archivo del staging
```bash
git reset HEAD archivo.cs
```

### Eliminar archivo del repositorio pero mantenerlo local
```bash
git rm --cached archivo.txt
```

---

## 📚 Recursos Adicionales

- **Documentación oficial de Git:** https://git-scm.com/doc
- **GitHub Docs:** https://docs.github.com
- **Git Cheat Sheet:** https://education.github.com/git-cheat-sheet-education.pdf
- **Aprender Git:** https://learngitbranching.js.org/

---

## 📞 Contacto y Soporte

Si encuentra problemas durante el proceso, puede:

1. Revisar la documentación de GitHub: https://docs.github.com
2. Buscar el error específico en Google o Stack Overflow
3. Contactar al administrador del proyecto

---

## 📄 Licencia

Este proyecto es de uso privado. Todos los derechos reservados.

---

**Última actualización:** 2025-01-20
**Versión del documento:** 1.0
