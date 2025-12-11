using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DA_N6.Repositories;

namespace DA_N6.Views.Authorization
{
    public partial class AuthorizationForm : Form
    {
        private readonly AuthorizationRepository _repo;

        public AuthorizationForm()
        {
            InitializeComponent();
            _repo = new AuthorizationRepository();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Load Users
                var users = _repo.GetAllUsers();
                cboUsers.DataSource = new List<string>(users);
                cboAssignUsers.DataSource = new List<string>(users);

                // Load Tables
                var tables = _repo.GetAllTables();
                cboTables.DataSource = new List<string>(tables);
                cboRoleTables.DataSource = new List<string>(tables);

                // Load Roles
                LoadRoles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
            }
        }

        private void LoadRoles()
        {
            try
            {
                var roles = _repo.GetAllRoles();
                cboRoles.DataSource = new List<string>(roles);
                cboAssignRoles.DataSource = new List<string>(roles);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load role: " + ex.Message);
            }
        }

        private void cboUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckUserPermissions();
        }

        private void cboTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckUserPermissions();
        }

        private void CheckUserPermissions()
        {
            if (cboUsers.SelectedItem == null || cboTables.SelectedItem == null) return;

            string user = cboUsers.SelectedItem.ToString();
            string table = cboTables.SelectedItem.ToString();

            try
            {
                var perms = _repo.CheckPermission(user, table);
                chkSelect.Checked = perms["SELECT"];
                chkInsert.Checked = perms["INSERT"];
                chkUpdate.Checked = perms["UPDATE"];
                chkDelete.Checked = perms["DELETE"];
            }
            catch (Exception ex)
            {
                
                ResetCheckboxes();
            }
        }

        private void ResetCheckboxes()
        {
            chkSelect.Checked = false;
            chkInsert.Checked = false;
            chkUpdate.Checked = false;
            chkDelete.Checked = false;
        }

        private void btnGrant_Click(object sender, EventArgs e)
        {
            if (cboUsers.SelectedItem == null || cboTables.SelectedItem == null) return;
            string user = cboUsers.SelectedItem.ToString();
            string table = cboTables.SelectedItem.ToString();

            try
            {
                _repo.GrantPermission(user, table, chkSelect.Checked, chkInsert.Checked, chkUpdate.Checked, chkDelete.Checked);
                MessageBox.Show($"Cấp quyền thành công cho {user} trên bảng {table}");
                CheckUserPermissions(); // Refresh UI
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cấp quyền: " + ex.Message);
            }
        }

        private void btnRevoke_Click(object sender, EventArgs e)
        {
            if (cboUsers.SelectedItem == null || cboTables.SelectedItem == null) return;
            string user = cboUsers.SelectedItem.ToString();
            string table = cboTables.SelectedItem.ToString();

            try
            {
                // Sửa logic: Revoke những cái đang được chọn
                _repo.RevokePermission(user, table, chkSelect.Checked, chkInsert.Checked, chkUpdate.Checked, chkDelete.Checked);
                
                MessageBox.Show($"Thu hồi quyền thành công của {user} trên bảng {table}");
                CheckUserPermissions(); // Refresh UI
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thu hồi quyền: " + ex.Message);
            }
        }

        // ================= ROLE =================

        private void btnCreateRole_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRoleName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên Role");
                return;
            }

            try
            {
                _repo.CreateRole(txtRoleName.Text.Trim());
                MessageBox.Show("Tạo Role thành công!");
                LoadRoles(); // Reload list
                txtRoleName.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo role: " + ex.Message);
            }
        }

        private void btnGrantRolePerm_Click(object sender, EventArgs e)
        {
            if (cboRoles.SelectedItem == null || cboRoleTables.SelectedItem == null) return;
            string role = cboRoles.SelectedItem.ToString();
            string table = cboRoleTables.SelectedItem.ToString();

            try
            {
                _repo.GrantPermissionToRole(role, table, chkRoleSelect.Checked, chkRoleInsert.Checked, chkRoleUpdate.Checked, chkRoleDelete.Checked);
                MessageBox.Show($"Cấp quyền thành công cho Role {role} trên bảng {table}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cấp quyền Role: " + ex.Message);
            }
        }

        private void btnAssignRole_Click(object sender, EventArgs e)

        {
            if (cboAssignUsers.SelectedItem == null || cboAssignRoles.SelectedItem == null) return;
            string user = cboAssignUsers.SelectedItem.ToString();
            string role = cboAssignRoles.SelectedItem.ToString();

            try
            {
                _repo.GrantRoleToUser(role, user);
                MessageBox.Show($"Đã gán Role {role} cho User {user}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gán Role: " + ex.Message);
            }
        }
    }
}
