# Generar Diagrama de Base de Datos como Imagen

He creado dos archivos para visualizar el diagrama de base de datos:

## Opción 1: Visualización HTML (Recomendada)

**Archivo:** `Scripts/DiagramaBaseDatos.html`

### Cómo usar:
1. Abre el archivo `Scripts/DiagramaBaseDatos.html` en tu navegador web
2. Verás tres diagramas interactivos:
   - Diagrama del Esquema DIM (Dimensiones)
   - Diagrama del Esquema FACT (Hechos)
   - Diagrama completo de relaciones

### Exportar como imagen:
1. Abre el archivo HTML en Chrome o Edge
2. Presiona `F12` para abrir las herramientas de desarrollador
3. Haz clic derecho en el diagrama
4. Selecciona "Guardar imagen como..." o usa la opción de exportar del navegador

## Opción 2: Usar Graphviz (Formato DOT)

**Archivo:** `Scripts/DiagramaBaseDatos.dot`

### Requisitos:
- Instalar Graphviz desde: https://graphviz.org/download/

### Generar imagen desde línea de comandos:

```bash
# Generar PNG
dot -Tpng Scripts/DiagramaBaseDatos.dot -o Scripts/DiagramaBaseDatos.png

# Generar SVG (mejor calidad)
dot -Tsvg Scripts/DiagramaBaseDatos.dot -o Scripts/DiagramaBaseDatos.svg

# Generar PDF
dot -Tpdf Scripts/DiagramaBaseDatos.dot -o Scripts/DiagramaBaseDatos.pdf
```

### Usar servicios online (sin instalar):
1. Ve a: https://dreampuf.github.io/GraphvizOnline/
2. Copia el contenido de `Scripts/DiagramaBaseDatos.dot`
3. Pega en el editor
4. Descarga la imagen generada

## Opción 3: Usar herramientas de SQL Server

Si tienes SQL Server Management Studio (SSMS):
1. Conéctate a tu base de datos
2. Expande la base de datos `HRM_SantaLucia`
3. Click derecho en "Database Diagrams"
4. Selecciona "New Database Diagram"
5. Agrega todas las tablas
6. Guarda el diagrama

## Estructura del Diagrama

### Esquema DIM (Dimensiones) - Color Azul
- **Banco**: Información de bancos
- **Puesto**: Puestos de trabajo
- **Departamento**: Estructura organizacional (con jerarquía)
- **Empleado**: Tabla central con información de empleados

### Esquema FACT (Hechos) - Color Amarillo
- **Nomina**: Registros mensuales de nómina
- **Asistencia**: Registros diarios de asistencia
- **Vacaciones**: Solicitudes y registros de vacaciones
- **Rendimiento**: Evaluaciones de desempeño

### Relaciones
- **Líneas azules punteadas**: Relaciones opcionales (FK nullable)
- **Líneas rojas sólidas**: Relaciones requeridas (FK NOT NULL)
- **Línea naranja**: Auto-referencia en Departamento
- **Líneas verde/morada**: Relaciones especiales (Aprobador, Evaluador)

## Notas Importantes

1. **Columnas Calculadas**: `NombreCompleto` y `Edad` en Empleado son calculadas automáticamente por la base de datos
2. **Índice Único**: Asistencia tiene un índice único en (EmpleadoKey, FechaKey) para evitar duplicados
3. **Auto-referencia**: Departamento puede tener un departamento padre (estructura jerárquica)
4. **Relaciones Múltiples**: Empleado tiene múltiples relaciones con Vacaciones y Rendimiento (como solicitante/aprobador y como evaluado/evaluador)

