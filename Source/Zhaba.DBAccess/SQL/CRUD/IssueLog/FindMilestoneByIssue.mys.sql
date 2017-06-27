SELECT * 
FROM tbl_milestone
where COUNTER in (
select C_MILESTONE from  tbl_issuelog
where C_ISSUE = ?C_Issue AND STATUS_DATE = (SELECT MAX(STATUS_DATE) FROM tbl_issuelog WHERE C_ISSUE = ?C_Issue)
order by COUNTER desc
);