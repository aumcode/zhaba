SELECT *
FROM tbl_user
WHERE IN_USE = 'T' AND 
COUNTER NOT IN (
  SELECT C_USER 
  FROM tbl_issueassign 
  WHERE ( C_ISSUE = ?C_Issue ) AND 
        ( ?DateUTC < CLOSE_TS OR CLOSE_TS IS NULL )
);