# Script de ejecución rápida para HRM Santa Lucía
# Ejecutar: .\ejecutar.ps1

Write-Host "=== Sistema HRM - Santa Lucía ===" -ForegroundColor Cyan
Write-Host ""

# Verificar .NET SDK
Write-Host "Verificando .NET SDK..." -ForegroundColor Yellow
$dotnetVersion = dotnet --version
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ .NET SDK $dotnetVersion encontrado" -ForegroundColor Green
} else {
    Write-Host "✗ .NET SDK no encontrado. Por favor instala .NET 9.0 SDK" -ForegroundColor Red
    exit 1
}

# Verificar herramientas de EF Core
Write-Host "Verificando herramientas de EF Core..." -ForegroundColor Yellow
$efTools = dotnet ef --version 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Herramientas de EF Core encontradas" -ForegroundColor Green
} else {
    Write-Host "Instalando herramientas de EF Core..." -ForegroundColor Yellow
    dotnet tool install --global dotnet-ef
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ Herramientas de EF Core instaladas" -ForegroundColor Green
    } else {
        Write-Host "✗ Error al instalar herramientas de EF Core" -ForegroundColor Red
        exit 1
    }
}

# Restaurar paquetes
Write-Host "Restaurando paquetes NuGet..." -ForegroundColor Yellow
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "✗ Error al restaurar paquetes" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Paquetes restaurados" -ForegroundColor Green

# Verificar migraciones
Write-Host "Verificando migraciones..." -ForegroundColor Yellow
$migrations = dotnet ef migrations list 2>&1
if ($migrations -match "No migrations") {
    Write-Host "Creando migración inicial..." -ForegroundColor Yellow
    dotnet ef migrations add InitialCreate
    if ($LASTEXITCODE -ne 0) {
        Write-Host "✗ Error al crear migración" -ForegroundColor Red
        Write-Host "Asegúrate de que la cadena de conexión en appsettings.json sea correcta" -ForegroundColor Yellow
        exit 1
    }
    Write-Host "✓ Migración creada" -ForegroundColor Green
} else {
    Write-Host "✓ Migraciones encontradas" -ForegroundColor Green
}

# Aplicar migraciones
Write-Host "Aplicando migraciones a la base de datos..." -ForegroundColor Yellow
dotnet ef database update
if ($LASTEXITCODE -ne 0) {
    Write-Host "✗ Error al aplicar migraciones" -ForegroundColor Red
    Write-Host "Verifica que:" -ForegroundColor Yellow
    Write-Host "  1. SQL Server esté ejecutándose" -ForegroundColor Yellow
    Write-Host "  2. La cadena de conexión en appsettings.json sea correcta" -ForegroundColor Yellow
    Write-Host "  3. Tengas permisos para crear la base de datos" -ForegroundColor Yellow
    exit 1
}
Write-Host "✓ Base de datos actualizada" -ForegroundColor Green

# Compilar
Write-Host "Compilando proyecto..." -ForegroundColor Yellow
dotnet build
if ($LASTEXITCODE -ne 0) {
    Write-Host "✗ Error al compilar" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Proyecto compilado correctamente" -ForegroundColor Green

Write-Host ""
Write-Host "=== ¡Todo listo! ===" -ForegroundColor Green
Write-Host ""
Write-Host "La aplicación se iniciará en:" -ForegroundColor Cyan
Write-Host "  HTTP:  http://localhost:5266" -ForegroundColor White
Write-Host "  HTTPS: https://localhost:7032" -ForegroundColor White
Write-Host ""
Write-Host "Presiona Ctrl+C para detener la aplicación" -ForegroundColor Yellow
Write-Host ""

# Ejecutar aplicación
dotnet run

