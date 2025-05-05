-- Creaci�n condicional de la base de datos
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'persona_db')
BEGIN
    CREATE DATABASE persona_db;
    PRINT 'Base de datos persona_db creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La base de datos persona_db ya existe.';
END
GO

USE persona_db;
GO

-- Tabla profesi�n (mejorada con IDENTITY y NVARCHAR)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'profesion')
BEGIN
    CREATE TABLE profesion (
        id INT IDENTITY(1,1) NOT NULL,
        nom NVARCHAR(90) NOT NULL,
        des NVARCHAR(MAX) NULL,
        CONSTRAINT PK_Profesion PRIMARY KEY (id)
    );
    PRINT 'Tabla profesion creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La tabla profesion ya existe.';
END
GO

-- Tabla persona (conservando BIGINT para cc como en tu proyecto original)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'persona')
BEGIN
    CREATE TABLE persona (
        cc INT NOT NULL,
        nombre NVARCHAR(45) NOT NULL,
        apellido NVARCHAR(45) NOT NULL,
        genero CHAR(1) NOT NULL,
        edad INT NOT NULL,
        CONSTRAINT PK_Persona PRIMARY KEY (cc),
        CONSTRAINT CK_Genero CHECK (genero IN ('M', 'F'))
    );
    PRINT 'Tabla persona creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La tabla persona ya existe.';
END
GO

-- Tabla estudios (con nombres expl�citos para constraints)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'estudios')
BEGIN
    CREATE TABLE estudios (
        id_prof INT NOT NULL,
        cc_per INT NOT NULL,  -- Cambiado a BIGINT para coincidir con persona.cc
        fecha DATE NOT NULL,
        univer NVARCHAR(50) NOT NULL,
        CONSTRAINT PK_Estudios PRIMARY KEY (id_prof, cc_per),
        CONSTRAINT FK_Estudios_Profesion FOREIGN KEY (id_prof) REFERENCES profesion(id),
        CONSTRAINT FK_Estudios_Persona FOREIGN KEY (cc_per) REFERENCES persona(cc)
    );
    PRINT 'Tabla estudios creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La tabla estudios ya existe.';
END
GO

-- Tabla tel�fono (mejorada con NVARCHAR)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'telefono')
BEGIN
    CREATE TABLE telefono (
        num NVARCHAR(15) NOT NULL,
        oper NVARCHAR(45) NOT NULL,
        duenio INT NOT NULL,  -- Cambiado a BIGINT para coincidir con persona.cc
        CONSTRAINT PK_Telefono PRIMARY KEY (num),
        CONSTRAINT FK_Telefono_Persona FOREIGN KEY (duenio) REFERENCES persona(cc)
    );
    PRINT 'Tabla telefono creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La tabla telefono ya existe.';
END
GO

-- �ndices adicionales para mejorar rendimiento
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Telefono_Duenio')
BEGIN
    CREATE INDEX IX_Telefono_Duenio ON telefono(duenio);
    PRINT '�ndice IX_Telefono_Duenio creado exitosamente.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Estudios_CcPer')
BEGIN
    CREATE INDEX IX_Estudios_CcPer ON estudios(cc_per);
    PRINT '�ndice IX_Estudios_CcPer creado exitosamente.';
END
GO

PRINT 'Script ejecutado completamente. Verifique los mensajes anteriores para confirmar.';


/* ----------------------------------------------------
   Semilla de datos para persona_db
   Ejecutar después de haber creado las tablas
   ---------------------------------------------------*/
USE persona_db;
GO

/* ----------  PROFESION  ---------- */
SET IDENTITY_INSERT profesion ON;
INSERT INTO profesion (id, nom, des) VALUES
(1, 'Ingeniería de Software', 'Desarrollo y mantenimiento de aplicaciones'),
(2, 'Medicina',               'Diagnóstico y tratamiento de enfermedades'),
(3, 'Diseño Gráfico',         'Comunicación visual y branding');
SET IDENTITY_INSERT profesion OFF;
GO

/* ----------  PERSONA  ------------ */
INSERT INTO persona (cc, nombre, apellido, genero, edad) VALUES
(10000001, 'Laura',  'Ovalle', 'F', 25),
(10000002, 'Juan',   'Pérez',  'M', 30),
(10000003, 'Mariana','Gómez',  'F', 28);
GO

/* ----------  TELEFONO  ----------- */
INSERT INTO telefono (num, oper, duenio) VALUES
('3001112233', 'Movistar', 10000001),
('3015556677', 'Claro',    10000002),
('3029998888', 'Tigo',     10000003);
GO

/* ----------  ESTUDIOS  ----------- */
INSERT INTO estudios (id_prof, cc_per, fecha, univer) VALUES
(1, 10000001, '2023‑12‑01', 'Universidad Nacional'),
(2, 10000002, '2020‑06‑15', 'Universidad de los Andes'),
(3, 10000003, '2022‑03‑10', 'Universidad Javeriana');
GO