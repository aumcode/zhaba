UPDATE tbl_issueassign 
SET 
  CLOSE_TS = ?pClose_TS, 
  C_CLOSE_OPER = ?pC_User
WHERE (C_ISSUE = ?pC_Issue) AND (?pClose_TS <= CLOSE_TS OR CLOSE_TS IS NULL);
