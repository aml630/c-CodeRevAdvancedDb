##DB Usage

##Author
Alex Larson

##Project
This project uses the Nancy framework to display data which has been created, saved, updated, and destroyed.  It also utilizes join statements for many to many relationships.  

##Description
A user can input any number of stores and any number of brands being sold in those stores.  A user can see which brands are being sold for each store, as well as which store carries which brands.  This is accomplished via join statements and join tables.    

##Directions
Setup/Installation Requirements

Clone this repository.
Use the scripts.sql in the root directory to make the databases. Or follow these commands in SQLCMD/SQL Server to create shoe_stores and shoe_stores_test:
CREATE DATABASE shoe_stores;
GO
CREATE TABLE stores (id INT IDENTITY(1,1), name VARCHAR(255));
CREATE TABLE brands (id INT IDENTITY(1,1), name VARCHAR(255));
CREATE TABLE brands_stores (id INT IDENTITY(1,1), shoes_id INT, brands_id INT);
GO
Install Nancy the web viewer
Build the project using "dnu restore".
Run the project by calling "dnx kestrel"

##Tools Used
C#
Nancy  
Kestrel
DNX451  
Razor
MSSQL

##License
This project is released under the [MIT License]
