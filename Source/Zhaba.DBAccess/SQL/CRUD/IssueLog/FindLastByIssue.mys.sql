select * from  tbl_issuelog
where C_Issue = ?C_Issue
order by COUNTER desc
limit 1