UPDATE tbl_issue 
SET IN_USE = 'F'
WHERE
 (C_PROJECT = ?C_Project) AND (Counter = ?C_Issue);
