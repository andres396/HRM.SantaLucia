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