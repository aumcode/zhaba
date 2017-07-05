select * 
from tbl_issuechat
where (COUNTER = ?id)
AND (C_ISSUE = ?pIID)
AND (EXISTS (SELECT COUNTER FROM tbl_issue WHERE tbl_issue.COUNTER = ?pIID AND tbl_issue.C_PROJECT = ?pPID))