--
-- Скрипт сгенерирован Devart dbForge Studio for MySQL, Версия 7.1.20.0
-- Домашняя страница продукта: http://www.devart.com/ru/dbforge/mysql/studio
-- Дата скрипта: 14.11.2016 21:13:15
-- Версия сервера: 5.5.5-10.1.10-MariaDB
-- Версия клиента: 4.1
--


-- 
-- Отключение внешних ключей
-- 
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;

-- 
-- Установить режим SQL (SQL mode)
-- 
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- 
-- Установка кодировки, с использованием которой клиент будет посылать запросы на сервер
--
SET NAMES 'utf8';

-- 
-- Установка базы данных по умолчанию
--
USE `zhaba.data`;

--
-- Описание для таблицы tbl_component
--
DROP TABLE IF EXISTS tbl_component;
CREATE TABLE tbl_component (
  ID BIGINT(8) UNSIGNED NOT NULL,
  Name VARCHAR(50) NOT NULL,
  Description TEXT DEFAULT NULL,
  PRIMARY KEY (ID)
)
ENGINE = INNODB
CHARACTER SET utf8
COLLATE utf8_general_ci;

--
-- Описание для таблицы tbl_project
--
DROP TABLE IF EXISTS tbl_project;
CREATE TABLE tbl_project (
  ID BIGINT(8) UNSIGNED NOT NULL,
  Name VARCHAR(50) NOT NULL,
  Description TEXT DEFAULT NULL,
  PRIMARY KEY (ID)
)
ENGINE = INNODB
CHARACTER SET utf8
COLLATE utf8_general_ci;

--
-- Описание для таблицы tbl_user
--
DROP TABLE IF EXISTS tbl_user;
CREATE TABLE tbl_user (
  ID BIGINT(8) UNSIGNED NOT NULL,
  Login VARCHAR(50) NOT NULL,
  FirstName VARCHAR(50) DEFAULT NULL,
  LastName VARCHAR(50) DEFAULT NULL,
  Role VARCHAR(50) NOT NULL DEFAULT 'USER',
  PasswordHash VARCHAR(128) NOT NULL,
  PasswordSalt VARCHAR(32) NOT NULL,
  PRIMARY KEY (ID)
)
ENGINE = INNODB
CHARACTER SET utf8
COLLATE utf8_general_ci;

--
-- Описание для таблицы tbl_area
--
DROP TABLE IF EXISTS tbl_area;
CREATE TABLE tbl_area (
  ID BIGINT(8) UNSIGNED NOT NULL,
  Name VARCHAR(50) NOT NULL,
  Description TEXT DEFAULT NULL,
  Project BIGINT(8) UNSIGNED NOT NULL,
  PRIMARY KEY (ID),
  CONSTRAINT FK_tbl_area_tbl_project_ID FOREIGN KEY (Project)
    REFERENCES tbl_project(ID) ON DELETE RESTRICT ON UPDATE RESTRICT
)
ENGINE = INNODB
CHARACTER SET utf8
COLLATE utf8_general_ci;

--
-- Описание для таблицы tbl_milestone
--
DROP TABLE IF EXISTS tbl_milestone;
CREATE TABLE tbl_milestone (
  ID BIGINT(8) UNSIGNED NOT NULL,
  Name VARCHAR(50) NOT NULL,
  Description TEXT DEFAULT NULL,
  Project BIGINT(8) UNSIGNED NOT NULL,
  StartDate DATE DEFAULT NULL,
  PlanDate DATE DEFAULT NULL,
  CompleteDate DATE DEFAULT NULL,
  PRIMARY KEY (ID),
  CONSTRAINT FK_tbl_milestone_tbl_project_ID FOREIGN KEY (Project)
    REFERENCES tbl_project(ID) ON DELETE RESTRICT ON UPDATE RESTRICT
)
ENGINE = INNODB
CHARACTER SET utf8
COLLATE utf8_general_ci;

--
-- Описание для таблицы tbl_issue
--
DROP TABLE IF EXISTS tbl_issue;
CREATE TABLE tbl_issue (
  ID BIGINT(8) UNSIGNED NOT NULL,
  Name VARCHAR(50) NOT NULL,
  Description TEXT DEFAULT NULL,
  Project BIGINT(8) UNSIGNED NOT NULL,
  Milestone BIGINT(8) UNSIGNED DEFAULT NULL,
  Area BIGINT(8) UNSIGNED DEFAULT NULL,
  Component BIGINT(8) UNSIGNED DEFAULT NULL,
  Parent BIGINT(8) UNSIGNED DEFAULT NULL,
  Status VARCHAR(50) NOT NULL,
  Creator VARCHAR(50) NOT NULL,
  Owner VARCHAR(50) DEFAULT NULL,
  CreationDate DATE NOT NULL,
  ChangeDate DATE NOT NULL,
  PRIMARY KEY (ID),
  CONSTRAINT FK_tbl_issue_tbl_area_ID FOREIGN KEY (Area)
    REFERENCES tbl_area(ID) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT FK_tbl_issue_tbl_component_ID FOREIGN KEY (Component)
    REFERENCES tbl_component(ID) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT FK_tbl_issue_tbl_issue_ID FOREIGN KEY (Parent)
    REFERENCES tbl_issue(ID) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT FK_tbl_issue_tbl_milestone_ID FOREIGN KEY (Milestone)
    REFERENCES tbl_milestone(ID) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT FK_tbl_issue_tbl_project_ID FOREIGN KEY (Project)
    REFERENCES tbl_project(ID) ON DELETE RESTRICT ON UPDATE RESTRICT
)
ENGINE = INNODB
CHARACTER SET utf8
COLLATE utf8_general_ci;

--
-- Описание для таблицы tbl_issuelog
--
DROP TABLE IF EXISTS tbl_issuelog;
CREATE TABLE tbl_issuelog (
  ID BIGINT(8) UNSIGNED NOT NULL,
  Issue BIGINT(8) UNSIGNED NOT NULL,
  Description TEXT NOT NULL,
  Status VARCHAR(50) NOT NULL,
  Creator VARCHAR(50) NOT NULL,
  CreationDate DATE NOT NULL,
  PRIMARY KEY (ID),
  CONSTRAINT FK_tbl_issuelog_tbl_issue_ID FOREIGN KEY (Issue)
    REFERENCES tbl_issue(ID) ON DELETE RESTRICT ON UPDATE RESTRICT
)
ENGINE = INNODB
CHARACTER SET utf8
COLLATE utf8_general_ci;

-- 
-- Вывод данных для таблицы tbl_component
--

-- Таблица `zhaba.data`.tbl_component не содержит данных

-- 
-- Вывод данных для таблицы tbl_project
--

-- Таблица `zhaba.data`.tbl_project не содержит данных

-- 
-- Вывод данных для таблицы tbl_user
--

-- Таблица `zhaba.data`.tbl_user не содержит данных

-- 
-- Вывод данных для таблицы tbl_area
--

-- Таблица `zhaba.data`.tbl_area не содержит данных

-- 
-- Вывод данных для таблицы tbl_milestone
--

-- Таблица `zhaba.data`.tbl_milestone не содержит данных

-- 
-- Вывод данных для таблицы tbl_issue
--

-- Таблица `zhaba.data`.tbl_issue не содержит данных

-- 
-- Вывод данных для таблицы tbl_issuelog
--

-- Таблица `zhaba.data`.tbl_issuelog не содержит данных

-- 
-- Восстановить предыдущий режим SQL (SQL mode)
-- 
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;

-- 
-- Включение внешних ключей
-- 
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;