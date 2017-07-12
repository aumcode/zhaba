SELECT T1.*
FROM tbl_component as T1
WHERE T1.COUNTER IN (
                      SELECT T2.C_COMPONENT 
                      FROM tbl_issuecomponent T2 
                      WHERE T2.C_ISSUE = ?pIssue
                    )
