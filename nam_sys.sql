
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

-- 5. TAO PACKAGE QUAN LY LOGOUT (KILL SESSION)
CREATE OR REPLACE PACKAGE PKG_LOGOUT
AUTHID DEFINER AS
  PROCEDURE P_LOGOUT_CURRENT(p_username IN VARCHAR2);
  PROCEDURE P_LOGOUT_ALL(p_username IN VARCHAR2);
  PROCEDURE P_LOGOUT_BY_MACHINE(p_username IN VARCHAR2, p_machine IN VARCHAR2);
END PKG_LOGOUT;
/

CREATE OR REPLACE PACKAGE BODY PKG_LOGOUT AS
  -- Dang xuat thiet bi hien tai cua user dang dang nhap
  PROCEDURE P_LOGOUT_CURRENT(p_username IN VARCHAR2) 
  AS
    v_sid    NUMBER;
    v_serial NUMBER;
  BEGIN
    -- Lay dung SID va SERIAL# cua user hien tai dang dang nhap
    SELECT sid, serial#
      INTO v_sid, v_serial
      FROM v$session
     WHERE username = p_username
       AND audsid = USERENV('SESSIONID');

    -- Kill chinh session do
    EXECUTE IMMEDIATE 
      'ALTER SYSTEM KILL SESSION ''' || v_sid || ',' || v_serial || ''' IMMEDIATE';

  EXCEPTION
    WHEN NO_DATA_FOUND THEN
      DBMS_OUTPUT.PUT_LINE('Khong tim thay session hien tai cua ' || p_username);
    WHEN OTHERS THEN
      DBMS_OUTPUT.PUT_LINE('Loi khi logout hien tai: ' || SQLERRM);
  END P_LOGOUT_CURRENT;


  -- Dang xuat toan bo thiet bi
  PROCEDURE P_LOGOUT_ALL(p_username IN VARCHAR2)
  AS
  BEGIN
    FOR rec IN (
      SELECT sid, serial#
        FROM v$session
       WHERE username = p_username
         AND status IN ('ACTIVE', 'INACTIVE')
    ) LOOP
      BEGIN
        EXECUTE IMMEDIATE 
          'ALTER SYSTEM KILL SESSION ''' || rec.sid || ',' || rec.serial# || ''' IMMEDIATE';
      EXCEPTION
        WHEN OTHERS THEN
          DBMS_OUTPUT.PUT_LINE('Khong the kill session ' || rec.sid || ',' || rec.serial#);
      END;
    END LOOP;
  EXCEPTION
    WHEN OTHERS THEN
      DBMS_OUTPUT.PUT_LINE('Loi logout toan bo thiet bi: ' || SQLERRM);
  END P_LOGOUT_ALL;


  -- Dang xuat thiet bi cu the
  PROCEDURE P_LOGOUT_BY_MACHINE(p_username IN VARCHAR2, p_machine IN VARCHAR2)
  AS
    v_count NUMBER := 0;
  BEGIN
    FOR rec IN (
      SELECT sid, serial#, machine
        FROM v$session
       WHERE username = p_username
         AND machine LIKE '%' || p_machine|| '%'
    ) LOOP
      BEGIN
        EXECUTE IMMEDIATE 
          'ALTER SYSTEM KILL SESSION ''' || rec.sid || ',' || rec.serial# || ''' IMMEDIATE';
        v_count := v_count + 1;
      EXCEPTION
        WHEN OTHERS THEN
          DBMS_OUTPUT.PUT_LINE('Khong the kill session tren may ' || rec.machine);
      END;
    END LOOP;

    IF v_count = 0 THEN
      DBMS_OUTPUT.PUT_LINE('Khong tim thay session tren may ' || p_machine);
    END IF;
  EXCEPTION
    WHEN OTHERS THEN
      DBMS_OUTPUT.PUT_LINE('Loi logout theo may: ' || SQLERRM);
  END P_LOGOUT_BY_MACHINE;

END PKG_LOGOUT;
/

-- Cap quyen thuc thi package nay cho NAM_DOAN
GRANT EXECUTE ON PKG_LOGOUT TO NAM_DOAN;

-- 6. CAP QUYEN GIAM SAT (AUDITING)
-- Gan quyen giam sat he thong cho NAM_DOAN
GRANT AUDIT SYSTEM TO NAM_DOAN;
GRANT AUDIT ANY TO NAM_DOAN;

-- Cho phep xem tat ca log audit
GRANT SELECT ANY DICTIONARY TO NAM_DOAN;

-- Kiem tra user da tao thanh cong
SELECT * FROM ALL_USERS WHERE USERNAME = 'NAM_DOAN';