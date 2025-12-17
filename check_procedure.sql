-- ===============================================
-- SCRIPT KIỂM TRA LỖI PROCEDURE
-- ===============================================

-- 1. Kiểm tra procedure có tồn tại không
SELECT object_name, object_type, status, created
FROM dba_objects
WHERE object_name = 'P_GET_ALL_DB_USERS'
  AND owner = 'NAM_DOAN';
-- Kết quả mong đợi: 1 dòng với STATUS = 'VALID'
-- Nếu không có dòng nào → chưa compile
-- Nếu STATUS = 'INVALID' → compile lỗi

-- 2. Nếu INVALID, xem lỗi compile ở đâu
SELECT line, position, text
FROM dba_errors
WHERE owner = 'NAM_DOAN'
  AND name = 'P_GET_ALL_DB_USERS'
ORDER BY sequence;

-- 3. Kiểm tra quyền EXECUTE
SELECT grantee, privilege, grantable
FROM dba_tab_privs
WHERE owner = 'NAM_DOAN'
  AND table_name = 'P_GET_ALL_DB_USERS';
-- Phải có: GRANTEE = 'PUBLIC' hoặc user đang dùng

-- 4. Thử chạy procedure trực tiếp
DECLARE
    v_cursor SYS_REFCURSOR;
BEGIN
    NAM_DOAN.P_GET_ALL_DB_USERS(v_cursor);
    DBMS_OUTPUT.PUT_LINE('Procedure chạy thành công!');
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('LỖI: ' || SQLERRM);
END;
/

-- ===============================================
-- HƯỚNG DẪN SỬA LỖI
-- ===============================================

-- Nếu procedure INVALID hoặc không tồn tại:
-- Chạy lại phần tạo procedure (từ NAM_DOAN.sql line 771-781)

-- Nếu thiếu quyền EXECUTE:
GRANT EXECUTE ON NAM_DOAN.P_GET_ALL_DB_USERS TO PUBLIC;
-- hoặc
GRANT EXECUTE ON NAM_DOAN.P_GET_ALL_DB_USERS TO ROLE_USERS;

-- Nếu vẫn lỗi, compile lại từ đầu:
CREATE OR REPLACE PROCEDURE NAM_DOAN.P_GET_ALL_DB_USERS (
    p_cursor OUT SYS_REFCURSOR
) IS
BEGIN
    OPEN p_cursor FOR
        SELECT username 
        FROM all_users 
        WHERE username NOT IN ('SYS', 'SYSTEM', 'NAM_DOAN', 'OUTLN', 'DBSNMP')
        ORDER BY username;
END;
/

-- Sau đó cấp quyền:
GRANT EXECUTE ON NAM_DOAN.P_GET_ALL_DB_USERS TO PUBLIC;
