using DA_N6.Database;
using DA_N6.Models;
using DA_N6.views.orders; 
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DA_N6.views.products
{
    public partial class ProductsForm : Form
    {
        private int _currentUserId;
        private bool _isSysUser;

        public ProductsForm(int userId, bool isSysUser)
        {
            InitializeComponent();
            _currentUserId = userId;
            _isSysUser = isSysUser;

            ApplyPermission();
            LoadData();
        }

        private void ApplyPermission()
        {
            if (_isSysUser)
            {
                // Admin: Hiện CRUD, Ẩn Mua hàng
                btnAdd.Visible = true;
                btnUpdate.Visible = true;
                btnDelete.Visible = true;

                btnAddToCart.Visible = false;
                btnViewCart.Visible = false;

                txtName.ReadOnly = false;
                txtCategory.ReadOnly = false;
                txtQuantity.ReadOnly = false;
                txtPrice.ReadOnly = false;

                this.Text = "Quản lý Sản phẩm (Admin)";
            }
            else
            {
                // User: Ẩn CRUD, Hiện Mua hàng
                btnAdd.Visible = false;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;

                btnAddToCart.Visible = true;
                btnViewCart.Visible = true;

                txtName.ReadOnly = true;
                txtCategory.ReadOnly = true;
                txtQuantity.ReadOnly = true;
                txtPrice.ReadOnly = true;

                this.Text = "Danh sách Sản phẩm - Mua sắm";
            }
        }

        // LOAD DATA 
        private void LoadData()
        {
            try
            {
                // Gọi thủ tục P_GET_ALL_PRODUCTS
                var outCursor = new OracleParameter("p_rc", OracleDbType.RefCursor, ParameterDirection.Output);

                DataTable dt = DatabaseHelper.ExecuteProcedureToTable("NAM_DOAN.P_GET_ALL_PRODUCTS", outCursor);
                dgv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        // XỬ LÝ CLICK GRID
        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv.Rows[e.RowIndex];

                if (row.Cells["PRODUCTID"].Value != null)
                    txtId.Text = row.Cells["PRODUCTID"].Value.ToString();

                if (row.Cells["PRODUCTNAME"].Value != null)
                    txtName.Text = row.Cells["PRODUCTNAME"].Value.ToString();

                if (row.Cells["CATEGORYID"].Value != null)
                    txtCategory.Text = row.Cells["CATEGORYID"].Value.ToString();

                if (row.Cells["QUANTITY"].Value != null)
                    txtQuantity.Text = row.Cells["QUANTITY"].Value.ToString();

                if (row.Cells["PRICE"].Value != null)
                    txtPrice.Text = row.Cells["PRICE"].Value.ToString();
            }
        }

        // =========================================================================
        // CHỨC NĂNG USER (GIỎ HÀNG)
        // =========================================================================
        private void BtnAddToCart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm!");
                return;
            }

            int id = int.Parse(txtId.Text);
            string name = txtName.Text;
            decimal price = decimal.Parse(txtPrice.Text);
            int stock = int.Parse(txtQuantity.Text);

            if (stock <= 0)
            {
                MessageBox.Show("Sản phẩm này đã hết hàng!", "Hết hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ShoppingCart.AddToCart(id, name, price, 1);
            MessageBox.Show($"Đã thêm '{name}' vào giỏ hàng!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnViewCart_Click(object sender, EventArgs e)
        {
            CartForm cartForm = new CartForm(_currentUserId);
            cartForm.ShowDialog();
            LoadData(); // Load lại data sau khi đóng giỏ hàng
        }

        // =========================================================================
        // CHỨC NĂNG ADMIN (GỌI THỦ TỤC THÊM/SỬA/XÓA)
        // =========================================================================

        // THÊM SẢN PHẨM
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text)) return;

            try
            {
                // Gọi P_INSERT_PRODUCT
                DatabaseHelper.ExecuteProcedure("NAM_DOAN.P_INSERT_PRODUCT",
                    new OracleParameter("p_name", OracleDbType.NVarchar2) { Value = txtName.Text },
                    new OracleParameter("p_category_id", OracleDbType.Int32) { Value = int.Parse(txtCategory.Text) }, // Lưu ý: Nhập số
                    new OracleParameter("p_quantity", OracleDbType.Int32) { Value = int.Parse(txtQuantity.Text) },
                    new OracleParameter("p_price", OracleDbType.Decimal) { Value = decimal.Parse(txtPrice.Text) }
                );

                MessageBox.Show("Thêm thành công!");
                LoadData();
                ClearInputs();
            }
            catch (FormatException)
            {
                MessageBox.Show("Vui lòng nhập Category ID, Số lượng và Giá là dạng số!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // SỬA SẢN PHẨM
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text)) return;

            try
            {
                // Gọi P_UPDATE_PRODUCT
                DatabaseHelper.ExecuteProcedure("NAM_DOAN.P_UPDATE_PRODUCT",
                    new OracleParameter("p_id", OracleDbType.Int32) { Value = int.Parse(txtId.Text) },
                    new OracleParameter("p_name", OracleDbType.NVarchar2) { Value = txtName.Text },
                    new OracleParameter("p_category_id", OracleDbType.Int32) { Value = int.Parse(txtCategory.Text) },
                    new OracleParameter("p_quantity", OracleDbType.Int32) { Value = int.Parse(txtQuantity.Text) },
                    new OracleParameter("p_price", OracleDbType.Decimal) { Value = decimal.Parse(txtPrice.Text) }
                );

                MessageBox.Show("Cập nhật thành công!");
                LoadData();
                ClearInputs();
            }
            catch (FormatException)
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng số!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // XÓA SẢN PHẨM
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text)) return;

            if (MessageBox.Show("Bạn chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    // Gọi P_DELETE_PRODUCT
                    DatabaseHelper.ExecuteProcedure("NAM_DOAN.P_DELETE_PRODUCT",
                        new OracleParameter("p_id", OracleDbType.Int32) { Value = int.Parse(txtId.Text) }
                    );

                    MessageBox.Show("Xóa thành công!");
                    LoadData();
                    ClearInputs();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void ClearInputs()
        {
            txtId.Clear();
            txtName.Clear();
            txtCategory.Clear();
            txtQuantity.Clear();
            txtPrice.Clear();
        }
    }
}