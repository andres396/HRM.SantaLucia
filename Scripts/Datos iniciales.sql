-- Script de datos iniciales para HRM Santa Lucía
-- Ejecutar después de crear la base de datos y aplicar las migraciones

USE [HRM_SantaLucia]
GO

-- Insertar Departamentos
INSERT INTO [DIM].[Departamento] ([NombreDepartamento], [Descripcion], [Activo], [FechaCreacion])
VALUES 
    ('Recursos Humanos', 'Gestión de personal y talento humano', 1, GETDATE()),
    ('Tecnología', 'Departamento de sistemas y tecnología', 1, GETDATE()),
    ('Administración', 'Gestión administrativa', 1, GETDATE()),
    ('Académico', 'Departamento académico', 1, GETDATE()),
    ('Finanzas', 'Gestión financiera', 1, GETDATE())
GO

-- Insertar Puestos
INSERT INTO [DIM].[Puesto] ([NombrePuesto], [Descripcion], [SalarioMinimo], [SalarioMaximo], [TipoPuesto], [Activo], [FechaCreacion])
VALUES 
    ('Director', 'Director general', 2000000, 3000000, 'Administrativo', 1, GETDATE()),
    ('Gerente de Recursos Humanos', 'Gerente de RRHH', 1500000, 2000000, 'Administrativo', 1, GETDATE()),
    ('Analista de Sistemas', 'Analista de TI', 800000, 1200000, 'Administrativo', 1, GETDATE()),
    ('Profesor', 'Profesor de tiempo completo', 600000, 1000000, 'Docente', 1, GETDATE()),
    ('Asistente Administrativo', 'Asistente administrativo', 500000, 700000, 'Administrativo', 1, GETDATE()),
    ('Contador', 'Contador general', 700000, 1000000, 'Administrativo', 1, GETDATE())
GO

-- Insertar Bancos
INSERT INTO [DIM].[Banco] ([NombreBanco], [CodigoBanco], [Descripcion], [Activo], [FechaCreacion])
VALUES 
    ('Banco Nacional de Costa Rica', 'BNCR', 'Banco Nacional', 1, GETDATE()),
    ('Banco de Costa Rica', 'BCR', 'Banco de Costa Rica', 1, GETDATE()),
    ('Banco Popular', 'BPOP', 'Banco Popular y de Desarrollo Comunal', 1, GETDATE()),
    ('Scotiabank', 'SCOT', 'Scotiabank Costa Rica', 1, GETDATE()),
    ('BAC Credomatic', 'BAC', 'BAC Credomatic', 1, GETDATE())
GO

PRINT 'Datos iniciales insertados correctamente'
GO