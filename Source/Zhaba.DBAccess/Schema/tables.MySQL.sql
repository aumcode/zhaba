delimiter ;.
-- Table TBL_SEQUENCE
create table `TBL_SEQUENCE`
(
 `SCOPE_NAME`     VARCHAR(50)    not null,
 `SEQUENCE_NAME`  VARCHAR(50)    not null,
  constraint `PK_TBL_SEQUENCE_PRIMARY` primary key (`SCOPE_NAME`, `SEQUENCE_NAME`)
)
;.

delimiter ;.
-- Table TBL_USER
create table `TBL_USER`
(
 `LOGIN`          VARCHAR(50)    not null,
 `FIRST_NAME`     VARCHAR(50)    not null,
 `LAST_NAME`      VARCHAR(50)    not null,
 `ROLE`           VARCHAR(50)    not null
)
;.

delimiter ;.
  create unique index `IDX_TBL_USER_UK` on `TBL_USER`(`LOGIN`);.
delimiter ;.
-- Table TBL_COMPONENT
create table `TBL_COMPONENT`
(
 `NAME`           VARCHAR(50)    not null
)
;.

delimiter ;.
-- Table TBL_AREA
create table `TBL_AREA`
(
 `NAME`           VARCHAR(50)    not null
)
;.

delimiter ;.
-- Table TBL_PROJECT
create table `TBL_PROJECT`
(
 `NAME`           VARCHAR(50)    not null
)
;.

delimiter ;.
-- Table TBL_MILESTONE
create table `TBL_MILESTONE`
(
 `NAME`           VARCHAR(50)    not null,
 `START_DATE`     DATE,
 `PLAN_DATE`      DATE,
 `COMPLETE_DATE`  DATE
)
;.

delimiter ;.
-- Table TBL_ISSUE
create table `TBL_ISSUE`
(
 `NAME`           VARCHAR(50)    not null,
 `STATUS`         VARCHAR(50)    not null,
 `CREATOR`        VARCHAR(50)    not null,
 `OWNER`          VARCHAR(50)
)
;.

delimiter ;.
-- Table TBL_ISSUELOG
create table `TBL_ISSUELOG`
(
 `STATUS`         VARCHAR(50)    not null,
 `CREATOR`        VARCHAR(50)    not null
)
;.

