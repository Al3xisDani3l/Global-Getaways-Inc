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

create table Packages(
Id int identity(1,1) primary key not null,

)

Create table Places(
Id int identity(1,1) primary key not null,

)


create table Reviews(
Id int identity(1,1) primary key not null,
UserId int not null,
PlaceId int not null,
Rating int check ( Rating >= 1 and Rating <=5),
constraint fk_pu foreign key (PlaceId) references Places(id),
constraint fk_ru foreign key (userId) references Users(id)
)


Create table Keywords(
Id int identity(1,1) primary key not null,
UserId int not null , 
Words varchar not null ,
constraint fk_ku foreign key (UserId) references Users(Id)
)




