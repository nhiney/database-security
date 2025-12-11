using DA_N6.Repositories;
using System;
using System.Windows.Forms;

namespace DA_N6.views.orders
{
    public partial class OrdersForm : Form
    {
        private int _userId;
        private string _userName;
        private bool _isSysUser;

        public OrdersForm(int userId, string userName, bool isSysUser)
        {
            InitializeComponent();
            _userId = userId;
            _userName = userName;
            LoadOrders();
            _isSysUser = isSysUser;
        }

        private bool IsAdmin()
        {
            string u = _userName.ToUpper();
            return (u == "SYS" || u == "NAM_DOAN");
        }

        private void LoadOrders()
        {
            try
            {
                OrderRepository orderRepo = new OrderRepository();

                if (IsAdmin())
                    dgvOrders.DataSource = orderRepo.GetAllOrders();
                else
                    dgvOrders.DataSource = orderRepo.GetOrdersByUser(_userId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải đơn hàng: " + ex.Message);
            }
        }

        private void dgvOrders_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int orderId = Convert.ToInt32(dgvOrders.Rows[e.RowIndex].Cells["ORDER_ID"].Value);

            OrderDetailForm detail = new OrderDetailForm(orderId, _isSysUser);
            detail.ShowDialog();
        }
    }
}
