using DA_N6.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DA_N6.Database;
using DA_N6.Utils;
using Oracle.ManagedDataAccess.Client;
using System.Drawing;

namespace DA_N6.Views.Users
{
    public partial class UsersForm : Form
    {
        private readonly bool _isSysUser;
        private readonly UserRepository _userRepository;

        public UsersForm(bool isSysUser)
        {
            InitializeComponent();

            _isSysUser = isSysUser;
            _userRepository = new UserRepository();

            this.Load += UserForm_Load;

            this.dgvUsers.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(this.dgvUsers_DataBindingComplete);
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            LoadUserList();
        }

        // LOCK, UNLOCK, DELETE USER
        // LOAD DANH SÁCH USER
        private void LoadUserList()
        {
            try
            {
                DataTable dtUsers = _userRepository.GetAllUsers();
                dgvUsers.DataSource = dtUsers;

                // Ẩn cột ID và IS_LOCKED (không cần hiện số 0/1 cho người dùng thấy)
                if (dgvUsers.Columns["USER_ID"] != null) dgvUsers.Columns["USER_ID"].Visible = false;
                if (dgvUsers.Columns["IS_LOCKED"] != null) dgvUsers.Columns["IS_LOCKED"].Visible = false;

                // Đặt tên tiếng Việt
                if (dgvUsers.Columns["USER_NAME"] != null) dgvUsers.Columns["USER_NAME"].HeaderText = "Tên đăng nhập";
                if (dgvUsers.Columns["EMAIL"] != null) dgvUsers.Columns["EMAIL"].HeaderText = "Email";
                if (dgvUsers.Columns["ADDRESS"] != null) dgvUsers.Columns["ADDRESS"].HeaderText = "Địa chỉ";

                // --- TÔ MÀU USER BỊ KHÓA ---
                foreach (DataGridViewRow row in dgvUsers.Rows)
                {
                    // Kiểm tra giá trị cột IS_LOCKED
                    if (row.Cells["IS_LOCKED"].Value != DBNull.Value)
                    {
                        int isLocked = Convert.ToInt32(row.Cells["IS_LOCKED"].Value);
                        if (isLocked == 1)
                        {
                            // User bị khóa: Chữ đỏ, nền xám nhạt
                            row.DefaultCellStyle.ForeColor = Color.Red;
                            row.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
                            row.DefaultCellStyle.SelectionForeColor = Color.Red;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        // 2. Chức năng KHÓA (Lock) - Gọi khi bấm btnLock
        private void btnLock_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có dòng nào được chọn không
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn user cần khóa!", "Thông báo");
                return;
            }

            // Lấy thông tin dòng đang chọn
            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["USER_ID"].Value);
            string username = dgvUsers.SelectedRows[0].Cells["USER_NAME"].Value.ToString();

            if (MessageBox.Show($"Bạn có chắc muốn KHÓA tài khoản '{username}' không?",
                                "Xác nhận khóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _userRepository.UpdateUserStatus(userId, 1); // 1 = Khóa
                    MessageBox.Show("Đã khóa tài khoản thành công!");
                    LoadUserList(); // Tải lại danh sách để cập nhật màu sắc
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // 3. Chức năng MỞ KHÓA (Unlock) - Gọi khi bấm btnUnlock
        private void btnUnlock_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn user cần mở khóa!", "Thông báo");
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["USER_ID"].Value);
            string username = dgvUsers.SelectedRows[0].Cells["USER_NAME"].Value.ToString();

            if (MessageBox.Show($"Bạn có chắc muốn MỞ KHÓA cho tài khoản '{username}'?",
                                "Xác nhận mở khóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _userRepository.UpdateUserStatus(userId, 0); // 0 = Mở
                    MessageBox.Show("Đã mở khóa tài khoản thành công!");
                    LoadUserList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // 4. Chức năng XÓA (Delete) - Gọi khi bấm btnDrop
        private void btnDrop_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn user cần xóa!", "Thông báo");
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["USER_ID"].Value);
            string username = dgvUsers.SelectedRows[0].Cells["USER_NAME"].Value.ToString();

            // Cảnh báo nghiêm trọng hơn
            if (MessageBox.Show($"CẢNH BÁO: Bạn sắp xóa vĩnh viễn user '{username}'.\nHành động này không thể hoàn tác.\nBạn có chắc chắn không?",
                                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    _userRepository.DeleteUser(userId);
                    MessageBox.Show("Đã xóa user thành công!");
                    LoadUserList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể xóa user này. Có thể user đang liên kết với dữ liệu khác.\nLỗi: " + ex.Message);
                }
            }
        }

        private void dgvUsers_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvUsers.Rows)
            {
                // Kiểm tra giá trị cột IS_LOCKED
                if (row.Cells["IS_LOCKED"].Value != null && row.Cells["IS_LOCKED"].Value != DBNull.Value)
                {
                    int isLocked = Convert.ToInt32(row.Cells["IS_LOCKED"].Value);
                    if (isLocked == 1)
                    {
                        // User bị khóa: Chữ đỏ, nền xám nhạt
                        row.DefaultCellStyle.ForeColor = Color.Red;
                        row.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
                    }
                }
            }
        }

        // TABLE SPACE VÀ QUOTA
        private void btnCreateTablespace_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTbName.Text) || string.IsNullOrWhiteSpace(txtDataFile.Text))
            {
                MessageBox.Show("Vui lòng nhập tên Tablespace và tên File data!", "Thiếu thông tin");
                return;
            }

            try
            {
                _userRepository.CreateTablespace(
                    txtTbName.Text.Trim(),
                    txtDataFile.Text.Trim(),
                    (int)numTbSize.Value
                );
                MessageBox.Show("Tạo Tablespace thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Tự động điền tên tablespace vừa tạo sang khung cấp Quota cho tiện
                txtQuotaTbName.Text = txtTbName.Text.Trim();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi tạo Tablespace", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGrantQuota_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra đầu vào
            if (string.IsNullOrWhiteSpace(txtQuotaUsername.Text) || string.IsNullOrWhiteSpace(txtQuotaTbName.Text))
            {
                MessageBox.Show("Vui lòng nhập Username và tên Tablespace!", "Thiếu thông tin");
                return;
            }

            string rawUsername = txtQuotaUsername.Text.Trim();
            string tablespaceName = txtQuotaTbName.Text.Trim();
            int quotaSize = (int)numQuotaSize.Value;

            try
            {

                // --- BƯỚC 1: Mã hóa Tầng 1 (Tại App - Key 7) ---
                string encryptedUserLv1 = EncryptUtils.EncryptMultiplicative(rawUsername, 7);

                // --- BƯỚC 2: Mã hóa Tầng 2 (Tại DB - Key 9) ---
                // Gọi hàm Oracle F_ENCRYPT_DB thông qua DatabaseHelper
                object encUserLv2Obj = DatabaseHelper.ExecuteFunction(
                    "NAM_DOAN.ENCRYPT_PKG.F_ENCRYPT_DB",
                    OracleDbType.NVarchar2,
                    new OracleParameter("p_input", OracleDbType.NVarchar2, encryptedUserLv1, ParameterDirection.Input),
                    new OracleParameter("p_key", OracleDbType.Int32, 9, ParameterDirection.Input)
                );

                string finalEncryptedUsername = encUserLv2Obj?.ToString();

                // Kiểm tra an toàn
                if (string.IsNullOrEmpty(finalEncryptedUsername))
                {
                    MessageBox.Show("Lỗi: Không thể mã hóa tên người dùng!", "Lỗi Mã hóa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _userRepository.GrantQuota(
                    finalEncryptedUsername, // Truyền chuỗi đã mã hóa xuống DB
                    tablespaceName,
                    quotaSize
                );

                // Thông báo thành công (Hiển thị tên thật cho dễ đọc)
                MessageBox.Show($"Đã cấp Quota thành công cho user {rawUsername}!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Gợi ý lỗi thường gặp
                if (ex.Message.Contains("ORA-01918"))
                {
                    MessageBox.Show($"User '{rawUsername}' (Mã hóa: {txtQuotaUsername.Text}) không tồn tại trong Oracle!", "Lỗi User", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi cấp Quota", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

