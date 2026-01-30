-- =============================================
-- Script de creación de base de datos
-- Sistema HRM Santa Lucía
-- =============================================
-- Crear base de datos si no existe
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'HRM_SantaLucia')
BEGIN
    CREATE DATABASE [HRM_SantaLucia]
    COLLATE SQL_Latin1_General_CP1_CI_AS
END
GO
USE [HRM_SantaLucia]
GO
-- =============================================
-- Crear esquemas
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'DIM')
BEGIN
    EXEC('CREATE SCHEMA [DIM]')
END
GO
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'FACT')
BEGIN
    EXEC('CREATE SCHEMA [FACT]')
END
GO
-- =============================================
-- Tablas de Dimensiones (DIM)
-- =============================================
-- Tabla: Banco
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DIM].[Banco]') AND type in (N'U'))
BEGIN
    CREATE TABLE [DIM].[Banco] (
        [BancoKey] INT IDENTITY(1,1) NOT NULL,
        [NombreBanco] NVARCHAR(100) NOT NULL,
        [CodigoBanco] NVARCHAR(20) NULL,
        [Descripcion] NVARCHAR(500) NULL,
        [Activo] BIT NOT NULL DEFAULT 1,
        [UsuarioCreacion] NVARCHAR(100) NULL,
        [FechaCreacion] DATETIME NULL,
        [UsuarioModificacion] NVARCHAR(100) NULL,
        [FechaModificacion] DATETIME NULL,
        CONSTRAINT [PK_Banco] PRIMARY KEY CLUSTERED ([BancoKey] ASC)
    )
END
GO
-- Tabla: Puesto
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DIM].[Puesto]') AND type in (N'U'))
BEGIN
    CREATE TABLE [DIM].[Puesto] (
        [PuestoKey] INT IDENTITY(1,1) NOT NULL,
        [NombrePuesto] NVARCHAR(100) NOT NULL,
        [Descripcion] NVARCHAR(500) NULL,
        [SalarioMinimo] DECIMAL(18,2) NULL,
        [SalarioMaximo] DECIMAL(18,2) NULL,
        [TipoPuesto] NVARCHAR(50) NULL,
        [Activo] BIT NOT NULL DEFAULT 1,
        [UsuarioCreacion] NVARCHAR(100) NULL,
        [FechaCreacion] DATETIME NULL,
        [UsuarioModificacion] NVARCHAR(100) NULL,
        [FechaModificacion] DATETIME NULL,
        CONSTRAINT [PK_Puesto] PRIMARY KEY CLUSTERED ([PuestoKey] ASC)
    )
END
GO
-- Tabla: Departamento
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DIM].[Departamento]') AND type in (N'U'))
BEGIN
    CREATE TABLE [DIM].[Departamento] (
        [DepartamentoKey] INT IDENTITY(1,1) NOT NULL,
        [NombreDepartamento] NVARCHAR(100) NOT NULL,
        [Descripcion] NVARCHAR(500) NULL,
        [DepartamentoPadreKey] INT NULL,
        [JefeKey] INT NULL,
        [Activo] BIT NOT NULL DEFAULT 1,
        [UsuarioCreacion] NVARCHAR(100) NULL,
        [FechaCreacion] DATETIME NULL,
        [UsuarioModificacion] NVARCHAR(100) NULL,
        [FechaModificacion] DATETIME NULL,
        CONSTRAINT [PK_Departamento] PRIMARY KEY CLUSTERED ([DepartamentoKey] ASC)
    )
END
GO
-- Tabla: Empleado
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DIM].[Empleado]') AND type in (N'U'))
BEGIN
    CREATE TABLE [DIM].[Empleado] (
        [EmpleadoKey] INT IDENTITY(1,1) NOT NULL,
        [EmpleadoID] NVARCHAR(20) NOT NULL,
        [Cedula] NVARCHAR(20) NOT NULL,
        [Nombre] NVARCHAR(100) NOT NULL,
        [Apellido1] NVARCHAR(100) NOT NULL,
        [Apellido2] NVARCHAR(100) NULL,
        [NombreCompleto] AS ([Nombre] + ' ' + [Apellido1] + ' ' + ISNULL([Apellido2], '')) PERSISTED,
        [Email] NVARCHAR(100) NOT NULL,
        [EmailPersonal] NVARCHAR(100) NULL,
        [Telefono] NVARCHAR(20) NOT NULL,
        [TelefonoEmergencia] NVARCHAR(20) NULL,
        [ContactoEmergencia] NVARCHAR(200) NULL,
        [FechaNacimiento] DATE NOT NULL,
        [Edad] AS (DATEDIFF(YEAR, [FechaNacimiento], GETDATE()) - CASE WHEN DATEADD(YEAR, DATEDIFF(YEAR, [FechaNacimiento], GETDATE()), [FechaNacimiento]) > GETDATE() THEN 1 ELSE 0 END) PERSISTED,
        [Genero] NVARCHAR(20) NOT NULL,
        [EstadoCivil] NVARCHAR(20) NULL,
        [Nacionalidad] NVARCHAR(50) NOT NULL,
        [Provincia] NVARCHAR(50) NOT NULL,
        [Canton] NVARCHAR(50) NOT NULL,
        [Distrito] NVARCHAR(50) NOT NULL,
        [DireccionExacta] NVARCHAR(500) NOT NULL,
        [FechaIngreso] DATE NOT NULL,
        [FechaSalida] DATE NULL,
        [TipoContrato] NVARCHAR(50) NOT NULL,
        [PuestoKey] INT NULL,
        [DepartamentoKey] INT NULL,
        [CuentaBancaria] NVARCHAR(50) NULL,
        [BancoKey] INT NULL,
        [NivelEducativo] NVARCHAR(50) NULL,
        [Foto] NVARCHAR(500) NULL,
        [Activo] BIT NOT NULL DEFAULT 1,
        [UsuarioCreacion] NVARCHAR(100) NULL,
        [FechaCreacion] DATETIME NULL,
        [UsuarioModificacion] NVARCHAR(100) NULL,
        [FechaModificacion] DATETIME NULL,
        CONSTRAINT [PK_Empleado] PRIMARY KEY CLUSTERED ([EmpleadoKey] ASC)
    )
END
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
GO
-- =============================================
-- Crear Foreign Keys
-- =============================================

-- Foreign Keys para Empleado
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Empleado_Puesto')
BEGIN
    ALTER TABLE [DIM].[Empleado]
    ADD CONSTRAINT [FK_Empleado_Puesto] 
    FOREIGN KEY ([PuestoKey]) REFERENCES [DIM].[Puesto] ([PuestoKey])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Empleado_Departamento')
BEGIN
    ALTER TABLE [DIM].[Empleado]
    ADD CONSTRAINT [FK_Empleado_Departamento] 
    FOREIGN KEY ([DepartamentoKey]) REFERENCES [DIM].[Departamento] ([DepartamentoKey])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Empleado_Banco')
BEGIN
    ALTER TABLE [DIM].[Empleado]
    ADD CONSTRAINT [FK_Empleado_Banco] 
    FOREIGN KEY ([BancoKey]) REFERENCES [DIM].[Banco] ([BancoKey])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
END
GO
-- Foreign Keys para Departamento (auto-referencia)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Departamento_DepartamentoPadre')
BEGIN
    ALTER TABLE [DIM].[Departamento]
    ADD CONSTRAINT [FK_Departamento_DepartamentoPadre] 
    FOREIGN KEY ([DepartamentoPadreKey]) REFERENCES [DIM].[Departamento] ([DepartamentoKey])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Departamento_Jefe')
BEGIN
    ALTER TABLE [DIM].[Departamento]
    ADD CONSTRAINT [FK_Departamento_Jefe] 
    FOREIGN KEY ([JefeKey]) REFERENCES [DIM].[Empleado] ([EmpleadoKey])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
END
GO
-- Foreign Keys para Nomina
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Nomina_Empleado')
BEGIN
    ALTER TABLE [FACT].[Nomina]
    ADD CONSTRAINT [FK_Nomina_Empleado] 
    FOREIGN KEY ([EmpleadoKey]) REFERENCES [DIM].[Empleado] ([EmpleadoKey])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Nomina_Puesto')
BEGIN
    ALTER TABLE [FACT].[Nomina]
    ADD CONSTRAINT [FK_Nomina_Puesto] 
    FOREIGN KEY ([PuestoKey]) REFERENCES [DIM].[Puesto] ([PuestoKey])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Nomina_Departamento')
BEGIN
    ALTER TABLE [FACT].[Nomina]
    ADD CONSTRAINT [FK_Nomina_Departamento] 
    FOREIGN KEY ([DepartamentoKey]) REFERENCES [DIM].[Departamento] ([DepartamentoKey])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
END
GO
-- Foreign Keys para Asistencia
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Asistencia_Empleado')
BEGIN
    ALTER TABLE [FACT].[Asistencia]
    ADD CONSTRAINT [FK_Asistencia_Empleado] 
    FOREIGN KEY ([EmpleadoKey]) REFERENCES [DIM].[Empleado] ([EmpleadoKey])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
END
GO
-- Foreign Keys para Vacaciones
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Vacaciones_Empleado')
BEGIN
    ALTER TABLE [FACT].[Vacaciones]
    ADD CONSTRAINT [FK_Vacaciones_Empleado] 
    FOREIGN KEY ([EmpleadoKey]) REFERENCES [DIM].[Empleado] ([EmpleadoKey])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Vacaciones_Aprobador')
BEGIN
    ALTER TABLE [FACT].[Vacaciones]
    ADD CONSTRAINT [FK_Vacaciones_Aprobador] 
    FOREIGN KEY ([AprobadorKey]) REFERENCES [DIM].[Empleado] ([EmpleadoKey])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
END
GO
-- Foreign Keys para Rendimiento
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Rendimiento_Empleado')
BEGIN
    ALTER TABLE [FACT].[Rendimiento]
    ADD CONSTRAINT [FK_Rendimiento_Empleado] 
    FOREIGN KEY ([EmpleadoKey]) REFERENCES [DIM].[Empleado] ([EmpleadoKey])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Rendimiento_Evaluador')
BEGIN
    ALTER TABLE [FACT].[Rendimiento]
    ADD CONSTRAINT [FK_Rendimiento_Evaluador] 
    FOREIGN KEY ([EvaluadorKey]) REFERENCES [DIM].[Empleado] ([EmpleadoKey])
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
END
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


