select * 
from tbl_issueassign
where C_ISSUE = ?C_Issue and C_USER = ?C_User and ?date between OPEN_TS and CLOSE_TS