# Guía Rápida de Ejecución

## Pasos Rápidos (5 minutos)

### 1. Configurar Base de Datos
```bash
# Edita appsettings.json y cambia la cadena de conexión
# Ejemplo para LocalDB:
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HRM_SantaLucia;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
```

### 2. Crear Base de Datos
```bash
# Abre PowerShell o CMD en la carpeta del proyecto
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 3. Datos Iniciales (Opcional)
```bash
# Ejecuta el script SQL en SSMS o sqlcmd
# Archivo: Scripts/DatosIniciales.sql
```

### 4. Ejecutar
```bash
dotnet run
```

### 5. Abrir Navegador
```
http://localhost:5266
```

## Comandos Útiles

```bash
# Ver todas las migraciones
dotnet ef migrations list

# Eliminar última migración
dotnet ef migrations remove

# Actualizar base de datos
dotnet ef database update

# Ver estado de la base de datos
dotnet ef database info
```

## Verificación Rápida

1. ✅ La aplicación inicia sin errores
2. ✅ Puedes ver el Dashboard
3. ✅ Puedes crear un empleado
4. ✅ Puedes registrar asistencia

Si todos estos pasos funcionan, ¡la aplicación está lista!

