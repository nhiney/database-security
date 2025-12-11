using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace DA_N6.Repositories
{
    public class ProfileRepository
    {
        private readonly string _connectionString;

        public ProfileRepository()
        {
            _connectionString = $"User Id=NAM_DOAN;Password=NAM_DOAN;" +
                              $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))" +
                              $"(CONNECT_DATA=(SERVICE_NAME=FREEPDB1)))";
        }

        /// <summary>
        /// Lay danh sach cac chinh sach bao mat (Security Policies) tu DEFAULT profile
        /// </summary>
        public DataTable GetSecurityPolicies()
        {
            DataTable dt = new DataTable();

            using (OracleConnection conn = new OracleConnection(_connectionString))
            {
                try
                {
                    conn.Open();

                    // Query lay cac resource limit tu DBA_PROFILES
                    string sql = @"
                        SELECT PROFILE, RESOURCE_NAME, LIMIT 
                        FROM DBA_PROFILES 
                        WHERE PROFILE = 'DEFAULT' 
                          AND RESOURCE_TYPE = 'PASSWORD'
                        ORDER BY RESOURCE_NAME";

                    using (OracleDataAdapter adapter = new OracleDataAdapter(sql, conn))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Loi khi lay security policies: " + ex.Message);
                }
            }

            return dt;
        }

        /// <summary>
        /// Cap nhat gia tri gioi han cho mot resource cu the trong profile
        /// </summary>
        /// <param name="resourceName">Ten resource (VD: FAILED_LOGIN_ATTEMPTS)</param>
        /// <param name="newLimit">Gia tri moi (VD: UNLIMITED, 3, 10)</param>
        public void UpdateProfileLimit(string resourceName, string newLimit)
        {
            using (OracleConnection conn = new OracleConnection(_connectionString))
            {
                try
                {
                    conn.Open();

                    // Tao cau lenh ALTER PROFILE
                    string sql = $"ALTER PROFILE DEFAULT LIMIT {resourceName} {newLimit}";

                    using (OracleCommand cmd = new OracleCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (OracleException ex)
                {
                    throw new Exception($"Loi Oracle khi cap nhat profile: {ex.Message}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Loi khi cap nhat {resourceName}: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Lay thong tin profile cua mot user cu the
        /// </summary>
        public string GetUserProfile(string username)
        {
            string profile = string.Empty;

            using (OracleConnection conn = new OracleConnection(_connectionString))
            {
                try
                {
                    conn.Open();

                    string sql = "SELECT PROFILE FROM DBA_USERS WHERE USERNAME = :username";

                    using (OracleCommand cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add("username", OracleDbType.Varchar2).Value = username.ToUpper();
                        
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            profile = result.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Loi khi lay profile cua user: " + ex.Message);
                }
            }

            return profile;
        }
    }
}
