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