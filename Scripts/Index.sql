-- =============================================
-- Script de creación de base de datos
-- Sistema HRM Santa Lucía
-- =============================================
-- Crear base de datos si no existe

GO



GO
-- =============================================
-- Crear Índices
-- =============================================
-- Índice único para Asistencia (EmpleadoKey, FechaKey)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Asistencia_EmpleadoKey_FechaKey')
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Asistencia_EmpleadoKey_FechaKey]
    ON [FACT].[Asistencia] ([EmpleadoKey], [FechaKey])
END
GO
-- Índices para mejorar rendimiento en búsquedas comunes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Nomina_PeriodoKey')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Nomina_PeriodoKey]
    ON [FACT].[Nomina] ([PeriodoKey])
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Nomina_EmpleadoKey_PeriodoKey')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Nomina_EmpleadoKey_PeriodoKey]
    ON [FACT].[Nomina] ([EmpleadoKey], [PeriodoKey])
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Asistencia_FechaKey')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Asistencia_FechaKey]
    ON [FACT].[Asistencia] ([FechaKey])
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Vacaciones_EmpleadoKey')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Vacaciones_EmpleadoKey]
    ON [FACT].[Vacaciones] ([EmpleadoKey])
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Vacaciones_Estado')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Vacaciones_Estado]
    ON [FACT].[Vacaciones] ([Estado])
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Empleado_Activo')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Empleado_Activo]
    ON [DIM].[Empleado] ([Activo])
END
GO