using System;
using System.Windows.Forms;
using DA_N6.Database;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace DA_N6.views.devices
{
    public partial class DevicesForm : Form
    {
        public DevicesForm()
        {
            InitializeComponent();
            LoadDevices();
        }

        private void LoadDevices()
        {
            dgvDevices.DataSource = DatabaseHelper.ExecuteProcedureToTable(
                "NAM_DOAN.P_LIST_USER_DEVICES",
                new OracleParameter("p_rc", OracleDbType.RefCursor, ParameterDirection.Output)
            );
        }

        private void btnLogoutSelected_Click(object sender, EventArgs e)
        {
            if (dgvDevices.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn thiết bị!");
                return;
            }

            string device = dgvDevices.SelectedRows[0].Cells["DEVICE_NAME"].Value.ToString();

            DatabaseHelper.ExecuteProcedure(
                "NAM_DOAN.P_LOGOUT_SELECTED_DEVICE",
                new OracleParameter("p_username", OracleDbType.Varchar2, DataBase.Username, ParameterDirection.Input),
                new OracleParameter("p_machine", OracleDbType.Varchar2, device, ParameterDirection.Input)
            );

            MessageBox.Show("Thiết bị đã đăng xuất!");
            LoadDevices();
        }

        private void btnLogoutAll_Click(object sender, EventArgs e)
        {
            DatabaseHelper.ExecuteProcedure(
                "NAM_DOAN.P_LOGOUT_ALL_DEVICE",
                new OracleParameter("p_username", OracleDbType.Varchar2, DataBase.Username, ParameterDirection.Input)
            );

            MessageBox.Show("Đã đăng xuất toàn bộ thiết bị!");
            LoadDevices();

            try
            {
                DataBase.Disconnect();
            }
            catch { }

            // Đóng form và quay lại Login
            Hide();
            new Login().ShowDialog();
            Close();
        }

        private void btnLogoutSelected_Click_1(object sender, EventArgs e)
        {

        }
    }
}
