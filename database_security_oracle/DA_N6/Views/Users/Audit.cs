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
using DA_N6.Repositories;
using DA_N6.Utils;
using Oracle.ManagedDataAccess.Client;
using ClosedXML.Excel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DA_N6.Views.Users
{
    public partial class Audit : Form
    {
        private OracleConnection conn;
        private bool isSysUser;
        private bool isLoadingUser = false; //BIẾN CỜ ĐỂ LOAD USER

        // Lưu cấu hình audit tạm thời: BẢNG -> DANH SÁCH QUYỀN
        private Dictionary<string, List<string>> objectAuditBuffer
            = new Dictionary<string, List<string>>();


        public Audit(bool isSysUser)
        {
            this.isSysUser = isSysUser;

            InitializeComponent();
            CenterToScreen();
            conn = DataBase.Get_Connect();

           
            //LOAD BẢNG
            LoadTablesFromSP();

            load_Cbo_user(conn);
        }
       
        
        private void load_Cbo_user(OracleConnection conn)
        {
            try
            {
                using (OracleCommand command = new OracleCommand("sp_select_all_users", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    OracleParameter outParam = new OracleParameter("v_out", OracleDbType.RefCursor);
                    outParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outParam);

                    command.ExecuteNonQuery();

                    cbb_LIST_USER.Items.Clear();
                    cbb_LIST_USER.DisplayMember = "DisplayName";
                    cbb_LIST_USER.ValueMember = "EncryptedName";

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string encryptedNameDB = reader.GetString(0);
                            string originalName = "";

                            try
                            {
                                int key = EncryptUtils.GetEffectiveKey();
                                int m = 75;

                                // GỌI HÀM TÌM NGHỊCH ĐẢO TỪ EncryptUtils
                                int inverseKey = EncryptUtils.ModInverse(key, m);

                                if (inverseKey != -1)
                                {
                                    // GỌI HÀM GIẢI MÃ TỪ EncryptUtils
                                    originalName = EncryptUtils.DecryptMultiplicative(encryptedNameDB, inverseKey);
                                }
                                else
                                {
                                    originalName = $"[Mã hóa: {encryptedNameDB}]";
                                }
                            }
                            catch
                            {
                                originalName = $"[Mã hóa: {encryptedNameDB}]";
                            }

                            var userItem = new
                            {
                                DisplayName = originalName,
                                EncryptedName = encryptedNameDB
                            };

                            cbb_LIST_USER.Items.Add(userItem);
                        }
                    }

                    isLoadingUser = true;

                    if (cbb_LIST_USER.Items.Count > 0)
                        cbb_LIST_USER.SelectedIndex = 0;

                    isLoadingUser = false;

                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
        }
        private void EnableButtons(bool enable)
        {
            btnAD.Enabled = enable && isSysUser; // Chỉ enable nếu là sys user
            btnX.Enabled = enable && isSysUser;

            if (!isSysUser)
            {
                MessageBox.Show(" - Chỉ xem (không có quyền thay đổi)");
            }
        }

        private void cbb_LIST_USER_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoadingUser) return;
            if (cbb_LIST_USER.SelectedItem != null)
            {
                var selectedUser = (dynamic)cbb_LIST_USER.SelectedItem;
                string username = selectedUser.EncryptedName;
                LoadUserAuditStatus();
                LoadAuditLogsByUser(username);
                EnableButtons(true);
            }
            else
            {
                EnableButtons(false);
            }
           
        }

        private void ResetAllCheckboxes()
        {
            // Reset tất cả CheckBox về false
            // Giả sử bạn có các CheckBox với tên:
            // chk_CREATE_TABLE, chk_DELETE_TABLE, chk_INSERT_TABLE, etc.

            cbDT.Checked = false;
            cbIT.Checked = false;
            cbST.Checked = false;
            cbUT.Checked = false;
           
        }
        private void SetCheckboxByOption(string option, bool isChecked)
        {
            // Ánh xạ option sang CheckBox tương ứng
            switch (option)
            {

                case "DELETE TABLE":
                    cbDT.Checked = isChecked;        // DELETE TABLE
                    break;
                case "INSERT TABLE":
                    cbIT.Checked = isChecked;        // INSERT TABLE
                    break;
                case "SELECT TABLE":
                    cbST.Checked = isChecked;        // SELECT TABLE
                    break;
                case "UPDATE TABLE":
                    cbUT.Checked = isChecked;        // UPDATE TABLE
                    break;
               
            }
        }

        private void LoadUserAuditStatus()
        {
            if (cbb_LIST_USER.SelectedItem == null) return;

            // Lấy đối tượng user đã lưu
            var selectedUser = (dynamic)cbb_LIST_USER.SelectedItem;
            string encryptedUsername = selectedUser.EncryptedName; // Tên mã hóa
            string displayName = selectedUser.DisplayName;         // Tên gốc (chỉ để hiển thị)

            try
            {
                ResetAllCheckboxes();

                // Chỉ sys user mới có quyền xem DBA_STMT_AUDIT_OPTS
                if (isSysUser)
                {
                    using (OracleCommand cmd = new OracleCommand(
                        "SELECT AUDIT_OPTION FROM DBA_STMT_AUDIT_OPTS WHERE USER_NAME = :username", conn))
                    {
                        cmd.Parameters.Add("username", OracleDbType.Varchar2).Value = encryptedUsername; // Dùng tên mã hóa

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string option = reader.GetString(0).ToUpper();
                                SetCheckboxByOption(option, true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể kiểm tra trạng thái audit: {ex.Message}");
            }
        }


        private void btnAD_Click(object sender, EventArgs e)
        {
            if (cbb_LIST_USER.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn user");
                return;
            }

            if (objectAuditBuffer.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn bảng cần giám sát");
                return;
            }

            var selectedUser = (dynamic)cbb_LIST_USER.SelectedItem;
            string username = selectedUser.EncryptedName; // USER BỊ GIÁM SÁT

            try
            {
                foreach (var item in objectAuditBuffer)
                {
                    string tableName = item.Key;
                    string actions = string.Join(",", item.Value); // SELECT,INSERT,...

                    using (OracleCommand cmd = new OracleCommand("SP_CREATE_FGA_AUDIT", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username;
                        cmd.Parameters.Add("p_table_name", OracleDbType.Varchar2).Value = tableName;
                        cmd.Parameters.Add("p_actions", OracleDbType.Varchar2).Value = actions;
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Đã áp dụng giám sát thành công (FGA)",
                                "Thành công",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi áp dụng giám sát: " + ex.Message);
            }
        }



        private void btnX_Click(object sender, EventArgs e)
        {
            if (cbb_LIST_USER.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn user");
                return;
            }

            var selectedUser = (dynamic)cbb_LIST_USER.SelectedItem;
            string username = selectedUser.EncryptedName;

            if (clbTable.CheckedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn bảng cần xóa giám sát");
                return;
            }

            try
            {
                foreach (string tableName in clbTable.CheckedItems)
                {
                    using (OracleCommand cmd = new OracleCommand("SP_DROP_FGA_AUDIT", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username;
                        cmd.Parameters.Add("p_table_name", OracleDbType.Varchar2).Value = tableName;
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Đã xóa giám sát (FGA) thành công",
                                "Thành công",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                objectAuditBuffer.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa giám sát: " + ex.Message);
            }
        }

        private void ShowResultMessage(string action, string username, int total,
                                 int success, List<string> failedOptions)
        {
            string message = $"Đã {action.ToLower()} giám sát {success}/{total} hoạt động cho user {username}";

            if (failedOptions.Count > 0)
            {
                message += $"\n\nCác lỗi gặp phải ({failedOptions.Count}):";
                foreach (string error in failedOptions)
                {
                    message += $"\n• {error}";
                }

                MessageBox.Show(message, $"Kết quả {action}",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show(message, $"Thành công",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //HÀM LOAD BẢNG
        private void LoadTablesFromSP()
        {
            try
            {
                clbTable.Items.Clear();

                using (OracleCommand cmd = new OracleCommand("sp_select_all_tables", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_out", OracleDbType.RefCursor)
                                  .Direction = ParameterDirection.Output;

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tableName = reader.GetString(0);
                            clbTable.Items.Add(tableName, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể load bảng: " + ex.Message);
            }
        }

        //lấy quyền object đang chọn
        private string GetSelectedObjectActions()
        {
            List<string> actions = new List<string>();

            if (cbST.Checked) actions.Add("SELECT");
            if (cbIT.Checked) actions.Add("INSERT");
            if (cbUT.Checked) actions.Add("UPDATE");
            if (cbDT.Checked) actions.Add("DELETE");

            return string.Join(",", actions);
        }
        
        private void clbTable_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (cbb_LIST_USER.SelectedItem == null) return;

            if (!isSysUser)
            {
                MessageBox.Show("Bạn không có quyền thay đổi audit!");
                e.NewValue = e.CurrentValue;
                return;
            }

            string tableName = clbTable.Items[e.Index].ToString();
            string actionsStr = GetSelectedObjectActions(); // SELECT,INSERT,...

            if (string.IsNullOrEmpty(actionsStr))
            {
                MessageBox.Show("Vui lòng chọn ít nhất 1 quyền (SELECT / INSERT / UPDATE / DELETE)");
                e.NewValue = e.CurrentValue;
                return;
            }

            List<string> actions = actionsStr.Split(',').ToList();

            if (e.NewValue == CheckState.Checked)
            {
                // CHỈ LƯU – KHÔNG GỌI ORACLE
                objectAuditBuffer[tableName] = actions;
            }
            else
            {
                objectAuditBuffer.Remove(tableName);
            }
        }
        private void LoadAuditLogsByUser(string username)
        {
           
            try
            {
                using (OracleCommand cmd = new OracleCommand("SP_GET_AUDIT_LOGS_BY_USER", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor)
                                  .Direction = ParameterDirection.Output;

                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvLoadLog.DataSource = dt;

                    dgvLoadLog.AutoSizeColumnsMode =
                        DataGridViewAutoSizeColumnsMode.Fill;

                    dgvLoadLog.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể load audit log: " + ex.Message);
            }
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            if (dgvLoadLog.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
            sfd.FileName = "AuditLog.xlsx";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add("Audit Log");

                        // ===== HEADER =====
                        for (int i = 0; i < dgvLoadLog.Columns.Count; i++)
                        {
                            ws.Cell(1, i + 1).Value = dgvLoadLog.Columns[i].HeaderText;
                            ws.Cell(1, i + 1).Style.Font.Bold = true;
                        }

                        // ===== DATA =====
                        for (int i = 0; i < dgvLoadLog.Rows.Count; i++)
                        {
                            for (int j = 0; j < dgvLoadLog.Columns.Count; j++)
                            {
                                ws.Cell(i + 2, j + 1).Value =
                                    dgvLoadLog.Rows[i].Cells[j].Value?.ToString();
                            }
                        }

                        ws.Columns().AdjustToContents();
                        wb.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Xuất Excel thành công!",
                                    "Thành công",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xuất Excel: " + ex.Message);
                }
            }
        }
    }
}
