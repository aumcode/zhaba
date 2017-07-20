select 
  TI.COUNTER, 
  TI.NAME,
  T1.STATUS,
  T1.STATUS_DATE,
  T1.DESCRIPTION,
  T1.START_DATE,
  T1.COMPLETENESS, 
  T1.DUE_DATE,
  TU.LOGIN as OPERATOR,
  TC.NAME as CATEGORY_NAME,
  TP.COUNTER as C_PROJECT,
  TP.NAME as PROJECTNAME,
  (SELECT GROUP_CONCAT(_tt2.LOGIN SEPARATOR '; ')
     FROM tbl_issueassign _tt1
       JOIN tbl_user _tt2 ON _tt1.C_USER = _tt2.COUNTER
     WHERE (_tt1.C_ISSUE = T1.C_ISSUE) AND ( ?pAsOf <= _tt1.CLOSE_TS OR _tt1.CLOSE_TS IS NULL )
     GROUP BY C_ISSUE) AS ASSIGNEE,
  T1.PRIORITY,
  CASE WHEN T1.STATUS IN ('D', 'X', 'C') THEN T1.STATUS_DATE ELSE NULL END AS COMPLETE_DATE,
    CASE WHEN T1.STATUS IN ('D', 'X', 'C')
    THEN datediff(T1.DUE_DATE,T1.STATUS_DATE)
    ELSE datediff(T1.DUE_DATE,?pAsOf)
  END AS Remaining 
from tbl_issuelog as T1
  join tbl_issue as TI on T1.C_ISSUE = TI.COUNTER
  join tbl_user as TU on T1.C_OPERATOR = TU.COUNTER
  join tbl_category as TC on T1.C_CATEGORY = TC.COUNTER
  join tbl_milestone as TM on T1.C_MILESTONE = TM.COUNTER
  join tbl_project as TP on TI.C_PROJECT = TP.COUNTER
where (T1.STATUS_DATE = (Select MAX(STATUS_DATE) from tbl_issuelog where (T1.C_ISSUE = C_ISSUE) and (STATUS_DATE <= ?pAsOf)))
  AND (TP.COUNTER = ?pProject)
ORDER BY T1.DUE_DATE ASC