use master;
create database ReconocimientoFacial;
go

use ReconocimientoFacial;
-- tabla de Personas con la informacion de entrenamiento
create table Persona(
	ID int PRIMARY KEY IDENTITY (1, 1),
	nombre nvarchar(200) not null,

	trainData nvarchar(max),
);
-- tabla de imagenes de las personas, la idea es que se almacenen 10 imagenes por cada persona
-- luego de tomarle la foto a la persona se almacena y se re utilizan para reentrenar el algoritmo.
create table ImagenesPersona(
	ID int PRIMARY KEY IDENTITY (1, 1),
	personaID int not null,
	foto nvarchar(max) not null,
	fecha datetime not null,

	FOREIGN KEY (personaID) REFERENCES Persona (ID)
);