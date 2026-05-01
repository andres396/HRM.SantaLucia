using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRM.SantaLucia.Web.Migrations
{
    /// <inheritdoc />
    public partial class FixEmpleadoMonetaryColumnTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Idempotente: en bases ya adelantadas a mano las columnas existen pero a veces con
            // decimal(5,2) u otro tipo pequeño → "Arithmetic overflow converting numeric to data type numeric".
            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM sys.columns c INNER JOIN sys.tables t ON c.object_id = t.object_id INNER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = N'DIM' AND t.name = N'Empleado' AND c.name = N'SalarioBase')
  ALTER TABLE [DIM].[Empleado] ADD [SalarioBase] DECIMAL(18,2) NULL;
ELSE
  ALTER TABLE [DIM].[Empleado] ALTER COLUMN [SalarioBase] DECIMAL(18,2) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns c INNER JOIN sys.tables t ON c.object_id = t.object_id INNER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = N'DIM' AND t.name = N'Empleado' AND c.name = N'SalarioNeto')
  ALTER TABLE [DIM].[Empleado] ADD [SalarioNeto] DECIMAL(18,2) NULL;
ELSE
  ALTER TABLE [DIM].[Empleado] ALTER COLUMN [SalarioNeto] DECIMAL(18,2) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns c INNER JOIN sys.tables t ON c.object_id = t.object_id INNER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = N'DIM' AND t.name = N'Empleado' AND c.name = N'Rebajos')
  ALTER TABLE [DIM].[Empleado] ADD [Rebajos] DECIMAL(18,2) NULL;
ELSE
  ALTER TABLE [DIM].[Empleado] ALTER COLUMN [Rebajos] DECIMAL(18,2) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns c INNER JOIN sys.tables t ON c.object_id = t.object_id INNER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = N'DIM' AND t.name = N'Empleado' AND c.name = N'CCSS')
  ALTER TABLE [DIM].[Empleado] ADD [CCSS] DECIMAL(18,2) NULL;
ELSE
  ALTER TABLE [DIM].[Empleado] ALTER COLUMN [CCSS] DECIMAL(18,2) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns c INNER JOIN sys.tables t ON c.object_id = t.object_id INNER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = N'DIM' AND t.name = N'Empleado' AND c.name = N'JUPEMA')
  ALTER TABLE [DIM].[Empleado] ADD [JUPEMA] DECIMAL(18,2) NULL;
ELSE
  ALTER TABLE [DIM].[Empleado] ALTER COLUMN [JUPEMA] DECIMAL(18,2) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns c INNER JOIN sys.tables t ON c.object_id = t.object_id INNER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = N'DIM' AND t.name = N'Empleado' AND c.name = N'Magisterio')
  ALTER TABLE [DIM].[Empleado] ADD [Magisterio] DECIMAL(18,2) NULL;
ELSE
  ALTER TABLE [DIM].[Empleado] ALTER COLUMN [Magisterio] DECIMAL(18,2) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns c INNER JOIN sys.tables t ON c.object_id = t.object_id INNER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = N'DIM' AND t.name = N'Empleado' AND c.name = N'PorcentajeBP')
  ALTER TABLE [DIM].[Empleado] ADD [PorcentajeBP] DECIMAL(18,2) NULL;
ELSE
  ALTER TABLE [DIM].[Empleado] ALTER COLUMN [PorcentajeBP] DECIMAL(18,2) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns c INNER JOIN sys.tables t ON c.object_id = t.object_id INNER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = N'DIM' AND t.name = N'Empleado' AND c.name = N'Bonos')
  ALTER TABLE [DIM].[Empleado] ADD [Bonos] DECIMAL(18,2) NULL;
ELSE
  ALTER TABLE [DIM].[Empleado] ALTER COLUMN [Bonos] DECIMAL(18,2) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns c INNER JOIN sys.tables t ON c.object_id = t.object_id INNER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = N'DIM' AND t.name = N'Empleado' AND c.name = N'Sede')
  ALTER TABLE [DIM].[Empleado] ADD [Sede] NVARCHAR(50) NULL;
ELSE
  ALTER TABLE [DIM].[Empleado] ALTER COLUMN [Sede] NVARCHAR(50) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns c INNER JOIN sys.tables t ON c.object_id = t.object_id INNER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = N'FACT' AND t.name = N'Nomina' AND c.name = N'Aguinaldo')
  ALTER TABLE [FACT].[Nomina] ADD [Aguinaldo] DECIMAL(18,2) NOT NULL CONSTRAINT [DF_Nomina_Aguinaldo_FixMon] DEFAULT (0);
ELSE
BEGIN
  UPDATE [FACT].[Nomina] SET [Aguinaldo] = ISNULL([Aguinaldo], 0);
  ALTER TABLE [FACT].[Nomina] ALTER COLUMN [Aguinaldo] DECIMAL(18,2) NOT NULL;
END

IF NOT EXISTS (SELECT 1 FROM sys.columns c INNER JOIN sys.tables t ON c.object_id = t.object_id INNER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = N'FACT' AND t.name = N'Nomina' AND c.name = N'DeduccionesQuincenales')
  ALTER TABLE [FACT].[Nomina] ADD [DeduccionesQuincenales] DECIMAL(18,2) NOT NULL CONSTRAINT [DF_Nomina_DedQuin_FixMon] DEFAULT (0);
ELSE
BEGIN
  UPDATE [FACT].[Nomina] SET [DeduccionesQuincenales] = ISNULL([DeduccionesQuincenales], 0);
  ALTER TABLE [FACT].[Nomina] ALTER COLUMN [DeduccionesQuincenales] DECIMAL(18,2) NOT NULL;
END

IF NOT EXISTS (SELECT 1 FROM sys.columns c INNER JOIN sys.tables t ON c.object_id = t.object_id INNER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = N'FACT' AND t.name = N'Nomina' AND c.name = N'ExtrasQuincenales')
  ALTER TABLE [FACT].[Nomina] ADD [ExtrasQuincenales] DECIMAL(18,2) NOT NULL CONSTRAINT [DF_Nomina_ExtQuin_FixMon] DEFAULT (0);
ELSE
BEGIN
  UPDATE [FACT].[Nomina] SET [ExtrasQuincenales] = ISNULL([ExtrasQuincenales], 0);
  ALTER TABLE [FACT].[Nomina] ALTER COLUMN [ExtrasQuincenales] DECIMAL(18,2) NOT NULL;
END

IF NOT EXISTS (SELECT 1 FROM sys.columns c INNER JOIN sys.tables t ON c.object_id = t.object_id INNER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = N'FACT' AND t.name = N'Nomina' AND c.name = N'Feriados')
  ALTER TABLE [FACT].[Nomina] ADD [Feriados] DECIMAL(18,2) NOT NULL CONSTRAINT [DF_Nomina_Feriados_FixMon] DEFAULT (0);
ELSE
BEGIN
  UPDATE [FACT].[Nomina] SET [Feriados] = ISNULL([Feriados], 0);
  ALTER TABLE [FACT].[Nomina] ALTER COLUMN [Feriados] DECIMAL(18,2) NOT NULL;
END

IF NOT EXISTS (SELECT 1 FROM sys.columns c INNER JOIN sys.tables t ON c.object_id = t.object_id INNER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = N'FACT' AND t.name = N'Nomina' AND c.name = N'Miscelaneo')
  ALTER TABLE [FACT].[Nomina] ADD [Miscelaneo] DECIMAL(18,2) NOT NULL CONSTRAINT [DF_Nomina_Misc_FixMon] DEFAULT (0);
ELSE
BEGIN
  UPDATE [FACT].[Nomina] SET [Miscelaneo] = ISNULL([Miscelaneo], 0);
  ALTER TABLE [FACT].[Nomina] ALTER COLUMN [Miscelaneo] DECIMAL(18,2) NOT NULL;
END

IF NOT EXISTS (SELECT 1 FROM sys.columns c INNER JOIN sys.tables t ON c.object_id = t.object_id INNER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = N'FACT' AND t.name = N'Nomina' AND c.name = N'PagoHorasRegulares')
  ALTER TABLE [FACT].[Nomina] ADD [PagoHorasRegulares] DECIMAL(18,2) NOT NULL CONSTRAINT [DF_Nomina_PagoHrReg_FixMon] DEFAULT (0);
ELSE
BEGIN
  UPDATE [FACT].[Nomina] SET [PagoHorasRegulares] = ISNULL([PagoHorasRegulares], 0);
  ALTER TABLE [FACT].[Nomina] ALTER COLUMN [PagoHorasRegulares] DECIMAL(18,2) NOT NULL;
END

IF NOT EXISTS (SELECT 1 FROM sys.columns c INNER JOIN sys.tables t ON c.object_id = t.object_id INNER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = N'FACT' AND t.name = N'Nomina' AND c.name = N'QTYDiasFeriados')
  ALTER TABLE [FACT].[Nomina] ADD [QTYDiasFeriados] DECIMAL(18,2) NOT NULL CONSTRAINT [DF_Nomina_QTYDF_FixMon] DEFAULT (0);
ELSE
BEGIN
  UPDATE [FACT].[Nomina] SET [QTYDiasFeriados] = ISNULL([QTYDiasFeriados], 0);
  ALTER TABLE [FACT].[Nomina] ALTER COLUMN [QTYDiasFeriados] DECIMAL(18,2) NOT NULL;
END

IF NOT EXISTS (SELECT 1 FROM sys.columns c INNER JOIN sys.tables t ON c.object_id = t.object_id INNER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = N'FACT' AND t.name = N'Nomina' AND c.name = N'QTYHorasExtras')
  ALTER TABLE [FACT].[Nomina] ADD [QTYHorasExtras] DECIMAL(18,2) NOT NULL CONSTRAINT [DF_Nomina_QTYHE_FixMon] DEFAULT (0);
ELSE
BEGIN
  UPDATE [FACT].[Nomina] SET [QTYHorasExtras] = ISNULL([QTYHorasExtras], 0);
  ALTER TABLE [FACT].[Nomina] ALTER COLUMN [QTYHorasExtras] DECIMAL(18,2) NOT NULL;
END

IF NOT EXISTS (SELECT 1 FROM sys.columns c INNER JOIN sys.tables t ON c.object_id = t.object_id INNER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = N'FACT' AND t.name = N'Nomina' AND c.name = N'QTYHorasRegulares')
  ALTER TABLE [FACT].[Nomina] ADD [QTYHorasRegulares] DECIMAL(18,2) NOT NULL CONSTRAINT [DF_Nomina_QTYHR_FixMon] DEFAULT (0);
ELSE
BEGIN
  UPDATE [FACT].[Nomina] SET [QTYHorasRegulares] = ISNULL([QTYHorasRegulares], 0);
  ALTER TABLE [FACT].[Nomina] ALTER COLUMN [QTYHorasRegulares] DECIMAL(18,2) NOT NULL;
END

IF NOT EXISTS (SELECT 1 FROM sys.columns c INNER JOIN sys.tables t ON c.object_id = t.object_id INNER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = N'FACT' AND t.name = N'Nomina' AND c.name = N'Sede')
  ALTER TABLE [FACT].[Nomina] ADD [Sede] NVARCHAR(50) NULL;
ELSE
  ALTER TABLE [FACT].[Nomina] ALTER COLUMN [Sede] NVARCHAR(50) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.columns c INNER JOIN sys.tables t ON c.object_id = t.object_id INNER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = N'FACT' AND t.name = N'Asistencia' AND c.name = N'GoceSalario')
  ALTER TABLE [FACT].[Asistencia] ADD [GoceSalario] BIT NOT NULL CONSTRAINT [DF_Asist_GoceSal_FixMon] DEFAULT (0);
ELSE
BEGIN
  UPDATE [FACT].[Asistencia] SET [GoceSalario] = ISNULL([GoceSalario], 0);
  ALTER TABLE [FACT].[Asistencia] ALTER COLUMN [GoceSalario] BIT NOT NULL;
END

IF NOT EXISTS (SELECT 1 FROM sys.columns c INNER JOIN sys.tables t ON c.object_id = t.object_id INNER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = N'FACT' AND t.name = N'Asistencia' AND c.name = N'MotivoPermiso')
  ALTER TABLE [FACT].[Asistencia] ADD [MotivoPermiso] NVARCHAR(500) NULL;
ELSE
  ALTER TABLE [FACT].[Asistencia] ALTER COLUMN [MotivoPermiso] NVARCHAR(500) NULL;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Sin reversa: ampliar tipos / columnas nuevas no debe deshacerse en producción.
        }
    }
}
