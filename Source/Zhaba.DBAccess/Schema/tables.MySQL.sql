delimiter ;.
-- Table tbl_sequence
create table `tbl_sequence`
(
 `SCOPE_NAME`     char(80)       not null comment 'Scope that sequence is genereated in',
 `SEQUENCE_NAME`  char(80)       not null comment 'Sequence Name',
 `COUNTER`        BIGINT(8) UNSIGNED not null comment 'Current value of integer ID',
  constraint `pk_tbl_sequence_primary` primary key (`SCOPE_NAME`, `SEQUENCE_NAME`)
)
;.

delimiter ;.
-- Table tbl_user
create table `tbl_user`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null comment 'Integer ID',
 `LOGIN`          char(80)       not null comment 'User Login',
 `FIRST_NAME`     varchar(32)    not null comment 'First Name',
 `LAST_NAME`      varchar(32)    not null comment 'Last Name',
 `EMAIL`          varchar(64)    not null comment 'User EMail',
 `STATUS`         CHAR(1)        not null comment 'User access role',
 `PASSWORD`       varchar(1024)  not null comment 'User Password hash',
 `USER_RIGHTS`    TEXT           not null comment 'User Permissions Configuration',
 `IN_USE`         CHAR(1)        not null default 'T' comment 'Logical Deletion Flag',
  constraint `pk_tbl_user_counter` primary key (`COUNTER`)
)
;.

delimiter ;.
  create unique index `idx_tbl_user_uk` on `tbl_user`(`LOGIN`);.
delimiter ;.
-- Table tbl_category
create table `tbl_category`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null comment 'Integer ID',
 `NAME`           char(80)       not null comment 'Mnemonic ID',
 `DESCRIPTION`    varchar(128)    comment 'Short Description',
 `IN_USE`         CHAR(1)        not null default 'T' comment 'Logical Deletion Flag',
  constraint `pk_tbl_category_counter` primary key (`COUNTER`)
)
;.

delimiter ;.
  create unique index `idx_tbl_category_uk` on `tbl_category`(`NAME`);.
delimiter ;.
-- Table tbl_project
create table `tbl_project`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null comment 'Integer ID',
 `NAME`           char(80)       not null comment 'Mnemonic ID',
 `DESCRIPTION`    varchar(128)    comment 'Short Description',
 `C_CREATOR`      BIGINT(8) UNSIGNED not null comment 'Creator',
 `IN_USE`         CHAR(1)        not null default 'T' comment 'Logical Deletion Flag',
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
 `COUNTER`        BIGINT(8) UNSIGNED not null comment 'Integer ID',
 `C_PROJECT`      BIGINT(8) UNSIGNED not null comment 'Project that component belongs to',
 `NAME`           char(80)       not null comment 'Mnemonic ID',
 `DESCRIPTION`    varchar(128)    comment 'Short Description',
 `IN_USE`         CHAR(1)        not null default 'T' comment 'Logical Deletion Flag',
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
 `COUNTER`        BIGINT(8) UNSIGNED not null comment 'Integer ID',
 `C_PROJECT`      BIGINT(8) UNSIGNED not null comment 'Project that area belongs to',
 `NAME`           char(80)       not null comment 'Mnemonic ID',
 `DESCRIPTION`    varchar(128)    comment 'Short Description',
 `IN_USE`         CHAR(1)        not null default 'T' comment 'Logical Deletion Flag',
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
 `COUNTER`        BIGINT(8) UNSIGNED not null comment 'Integer ID',
 `C_PROJECT`      BIGINT(8) UNSIGNED not null comment 'Project that milestone belongs to',
 `NAME`           char(80)       not null comment 'Mnemonic ID',
 `DESCRIPTION`    varchar(128)    comment 'Short Description',
 `IN_USE`         CHAR(1)        not null default 'T' comment 'Logical Deletion Flag',
 `START_DATE`     DATE            comment 'Milestone Start Date',
 `PLAN_DATE`      DATE            comment 'Milestone Plan Date',
 `COMPLETE_DATE`  DATE            comment 'Milestone Completeness Date',
 `NOTE`           TEXT            comment 'Text Note',
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
 `COUNTER`        BIGINT(8) UNSIGNED not null comment 'Integer ID',
 `C_PROJECT`      BIGINT(8) UNSIGNED not null comment 'Project that issue belongs to',
 `NAME`           char(80)       not null comment 'Mnemonic ID',
 `C_PARENT`       BIGINT(8) UNSIGNED  comment 'Reference to parent issue',
 `IN_USE`         CHAR(1)        not null default 'T' comment 'Logical Deletion Flag',
  constraint `pk_tbl_issue_counter` primary key (`COUNTER`),
  constraint `fk_tbl_issue_project` foreign key (`C_PROJECT`) references `tbl_project`(`COUNTER`),
  constraint `fk_tbl_issue_parent` foreign key (`C_PARENT`) references `tbl_issue`(`COUNTER`)
)
;.

delimiter ;.
  create unique index `idx_tbl_issue_uk` on `tbl_issue`(`C_PROJECT`, `NAME`);.
delimiter ;.
-- Table tbl_issuecomponent
create table `tbl_issuecomponent`
(
 `C_ISSUE`        BIGINT(8) UNSIGNED not null comment 'Issue',
 `C_COMPONENT`    BIGINT(8) UNSIGNED not null comment 'Project Component that issue linked with',
  constraint `pk_tbl_issuecomponent_primary` primary key (`C_ISSUE`, `C_COMPONENT`),
  constraint `fk_tbl_issuecomponent_issue` foreign key (`C_ISSUE`) references `tbl_issue`(`COUNTER`),
  constraint `fk_tbl_issuecomponent_component` foreign key (`C_COMPONENT`) references `tbl_component`(`COUNTER`)
)
;.

delimiter ;.
-- Table tbl_issuearea
create table `tbl_issuearea`
(
 `C_ISSUE`        BIGINT(8) UNSIGNED not null comment 'Issue',
 `C_AREA`         BIGINT(8) UNSIGNED not null comment 'Project Area that issue linked with',
  constraint `pk_tbl_issuearea_primary` primary key (`C_ISSUE`, `C_AREA`),
  constraint `fk_tbl_issuearea_issue` foreign key (`C_ISSUE`) references `tbl_issue`(`COUNTER`),
  constraint `fk_tbl_issuearea_area` foreign key (`C_AREA`) references `tbl_area`(`COUNTER`)
)
;.

delimiter ;.
-- Table tbl_agenda
create table `tbl_agenda`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null comment 'Integer ID',
 `NAME`           char(80)       not null comment 'Mnemonic ID',
 `DESCRIPTION`    varchar(128)    comment 'Short Description',
 `START_DATE`     DATETIME       not null comment 'Agenda Start Date',
 `END_DATE`       DATETIME       not null comment 'Agenda End Date',
 `C_CREATOR`      BIGINT(8) UNSIGNED not null comment 'Creator',
  constraint `pk_tbl_agenda_counter` primary key (`COUNTER`),
  constraint `fk_tbl_agenda_creator` foreign key (`C_CREATOR`) references `tbl_user`(`COUNTER`)
)
;.

delimiter ;.
-- Table tbl_agendadissue
create table `tbl_agendadissue`
(
 `C_AGENDA`       BIGINT(8) UNSIGNED not null comment 'Agenda',
 `C_ISSUE`        BIGINT(8) UNSIGNED not null comment 'Issues included in agenda',
  constraint `pk_tbl_agendadissue_primary` primary key (`C_AGENDA`, `C_ISSUE`),
  constraint `fk_tbl_agendadissue_agenda` foreign key (`C_AGENDA`) references `tbl_agenda`(`COUNTER`),
  constraint `fk_tbl_agendadissue_issue` foreign key (`C_ISSUE`) references `tbl_issue`(`COUNTER`)
)
;.

delimiter ;.
-- Table tbl_meeting
create table `tbl_meeting`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null comment 'Integer ID',
 `C_AGENDA`       BIGINT(8) UNSIGNED not null comment 'Agenda that meeting based on',
 `START_DATE`     DATETIME       not null comment 'Start date of meeting',
 `END_DATE`       DATETIME        comment 'End date of meeting',
 `C_HOST`         BIGINT(8) UNSIGNED not null comment 'Meeting Host',
 `NOTE`           TEXT            comment 'Text Note',
  constraint `pk_tbl_meeting_counter` primary key (`COUNTER`),
  constraint `fk_tbl_meeting_agenda` foreign key (`C_AGENDA`) references `tbl_agenda`(`COUNTER`),
  constraint `fk_tbl_meeting_host` foreign key (`C_HOST`) references `tbl_user`(`COUNTER`)
)
;.

delimiter ;.
  create unique index `idx_tbl_meeting_uk` on `tbl_meeting`(`C_AGENDA`, `START_DATE`, `C_HOST`);.
delimiter ;.
-- Table tbl_meetingparticipant
create table `tbl_meetingparticipant`
(
 `C_MEETING`      BIGINT(8) UNSIGNED not null comment 'Meeting',
 `C_PARTICIPANT`  BIGINT(8) UNSIGNED not null comment 'User who participates in meeting',
 `PARTICIPATION_TYPE` CHAR(1)    not null default 'P' comment 'User Participation Type (remote, physical)',
 `ADMIN_NOTE`     TEXT,
 `PARTICIPANT_NOTE`   TEXT,
  constraint `pk_tbl_meetingparticipant_primary` primary key (`C_MEETING`, `C_PARTICIPANT`),
  constraint `fk_tbl_meetingparticipant_meeting` foreign key (`C_MEETING`) references `tbl_meeting`(`COUNTER`),
  constraint `fk_tbl_meetingparticipant_participant` foreign key (`C_PARTICIPANT`) references `tbl_user`(`COUNTER`)
)
;.

delimiter ;.
-- Table tbl_issuelog
create table `tbl_issuelog`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null comment 'Integer ID',
 `C_ISSUE`        BIGINT(8) UNSIGNED not null comment 'Root Issue',
 `STATUS_DATE`    DATETIME       not null comment 'Date when changes were made',
 `C_OPERATOR`     BIGINT(8) UNSIGNED not null comment 'User who made changes',
 `PRIORITY`       BIGINT(8)      not null comment 'Issue Priority',
 `DESCRIPTION`    varchar(128)    comment 'Short Description',
 `C_MILESTONE`    BIGINT(8) UNSIGNED not null comment 'Project Milestone',
 `C_CATEGORY`     BIGINT(8) UNSIGNED not null comment 'Issue Category',
 `STATUS`         CHAR(1)        not null comment 'Issue Status',
 `COMPLETENESS`   int unsigned   not null default '0' comment '0..100',
 `NOTE`           TEXT            comment 'Text Note',
 `C_MEETING`      BIGINT(8) UNSIGNED  comment 'Optional reference to meeting that causes changes',
  constraint `pk_tbl_issuelog_counter` primary key (`COUNTER`),
  constraint `fk_tbl_issuelog_issue` foreign key (`C_ISSUE`) references `tbl_issue`(`COUNTER`),
  constraint `fk_tbl_issuelog_operator` foreign key (`C_OPERATOR`) references `tbl_user`(`COUNTER`),
  constraint `fk_tbl_issuelog_milestone` foreign key (`C_MILESTONE`) references `tbl_milestone`(`COUNTER`),
  constraint `fk_tbl_issuelog_category` foreign key (`C_CATEGORY`) references `tbl_category`(`COUNTER`),
  constraint `fk_tbl_issuelog_meeting` foreign key (`C_MEETING`) references `tbl_meeting`(`COUNTER`)
)
;.

delimiter ;.
  create unique index `idx_tbl_issuelog_uk` on `tbl_issuelog`(`C_ISSUE`, `STATUS_DATE`, `C_OPERATOR`);.
delimiter ;.
-- Table tbl_issueassign
create table `tbl_issueassign`
(
 `COUNTER`        BIGINT(8) UNSIGNED not null comment 'Integer ID',
 `C_ISSUE`        BIGINT(8) UNSIGNED not null comment 'Issue',
 `C_USER`         BIGINT(8) UNSIGNED not null comment 'User',
 `OPEN_TS`        DATETIME       not null comment 'Issue assignment time for the user',
 `CLOSE_TS`       DATETIME        default '2500-01-01 00:00:00'   comment 'Issue close time for the user',
 `C_OPEN_OPER`    BIGINT(8) UNSIGNED not null comment 'Operator assignee',
 `C_CLOSE_OPER`   BIGINT(8) UNSIGNED  comment 'Operator assignee',
 `C_OPEN_MEETING` BIGINT(8) UNSIGNED  comment 'Open meeting',
 `C_CLOSE_MEETING`    BIGINT(8) UNSIGNED  comment 'Close meeting',
 `NOTE`           TEXT            comment 'Note',
  constraint `pk_tbl_issueassign_counter` primary key (`COUNTER`),
  constraint `fk_tbl_issueassign_issue` foreign key (`C_ISSUE`) references `tbl_issue`(`COUNTER`),
  constraint `fk_tbl_issueassign_user` foreign key (`C_USER`) references `tbl_user`(`COUNTER`),
  constraint `fk_tbl_issueassign_open_oper` foreign key (`C_OPEN_OPER`) references `tbl_user`(`COUNTER`),
  constraint `fk_tbl_issueassign_close_oper` foreign key (`C_CLOSE_OPER`) references `tbl_user`(`COUNTER`),
  constraint `fk_tbl_issueassign_open_meeting` foreign key (`C_OPEN_MEETING`) references `tbl_meeting`(`COUNTER`),
  constraint `fk_tbl_issueassign_close_meeting` foreign key (`C_CLOSE_MEETING`) references `tbl_meeting`(`COUNTER`)
)
;.

delimiter ;.
  create unique index `idx_tbl_issueassign_uk` on `tbl_issueassign`(`C_ISSUE`, `C_USER`, `OPEN_TS`);.
