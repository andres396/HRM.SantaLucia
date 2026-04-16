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