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