delimiter ;.
-- Table tbl_sequence
create table `tbl_sequence`
(
 `SCOPE_NAME`     VARCHAR(50)    not null,
 `SEQUENCE_NAME`  VARCHAR(50)    not null,
 `COUNTER`        BIGINT(8) UNSIGNED not null,
  constraint `pk_tbl_sequence_primary` primary key (`SCOPE_NAME`, `SEQUENCE_NAME`)
)
;.

delimiter ;.
-- Table tbl_user
create table `tbl_user`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null,
 `LOGIN`          VARCHAR(50)    not null,
 `FIRST_NAME`     VARCHAR(50)    not null,
 `LAST_NAME`      VARCHAR(50)    not null,
 `ROLE`           VARCHAR(50)    not null,
 `PASSWORD_HASH`  VARCHAR(128)   not null,
 `PASSWORD_SALT`  VARCHAR(32)    not null,
  constraint `pk_tbl_user_counter` primary key (`COUNTER`)
)
;.

delimiter ;.
-- Table tbl_component
create table `tbl_component`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null,
 `NAME`           VARCHAR(50)    not null,
 `DESCRIPTION`    TEXT,
  constraint `pk_tbl_component_counter` primary key (`COUNTER`)
)
;.

delimiter ;.
-- Table tbl_area
create table `tbl_area`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null,
 `NAME`           VARCHAR(50)    not null,
 `DESCRIPTION`    TEXT,
 `C_PROJECT`      BIGINT(8) UNSIGNED not null,
  constraint `pk_tbl_area_counter` primary key (`COUNTER`)
)
;.

delimiter ;.
-- Table tbl_project
create table `tbl_project`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null,
 `NAME`           VARCHAR(50)    not null,
 `DESCRIPTION`    TEXT,
  constraint `pk_tbl_project_counter` primary key (`COUNTER`)
)
;.

delimiter ;.
-- Table tbl_milestone
create table `tbl_milestone`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null,
 `NAME`           VARCHAR(50)    not null,
 `DESCRIPTION`    TEXT,
 `C_PROJECT`      BIGINT(8) UNSIGNED not null,
 `START_DATE`     DATE,
 `PLAN_DATE`      DATE,
 `COMPLETE_DATE`  DATE,
  constraint `pk_tbl_milestone_counter` primary key (`COUNTER`),
  constraint `fk_tbl_milestone_c_project` foreign key (`C_PROJECT`) references `tbl_project`(`COUNTER`)
)
;.

delimiter ;.
-- Table tbl_issue
create table `tbl_issue`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null,
 `NAME`           VARCHAR(50)    not null,
 `DESCRIPTION`    TEXT,
 `C_PROJECT`      BIGINT(8) UNSIGNED not null,
 `C_MILESTONE`    BIGINT(8) UNSIGNED,
 `C_AREA`         BIGINT(8) UNSIGNED,
 `C_COMPONENT`    BIGINT(8) UNSIGNED,
 `C_PARENT`       BIGINT(8) UNSIGNED,
 `STATUS`         VARCHAR(50)    not null,
 `CREATOR`        VARCHAR(50)    not null,
 `OWNER`          VARCHAR(50)    not null,
 `CREATION_DATE`  DATETIME       not null,
 `CHANGE_DATE`    DATETIME       not null,
  constraint `pk_tbl_issue_counter` primary key (`COUNTER`),
  constraint `fk_tbl_issue_c_project` foreign key (`C_PROJECT`) references `tbl_project`(`COUNTER`),
  constraint `fk_tbl_issue_c_milestone` foreign key (`C_MILESTONE`) references `tbl_milestone`(`COUNTER`),
  constraint `fk_tbl_issue_c_area` foreign key (`C_AREA`) references `tbl_area`(`COUNTER`),
  constraint `fk_tbl_issue_c_component` foreign key (`C_COMPONENT`) references `tbl_component`(`COUNTER`),
  constraint `fk_tbl_issue_c_parent` foreign key (`C_PARENT`) references `tbl_issue`(`COUNTER`)
)
;.

delimiter ;.
-- Table tbl_issuelog
create table `tbl_issuelog`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null,
 `C_ISSUE`        BIGINT(8) UNSIGNED not null,
 `DESCRIPTION`    TEXT,
 `STATUS`         VARCHAR(50)    not null,
 `CREATOR`        VARCHAR(50)    not null,
 `CREATION_DATE`  DATETIME       not null,
  constraint `pk_tbl_issuelog_counter` primary key (`COUNTER`),
  constraint `fk_tbl_issuelog_c_issue` foreign key (`C_ISSUE`) references `tbl_issue`(`COUNTER`)
)
;.

