-- =============================================
-- Script de migración: Agregar campos nuevos
-- Sistema HRM Santa Lucía
-- =============================================

USE [HRM_SantaLucia]
GO







-- =============================================
-- Crear Índice para Sede en Nomina
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Nomina_Sede')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Nomina_Sede]
    ON [FACT].[Nomina] ([Sede])
    PRINT 'Índice IX_Nomina_Sede creado'
END
GO

-- =============================================
-- Crear Índice para Sede en Empleado
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Empleado_Sede')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Empleado_Sede]
    ON [DIM].[Empleado] ([Sede])
    PRINT 'Índice IX_Empleado_Sede creado'
END
GO

PRINT 'Migración completada exitosamente'
GO