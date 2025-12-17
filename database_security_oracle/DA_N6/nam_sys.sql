
-- 1. TAO USER

-- DROP USER NAM_DOAN CASCADE;
CREATE USER NAM_DOAN IDENTIFIED BY NAM_DOAN;

-- 2. CAP QUYEN HE THONG CO BAN
GRANT CREATE SESSION TO NAM_DOAN;           -- Quyen dang nhap
GRANT CREATE TABLE TO NAM_DOAN;             -- Quyen tao bang
GRANT CREATE USER TO NAM_DOAN;              -- Quyen tao user con
GRANT CREATE PROCEDURE TO NAM_DOAN;         -- Quyen tao thu tuc
GRANT CREATE ROLE TO NAM_DOAN;              -- Quyen tao nhom quyen
GRANT ALTER USER TO NAM_DOAN;               -- Quyen sua doi user
GRANT DROP USER TO NAM_DOAN;                -- Quyen xoa user
GRANT GRANT ANY PRIVILEGE TO NAM_DOAN;      -- Quyen cap quyen cho user khac
GRANT INHERIT PRIVILEGES ON USER SYS TO NAM_DOAN;
GRANT ALTER SYSTEM TO NAM_DOAN;                 -- Quyen kill session (quan trong)

-- Cap cac role co san
GRANT CONNECT, RESOURCE TO NAM_DOAN;
GRANT SELECT ANY DICTIONARY TO NAM_DOAN;    -- De tra cuu bang he thong (DBA_TAB_PRIVS)
GRANT GRANT ANY ROLE TO NAM_DOAN;           -- De cap role cho user khac

-- 3. QUAN LY TABLESPACE (CHUA DU LIEU)
-- Cap quyen tao va xoa Tablespace
GRANT CREATE TABLESPACE TO NAM_DOAN;
GRANT DROP TABLESPACE TO NAM_DOAN;

-- Cap han muc (Quota) tren Users tablespace
ALTER USER NAM_DOAN QUOTA 100M ON USERS;

-- 4. CAP QUYEN TRUY CAP CAC VIEW HE THONG (DE QUAN LY SESSION)
-- Tao View boc V_$SESSION de de quan ly
CREATE OR REPLACE VIEW V_SESSION AS
SELECT * FROM V_$SESSION;

-- Cap quyen select view nay cho NAM_DOAN (kem quyen cap lai cho nguoi khac)
GRANT SELECT ON V_SESSION TO NAM_DOAN WITH GRANT OPTION;

-- 5. TAO PACKAGE QUAN LY LOGOUT (DA LOAI BO - CHUYEN VE NAM_DOAN QUAN LY)
-- DROP PACKAGE SYS.PKG_LOGOUT;

-- 6. CAP QUYEN GIAM SAT (AUDITING)
-- Gan quyen giam sat he thong cho NAM_DOAN
GRANT AUDIT SYSTEM TO NAM_DOAN;
GRANT AUDIT ANY TO NAM_DOAN;

-- Cho phep xem tat ca log audit
GRANT SELECT ANY DICTIONARY TO NAM_DOAN;

-- Kiem tra user da tao thanh cong
SELECT * FROM ALL_USERS WHERE USERNAME = 'NAM_DOAN';

-- 7. CAP QUYEN MA HOA (DBMS_CRYPTO)
-- Can thiet cho feature ma hoa file (DES_PKG)
GRANT EXECUTE ON SYS.DBMS_CRYPTO TO NAM_DOAN;
