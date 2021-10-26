if OBJECT_ID('ggdatabase') is not null
drop database ggdatabase;
go

create database ggdatabase;
go

use ggdatabase;
go

create table Users(
Id int identity(1,1) primary key not null,
"Guid" uniqueidentifier unique not null,
"Name" varchar(30) not null,
Lastname varchar(30) not null,
birthday date not null,
PhoneNumber varchar(10) null,
InternationalLada varchar(5) null,
Gender varchar check (Gender = 'Male' or Gender = 'Female'),
Monthlysalary money null,
nationality varchar not null

)

Create table Keywords(
Id int identity(1,1) primary key not null,
UserId int not null foreign key, 
Words varchar not null 





