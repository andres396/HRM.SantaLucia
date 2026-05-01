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