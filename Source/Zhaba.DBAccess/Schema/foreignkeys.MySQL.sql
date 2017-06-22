delimiter ;.
  alter table `tbl_issuelog` add   constraint `fk_tbl_issuelog_meeting` foreign key (`C_MEETING`) references `tbl_meeting`(`COUNTER`)
;.
