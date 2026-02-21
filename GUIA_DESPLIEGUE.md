# Guía de Despliegue – Sistema HRM Santa Lucía

Esta guía explica dos formas de poner la aplicación a disposición de los usuarios:

1. **Hostear en la web** – La aplicación corre en un servidor y se accede por URL (internet o red interna).
2. **Ejecutable / instalable local** – Un ejecutable o instalador para instalar y ejecutar en una PC (sin necesidad de internet para usarla).

---

## Requisitos previos

- La aplicación usa **SQL Server** (o LocalDB). Para producción necesitas:
  - **Web:** servidor con SQL Server accesible desde el servidor web (Azure SQL, SQL Server en VPS, etc.).
  - **Local:** SQL Server o SQL Server Express / LocalDB instalado en la misma máquina o en un servidor de la red.

- **.NET 9** – En hosting web el servidor debe tener el runtime; en “ejecutable” puedes publicar **autocontenido** para no depender de que el usuario instale .NET.

---

# Opción A: Hostear la aplicación en la Web

La aplicación se publica en un servidor y los usuarios entran por navegador (ej. `https://hrm.midominio.com`).

## A.1 Publicar la aplicación

En la carpeta del proyecto (donde está el `.csproj`):

```powershell
# Publicación para servidor Linux/macOS (más común en hosting)
dotnet publish -c Release -o ./publish

# O publicación autocontenida para Windows (si el servidor es Windows y no tiene .NET instalado)
dotnet publish -c Release -r win-x64 --self-contained -o ./publish
```

Los archivos listos para desplegar quedan en la carpeta `publish`.

**Nota:** Si publicas en otra carpeta (ej. `publish-win`), sustituye `publish` por esa ruta en los pasos siguientes.

## A.2 Dónde hostear

### 1. Azure (recomendado si quieres cloud)

- **App Service:** subes la carpeta `publish` (o conectas GitHub para despliegue automático).
- **Base de datos:** Azure SQL. En el portal creas un servidor y una base de datos, y usas la cadena de conexión que te dan.
- **Configuración:** En App Service → Configuration → Application settings, defines:
  - `ConnectionStrings__DefaultConnection` = cadena de conexión de Azure SQL (usuario/contraseña, no Integrated Security).
  - `ASPNETCORE_ENVIRONMENT` = `Production`.

### 2. VPS o servidor propio (Windows o Linux)

- **Windows con IIS:**
  - Instala el **Hosting Bundle de .NET 9** (ASP.NET Core Runtime).
  - Crea un sitio en IIS que apunte a la carpeta `publish` y un Application Pool “Sin código administrado”.
  - Configura la cadena de conexión en `appsettings.Production.json` o en variables de entorno.

- **Linux (Ubuntu/Debian) con Kestrel:**
  - Instala el runtime: `dotnet-runtime-9.0` (y si publicaste dependiente del framework, también el SDK o el runtime completo según lo que hayas publicado).
  - Copia la carpeta `publish` al servidor.
  - Ejecuta: `dotnet HRM.SantaLucia.Web.dll` desde esa carpeta (o usa systemd/Nginx como reverse proxy).
  - La cadena de conexión se configura en `appsettings.Production.json` o con variables de entorno.

### 3. Conexión a la base de datos en hosting

En el servidor la app no puede usar `Integrated Security=True` (inicio de sesión de Windows). Debes usar **usuario y contraseña de SQL**:

```text
Server=tu-servidor-sql.database.windows.net;Database=HRM_SantaLucia;User Id=usuario;Password=tu_password;TrustServerCertificate=True;Encrypt=True;
```

- Crea `appsettings.Production.json` en el proyecto (o configura en el panel del hosting) con:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=HRM_SantaLucia;User Id=...;Password=...;TrustServerCertificate=True;Encrypt=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

- En producción no dejes contraseñas en el repositorio: usa variables de entorno o secretos del hosting.

## A.3 Resumen pasos para “subirla a la web”

1. Tener un servidor (Azure, VPS, etc.) y una base SQL accesible desde ese servidor.
2. Crear la base de datos (ejecutar tus scripts `CrearBaseDatos.sql` / migraciones en ese servidor).
3. `dotnet publish -c Release -o ./publish`.
4. Subir el contenido de `publish` al servidor y configurar la cadena de conexión y `ASPNETCORE_ENVIRONMENT=Production`.
5. En IIS/Kestrel/systemd, arrancar la aplicación y (si aplica) configurar HTTPS y dominio.

---

# Opción B: Ejecutable / instalable para usar en una PC

Objetivo: que en una PC (por ejemplo en la oficina) se pueda “instalar” o ejecutar la aplicación y abrirla en el navegador como `http://localhost:puerto`.

## B.1 Publicar como ejecutable autocontenido (Windows)

No requiere instalar .NET en la PC del usuario. Incluye el runtime en la carpeta de publicación.

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -o ./publish-win
```

O en una sola carpeta con un solo .exe (más fácil de distribuir):

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish-win
```

Los archivos quedan en `publish-win`. El ejecutable principal es `HRM.SantaLucia.Web.exe`.

**Script de arranque:** En el proyecto hay un archivo `IniciarHRM.bat` que inicia la aplicación y abre el navegador. **Cópialo dentro de la carpeta publicada** (por ejemplo `publish-win`) para que el usuario solo tenga que hacer doble clic en ese .bat:

```powershell
Copy-Item IniciarHRM.bat -Destination ./publish-win/
```

## B.2 Cómo ejecutarlo

1. **Requisito:** En esa PC debe estar instalado **SQL Server** (Express o completo) o **LocalDB**, y la base de datos `HRM_SantaLucia` creada (con tus scripts).
2. Ajustar la cadena de conexión en `appsettings.json` dentro de la carpeta publicada (o usar `appsettings.Production.json`) para que apunte a esa instancia (por ejemplo `Server=.\SQLEXPRESS` o `Server=(localdb)\MSSQLLocalDB`).
3. Ejecutar:
   - **Si usaste PublishSingleFile:** doble clic en `HRM.SantaLucia.Web.exe`.
   - **Si no:** doble clic en `HRM.SantaLucia.Web.exe` dentro de `publish-win`.
4. La aplicación levanta el servidor web (Kestrel) y suele abrir el navegador en `http://localhost:5000` (o el puerto configurado). Si no abre solo, escribe en el navegador: `http://localhost:5000`.

Para que sea más claro para el usuario, puedes agregar un **script de arranque** (ver B.4).

## B.3 Ejecutar como Windows Service (opcional)

Así la aplicación queda instalada como servicio y puede iniciarse con Windows (sin abrir una ventana).

- Crea el servicio con `sc.exe` o con **NSSM** (Non-Sucking Service Manager), apuntando al ejecutable de la carpeta publicada.
- El ejecutable debe poder ejecutarse en modo “service” (ASP.NET Core soporta ejecutarse bajo un servicio si se configura correctamente; a veces se usa un wrapper como NSSM que ejecuta `dotnet run` o el .exe publicado).

## B.4 Script “Instalar y abrir navegador”

Puedes crear un `.bat` o `.ps1` que el usuario ejecute para:

1. Verificar que SQL Server/LocalDB esté disponible (o mostrar un mensaje).
2. Lanzar `HRM.SantaLucia.Web.exe` (o `dotnet HRM.SantaLucia.Web.dll`).
3. Esperar unos segundos y abrir `http://localhost:5000` en el navegador.

Ejemplo mínimo para un `.bat` en la misma carpeta que el .exe:

```batch
@echo off
echo Iniciando Sistema HRM Santa Lucia...
start "" "HRM.SantaLucia.Web.exe"
timeout /t 5 /nobreak >nul
start http://localhost:5000
echo Abriendo navegador. Si no abre, visite: http://localhost:5000
pause
```

(El puerto puede ser otro según tu `applicationUrl` o configuración de Kestrel; en publicación por defecto suele ser 5000 o el que definas en `applicationUrl`.)

## B.5 “Instalador” (opcional)

Para algo más “profesional”:

- **Inno Setup** o **WiX Toolset:** crean un instalador (.exe o .msi) que:
  - Copia la carpeta `publish-win` a `Program Files` o a una carpeta elegida.
  - Crea un acceso directo en el escritorio o en el menú Inicio que ejecute el .bat o el .exe.
  - Opcionalmente puede instalar LocalDB si lo incluyes en el paquete (licencias y tamaño a considerar).

La aplicación en sí **no se convierte en un .exe de escritorio** tipo WinForms: sigue siendo una web que se abre en el navegador; el “ejecutable” es el servidor web que se inicia en la PC.

---

# Resumen rápido

| Objetivo | Qué hacer |
|----------|-----------|
| **Ponerla en internet / red** | Publicar con `dotnet publish -c Release -o ./publish`, subir a Azure/VPS, configurar SQL y cadena de conexión, y arrancar en el servidor (IIS/Kestrel). |
| **Ejecutable en una PC** | Publicar con `-r win-x64 --self-contained true` (y opcionalmente `PublishSingleFile=true`), asegurar SQL Server/LocalDB y la base de datos en esa PC, ejecutar el .exe y abrir `http://localhost:puerto`. |
| **Algo tipo “instalador”** | Usar la carpeta publicada autocontenida + un script de arranque (.bat) y, si quieres, un instalador (Inno Setup / WiX) que copie archivos y cree el acceso directo. |

Si indicas si prefieres **solo web**, **solo ejecutable en PC** o **ambos**, se pueden detallar solo los pasos que te interesan (por ejemplo, solo Azure o solo el .bat de arranque).
