using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using DA_N6.Database;

namespace DA_N6.Repositories
{
    public class OrderRepository
    {
        // Lấy tất cả đơn hàng
        public DataTable GetAllOrders()
        {
            return DatabaseHelper.ExecuteFunctionToTable(
                "NAM_DOAN.F_GET_ALL_ORDERS",
                new OracleParameter("result", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.ReturnValue
                }
            );
        }

        // Lấy đơn theo USER_ID
        public DataTable GetOrdersByUser(int userId)
        {
            return DatabaseHelper.ExecuteFunctionToTable(
                "NAM_DOAN.F_GET_ORDERS_BY_USER",
                new OracleParameter("result", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.ReturnValue
                },
                new OracleParameter("p_user_id", OracleDbType.Int32)
                {
                    Value = userId,
                    Direction = ParameterDirection.Input
                }
            );
        }

       
    }
}
