@echo off
REM Script de ejecución rápida para HRM Santa Lucía (Windows)
REM Ejecutar: ejecutar.bat

echo === Sistema HRM - Santa Lucia ===
echo.

echo Verificando .NET SDK...
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo [ERROR] .NET SDK no encontrado. Por favor instala .NET 9.0 SDK
    pause
    exit /b 1
)
echo [OK] .NET SDK encontrado

echo.
echo Verificando herramientas de EF Core...
dotnet ef --version >nul 2>&1
if %errorlevel% neq 0 (
    echo Instalando herramientas de EF Core...
    dotnet tool install --global dotnet-ef
    if %errorlevel% neq 0 (
        echo [ERROR] Error al instalar herramientas de EF Core
        pause
        exit /b 1
    )
)
echo [OK] Herramientas de EF Core listas

echo.
echo Restaurando paquetes NuGet...
dotnet restore
if %errorlevel% neq 0 (
    echo [ERROR] Error al restaurar paquetes
    pause
    exit /b 1
)
echo [OK] Paquetes restaurados

echo.
echo Verificando migraciones...
dotnet ef migrations list >nul 2>&1
if %errorlevel% neq 0 (
    echo Creando migracion inicial...
    dotnet ef migrations add InitialCreate
    if %errorlevel% neq 0 (
        echo [ERROR] Error al crear migracion
        echo Asegurate de que la cadena de conexion en appsettings.json sea correcta
        pause
        exit /b 1
    )
    echo [OK] Migracion creada
) else (
    echo [OK] Migraciones encontradas
)

echo.
echo Aplicando migraciones a la base de datos...
dotnet ef database update
if %errorlevel% neq 0 (
    echo [ERROR] Error al aplicar migraciones
    echo Verifica que:
    echo   1. SQL Server este ejecutandose
    echo   2. La cadena de conexion en appsettings.json sea correcta
    echo   3. Tengas permisos para crear la base de datos
    pause
    exit /b 1
)
echo [OK] Base de datos actualizada

echo.
echo Compilando proyecto...
dotnet build
if %errorlevel% neq 0 (
    echo [ERROR] Error al compilar
    pause
    exit /b 1
)
echo [OK] Proyecto compilado correctamente

echo.
echo === Todo listo! ===
echo.
echo La aplicacion se iniciara en:
echo   HTTP:  http://localhost:5266
echo   HTTPS: https://localhost:7032
echo.
echo Presiona Ctrl+C para detener la aplicacion
echo.

dotnet run

pause

