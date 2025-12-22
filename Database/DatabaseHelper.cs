using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace DA_N6.Database
{
    public static class DatabaseHelper
    {
        /// Lấy kết nối hiện tại
        private static OracleConnection GetActiveConnection()
        {
            var conn = DataBase.GetCurrentConnection();
            if (conn == null || conn.State != ConnectionState.Open)
                conn = DataBase.Get_Connect(); // fallback nếu chưa có
            return conn;
        }

        /// Gọi thủ tục không trả dữ liệu
        public static void ExecuteProcedure(string procedureName, params OracleParameter[] parameters)
        {
            using (var cmd = new OracleCommand(procedureName, GetActiveConnection()))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi khi gọi thủ tục {procedureName}: {ex.Message}", ex);
                }
            }
        }

        /// Gọi thủ tục có trả về DataTable
        public static DataTable ExecuteProcedureToTable(string procedureName, params OracleParameter[] parameters)
        {
            var conn = GetActiveConnection();
            using (var cmd = new OracleCommand(procedureName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                try
                {
                    cmd.ExecuteNonQuery();

                    // Find RefCursor parameter
                    OracleParameter cursorParam = null;
                    foreach (OracleParameter param in cmd.Parameters)
                    {
                        if (param.OracleDbType == OracleDbType.RefCursor && param.Direction == ParameterDirection.Output)
                        {
                            cursorParam = param;
                            break;
                        }
                    }

                    if (cursorParam == null)
                        throw new Exception("No RefCursor output parameter found");

                    // Read data from RefCursor
                    DataTable dt = new DataTable();
                    using (OracleDataReader reader = ((OracleRefCursor)cursorParam.Value).GetDataReader())
                    {
                        dt.Load(reader);
                    }
                    
                    return dt;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi khi gọi thủ tục {procedureName}: {ex.Message}", ex);
                }
            }
        }

        /// Gọi FUNCTION trả về REF CURSOR, trả về DataTable
        public static DataTable ExecuteFunctionToTable(string functionName, params OracleParameter[] parameters)
        {
            using (var cmd = new OracleCommand(functionName, GetActiveConnection()))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Thêm các parameter (bao gồm RETURN VALUE)
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                using (var adapter = new OracleDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();

                    try
                    {
                        adapter.Fill(dt);
                        return dt;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Lỗi khi gọi FUNCTION {functionName}: {ex.Message}", ex);
                    }
                }
            }
        }

        /// Gọi hàm có RETURN VALUE
        public static object ExecuteFunction(string functionName, OracleDbType returnType, params OracleParameter[] parameters)
        {
            using (var cmd = new OracleCommand(functionName, GetActiveConnection()))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var returnParam = new OracleParameter("result", returnType, ParameterDirection.ReturnValue);

                if (returnType == OracleDbType.Varchar2 || returnType == OracleDbType.NVarchar2)
                    returnParam.Size = 4000;
                else if (returnType == OracleDbType.Clob)
                    returnParam.Size = int.MaxValue;

                cmd.Parameters.Add(returnParam);

                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                try
                {
                    cmd.ExecuteNonQuery();
                    return cmd.Parameters["result"].Value;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi khi gọi hàm {functionName}: {ex.Message}", ex);
                }
            }
        }

        /// Thực thi SQL trực tiếp
        public static DataTable ExecuteQuery(string sql, params OracleParameter[] parameters)
        {
            using (var cmd = new OracleCommand(sql, GetActiveConnection()))
            {
                cmd.CommandType = CommandType.Text;
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                using (var adapter = new OracleDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    try
                    {
                        adapter.Fill(dt);
                        return dt;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Lỗi khi chạy truy vấn: {ex.Message}", ex);
                    }
                }
            }
        }
    }
}
