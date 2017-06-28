UPDATE tbl_issue 
SET IN_USE = 'T'
WHERE
 (C_PROJECT = ?C_Project) AND (Counter = ?C_Issue);
