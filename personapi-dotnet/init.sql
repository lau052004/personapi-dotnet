CREATE DATABASE persona_db;
GO
USE persona_db;
GO

CREATE TABLE profesion (
  id   INT IDENTITY(1,1) PRIMARY KEY,
  nom  VARCHAR(90)   NOT NULL,
  des  TEXT          NULL
);
GO

CREATE TABLE persona (
  cc       BIGINT      PRIMARY KEY,
  nombre   VARCHAR(45) NOT NULL,
  apellido VARCHAR(45) NOT NULL,
  genero   CHAR(1)     NOT NULL CHECK (genero IN('M','F')),
  edad     INT         NOT NULL
);
GO

CREATE TABLE estudios (
  IdProf INT    NOT NULL,
  CcPer  BIGINT NOT NULL,
  Fecha  DATE   NOT NULL,
  Univer VARCHAR(50) NOT NULL,
  CONSTRAINT PK_Estudios PRIMARY KEY (IdProf, CcPer),
  CONSTRAINT FK_Estudios_Profesion FOREIGN KEY (IdProf) REFERENCES profesion(id),
  CONSTRAINT FK_Estudios_Persona FOREIGN KEY (CcPer)  REFERENCES persona(cc)
);
GO

CREATE TABLE telefono (
  Num    VARCHAR(15) PRIMARY KEY,
  Oper   VARCHAR(45) NOT NULL,
  Duenio BIGINT      NOT NULL,
  CONSTRAINT FK_Telefono_Persona FOREIGN KEY (Duenio) REFERENCES persona(cc)
);
GO
