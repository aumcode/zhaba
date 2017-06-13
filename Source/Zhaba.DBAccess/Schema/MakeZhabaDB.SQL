delimiter ;

DROP SCHEMA IF EXISTS `Zhaba1`;
CREATE SCHEMA `Zhaba1`;

USE `Zhaba1`;
source tables.MySQL.sql;
delimiter ;
source foreignkeys.MySQL.sql;
