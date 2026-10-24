/* =====================================================================
   Base de datos LOCAL - API.Samuel (Estudio Fotográfico Enfoque)
   ---------------------------------------------------------------------
   Crea la base EstudioEnfoqueDB, las tablas, los procedimientos
   almacenados y datos de prueba ficticios.
   Se puede ejecutar varias veces sin problema (F5 en SSMS).
   ===================================================================== */

IF DB_ID('EstudioEnfoqueDB') IS NULL
    CREATE DATABASE EstudioEnfoqueDB;
GO

USE EstudioEnfoqueDB;
GO

-- =====================================================================
-- TABLAS
-- =====================================================================

IF OBJECT_ID('dbo.TiposSesion', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.TiposSesion
    (
        IdTipoSesion      INT IDENTITY(1,1) PRIMARY KEY,
        NombreTipoSesion  NVARCHAR(50) NOT NULL
    );
END
GO

IF OBJECT_ID('dbo.Clientes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Clientes
    (
        IdCliente      INT IDENTITY(1,1) PRIMARY KEY,
        Correo         NVARCHAR(100) NOT NULL CONSTRAINT UQ_Clientes_Correo UNIQUE,
        Contrasena     NVARCHAR(50)  NOT NULL,
        Nombre         NVARCHAR(50)  NOT NULL,
        Apellido1      NVARCHAR(50)  NOT NULL,
        Apellido2      NVARCHAR(50)  NOT NULL,
        Telefono       NVARCHAR(20)  NOT NULL,
        Provincia      NVARCHAR(30)  NOT NULL,
        IdTipoSesion   INT           NOT NULL,
        FechaRegistro  DATETIME      NOT NULL CONSTRAINT DF_Clientes_FechaRegistro DEFAULT GETDATE(),
        CONSTRAINT FK_Clientes_TiposSesion FOREIGN KEY (IdTipoSesion) REFERENCES dbo.TiposSesion(IdTipoSesion)
    );
END
GO

-- =====================================================================
-- DATOS DE PRUEBA (ficticios)
-- =====================================================================

IF NOT EXISTS (SELECT 1 FROM dbo.TiposSesion)
BEGIN
    INSERT INTO dbo.TiposSesion (NombreTipoSesion)
    VALUES (N'Boda'), (N'Quince años'), (N'Retrato profesional'), (N'Familiar'), (N'Producto / comercial'), (N'Graduación');
END
GO

-- Contraseña de prueba de todos los clientes: Enfoque2026*
IF NOT EXISTS (SELECT 1 FROM dbo.Clientes)
BEGIN
    INSERT INTO dbo.Clientes (Correo, Contrasena, Nombre, Apellido1, Apellido2, Telefono, Provincia, IdTipoSesion)
    VALUES
    (N'valeria.mora@ejemplo.com',     N'Enfoque2026*', N'Valeria',  N'Mora',      N'Quesada',  N'+506 8712-4590', N'San José',   1),
    (N'diego.solano@ejemplo.com',     N'Enfoque2026*', N'Diego',    N'Solano',    N'Araya',    N'+506 6034-2218', N'Heredia',    3),
    (N'mariana.jimenez@ejemplo.com',  N'Enfoque2026*', N'Mariana',  N'Jiménez',   N'Rojas',    N'+506 8845-7712', N'Alajuela',   2),
    (N'andres.vargas@ejemplo.com',    N'Enfoque2026*', N'Andrés',   N'Vargas',    N'Castro',   N'+506 7290-1534', N'Cartago',    4),
    (N'sofia.chaves@ejemplo.com',     N'Enfoque2026*', N'Sofía',    N'Chaves',    N'Méndez',   N'+506 8401-6627', N'Puntarenas', 6),
    (N'kevin.ramirez@ejemplo.com',    N'Enfoque2026*', N'Kevin',    N'Ramírez',   N'Fallas',   N'+506 6158-9043', N'Limón',      5);
END
GO

-- =====================================================================
-- PROCEDIMIENTOS ALMACENADOS
-- =====================================================================

-- Iniciar sesión: devuelve Existe, Mensaje, IdCliente y NombreCompleto
CREATE OR ALTER PROCEDURE dbo.spIniciarSesionCliente
    @pCorreo      NVARCHAR(100),
    @pContrasena  NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.Clientes WHERE Correo = @pCorreo AND Contrasena = @pContrasena)
        SELECT TOP 1
            CAST(1 AS BIT)                               AS Existe,
            N'Inicio de sesión exitoso. ¡Bienvenido(a) a Enfoque!' AS Mensaje,
            IdCliente,
            Nombre + ' ' + Apellido1 + ' ' + Apellido2   AS NombreCompleto
        FROM dbo.Clientes
        WHERE Correo = @pCorreo AND Contrasena = @pContrasena;
    ELSE
        SELECT
            CAST(0 AS BIT)                    AS Existe,
            N'Correo o contraseña incorrectos' AS Mensaje,
            0                                 AS IdCliente,
            ''                                AS NombreCompleto;
END
GO

-- Insertar: devuelve el Id del cliente nuevo, o -1 si el correo ya existe
CREATE OR ALTER PROCEDURE dbo.spInsertarCliente
    @pCorreo        NVARCHAR(100),
    @pContrasena    NVARCHAR(50),
    @pNombre        NVARCHAR(50),
    @pApellido1     NVARCHAR(50),
    @pApellido2     NVARCHAR(50),
    @pTelefono      NVARCHAR(20),
    @pProvincia     NVARCHAR(30),
    @pIdTipoSesion  INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.Clientes WHERE Correo = @pCorreo)
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END

    INSERT INTO dbo.Clientes (Correo, Contrasena, Nombre, Apellido1, Apellido2, Telefono, Provincia, IdTipoSesion)
    VALUES (@pCorreo, @pContrasena, @pNombre, @pApellido1, @pApellido2, @pTelefono, @pProvincia, @pIdTipoSesion);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Resultado;
END
GO

-- Consultar todos los clientes
CREATE OR ALTER PROCEDURE dbo.spObtenerClientes
AS
BEGIN
    SET NOCOUNT ON;

    SELECT c.IdCliente, c.Correo, c.Nombre, c.Apellido1, c.Apellido2, c.Telefono,
           c.Provincia, c.IdTipoSesion, t.NombreTipoSesion, c.FechaRegistro
    FROM dbo.Clientes c
    INNER JOIN dbo.TiposSesion t ON t.IdTipoSesion = c.IdTipoSesion
    ORDER BY c.IdCliente;
END
GO

-- Consultar un cliente por Id
CREATE OR ALTER PROCEDURE dbo.spObtenerClientePorId
    @pIdCliente INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT c.IdCliente, c.Correo, c.Nombre, c.Apellido1, c.Apellido2, c.Telefono,
           c.Provincia, c.IdTipoSesion, t.NombreTipoSesion, c.FechaRegistro
    FROM dbo.Clientes c
    INNER JOIN dbo.TiposSesion t ON t.IdTipoSesion = c.IdTipoSesion
    WHERE c.IdCliente = @pIdCliente;
END
GO

-- Actualizar: devuelve 1 si actualizó, 0 si no existe, -1 si el correo lo usa otro cliente
CREATE OR ALTER PROCEDURE dbo.spActualizarCliente
    @pIdCliente     INT,
    @pCorreo        NVARCHAR(100),
    @pContrasena    NVARCHAR(50),
    @pNombre        NVARCHAR(50),
    @pApellido1     NVARCHAR(50),
    @pApellido2     NVARCHAR(50),
    @pTelefono      NVARCHAR(20),
    @pProvincia     NVARCHAR(30),
    @pIdTipoSesion  INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.Clientes WHERE Correo = @pCorreo AND IdCliente <> @pIdCliente)
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END

    UPDATE dbo.Clientes
    SET Correo       = @pCorreo,
        Contrasena   = @pContrasena,
        Nombre       = @pNombre,
        Apellido1    = @pApellido1,
        Apellido2    = @pApellido2,
        Telefono     = @pTelefono,
        Provincia    = @pProvincia,
        IdTipoSesion = @pIdTipoSesion
    WHERE IdCliente = @pIdCliente;

    SELECT @@ROWCOUNT AS Resultado;
END
GO

-- Eliminar: devuelve 1 si eliminó, 0 si no existe
CREATE OR ALTER PROCEDURE dbo.spEliminarCliente
    @pIdCliente INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.Clientes
    WHERE IdCliente = @pIdCliente;

    SELECT @@ROWCOUNT AS Resultado;
END
GO

PRINT 'Base de datos EstudioEnfoqueDB lista.';
