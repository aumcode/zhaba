delimiter ;

DROP SCHEMA IF EXISTS `Zhaba`;
CREATE SCHEMA `Zhaba`;

USE `Zhaba`;
source tables.MySQL.sql;
delimiter ;
source foreignkeys.MySQL.sql;
