SELECT
  T3.STATUS AS STATUS,
  COUNT(T2.COUNTER) AS COUNT
FROM tbl_project T1
  JOIN tbl_issue T2 ON T2.C_PROJECT = T1.COUNTER
  JOIN tbl_issuelog T3 ON T3.C_ISSUE = T2.COUNTER
WHERE (T3.STATUS_DATE = (SELECT MAX(STATUS_DATE) FROM tbl_issuelog  WHERE (C_ISSUE = T2.COUNTER) and (STATUS_DATE <= ?pAsOf)))
  AND ((T1.COUNTER = ?pProject) OR ( ?pProject IS NULL))
GROUP BY T3.STATUS
ORDER BY T3.STATUS