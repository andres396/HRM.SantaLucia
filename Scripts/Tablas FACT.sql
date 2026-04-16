GO
-- =============================================
-- Tablas de Hechos (FACT)
-- =============================================
-- Tabla: Nomina
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FACT].[Nomina]') AND type in (N'U'))
BEGIN
    CREATE TABLE [FACT].[Nomina] (
        [NominaKey] INT IDENTITY(1,1) NOT NULL,
        [EmpleadoKey] INT NOT NULL,
        [PuestoKey] INT NOT NULL,
        [DepartamentoKey] INT NOT NULL,
        [PeriodoKey] INT NOT NULL,
        [SalarioBase] DECIMAL(18,2) NOT NULL,
        [HorasTrabajadas] DECIMAL(18,2) NOT NULL,
        [HorasExtra] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [PagoHorasExtra] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [Bonificaciones] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [Comisiones] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [TotalExtras] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [SeguroSocial] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [Renta] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [OtrasDeducciones] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [TotalDeducciones] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [SalarioNeto] DECIMAL(18,2) NOT NULL,
        [UsuarioCreacion] NVARCHAR(100) NULL,
        [FechaCreacion] DATETIME NULL,
        [UsuarioModificacion] NVARCHAR(100) NULL,
        [FechaModificacion] DATETIME NULL,
        CONSTRAINT [PK_Nomina] PRIMARY KEY CLUSTERED ([NominaKey] ASC)
    )
END
GO
-- Tabla: Asistencia
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FACT].[Asistencia]') AND type in (N'U'))
BEGIN
    CREATE TABLE [FACT].[Asistencia] (
        [AsistenciaKey] INT IDENTITY(1,1) NOT NULL,
        [EmpleadoKey] INT NOT NULL,
        [FechaKey] INT NOT NULL,
        [HoraEntrada] TIME NULL,
        [HoraSalida] TIME NULL,
        [HorasTrabajadas] DECIMAL(5,2) NOT NULL DEFAULT 0,
        [MinutosTarde] INT NOT NULL DEFAULT 0,
        [Estado] NVARCHAR(50) NOT NULL,
        [Justificacion] NVARCHAR(500) NULL,
        [UsuarioCreacion] NVARCHAR(100) NULL,
        [FechaCreacion] DATETIME NULL,
        [UsuarioModificacion] NVARCHAR(100) NULL,
        [FechaModificacion] DATETIME NULL,
        CONSTRAINT [PK_Asistencia] PRIMARY KEY CLUSTERED ([AsistenciaKey] ASC)
    )
END
GO
-- Tabla: Vacaciones
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FACT].[Vacaciones]') AND type in (N'U'))
BEGIN
    CREATE TABLE [FACT].[Vacaciones] (
        [VacacionKey] INT IDENTITY(1,1) NOT NULL,
        [EmpleadoKey] INT NOT NULL,
        [FechaInicioKey] INT NOT NULL,
        [FechaFinKey] INT NOT NULL,
        [DiasSolicitados] INT NOT NULL,
        [Estado] NVARCHAR(50) NOT NULL,
        [Observaciones] NVARCHAR(500) NULL,
        [AprobadorKey] INT NULL,
        [FechaSolicitud] DATETIME NULL,
        [FechaAprobacion] DATETIME NULL,
        [UsuarioCreacion] NVARCHAR(100) NULL,
        [FechaCreacion] DATETIME NULL,
        [UsuarioModificacion] NVARCHAR(100) NULL,
        [FechaModificacion] DATETIME NULL,
        CONSTRAINT [PK_Vacaciones] PRIMARY KEY CLUSTERED ([VacacionKey] ASC)
    )
END
GO
-- Tabla: Rendimiento
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FACT].[Rendimiento]') AND type in (N'U'))
BEGIN
    CREATE TABLE [FACT].[Rendimiento] (
        [RendimientoKey] INT IDENTITY(1,1) NOT NULL,
        [EmpleadoKey] INT NOT NULL,
        [EvaluadorKey] INT NOT NULL,
        [FechaKey] INT NOT NULL,
        [CalificacionGeneral] DECIMAL(5,2) NOT NULL,
        [MetasPropuestas] INT NOT NULL DEFAULT 0,
        [MetasAlcanzadas] INT NOT NULL DEFAULT 0,
        [Comentarios] NVARCHAR(1000) NULL,
        [PeriodoEvaluacion] NVARCHAR(50) NULL,
        [UsuarioCreacion] NVARCHAR(100) NULL,
        [FechaCreacion] DATETIME NULL,
        [UsuarioModificacion] NVARCHAR(100) NULL,
        [FechaModificacion] DATETIME NULL,
        CONSTRAINT [PK_Rendimiento] PRIMARY KEY CLUSTERED ([RendimientoKey] ASC)
    )
END