delimiter ;.
  alter table `tbl_area` add   constraint `fk_tbl_area_c_project` foreign key (`C_PROJECT`) references `tbl_project`(`COUNTER`)
;.
