delimiter ;

DROP SCHEMA IF EXISTS `Zhaba`;
CREATE SCHEMA `Zhaba`;

USE `Zhaba`;
source ../Schema/tables.MySQL.sql;
delimiter ;
source init_data.MySQL.sql;
