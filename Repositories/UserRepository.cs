using DA_N6.Database;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace DA_N6.Repositories
{
    public class UserRepository
    {
        // Hàm lấy danh sách User từ bảng USERS
        public DataTable GetAllUsers()
        {
            try
            {
                // Tham số đầu ra cursor
                var outCursor = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

                // Gọi thủ tục P_GET_APP_USERS vừa tạo
                return DatabaseHelper.ExecuteProcedureToTable("NAM_DOAN.P_GET_ALL_USERS", outCursor);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách Users: " + ex.Message);
            }
        }

        public void LockOracleUser(string username)
        {
            DatabaseHelper.ExecuteProcedure("NAM_DOAN.P_LOCK_ORACLE_USER",
                new OracleParameter("p_username", OracleDbType.Varchar2) { Value = username }
            );
        }

        public void UnlockOracleUser(string username)
        {
            DatabaseHelper.ExecuteProcedure("NAM_DOAN.P_UNLOCK_ORACLE_USER",
                new OracleParameter("p_username", OracleDbType.Varchar2) { Value = username }
            );
        }

        public void DropOracleUser(string username)
        {
            DatabaseHelper.ExecuteProcedure("NAM_DOAN.P_DROP_ORACLE_USER",
                new OracleParameter("p_username", OracleDbType.Varchar2) { Value = username }
            );
        }

        public void UpdateUserStatus(int userId, int status)
        {
            try
            {
                // Gọi thủ tục P_UPDATE_USER_STATUS
                DatabaseHelper.ExecuteProcedure("NAM_DOAN.P_UPDATE_USER_STATUS",
                    new OracleParameter("p_user_id", OracleDbType.Int32) { Value = userId },
                    new OracleParameter("p_status", OracleDbType.Int32) { Value = status }
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi cập nhật trạng thái: " + ex.Message);
            }
        }

        public void DeleteUser(int userId)
        {
            try
            {
                // Gọi thủ tục P_DELETE_APP_USER
                DatabaseHelper.ExecuteProcedure("NAM_DOAN.P_DELETE_APP_USER",
                    new OracleParameter("p_user_id", OracleDbType.Int32) { Value = userId }
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xóa người dùng: " + ex.Message);
            }
        }

        // Hàm tạo Tablespace
        public void CreateTablespace(string tablespaceName, string fileName, int sizeMb)
        {
            try
            {
                DatabaseHelper.ExecuteProcedure("NAM_DOAN.P_CREATE_TABLESPACE",
                    new OracleParameter("p_tablespace_name", OracleDbType.Varchar2) { Value = tablespaceName },
                    new OracleParameter("p_file_name", OracleDbType.Varchar2) { Value = fileName },
                    new OracleParameter("p_size_mb", OracleDbType.Int32) { Value = sizeMb }
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi tạo Tablespace: " + ex.Message);
            }
        }

        // Hàm cấp Quota
        public void GrantQuota(string username, string tablespaceName, int sizeMb)
        {
            try
            {
                DatabaseHelper.ExecuteProcedure("NAM_DOAN.P_GRANT_QUOTA",
                    new OracleParameter("p_username", OracleDbType.Varchar2) { Value = username },
                    new OracleParameter("p_tablespace_name", OracleDbType.Varchar2) { Value = tablespaceName },
                    new OracleParameter("p_size_mb", OracleDbType.Int32) { Value = sizeMb }
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi cấp Quota: " + ex.Message);
            }
        }
        // =========================================================
        // FACE ID & QR CODE METHODS
        // =========================================================

        public void UpdateFaceImage(string username, byte[] faceImage)
        {
            try
            {
                DatabaseHelper.ExecuteProcedure("NAM_DOAN.AUTH_EXT_PKG.P_UPDATE_FACE_IMG",
                    new OracleParameter("p_username", OracleDbType.Varchar2) { Value = username },
                    new OracleParameter("p_img", OracleDbType.Blob) { Value = faceImage }
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi cập nhật Face Image: " + ex.Message);
            }
        }

        public void UpdateQRCode(string username, string qrCode)
        {
            try
            {
                DatabaseHelper.ExecuteProcedure("NAM_DOAN.AUTH_EXT_PKG.P_UPDATE_QR_CODE",
                    new OracleParameter("p_username", OracleDbType.Varchar2) { Value = username },
                    new OracleParameter("p_qr", OracleDbType.Varchar2) { Value = qrCode }
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi cập nhật QR Code: " + ex.Message);
            }
        }

        public string GetUserByQR(string qrCode)
        {
            try
            {
                object result = DatabaseHelper.ExecuteFunction("NAM_DOAN.AUTH_EXT_PKG.F_GET_USER_BY_QR",
                    OracleDbType.Varchar2,
                    new OracleParameter("p_qr", OracleDbType.Varchar2) { Value = qrCode }
                );
                return result?.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi tra cứu QR Code: " + ex.Message);
            }
        }

        public DataTable GetAllFaceImages()
        {
            try
            {
                var outCursor = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                return DatabaseHelper.ExecuteProcedureToTable("NAM_DOAN.AUTH_EXT_PKG.P_GET_ALL_FACES", outCursor);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách khuôn mặt: " + ex.Message);
            }
        }

        public void GetUserCredentials(string username, out string encUser, out string encPass)
        {
            try
            {
                var pUser = new OracleParameter("p_enc_user", OracleDbType.Varchar2, 4000) { Direction = ParameterDirection.Output };
                var pPass = new OracleParameter("p_enc_pass", OracleDbType.Varchar2, 4000) { Direction = ParameterDirection.Output };

                DatabaseHelper.ExecuteProcedure("NAM_DOAN.AUTH_EXT_PKG.P_GET_USER_CREDENTIALS",
                    new OracleParameter("p_username", OracleDbType.Varchar2) { Value = username },
                    pUser,
                    pPass
                );

                encUser = pUser.Value?.ToString();
                encPass = pPass.Value?.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy thông tin định danh: " + ex.Message);
            }
        }
    }
}