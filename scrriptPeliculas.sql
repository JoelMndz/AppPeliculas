create database peliculasdb;
go
use peliculasdb
go
create table usuarios(
	id int identity(1,1) primary key,
	nombres varchar(50),
	apellidos varchar(50),
	email varchar(50),
	password varchar(50)
);

create table generos(
	id int identity(1,1) primary key,
	descripcion varchar(50)
);

create table peliculas(
	id int identity(1,1) primary key,
	titulo varchar(50),
	anio int,
	director varchar(100),
	fotoURL text,
	id_genero int,
	id_usuario int,
	foreign key(id_genero) references generos(id),
	foreign key(id_usuario) references usuarios(id)
);