using DA_N6.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace DA_N6.Repositories
{
    public class OrderDetailRepository
    {
        // Lấy chi tiết đơn hàng
        public DataTable GetOrderDetails(int orderId)
        {
            return DatabaseHelper.ExecuteFunctionToTable(
                "NAM_DOAN.F_GET_ORDER_DETAILS",
                new OracleParameter("result", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.ReturnValue
                },
                new OracleParameter("p_order_id", OracleDbType.Int32)
                {
                    Value = orderId,
                    Direction = ParameterDirection.Input
                }
            );
        }

        public void UpdateSignature(int orderId, string signatureData)
        {
            try
            {
                DatabaseHelper.ExecuteProcedure("NAM_DOAN.P_UPDATE_ORDER_SIGNATURE",
                    new OracleParameter("p_order_id", OracleDbType.Int32) { Value = orderId },
                    new OracleParameter("p_signature_data", OracleDbType.Clob) { Value = signatureData }
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lưu chữ ký vào DB: " + ex.Message);
            }
        }
    }
}
