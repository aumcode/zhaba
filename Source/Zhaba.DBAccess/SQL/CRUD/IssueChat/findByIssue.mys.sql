SELECT t1.COUNTER,
       t1.C_ISSUE,
       t1.C_USER,
       t1.NOTE_DATE,
       t1.NOTE,
       t2.FIRST_NAME,
       t2.LAST_NAME,
       t2.LOGIN
FROM tbl_issuechat t1
JOIN tbl_user t2 ON t2.COUNTER = T1.C_USER
WHERE t1.C_ISSUE = ?pIssue
order by t1.NOTE_DATE