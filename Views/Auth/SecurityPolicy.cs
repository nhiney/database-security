using DA_N6.Repositories;
using System;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DA_N6.views
{
    public partial class SecurityPolicyForm : Form
    {
        private ProfileRepository _repo;

        public SecurityPolicyForm()
        {
            InitializeComponent();
            _repo = new ProfileRepository();
            LoadPolicies();
        }

        private void LoadPolicies()
        {
            try
            {
                dgvPolicies.DataSource = _repo.GetSecurityPolicies();

                // Format cột cho đẹp
                dgvPolicies.Columns["PROFILE"].Visible = false; // Ẩn tên profile đi cho gọn
                dgvPolicies.Columns["RESOURCE_NAME"].HeaderText = "Tên Chính Sách";
                dgvPolicies.Columns["LIMIT"].HeaderText = "Giá Trị Hiện Tại";
                dgvPolicies.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Sự kiện khi click vào dòng trong Grid 
        private void dgvPolicies_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtName.Text = dgvPolicies.Rows[e.RowIndex].Cells["RESOURCE_NAME"].Value.ToString();
                txtValue.Text = dgvPolicies.Rows[e.RowIndex].Cells["LIMIT"].Value.ToString();
            }
        }

        // Sự kiện nút Lưu
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtValue.Text)) return;

            try
            {
                // Gọi Repository để update
                _repo.UpdateProfileLimit(txtName.Text, txtValue.Text.Trim().ToUpper());

                MessageBox.Show($"Đã cập nhật {txtName.Text} thành {txtValue.Text} thành công!");
                LoadPolicies(); // Load lại để thấy thay đổi
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message + "\n(Giá trị nhập vào có thể không hợp lệ)");
            }
        }
    }
}