using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace DA_N6.Database
{
    public static class DataBase
    {
        public static string Host { get; private set; }
        public static string Port { get; private set; }
        public static string Sid { get; private set; }
        public static string Username { get; private set; }
        public static string Password { get; private set; }

        private static OracleConnection currentConnection;

        public static void Set_DataBase(string host, string port, string sid, string user, string pass)
        {
            Host = host;
            Port = port;
            Sid = sid;
            Username = user;
            Password = pass;
        }

        public static OracleConnection Get_Connect()
        {
            string connString;

            if (Username.ToUpper().Equals("SYS"))
            {
                connString =
                    $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={Host})(PORT={Port}))" +
                    $"(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={Sid})));" +
                    $"User ID={Username};Password={Password};DBA Privilege=SYSDBA;";
            }
            else
            {
                connString =
                    $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={Host})(PORT={Port}))" +
                    $"(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={Sid})));" +
                    $"User ID=\"{Username}\";Password={Password};";
            }

            var conn = new OracleConnection(connString);
            conn.Open();
            return conn;
        }

        /// Kiểm tra kết nối (nếu keepAlive = true thì giữ kết nối toàn cục)
        public static bool Connect(bool keepAlive = false)
        {
            try
            {
                var conn = Get_Connect();
                if (keepAlive)
                    currentConnection = conn;
                else
                    conn.Dispose();

                return true;
            }
            catch (OracleException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// Đóng kết nối
        public static void Disconnect()
        {
            try
            {
                if (currentConnection != null && currentConnection.State != ConnectionState.Closed)
                {
                    currentConnection.Close();
                    currentConnection.Dispose();
                    currentConnection = null;
                    Console.WriteLine("Đã ngắt kết nối hiện tại.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi đóng kết nối: " + ex.Message);
            }
        }


        /// Lấy kết nối hiện tại
        public static OracleConnection GetCurrentConnection()
        {
            return currentConnection;
        }
    }
}
