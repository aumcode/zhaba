SELECT
  T1.COUNTER AS C_PROJECT,
  T1.NAME,
  COUNT(T2.COUNTER) AS IssueCount
FROM tbl_project T1
JOIN tbl_issue T2 ON T2.C_PROJECT = T1.COUNTER
WHERE ((T1.COUNTER = ?pProject) OR ( ?pProject IS NULL))
  AND (T2.COUNTER IN (SELECT T3.C_ISSUE FROM tbl_issuelog T3 WHERE T3.STATUS_DATE <= ?pAsOf))
GROUP BY T1.COUNTER, T1.NAME
ORDER BY T1.NAME