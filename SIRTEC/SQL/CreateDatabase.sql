USE master;
GO

-- Crear base de datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'SIRTEC')
BEGIN
    CREATE DATABASE SIRTEC;
END
GO

USE SIRTEC;
GO

-- Tabla Semestre
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Semestre')
BEGIN
    CREATE TABLE Semestre (
        id_semestre INT PRIMARY KEY IDENTITY(1,1),
        n_semestre INT NOT NULL
    );
END
GO

-- Tabla Docentes
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Docentes')
BEGIN
    CREATE TABLE Docentes (
        id_docente INT PRIMARY KEY IDENTITY(1,1),
        nombre NVARCHAR(100) NOT NULL,
        habilitado BIT DEFAULT 1
    );
END
GO

-- Tabla Materias
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Materias')
BEGIN
    CREATE TABLE Materias (
        id_materia INT PRIMARY KEY IDENTITY(1,1),
        n_materia NVARCHAR(100) NOT NULL,
        hora TIME NOT NULL,
        aula NVARCHAR(20) NOT NULL,
        grupo NVARCHAR(10) NOT NULL,
        id_docente INT,
        FOREIGN KEY (id_docente) REFERENCES Docentes(id_docente)
    );
END
GO

-- Tabla Alumnos
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Alumnos')
BEGIN
    CREATE TABLE Alumnos (
        id_alumno INT PRIMARY KEY IDENTITY(1,1),
        n_control NVARCHAR(20) NOT NULL UNIQUE,
        nombre NVARCHAR(50) NOT NULL,
        a_paterno NVARCHAR(50) NOT NULL,
        a_materno NVARCHAR(50) NOT NULL,
        id_semestre INT,
        e_mail NVARCHAR(100),
        f_nacimiento DATE,
        calle NVARCHAR(100),
        colonia NVARCHAR(100),
        codpostal NVARCHAR(10),
        num_exterior NVARCHAR(10),
        ciudad NVARCHAR(100),
        estado NVARCHAR(50),
        FOREIGN KEY (id_semestre) REFERENCES Semestre(id_semestre)
    );
END
GO

-- Tabla Paquetes
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Paquetes')
BEGIN
    CREATE TABLE Paquetes (
        id_paquete INT PRIMARY KEY IDENTITY(1,1),
        id_materia INT NOT NULL,
        id_semestre INT NOT NULL,
        n_paquete INT NOT NULL,
        FOREIGN KEY (id_materia) REFERENCES Materias(id_materia),
        FOREIGN KEY (id_semestre) REFERENCES Semestre(id_semestre)
    );
END
GO

-- Tabla Usuarios
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Usuarios')
BEGIN
    CREATE TABLE Usuarios (
        id_usuarios INT PRIMARY KEY IDENTITY(1,1),
        tipo_usuario NVARCHAR(20) NOT NULL CHECK (tipo_usuario IN ('coordinador', 'docente', 'alumno')),
        username NVARCHAR(100) NOT NULL UNIQUE,
        password NVARCHAR(100) NOT NULL,
        id_docente INT NULL,
        id_alumno INT NULL,
        FOREIGN KEY (id_docente) REFERENCES Docentes(id_docente),
        FOREIGN KEY (id_alumno) REFERENCES Alumnos(id_alumno)
    );
END
GO

-- Tabla Horarios
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Horarios')
BEGIN
    CREATE TABLE Horarios (
        id_horario INT PRIMARY KEY IDENTITY(1,1),
        id_alumno INT NOT NULL,
        id_materia INT NOT NULL,
        FOREIGN KEY (id_alumno) REFERENCES Alumnos(id_alumno),
        FOREIGN KEY (id_materia) REFERENCES Materias(id_materia)
    );
END
GO

-- Tabla Calificaciones
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Calificaciones')
BEGIN
    CREATE TABLE Calificaciones (
        id_calificacion INT PRIMARY KEY IDENTITY(1,1),
        id_alumno INT NOT NULL,
        id_materia INT NOT NULL,
        calificacion DECIMAL(5,2) NOT NULL,
        fecha_registro DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (id_alumno) REFERENCES Alumnos(id_alumno),
        FOREIGN KEY (id_materia) REFERENCES Materias(id_materia)
    );
END
GO

-- Tabla Documentos
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Documentos')
BEGIN
    CREATE TABLE Documentos (
        id_documento INT PRIMARY KEY IDENTITY(1,1),
        n_documento NVARCHAR(100) NOT NULL,
        tipo_documento NVARCHAR(50) NOT NULL,
        extension NVARCHAR(10) NOT NULL,
        contenido VARBINARY(MAX) NOT NULL,
        id_alumno INT NOT NULL,
        FOREIGN KEY (id_alumno) REFERENCES Alumnos(id_alumno)
    );
END
GO

-- Datos iniciales para semestres
IF NOT EXISTS (SELECT * FROM Semestre)
BEGIN
    INSERT INTO Semestre (n_semestre) VALUES 
    (1), (2), (3), (4), (5), (6), (7), (8);
END
GO