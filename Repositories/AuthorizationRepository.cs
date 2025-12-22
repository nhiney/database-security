using DA_N6.Database;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace DA_N6.Repositories
{
    public class AuthorizationRepository
    {
        // 1. Lấy danh sách Table
        public List<string> GetAllTables()
        {
            List<string> tables = new List<string>();
            try
            {
                // Gọi Procedure P_GET_ALL_TABLES
                var parameters = new OracleParameter[] {
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
                };

                DataTable dt = DatabaseHelper.ExecuteProcedureToTable("NAM_DOAN.P_GET_ALL_TABLES", parameters);
                foreach (DataRow row in dt.Rows)
                {
                    tables.Add(row["TABLE_NAME"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách bảng: " + ex.Message);
            }
            return tables;
        }

        // 2. Lấy danh sách User
        public List<string> GetAllUsers()
        {
            List<string> users = new List<string>();
            try
            {
                var parameters = new OracleParameter[] {
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
                };

                DataTable dt = DatabaseHelper.ExecuteProcedureToTable("NAM_DOAN.P_GET_ALL_DB_USERS", parameters);
                foreach (DataRow row in dt.Rows)
                {
                    users.Add(row["USERNAME"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách User: " + ex.Message);
            }
            return users;
        }

        // 3. Cấp quyền
        public void GrantPermission(string user, string table, bool select, bool insert, bool update, bool delete)
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_user", OracleDbType.Varchar2) { Value = user },
                new OracleParameter("p_table", OracleDbType.Varchar2) { Value = table },
                new OracleParameter("p_select", OracleDbType.Int32) { Value = select ? 1 : 0 },
                new OracleParameter("p_insert", OracleDbType.Int32) { Value = insert ? 1 : 0 },
                new OracleParameter("p_update", OracleDbType.Int32) { Value = update ? 1 : 0 },
                new OracleParameter("p_delete", OracleDbType.Int32) { Value = delete ? 1 : 0 }
            };

            DatabaseHelper.ExecuteProcedure("NAM_DOAN.P_GRANT_PERMISSION", parameters);
        }

        // 4. Thu hồi quyền (Revoke)
        public void RevokePermission(string user, string table, bool select, bool insert, bool update, bool delete)
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_user", OracleDbType.Varchar2) { Value = user },
                new OracleParameter("p_table", OracleDbType.Varchar2) { Value = table },
                new OracleParameter("p_select", OracleDbType.Int32) { Value = select ? 1 : 0 },
                new OracleParameter("p_insert", OracleDbType.Int32) { Value = insert ? 1 : 0 },
                new OracleParameter("p_update", OracleDbType.Int32) { Value = update ? 1 : 0 },
                new OracleParameter("p_delete", OracleDbType.Int32) { Value = delete ? 1 : 0 }
            };

            DatabaseHelper.ExecuteProcedure("NAM_DOAN.P_REVOKE_PERMISSION", parameters);
        }

        // 5. Kiểm tra quyền hiện tại (để hiển thị lên UI)
        public Dictionary<string, bool> CheckPermission(string user, string table)
        {
            var result = new Dictionary<string, bool> {
                {"SELECT", false}, {"INSERT", false}, {"UPDATE", false}, {"DELETE", false}
            };

            var pSelect = new OracleParameter("p_select", OracleDbType.Int32, ParameterDirection.Output);
            var pInsert = new OracleParameter("p_insert", OracleDbType.Int32, ParameterDirection.Output);
            var pUpdate = new OracleParameter("p_update", OracleDbType.Int32, ParameterDirection.Output);
            var pDelete = new OracleParameter("p_delete", OracleDbType.Int32, ParameterDirection.Output);

            var parameters = new OracleParameter[] {
                new OracleParameter("p_username", OracleDbType.Varchar2) { Value = user },
                new OracleParameter("p_table", OracleDbType.Varchar2) { Value = table },
                pSelect, pInsert, pUpdate, pDelete
            };

            DatabaseHelper.ExecuteProcedure("NAM_DOAN.P_CHECK_USER_PERMISSION", parameters);

            // Đọc giá trị Output (sử dụng OracleDecimal hoặc int)
            result["SELECT"] = ConvertToInt(pSelect.Value) == 1;
            result["INSERT"] = ConvertToInt(pInsert.Value) == 1;
            result["UPDATE"] = ConvertToInt(pUpdate.Value) == 1;
            result["DELETE"] = ConvertToInt(pDelete.Value) == 1;

            return result;
        }

        private int ConvertToInt(object oracleValue)
        {
            if (oracleValue == null || oracleValue == DBNull.Value) return 0;
            if (oracleValue is Oracle.ManagedDataAccess.Types.OracleDecimal decimalVal)
                return decimalVal.IsNull ? 0 : decimalVal.ToInt32();
            return Convert.ToInt32(oracleValue);
        }

        // ================= ROLE =================

        // 6. Lấy danh sách Roles
        public List<string> GetAllRoles()
        {
            List<string> roles = new List<string>();
            try
            {
                var parameters = new OracleParameter[] {
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
                };

                DataTable dt = DatabaseHelper.ExecuteProcedureToTable("NAM_DOAN.P_GET_ALL_ROLES", parameters);
                foreach (DataRow row in dt.Rows)
                {
                    roles.Add(row["ROLE"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách Role: " + ex.Message);
            }
            return roles;
        }

        // 7. Tạo Role
        public void CreateRole(string roleName)
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_role_name", OracleDbType.Varchar2) { Value = roleName }
            };
            DatabaseHelper.ExecuteProcedure("NAM_DOAN.P_CREATE_ROLE", parameters);
        }

        // 8. Cấp quyền cho Role
        public void GrantPermissionToRole(string role, string table, bool select, bool insert, bool update, bool delete)
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_role", OracleDbType.Varchar2) { Value = role },
                new OracleParameter("p_table", OracleDbType.Varchar2) { Value = table },
                new OracleParameter("p_select", OracleDbType.Int32) { Value = select ? 1 : 0 },
                new OracleParameter("p_insert", OracleDbType.Int32) { Value = insert ? 1 : 0 },
                new OracleParameter("p_update", OracleDbType.Int32) { Value = update ? 1 : 0 },
                new OracleParameter("p_delete", OracleDbType.Int32) { Value = delete ? 1 : 0 }
            };
            DatabaseHelper.ExecuteProcedure("NAM_DOAN.P_GRANT_PERMISSION_TO_ROLE", parameters);
        }

        // 9. Cấp Role cho User
        public void GrantRoleToUser(string roleName, string username)
        {
            var parameters = new OracleParameter[] {
                new OracleParameter("p_role_name", OracleDbType.Varchar2) { Value = roleName },
                new OracleParameter("p_username", OracleDbType.Varchar2) { Value = username }
            };
            DatabaseHelper.ExecuteProcedure("NAM_DOAN.P_GRANT_ROLE_TO_USER", parameters);
        }
    }
}
