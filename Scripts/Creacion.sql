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