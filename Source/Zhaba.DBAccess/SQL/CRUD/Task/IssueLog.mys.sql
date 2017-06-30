﻿SELECT 
  TI.COUNTER, 
  TI.NAME,
  T1.STATUS,
  T1.STATUS_DATE,
  T1.DESCRIPTION,
  TM.COMPLETE_DATE, 
  TM.START_DATE,
  T1.COMPLETENESS, 
  TM.PLAN_DATE,
  TC.NAME as CATEGORY_NAME,
  TP.COUNTER as C_PROJECT,
  TP.NAME as PROJECTNAME,
  T1.NOTE
FROM tbl_issuelog as T1
  join tbl_issue as TI on T1.C_ISSUE = TI.COUNTER
  join tbl_category as TC on T1.C_CATEGORY = TC.COUNTER
  join tbl_milestone as TM on T1.C_MILESTONE = TM.COUNTER
  join tbl_project as TP on TI.C_PROJECT = TP.COUNTER
WHERE 
  (T1.C_ISSUE = ?C_Issue) 
  AND (T1.STATUS_DATE <= ?dateUTC)
ORDER BY T1.STATUS_DATE DESC
LIMIT 5