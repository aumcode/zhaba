SELECT T1.*
FROM tbl_area as T1
WHERE T1.COUNTER IN (
                      SELECT T2.C_AREA 
                      FROM tbl_issuearea T2 
                      WHERE T2.C_ISSUE = ?pIssue
                    )
