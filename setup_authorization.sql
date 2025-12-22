-- =====================================================================
-- SCRIPT SETUP HOÀN CHỈNH CHO AUTHORIZATION FEATURE
-- Chạy bằng user SYS hoặc có quyền DBA
-- =====================================================================

-- Bước 1: Tạo tất cả procedures (chạy từng block riêng)

-- 1.1 Procedure lấy danh sách bảng
CREATE OR REPLACE PROCEDURE NAM_DOAN.P_GET_ALL_TABLES (
    p_cursor OUT SYS_REFCURSOR
) IS
BEGIN
    OPEN p_cursor FOR
        SELECT table_name 
        FROM user_tables
        ORDER BY table_name;
END P_GET_ALL_TABLES;
/

-- 1.2 Procedure lấy danh sách users
CREATE OR REPLACE PROCEDURE NAM_DOAN.P_GET_ALL_DB_USERS (
    p_cursor OUT SYS_REFCURSOR
) IS
BEGIN
    OPEN p_cursor FOR
        SELECT username 
        FROM all_users 
        WHERE username NOT IN ('SYS', 'SYSTEM', 'NAM_DOAN', 'OUTLN', 'DBSNMP')
        ORDER BY username;
END P_GET_ALL_DB_USERS;
/

-- 1.3 Procedure cấp quyền cho user
CREATE OR REPLACE PROCEDURE NAM_DOAN.P_GRANT_PERMISSION (
    p_user      IN VARCHAR2,
    p_table     IN VARCHAR2,
    p_select    IN NUMBER,
    p_insert    IN NUMBER,
    p_update    IN NUMBER,
    p_delete    IN NUMBER
) AUTHID CURRENT_USER
IS
BEGIN
    IF p_select = 1 THEN EXECUTE IMMEDIATE 'GRANT SELECT ON NAM_DOAN.' || p_table || ' TO "' || p_user || '"'; END IF;
    IF p_insert = 1 THEN EXECUTE IMMEDIATE 'GRANT INSERT ON NAM_DOAN.' || p_table || ' TO "' || p_user || '"'; END IF;
    IF p_update = 1 THEN EXECUTE IMMEDIATE 'GRANT UPDATE ON NAM_DOAN.' || p_table || ' TO "' || p_user || '"'; END IF;
    IF p_delete = 1 THEN EXECUTE IMMEDIATE 'GRANT DELETE ON NAM_DOAN.' || p_table || ' TO "' || p_user || '"'; END IF;
END P_GRANT_PERMISSION;
/

-- 1.4 Procedure thu hồi quyền
CREATE OR REPLACE PROCEDURE NAM_DOAN.P_REVOKE_PERMISSION (
    p_user      IN VARCHAR2,
    p_table     IN VARCHAR2,
    p_select    IN NUMBER,
    p_insert    IN NUMBER,
    p_update    IN NUMBER,
    p_delete    IN NUMBER
) AUTHID CURRENT_USER
IS
BEGIN
    IF p_select = 1 THEN 
        BEGIN EXECUTE IMMEDIATE 'REVOKE SELECT ON NAM_DOAN.' || p_table || ' FROM "' || p_user || '"'; EXCEPTION WHEN OTHERS THEN NULL; END; 
    END IF;
    IF p_insert = 1 THEN 
        BEGIN EXECUTE IMMEDIATE 'REVOKE INSERT ON NAM_DOAN.' || p_table || ' FROM "' || p_user || '"'; EXCEPTION WHEN OTHERS THEN NULL; END; 
    END IF;
    IF p_update = 1 THEN 
        BEGIN EXECUTE IMMEDIATE 'REVOKE UPDATE ON NAM_DOAN.' || p_table || ' FROM "' || p_user || '"'; EXCEPTION WHEN OTHERS THEN NULL; END; 
    END IF;
    IF p_delete = 1 THEN 
        BEGIN EXECUTE IMMEDIATE 'REVOKE DELETE ON NAM_DOAN.' || p_table || ' FROM "' || p_user || '"'; EXCEPTION WHEN OTHERS THEN NULL; END; 
    END IF;
END P_REVOKE_PERMISSION;
/

-- 1.5 Procedure tạo role
CREATE OR REPLACE PROCEDURE NAM_DOAN.P_CREATE_ROLE (
    p_role_name IN VARCHAR2
) AUTHID CURRENT_USER
IS
BEGIN
    EXECUTE IMMEDIATE 'CREATE ROLE "' || p_role_name || '"';
END P_CREATE_ROLE;
/

-- 1.6 Procedure cấp quyền cho role
CREATE OR REPLACE PROCEDURE NAM_DOAN.P_GRANT_PERMISSION_TO_ROLE (
    p_role      IN VARCHAR2,
    p_table     IN VARCHAR2,
    p_select    IN NUMBER,
    p_insert    IN NUMBER,
    p_update    IN NUMBER,
    p_delete    IN NUMBER
) AUTHID CURRENT_USER
IS
BEGIN
    NAM_DOAN.P_GRANT_PERMISSION(p_role, p_table, p_select, p_insert, p_update, p_delete);
END P_GRANT_PERMISSION_TO_ROLE;
/

-- 1.7 Procedure cấp role cho user
CREATE OR REPLACE PROCEDURE NAM_DOAN.P_GRANT_ROLE_TO_USER (
    p_role_name IN VARCHAR2,
    p_username  IN VARCHAR2
) AUTHID CURRENT_USER
IS
BEGIN
    EXECUTE IMMEDIATE 'GRANT "' || p_role_name || '" TO "' || p_username || '"';
END P_GRANT_ROLE_TO_USER;
/

-- 1.8 Procedure lấy danh sách roles
CREATE OR REPLACE PROCEDURE NAM_DOAN.P_GET_ALL_ROLES (
    p_cursor OUT SYS_REFCURSOR
) IS
BEGIN
    OPEN p_cursor FOR
        SELECT role 
        FROM dba_roles
        WHERE role NOT LIKE 'ORA%'
          AND role NOT LIKE 'XDB%'
        ORDER BY role;
END P_GET_ALL_ROLES;
/

-- 1.9 Procedure kiểm tra quyền user
CREATE OR REPLACE PROCEDURE NAM_DOAN.P_CHECK_USER_PERMISSION (
    p_username IN VARCHAR2,
    p_table    IN VARCHAR2,
    p_select   OUT NUMBER,
    p_insert   OUT NUMBER,
    p_update   OUT NUMBER,
    p_delete   OUT NUMBER
) IS
BEGIN
    p_select := 0; p_insert := 0; p_update := 0; p_delete := 0;

    FOR r IN (
        SELECT PRIVILEGE 
        FROM dba_tab_privs 
        WHERE GRANTEE = p_username 
          AND TABLE_NAME = p_table
          AND OWNER = 'NAM_DOAN'
    ) LOOP
        IF r.PRIVILEGE = 'SELECT' THEN p_select := 1; END IF;
        IF r.PRIVILEGE = 'INSERT' THEN p_insert := 1; END IF;
        IF r.PRIVILEGE = 'UPDATE' THEN p_update := 1; END IF;
        IF r.PRIVILEGE = 'DELETE' THEN p_delete := 1; END IF;
    END LOOP;
END P_CHECK_USER_PERMISSION;
/

-- =====================================================================
-- Bước 2: Kiểm tra compilation
-- =====================================================================
SELECT object_name, status
FROM user_objects
WHERE object_type = 'PROCEDURE'
  AND object_name LIKE 'P_%'
ORDER BY object_name;
-- TẤT CẢ phải có STATUS = 'VALID'

-- =====================================================================
-- Bước 3: Cấp quyền EXECUTE cho PUBLIC (hoặc ROLE_USERS)
-- =====================================================================
GRANT EXECUTE ON NAM_DOAN.P_GET_ALL_TABLES TO PUBLIC;
GRANT EXECUTE ON NAM_DOAN.P_GET_ALL_DB_USERS TO PUBLIC;
GRANT EXECUTE ON NAM_DOAN.P_GRANT_PERMISSION TO PUBLIC;
GRANT EXECUTE ON NAM_DOAN.P_REVOKE_PERMISSION TO PUBLIC;
GRANT EXECUTE ON NAM_DOAN.P_CREATE_ROLE TO PUBLIC;
GRANT EXECUTE ON NAM_DOAN.P_GRANT_PERMISSION_TO_ROLE TO PUBLIC;
GRANT EXECUTE ON NAM_DOAN.P_GRANT_ROLE_TO_USER TO PUBLIC;
GRANT EXECUTE ON NAM_DOAN.P_GET_ALL_ROLES TO PUBLIC;
GRANT EXECUTE ON NAM_DOAN.P_CHECK_USER_PERMISSION TO PUBLIC;

-- =====================================================================
-- Bước 4: Test procedure
-- =====================================================================
DECLARE
    v_cursor SYS_REFCURSOR;
    v_username VARCHAR2(100);
BEGIN
    DBMS_OUTPUT.PUT_LINE('=== Test P_GET_ALL_DB_USERS ===');
    NAM_DOAN.P_GET_ALL_DB_USERS(v_cursor);
    
    LOOP
        FETCH v_cursor INTO v_username;
        EXIT WHEN v_cursor%NOTFOUND;
        DBMS_OUTPUT.PUT_LINE('User: ' || v_username);
    END LOOP;
    
    CLOSE v_cursor;
    DBMS_OUTPUT.PUT_LINE('=== Test thành công! ===');
END;
/

-- =====================================================================
-- HƯỚNG DẪN SỬ DỤNG
-- =====================================================================
-- 1. Mở SQL Developer hoặc SQL*Plus
-- 2. Kết nối bằng user SYS AS SYSDBA hoặc NAM_DOAN (với quyền AUTHID CURRENT_USER)
-- 3. Chạy toàn bộ script này (F5 hoặc Run Script)
-- 4. Kiểm tra output - tất cả phải VALID
-- 5. Chạy lại ứng dụng C#
