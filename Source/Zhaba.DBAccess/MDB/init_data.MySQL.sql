INSERT INTO `tbl_sequence` (`SCOPE_NAME`, `SEQUENCE_NAME`, `COUNTER`)
VALUES ('Zhaba', 'User', 0);

INSERT INTO `tbl_user` (`COUNTER`, `LOGIN`, `FIRST_NAME`, `LAST_NAME`, `EMAIL`, `STATUS`, `PASSWORD`, `USER_RIGHTS`, `IN_USE`)
VALUES (0, 'admin', 'System', 'Admin', 'sa@example.com', 'S', '{"algo":"MD5","fam":"Text","hash":"p9qwo9yBi\/rIybeyDHTElQ==","salt":"K9XMZkfABPVHJ\/JrkzWNOK\/hCrBlhw=="}', '{z:{rights:{}}}', 'T');


