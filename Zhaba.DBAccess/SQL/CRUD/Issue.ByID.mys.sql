SELECT *
FROM tbl_issue
WHERE
 (C_PROJECT = ?pProj_ID) AND (Counter = ?pID);
