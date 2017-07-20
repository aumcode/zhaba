﻿SELECT 
  T1.COUNTER, 
  TI.NAME,
  T1.STATUS,
  T1.STATUS_DATE,
  T1.DESCRIPTION,
  NULL AS COMPLETE_DATE, 
  T1.START_DATE,
  T1.COMPLETENESS, 
  T1.DUE_DATE,
  T1.PRIORITY as PRIORITY,
  TC.NAME as CATEGORY_NAME,
  TP.COUNTER as C_PROJECT,
  TP.NAME as PROJECTNAME,
  TU.LOGIN as OPERATOR,
  (SELECT GROUP_CONCAT(_tt2.LOGIN SEPARATOR '; ')
     FROM tbl_issueassign _tt1
       JOIN tbl_user _tt2 ON _tt1.C_USER = _tt2.COUNTER
     WHERE (_tt1.C_ISSUE = T1.C_ISSUE) 
           AND (_tt1.OPEN_TS <= DATE_ADD(DATE (T1.STATUS_DATE), INTERVAL 1 DAY )) 
           AND ( DATE_ADD(DATE (T1.STATUS_DATE), INTERVAL 1 day) <= _tt1.CLOSE_TS OR _tt1.CLOSE_TS IS NULL )
     GROUP BY C_ISSUE) AS ASSIGNEE,
  CASE WHEN T1.STATUS IN ('D', 'X', 'C') THEN T1.STATUS_DATE ELSE NULL END AS COMPLETE_DATE
FROM tbl_issuelog as T1
  join tbl_issue as TI on T1.C_ISSUE = TI.COUNTER
  join tbl_category as TC on T1.C_CATEGORY = TC.COUNTER
  join tbl_user as TU on T1.C_OPERATOR = TU.COUNTER
  join tbl_milestone as TM on T1.C_MILESTONE = TM.COUNTER
  join tbl_project as TP on TI.C_PROJECT = TP.COUNTER
WHERE 
  (T1.C_ISSUE = ?C_Issue) 
  AND (T1.STATUS_DATE <= ?dateUTC)
ORDER BY T1.STATUS_DATE DESC
LIMIT 5