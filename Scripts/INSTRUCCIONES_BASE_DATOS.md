# Instrucciones para Configurar la Base de Datos

## Requisitos Previos
- SQL Server 2019 o superior instalado
- Permisos para crear bases de datos y esquemas
- SQL Server Management Studio (SSMS) o cualquier cliente SQL

## Pasos para Configurar la Base de Datos

### Opción 1: Usando SQL Server Management Studio (SSMS)

1. **Abrir SQL Server Management Studio**
   - Conectarse a tu instancia de SQL Server (localhost o servidor remoto)

2. **Ejecutar el Script de Creación**
   - Abrir el archivo `Scripts/CrearBaseDatos.sql`
   - Ejecutar el script completo (F5 o botón Ejecutar)
   - Verificar que no haya errores en la ventana de mensajes

3. **Ejecutar el Script de Datos Iniciales**
   - Abrir el archivo `Scripts/DatosIniciales.sql`
   - Ejecutar el script completo (F5 o botón Ejecutar)
   - Esto insertará datos de ejemplo (departamentos, puestos, bancos)

4. **Verificar la Conexión**
   - Verificar que la cadena de conexión en `appsettings.json` sea correcta
   - Ejecutar la aplicación y verificar que se conecte correctamente

### Opción 2: Usando la Línea de Comandos (sqlcmd)

```bash
# Conectarse a SQL Server
sqlcmd -S localhost -E

# Ejecutar el script de creación
sqlcmd -S localhost -E -i Scripts\CrearBaseDatos.sql

# Ejecutar el script de datos iniciales
sqlcmd -S localhost -E -i Scripts\DatosIniciales.sql
```

Si usas autenticación SQL Server:
```bash
sqlcmd -S localhost -U sa -P TuPassword -i Scripts\CrearBaseDatos.sql
sqlcmd -S localhost -U sa -P TuPassword -i Scripts\DatosIniciales.sql
```

### Opción 3: Usando Entity Framework Migrations (Recomendado)

Si prefieres usar las migraciones de Entity Framework:

```bash
# En la carpeta del proyecto, ejecutar:
dotnet ef database update
```

Esto creará automáticamente la base de datos basándose en las entidades y el DbContext.

## Estructura de la Base de Datos

La base de datos se organiza en dos esquemas:

### Esquema DIM (Dimensiones)
- **Banco**: Información de bancos
- **Puesto**: Puestos de trabajo
- **Departamento**: Departamentos organizacionales
- **Empleado**: Información de empleados

### Esquema FACT (Hechos)
- **Nomina**: Registros de nómina mensual
- **Asistencia**: Registros de asistencia diaria
- **Vacaciones**: Solicitudes y registros de vacaciones
- **Rendimiento**: Evaluaciones de rendimiento

## Configuración de la Cadena de Conexión

Asegúrate de que tu `appsettings.json` tenga la cadena de conexión correcta:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HRM_SantaLucia;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True;Encrypt=True;Connection Timeout=30"
  }
}
```

Para autenticación SQL Server:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HRM_SantaLucia;User Id=sa;Password=TuPassword;TrustServerCertificate=True;MultipleActiveResultSets=True"
  }
}
```

## Verificación

Después de ejecutar los scripts, puedes verificar que todo esté correcto ejecutando:

```sql
USE HRM_SantaLucia
GO

-- Verificar tablas creadas
SELECT TABLE_SCHEMA, TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
ORDER BY TABLE_SCHEMA, TABLE_NAME

-- Verificar datos iniciales
SELECT COUNT(*) AS TotalDepartamentos FROM [DIM].[Departamento]
SELECT COUNT(*) AS TotalPuestos FROM [DIM].[Puesto]
SELECT COUNT(*) AS TotalBancos FROM [DIM].[Banco]
```

## Solución de Problemas

### Error: "Cannot create database because it already exists"
- La base de datos ya existe. Puedes eliminarla primero o usar `IF NOT EXISTS` (ya incluido en el script)

### Error: "Login failed for user"
- Verifica las credenciales en la cadena de conexión
- Asegúrate de que el usuario tenga permisos para crear bases de datos

### Error: "Cannot find the object"
- Ejecuta primero el script `CrearBaseDatos.sql` antes de `DatosIniciales.sql`

## Notas Importantes

1. El script `CrearBaseDatos.sql` es idempotente (puede ejecutarse múltiples veces sin causar errores)
2. El script `DatosIniciales.sql` inserta datos de ejemplo. Si los ejecutas múltiples veces, puede generar duplicados
3. Las columnas calculadas `NombreCompleto` y `Edad` en la tabla Empleado se actualizan automáticamente
4. El índice único en `Asistencia(EmpleadoKey, FechaKey)` previene registros duplicados de asistencia para el mismo empleado en la misma fecha
5. Se agregaron 2 script mas para hacer updates en las tablas, ejecutarlos despues de crear la base de datos y las tablas.
