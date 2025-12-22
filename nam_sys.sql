
CREATE USER NAM_DOAN IDENTIFIED BY NAM_DOAN;

GRANT INHERIT PRIVILEGES ON USER SYS TO NAM_DOAN;

GRANT CREATE TABLE TO NAM_DOAN;
GRANT CREATE USER TO NAM_DOAN;
GRANT CREATE SESSION TO NAM_DOAN;
GRANT GRANT ANY PRIVILEGE TO NAM_DOAN;
GRANT ALTER USER TO NAM_DOAN;
GRANT CONNECT, RESOURCE TO NAM_DOAN;
GRANT CREATE PROCEDURE TO NAM_DOAN;
GRANT CREATE ROLE TO NAM_DOAN;
GRANT EXECUTE ON PKG_LOGOUT TO NAM_DOAN;

-- PROFILE
GRANT ALTER PROFILE TO NAM_DOAN;
GRANT SELECT ON DBA_PROFILES TO NAM_DOAN;

-- CREATE QUOTA
-- 1. Cấp quy�?n tạo Tablespace trực tiếp 
GRANT CREATE TABLESPACE TO NAM_DOAN;

-- 2. Cấp quy�?n xóa Tablespace
GRANT DROP TABLESPACE TO NAM_DOAN;

-- 3. Cấp quy�?n quản lý User trực tiếp
GRANT ALTER USER TO NAM_DOAN;
GRANT DROP USER TO NAM_DOAN;
GRANT CREATE USER TO NAM_DOAN;

ALTER USER NAM_DOAN QUOTA 100M ON USERS;

-- View V$SESSION
CREATE OR REPLACE VIEW V_SESSION AS
SELECT * FROM V_$SESSION;

-- View profile
CREATE OR REPLACE VIEW V_SECURITY_POLICIES AS
SELECT 
    PROFILE, 
    RESOURCE_NAME, 
    LIMIT
FROM DBA_PROFILES
WHERE RESOURCE_NAME IN (
    'FAILED_LOGIN_ATTEMPTS', -- Số lần đăng nhập sai tối đa
    'PASSWORD_LOCK_TIME',    -- Th�?i gian khóa (ngày)
    'PASSWORD_LIFE_TIME',    -- Th�?i gian hết hạn mật khẩu (ngày)
    'PASSWORD_GRACE_TIME',   -- Th�?i gian cảnh báo đổi pass (ngày)
    'SESSIONS_PER_USER',     -- Số session tối đa 1 user
    'IDLE_TIME'              -- Th�?i gian tự logout khi treo máy (phút)
)
AND PROFILE = 'DEFAULT' -- Thư�?ng ta chỉnh trên profile DEFAULT
ORDER BY RESOURCE_NAME;

GRANT SELECT ON V_SESSION TO NAM_DOAN WITH GRANT OPTION;
-- LOGOUT
CREATE OR REPLACE PACKAGE PKG_LOGOUT
AUTHID DEFINER AS
  PROCEDURE P_LOGOUT_CURRENT(p_username IN VARCHAR2);
  PROCEDURE P_LOGOUT_ALL(p_username IN VARCHAR2);
  PROCEDURE P_LOGOUT_BY_MACHINE(p_username IN VARCHAR2, p_machine IN VARCHAR2);
END PKG_LOGOUT;

CREATE OR REPLACE PACKAGE BODY PKG_LOGOUT AS
  -- �?ăng xuất thiết bị hiện tại của user đang đăng nhập
  PROCEDURE P_LOGOUT_CURRENT(p_username IN VARCHAR2) 
  AS
    v_sid    NUMBER;
    v_serial NUMBER;
  BEGIN
    -- Lấy đúng SID và SERIAL# của user hiện tại đang đăng nhập
    SELECT sid, serial#
      INTO v_sid, v_serial
      FROM v$session
     WHERE username = p_username
       AND audsid = USERENV('SESSIONID');

    -- Kill chính session đó
    EXECUTE IMMEDIATE 
      'ALTER SYSTEM KILL SESSION ''' || v_sid || ',' || v_serial || ''' IMMEDIATE';

  EXCEPTION
    WHEN NO_DATA_FOUND THEN
      DBMS_OUTPUT.PUT_LINE('Không tìm thấy session hiện tại của ' || p_username);
    WHEN OTHERS THEN
      DBMS_OUTPUT.PUT_LINE('Lỗi khi logout hiện tại: ' || SQLERRM);
  END P_LOGOUT_CURRENT;


  -- �?ăng xuất toàn bộ thiết bị
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
          DBMS_OUTPUT.PUT_LINE('Không thể kill session ' || rec.sid || ',' || rec.serial#);
      END;
    END LOOP;
  EXCEPTION
    WHEN OTHERS THEN
      DBMS_OUTPUT.PUT_LINE('Lỗi logout toàn bộ thiết bị: ' || SQLERRM);
  END P_LOGOUT_ALL;


  -- �?ăng xuất thiết bị cụ thể
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
          DBMS_OUTPUT.PUT_LINE('Không thể kill session trên máy ' || rec.machine);
      END;
    END LOOP;

    IF v_count = 0 THEN
      DBMS_OUTPUT.PUT_LINE('Không tìm thấy session trên máy ' || p_machine);
    END IF;
  EXCEPTION
    WHEN OTHERS THEN
      DBMS_OUTPUT.PUT_LINE('Lỗi logout theo máy: ' || SQLERRM);
  END P_LOGOUT_BY_MACHINE;

END PKG_LOGOUT;





SELECT username, account_status FROM DBA_USERS ORDER BY username;
--===================================================================================================
--gi�m s�t
--g�n quy?n gi�m s�t cho nan_doan
GRANT AUDIT SYSTEM TO NAM_DOAN;
GRANT AUDIT ANY TO NAM_DOAN;

-- Cho ph�p xem t?t c? log audit
GRANT SELECT ANY DICTIONARY TO NAM_DOAN;

GRANT EXECUTE ON DBMS_FGA TO NAM_DOAN;
GRANT SELECT ON DBA_FGA_AUDIT_TRAIL TO NAM_DOAN;

--CHECK XEM C� POLICY CH?A
SELECT
    policy_name,
    object_schema,
    object_name,
    policy_text,
    enabled
FROM dba_audit_policies
WHERE object_schema = 'NAM_DOAN';
--CHECK AI THAO TAC
SELECT
    db_user,
    object_schema,
    object_name,
    sql_text,
    timestamp
FROM dba_fga_audit_trail
WHERE object_schema = 'NAM_DOAN'
ORDER BY timestamp DESC;
--view xem
CREATE OR REPLACE VIEW V_FGA_LOGS AS
SELECT
    DB_USER,
    OBJECT_SCHEMA,
    OBJECT_NAME,
    SQL_TEXT,
    TIMESTAMP
FROM DBA_FGA_AUDIT_TRAIL
WHERE OBJECT_SCHEMA = 'NAM_DOAN';


SELECT * FROM DBA_AUDIT_TRAIL;
SELECT * FROM V_FGA_LOGS;

DROP USER "f";
SELECT USERNAME FROM DBA_USERS;

SHOW PARAMETER audit_trail;




GRANT SELECT ON V_FGA_LOGS TO NAM_DOAN;
GRANT SELECT ON DBA_FGA_AUDIT_TRAIL TO NAM_DOAN;




DESC DBA_AUDIT_POLICIES;

--===================================================================================================

-- Ki?m tra user c� t?n t?i kh�ng
SELECT username FROM dba_users 

-- Ki?m tra v?i t�n vi?t th??ng
SELECT username FROM dba_users WHERE LOWER(username) = LOWER('AXW');

-- Ki?m tra trong b?ng USERS c?a NAM_DOAN
SELECT USER_NAME FROM NAM_DOAN.USERS WHERE USER_NAME = 'AXW';

-- Ki?m tra t?t c? user trong h? th?ng
SELECT username FROM dba_users ORDER BY username;




-- Ki?m tra quy?n hi?n t?i c?a user
SELECT * FROM USER_SYS_PRIVS WHERE USERNAME = USER;

-- Ki?m tra role
SELECT * FROM USER_ROLE_PRIVS;

-- N?u user c� role DBA, ?� c� ?? quy?n
SELECT GRANTED_ROLE FROM USER_ROLE_PRIVS WHERE GRANTED_ROLE = 'DBA';