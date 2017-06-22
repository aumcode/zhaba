delimiter ;.
-- Table tbl_sequence
create table `tbl_sequence`
(
 `SCOPE_NAME`     char(80)       not null,
 `SEQUENCE_NAME`  char(80)       not null,
 `COUNTER`        BIGINT(8) UNSIGNED not null,
  constraint `pk_tbl_sequence_primary` primary key (`SCOPE_NAME`, `SEQUENCE_NAME`)
)
;.

delimiter ;.
-- Table tbl_user
create table `tbl_user`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null,
 `LOGIN`          char(80)       not null,
 `FIRST_NAME`     varchar(32)    not null,
 `LAST_NAME`      varchar(32)    not null,
 `EMAIL`          varchar(64)    not null,
 `STATUS`         CHAR(1)        not null,
 `PASSWORD`       varchar(1024)  not null,
 `USER_RIGHTS`    TEXT           not null,
 `IN_USE`         CHAR(1)        not null default 'T',
  constraint `pk_tbl_user_counter` primary key (`COUNTER`)
)
;.

delimiter ;.
  create unique index `idx_tbl_user_uk` on `tbl_user`(`LOGIN`);.
delimiter ;.
-- Table tbl_category
create table `tbl_category`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null,
 `NAME`           char(80)       not null,
 `DESCRIPTION`    varchar(128),
 `IN_USE`         CHAR(1)        not null default 'T',
  constraint `pk_tbl_category_counter` primary key (`COUNTER`)
)
;.

delimiter ;.
  create unique index `idx_tbl_category_uk` on `tbl_category`(`NAME`);.
delimiter ;.
-- Table tbl_project
create table `tbl_project`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null,
 `NAME`           char(80)       not null,
 `DESCRIPTION`    varchar(128),
 `C_CREATOR`      BIGINT(8) UNSIGNED not null,
 `IN_USE`         CHAR(1)        not null default 'T',
  constraint `pk_tbl_project_counter` primary key (`COUNTER`),
  constraint `fk_tbl_project_creator` foreign key (`C_CREATOR`) references `tbl_user`(`COUNTER`)
)
;.

delimiter ;.
  create unique index `idx_tbl_project_uk` on `tbl_project`(`NAME`);.
delimiter ;.
-- Table tbl_component
create table `tbl_component`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null,
 `NAME`           char(80)       not null,
 `DESCRIPTION`    varchar(128),
 `C_PROJECT`      BIGINT(8) UNSIGNED not null,
 `IN_USE`         CHAR(1)        not null default 'T',
  constraint `pk_tbl_component_counter` primary key (`COUNTER`),
  constraint `fk_tbl_component_project` foreign key (`C_PROJECT`) references `tbl_project`(`COUNTER`)
)
;.

delimiter ;.
  create unique index `idx_tbl_component_uk` on `tbl_component`(`C_PROJECT`, `NAME`);.
delimiter ;.
-- Table tbl_area
create table `tbl_area`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null,
 `NAME`           char(80)       not null,
 `DESCRIPTION`    varchar(128),
 `C_PROJECT`      BIGINT(8) UNSIGNED not null,
 `IN_USE`         CHAR(1)        not null default 'T',
  constraint `pk_tbl_area_counter` primary key (`COUNTER`),
  constraint `fk_tbl_area_project` foreign key (`C_PROJECT`) references `tbl_project`(`COUNTER`)
)
;.

delimiter ;.
  create unique index `idx_tbl_area_uk` on `tbl_area`(`C_PROJECT`, `NAME`);.
delimiter ;.
-- Table tbl_milestone
create table `tbl_milestone`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null,
 `NAME`           char(80)       not null,
 `DESCRIPTION`    varchar(128),
 `C_PROJECT`      BIGINT(8) UNSIGNED not null,
 `IN_USE`         CHAR(1)        not null default 'T',
 `START_DATE`     DATE,
 `PLAN_DATE`      DATE,
 `COMPLETE_DATE`  DATE,
  constraint `pk_tbl_milestone_counter` primary key (`COUNTER`),
  constraint `fk_tbl_milestone_project` foreign key (`C_PROJECT`) references `tbl_project`(`COUNTER`)
)
;.

delimiter ;.
  create unique index `idx_tbl_milestone_uk` on `tbl_milestone`(`C_PROJECT`, `NAME`);.
delimiter ;.
-- Table tbl_issue
create table `tbl_issue`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null,
 `NAME`           char(80)       not null,
 `C_PROJECT`      BIGINT(8) UNSIGNED not null,
 `C_PARENT`       BIGINT(8) UNSIGNED,
 `IN_USE`         CHAR(1)        not null default 'T',
  constraint `pk_tbl_issue_counter` primary key (`COUNTER`),
  constraint `fk_tbl_issue_project` foreign key (`C_PROJECT`) references `tbl_project`(`COUNTER`),
  constraint `fk_tbl_issue_parent` foreign key (`C_PARENT`) references `tbl_issue`(`COUNTER`)
)
;.

delimiter ;.
  create unique index `idx_tbl_issue_uk` on `tbl_issue`(`C_PROJECT`, `NAME`);.
delimiter ;.
-- Table tbl_issuelog
create table `tbl_issuelog`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null,
 `C_ISSUE`        BIGINT(8) UNSIGNED not null,
 `DESCRIPTION`    varchar(128),
 `C_MILESTONE`    BIGINT(8) UNSIGNED not null,
 `C_CATEGORY`     BIGINT(8) UNSIGNED not null,
 `STATUS`         CHAR(1)        not null,
 `C_ASSIGNEE`     BIGINT(8) UNSIGNED,
 `STATUS_DATE`    DATETIME       not null,
 `C_OPERATOR`     BIGINT(8) UNSIGNED not null,
 `COMPLETENESS`   int unsigned   not null default '0',
 `NOTE`           TEXT,
 `C_MEETING`      BIGINT(8) UNSIGNED,
  constraint `pk_tbl_issuelog_counter` primary key (`COUNTER`),
  constraint `fk_tbl_issuelog_issue` foreign key (`C_ISSUE`) references `tbl_issue`(`COUNTER`),
  constraint `fk_tbl_issuelog_milestone` foreign key (`C_MILESTONE`) references `tbl_milestone`(`COUNTER`),
  constraint `fk_tbl_issuelog_category` foreign key (`C_CATEGORY`) references `tbl_category`(`COUNTER`),
  constraint `fk_tbl_issuelog_assignee` foreign key (`C_ASSIGNEE`) references `tbl_user`(`COUNTER`),
  constraint `fk_tbl_issuelog_operator` foreign key (`C_OPERATOR`) references `tbl_user`(`COUNTER`)
)
;.

delimiter ;.
  create unique index `idx_tbl_issuelog_uk` on `tbl_issuelog`(`C_ISSUE`, `STATUS_DATE`, `C_OPERATOR`);.
delimiter ;.
-- Table tbl_issuecomponent
create table `tbl_issuecomponent`
(
 `C_ISSUE`        BIGINT(8) UNSIGNED not null,
 `C_COMPONENT`    BIGINT(8) UNSIGNED not null,
  constraint `pk_tbl_issuecomponent_primary` primary key (`C_ISSUE`, `C_COMPONENT`),
  constraint `fk_tbl_issuecomponent_issue` foreign key (`C_ISSUE`) references `tbl_issue`(`COUNTER`),
  constraint `fk_tbl_issuecomponent_component` foreign key (`C_COMPONENT`) references `tbl_component`(`COUNTER`)
)
;.

delimiter ;.
-- Table tbl_issuearea
create table `tbl_issuearea`
(
 `C_ISSUE`        BIGINT(8) UNSIGNED not null,
 `C_AREA`         BIGINT(8) UNSIGNED not null,
  constraint `pk_tbl_issuearea_primary` primary key (`C_ISSUE`, `C_AREA`),
  constraint `fk_tbl_issuearea_issue` foreign key (`C_ISSUE`) references `tbl_issue`(`COUNTER`),
  constraint `fk_tbl_issuearea_area` foreign key (`C_AREA`) references `tbl_area`(`COUNTER`)
)
;.

delimiter ;.
-- Table tbl_agenda
create table `tbl_agenda`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null,
 `NAME`           char(80)       not null,
 `DESCRIPTION`    varchar(128),
 `START_DATE`     DATETIME       not null,
 `END_DATE`       DATETIME       not null,
 `C_CREATOR`      BIGINT(8) UNSIGNED not null,
  constraint `pk_tbl_agenda_counter` primary key (`COUNTER`),
  constraint `fk_tbl_agenda_creator` foreign key (`C_CREATOR`) references `tbl_user`(`COUNTER`)
)
;.

delimiter ;.
-- Table tbl_agendadissue
create table `tbl_agendadissue`
(
 `C_AGENDA`       BIGINT(8) UNSIGNED not null,
 `C_ISSUE`        BIGINT(8) UNSIGNED not null,
  constraint `pk_tbl_agendadissue_primary` primary key (`C_AGENDA`, `C_ISSUE`),
  constraint `fk_tbl_agendadissue_agenda` foreign key (`C_AGENDA`) references `tbl_agenda`(`COUNTER`),
  constraint `fk_tbl_agendadissue_issue` foreign key (`C_ISSUE`) references `tbl_issue`(`COUNTER`)
)
;.

delimiter ;.
-- Table tbl_meeting
create table `tbl_meeting`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null,
 `C_AGENDA`       BIGINT(8) UNSIGNED not null,
 `DATE`           DATETIME       not null,
 `C_HOST`         BIGINT(8) UNSIGNED not null,
 `NOTE`           TEXT,
  constraint `pk_tbl_meeting_counter` primary key (`COUNTER`),
  constraint `fk_tbl_meeting_agenda` foreign key (`C_AGENDA`) references `tbl_agenda`(`COUNTER`),
  constraint `fk_tbl_meeting_host` foreign key (`C_HOST`) references `tbl_user`(`COUNTER`)
)
;.

delimiter ;.
  create unique index `idx_tbl_meeting_uk` on `tbl_meeting`(`C_AGENDA`, `DATE`, `C_HOST`);.
delimiter ;.
-- Table tbl_meetingparticipant
create table `tbl_meetingparticipant`
(
 `C_MEETING`      BIGINT(8) UNSIGNED not null,
 `C_PARTICIPANT`  BIGINT(8) UNSIGNED not null,
 `PARTICIPATION_TYPE` CHAR(1)    not null default 'P',
 `ADMIN_NOTE`     TEXT,
 `PARTICIPANT_NOTE`   TEXT,
  constraint `pk_tbl_meetingparticipant_primary` primary key (`C_MEETING`, `C_PARTICIPANT`),
  constraint `fk_tbl_meetingparticipant_meeting` foreign key (`C_MEETING`) references `tbl_meeting`(`COUNTER`),
  constraint `fk_tbl_meetingparticipant_participant` foreign key (`C_PARTICIPANT`) references `tbl_user`(`COUNTER`)
)
;.

