# Sistema HRM - Santa Lucía

Sistema de Gestión de Recursos Humanos desarrollado en ASP.NET Core MVC.

## Requisitos Previos

- .NET 9.0 SDK o superior
- SQL Server 2019 o superior (o SQL Server Express)
- Visual Studio 2022 o Visual Studio Code (opcional)

## Pasos para Ejecutar la Aplicación

### 1. Configurar la Base de Datos

#### Opción A: SQL Server Local
1. Asegúrate de que SQL Server esté instalado y ejecutándose
2. Abre SQL Server Management Studio (SSMS) o usa `sqlcmd`
3. Crea la base de datos (o se creará automáticamente con las migraciones):
   ```sql
   CREATE DATABASE HRM_SantaLucia;
   GO
   ```

#### Opción B: SQL Server Express LocalDB
Si usas LocalDB, la cadena de conexión será:
```
Server=(localdb)\mssqllocaldb;Database=HRM_SantaLucia;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True
```

### 2. Configurar la Cadena de Conexión

Edita el archivo `appsettings.json` y actualiza la cadena de conexión según tu configuración:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HRM_SantaLucia;User Id=sa;Password=TuPassword;TrustServerCertificate=True;MultipleActiveResultSets=True"
  }
}
```

**Para SQL Server con autenticación de Windows:**
```json
"DefaultConnection": "Server=localhost;Database=HRM_SantaLucia;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
```

**Para LocalDB:**
```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HRM_SantaLucia;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
```

### 3. Crear las Migraciones de Entity Framework

Abre una terminal en la carpeta del proyecto y ejecuta:

```bash
# Instalar herramientas de EF Core (si no están instaladas)
dotnet tool install --global dotnet-ef

# Crear la migración inicial
dotnet ef migrations add InitialCreate

# Aplicar las migraciones a la base de datos
dotnet ef database update
```

### 4. Insertar Datos Iniciales (Opcional)

Ejecuta el script SQL `Scripts/DatosIniciales.sql` en tu base de datos para insertar:
- Departamentos
- Puestos
- Bancos

O ejecuta manualmente desde SSMS o sqlcmd.

### 5. Ejecutar la Aplicación

#### Opción A: Desde Visual Studio
1. Abre el proyecto en Visual Studio
2. Presiona `F5` o haz clic en "Ejecutar"
3. La aplicación se abrirá en tu navegador

#### Opción B: Desde la Terminal
```bash
# Restaurar paquetes NuGet
dotnet restore

# Compilar el proyecto
dotnet build

# Ejecutar la aplicación
dotnet run
```

La aplicación estará disponible en:
- HTTP: `http://localhost:5266`
- HTTPS: `https://localhost:7032`

### 6. Acceder a la Aplicación

Abre tu navegador y navega a:
- `http://localhost:5266` o
- `https://localhost:7032`

## Estructura de la Aplicación

### Módulos Principales

1. **Dashboard** (`/Home`)
   - Vista general con estadísticas
   - Resumen de empleados, nómina, asistencia

2. **Empleados** (`/Empleado`)
   - Listar empleados
   - Crear nuevo empleado
   - Editar empleado
   - Ver detalles

3. **Asistencia** (`/Asistencia`)
   - Registrar asistencia diaria
   - Ver historial por empleado
   - Resumen mensual

4. **Nómina** (`/Nomina`)
   - Calcular nómina mensual
   - Consultar nóminas
   - Historial por empleado

5. **Vacaciones** (`/Vacaciones`)
   - Solicitar vacaciones
   - Aprobar/Rechazar solicitudes
   - Ver solicitudes pendientes

6. **Rendimiento** (`/Rendimiento`)
   - Crear evaluaciones de rendimiento
   - Ver historial de evaluaciones

## Pruebas Recomendadas

### 1. Crear un Empleado
1. Ve a **Empleados** → **Nuevo Empleado**
2. Completa el formulario con datos de prueba
3. Guarda y verifica que aparezca en la lista

### 2. Registrar Asistencia
1. Ve a **Asistencia** → **Registrar Asistencia**
2. Selecciona un empleado y registra su asistencia
3. Verifica en **Asistencia** que se muestre correctamente

### 3. Calcular Nómina
1. Ve a **Nómina** → **Calcular Nómina**
2. Selecciona año y mes
3. Haz clic en "Calcular Nómina"
4. Verifica en **Consultar** que se haya generado

### 4. Solicitar Vacaciones
1. Ve a **Vacaciones** → **Solicitar Vacaciones**
2. Selecciona un empleado y completa el formulario
3. Verifica que aparezca en **Vacaciones Pendientes**

### 5. Crear Evaluación de Rendimiento
1. Ve a **Rendimiento** → **Nueva Evaluación**
2. Completa el formulario
3. Verifica que aparezca en la lista

## Solución de Problemas

### Error: "Cannot open database"
- Verifica que SQL Server esté ejecutándose
- Verifica la cadena de conexión en `appsettings.json`
- Asegúrate de que la base de datos exista o que las migraciones se hayan aplicado

### Error: "Migration not found"
- Ejecuta `dotnet ef migrations add InitialCreate`
- Luego `dotnet ef database update`

### Error: "Table already exists"
- Elimina la base de datos y vuelve a crear las migraciones
- O ejecuta `dotnet ef database drop` y luego `dotnet ef database update`

### La aplicación no inicia
- Verifica que el puerto no esté en uso
- Cambia el puerto en `Properties/launchSettings.json`
- Verifica que todas las dependencias NuGet estén instaladas

## Tecnologías Utilizadas

- ASP.NET Core 9.0 MVC
- Entity Framework Core 9.0
- SQL Server
- AutoMapper
- Bootstrap 5
- jQuery

## Notas Importantes

- Asegúrate de tener datos iniciales (Puestos, Departamentos, Bancos) antes de crear empleados
- La aplicación usa eliminación lógica para empleados (marca como inactivo)
- Las fechas se almacenan como claves numéricas (yyyyMMdd) para optimización
- Los períodos de nómina se almacenan como yyyyMM

## Soporte

Para problemas o preguntas, revisa los logs en la consola o configura el logging en `appsettings.json`.

