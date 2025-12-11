using DA_N6.Database;
using DA_N6.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace DA_N6.Repositories
{
    public class ProductRepository
    {
        public DataTable GetAll()
        {
            return DatabaseHelper.ExecuteProcedureToTable(
                "NAM_DOAN.P_GET_ALL_PRODUCTS",
                new OracleParameter("p_rc", OracleDbType.RefCursor, ParameterDirection.Output)
            );
        }

        public void Insert(Product p)
        {
            DatabaseHelper.ExecuteProcedure(
                "NAM_DOAN.P_INSERT_PRODUCT",
                new OracleParameter("p_name", p.ProductName),
                new OracleParameter("p_category_id", p.CategoryId),
                new OracleParameter("p_quantity", p.Quantity),
                new OracleParameter("p_price", p.Price)
            );
        }

        public void Update(Product p)
        {
            DatabaseHelper.ExecuteProcedure(
                "NAM_DOAN.P_UPDATE_PRODUCT",
                new OracleParameter("p_id", p.ProductId),
                new OracleParameter("p_name", p.ProductName),
                new OracleParameter("p_category_id", p.CategoryId),
                new OracleParameter("p_quantity", p.Quantity),
                new OracleParameter("p_price", p.Price)
            );
        }

        public void Delete(int id)
        {
            DatabaseHelper.ExecuteProcedure(
                "NAM_DOAN.P_DELETE_PRODUCT",
                new OracleParameter("p_id", id)
            );
        }
    }
}
