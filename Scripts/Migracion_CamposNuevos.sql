-- =============================================
-- Script de migración: Agregar campos nuevos
-- Sistema HRM Santa Lucía
-- =============================================

USE [HRM_SantaLucia]
GO

-- =============================================
-- Agregar campos a DIM.Empleado
-- =============================================

-- Campo Sede
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[DIM].[Empleado]') AND name = 'Sede')
BEGIN
    ALTER TABLE [DIM].[Empleado]
    ADD [Sede] NVARCHAR(50) NULL
    PRINT 'Campo Sede agregado a DIM.Empleado'
END
GO

-- Campo SalarioBase
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[DIM].[Empleado]') AND name = 'SalarioBase')
BEGIN
    ALTER TABLE [DIM].[Empleado]
    ADD [SalarioBase] DECIMAL(18,2) NULL
    PRINT 'Campo SalarioBase agregado a DIM.Empleado'
END
GO

-- Campo SalarioNeto
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[DIM].[Empleado]') AND name = 'SalarioNeto')
BEGIN
    ALTER TABLE [DIM].[Empleado]
    ADD [SalarioNeto] DECIMAL(18,2) NULL
    PRINT 'Campo SalarioNeto agregado a DIM.Empleado'
END
GO

-- Campo Rebajos
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[DIM].[Empleado]') AND name = 'Rebajos')
BEGIN
    ALTER TABLE [DIM].[Empleado]
    ADD [Rebajos] DECIMAL(18,2) NULL
    PRINT 'Campo Rebajos agregado a DIM.Empleado'
END
GO

-- Campo CCSS
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[DIM].[Empleado]') AND name = 'CCSS')
BEGIN
    ALTER TABLE [DIM].[Empleado]
    ADD [CCSS] DECIMAL(18,2) NULL
    PRINT 'Campo CCSS agregado a DIM.Empleado'
END
GO

-- Campo JUPEMA
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[DIM].[Empleado]') AND name = 'JUPEMA')
BEGIN
    ALTER TABLE [DIM].[Empleado]
    ADD [JUPEMA] DECIMAL(18,2) NULL
    PRINT 'Campo JUPEMA agregado a DIM.Empleado'
END
GO

-- Campo Magisterio
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[DIM].[Empleado]') AND name = 'Magisterio')
BEGIN
    ALTER TABLE [DIM].[Empleado]
    ADD [Magisterio] DECIMAL(18,2) NULL
    PRINT 'Campo Magisterio agregado a DIM.Empleado'
END
GO

-- Campo PorcentajeBP
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[DIM].[Empleado]') AND name = 'PorcentajeBP')
BEGIN
    ALTER TABLE [DIM].[Empleado]
    ADD [PorcentajeBP] DECIMAL(5,2) NULL
    PRINT 'Campo PorcentajeBP agregado a DIM.Empleado'
END
GO

-- Campo Bonos
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[DIM].[Empleado]') AND name = 'Bonos')
BEGIN
    ALTER TABLE [DIM].[Empleado]
    ADD [Bonos] DECIMAL(18,2) NULL
    PRINT 'Campo Bonos agregado a DIM.Empleado'
END
GO

-- =============================================
-- Agregar campos a FACT.Asistencia
-- =============================================

-- Campo GoceSalario
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[FACT].[Asistencia]') AND name = 'GoceSalario')
BEGIN
    ALTER TABLE [FACT].[Asistencia]
    ADD [GoceSalario] BIT NOT NULL DEFAULT 0
    PRINT 'Campo GoceSalario agregado a FACT.Asistencia'
END
GO

-- Campo MotivoPermiso
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[FACT].[Asistencia]') AND name = 'MotivoPermiso')
BEGIN
    ALTER TABLE [FACT].[Asistencia]
    ADD [MotivoPermiso] NVARCHAR(500) NULL
    PRINT 'Campo MotivoPermiso agregado a FACT.Asistencia'
END
GO

-- =============================================
-- Agregar campos a FACT.Nomina
-- =============================================

-- Campo Sede
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[FACT].[Nomina]') AND name = 'Sede')
BEGIN
    ALTER TABLE [FACT].[Nomina]
    ADD [Sede] NVARCHAR(50) NULL
    PRINT 'Campo Sede agregado a FACT.Nomina'
END
GO

-- Campo QTYHorasExtras
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[FACT].[Nomina]') AND name = 'QTYHorasExtras')
BEGIN
    ALTER TABLE [FACT].[Nomina]
    ADD [QTYHorasExtras] DECIMAL(18,2) NOT NULL DEFAULT 0
    PRINT 'Campo QTYHorasExtras agregado a FACT.Nomina'
END
GO

-- Campo QTYHorasRegulares
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[FACT].[Nomina]') AND name = 'QTYHorasRegulares')
BEGIN
    ALTER TABLE [FACT].[Nomina]
    ADD [QTYHorasRegulares] DECIMAL(18,2) NOT NULL DEFAULT 0
    PRINT 'Campo QTYHorasRegulares agregado a FACT.Nomina'
END
GO

-- Campo Miscelaneo
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[FACT].[Nomina]') AND name = 'Miscelaneo')
BEGIN
    ALTER TABLE [FACT].[Nomina]
    ADD [Miscelaneo] DECIMAL(18,2) NOT NULL DEFAULT 0
    PRINT 'Campo Miscelaneo agregado a FACT.Nomina'
END
GO

-- Campo Feriados
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[FACT].[Nomina]') AND name = 'Feriados')
BEGIN
    ALTER TABLE [FACT].[Nomina]
    ADD [Feriados] DECIMAL(18,2) NOT NULL DEFAULT 0
    PRINT 'Campo Feriados agregado a FACT.Nomina'
END
GO

-- Campo Aguinaldo
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[FACT].[Nomina]') AND name = 'Aguinaldo')
BEGIN
    ALTER TABLE [FACT].[Nomina]
    ADD [Aguinaldo] DECIMAL(18,2) NOT NULL DEFAULT 0
    PRINT 'Campo Aguinaldo agregado a FACT.Nomina'
END
GO

-- Campo ExtrasQuincenales
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[FACT].[Nomina]') AND name = 'ExtrasQuincenales')
BEGIN
    ALTER TABLE [FACT].[Nomina]
    ADD [ExtrasQuincenales] DECIMAL(18,2) NOT NULL DEFAULT 0
    PRINT 'Campo ExtrasQuincenales agregado a FACT.Nomina'
END
GO

-- Campo DeduccionesQuincenales
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[FACT].[Nomina]') AND name = 'DeduccionesQuincenales')
BEGIN
    ALTER TABLE [FACT].[Nomina]
    ADD [DeduccionesQuincenales] DECIMAL(18,2) NOT NULL DEFAULT 0
    PRINT 'Campo DeduccionesQuincenales agregado a FACT.Nomina'
END
GO

-- Campo PagoHorasRegulares (pago por horas regulares = SalarioBase/240 * QTYHorasRegulares)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[FACT].[Nomina]') AND name = 'PagoHorasRegulares')
BEGIN
    ALTER TABLE [FACT].[Nomina]
    ADD [PagoHorasRegulares] DECIMAL(18,2) NOT NULL DEFAULT 0
    PRINT 'Campo PagoHorasRegulares agregado a FACT.Nomina'
END
GO

-- Campo QTYDiasFeriados (cantidad de días feriados para calcular monto)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[FACT].[Nomina]') AND name = 'QTYDiasFeriados')
BEGIN
    ALTER TABLE [FACT].[Nomina]
    ADD [QTYDiasFeriados] DECIMAL(18,2) NOT NULL DEFAULT 0
    PRINT 'Campo QTYDiasFeriados agregado a FACT.Nomina'
END
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
