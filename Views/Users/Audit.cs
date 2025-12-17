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

namespace DA_N6.Views.Users
{
    public partial class Audit : Form
    {
        private OracleConnection conn;
        private bool isSysUser;

        public Audit(bool isSysUser)
        {
            this.isSysUser = isSysUser;

            InitializeComponent();
            CenterToScreen();
            conn = DataBase.Get_Connect();

            SetupEventHandlers();
            //LOAD BẢNG
            LoadTablesFromSP();

            load_Cbo_user(conn);
        }
        private void SetupEventHandlers()
        {
            // Sự kiện khi chọn user
            cbb_LIST_USER.SelectedIndexChanged += cbb_LIST_USER_SelectedIndexChanged;

            // Nút Áp dụng
            btnAD.Click += btnAD_Click;

            // Nút Xóa
            btnX.Click += btnX_Click;

            //CHỌN CÁC BẢNG
            clbTable.ItemCheck += clbTable_ItemCheck;



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

                    if (cbb_LIST_USER.Items.Count > 0)
                        cbb_LIST_USER.SelectedIndex = 0;
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
            if(cbb_LIST_USER.SelectedItem != null)
            {
                LoadUserAuditStatus();
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

            cbCT.Checked = false;
            cbDT.Checked = false;
            cbIT.Checked = false;
            cbST.Checked = false;
            cbUT.Checked = false;
            cbDropAT.Checked = false;
            cbCAT.Checked = false;
            cbDAT.Checked = false;
            cbIAT.Checked = false;
            cbSAT.Checked = false;
            cbUAT.Checked = false;
        }
        private void SetCheckboxByOption(string option, bool isChecked)
        {
            // Ánh xạ option sang CheckBox tương ứng
            switch (option)
            {
                case "CREATE TABLE":
                    cbCT.Checked = isChecked;        // CREATE TABLE
                    break;
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
                case "DROP ANY TABLE":
                    cbDropAT.Checked = isChecked;    // DROP ANY TABLE
                    break;
                case "CREATE ANY TABLE":
                    cbCAT.Checked = isChecked;       // CREATE ANY TABLE
                    break;
                case "DELETE ANY TABLE":
                    cbDAT.Checked = isChecked;       // DELETE ANY TABLE
                    break;
                case "INSERT ANY TABLE":
                    cbIAT.Checked = isChecked;       // INSERT ANY TABLE
                    break;
                case "SELECT ANY TABLE":
                    cbSAT.Checked = isChecked;       // SELECT ANY TABLE
                    break;
                case "UPDATE ANY TABLE":
                    cbUAT.Checked = isChecked;       // UPDATE ANY TABLE
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

        private List<string> GetSelectedOptions()
        {
            List<string> selected = new List<string>();

            // Lấy các option đang được check theo tên viết tắt
            if (cbCT.Checked) selected.Add("CREATE TABLE");          // CREATE TABLE
            if (cbDT.Checked) selected.Add("DELETE TABLE");          // DELETE TABLE
            if (cbIT.Checked) selected.Add("INSERT TABLE");          // INSERT TABLE
            if (cbST.Checked) selected.Add("SELECT TABLE");          // SELECT TABLE
            if (cbUT.Checked) selected.Add("UPDATE TABLE");          // UPDATE TABLE
            if (cbDropAT.Checked) selected.Add("DROP ANY TABLE");    // DROP ANY TABLE
            if (cbCAT.Checked) selected.Add("CREATE ANY TABLE");     // CREATE ANY TABLE
            if (cbDAT.Checked) selected.Add("DELETE ANY TABLE");     // DELETE ANY TABLE
            if (cbIAT.Checked) selected.Add("INSERT ANY TABLE");     // INSERT ANY TABLE
            if (cbSAT.Checked) selected.Add("SELECT ANY TABLE");     // SELECT ANY TABLE
            if (cbUAT.Checked) selected.Add("UPDATE ANY TABLE");     // UPDATE ANY TABLE

            return selected;
        }
        private void btnAD_Click(object sender, EventArgs e)
        {
            if (cbb_LIST_USER.SelectedItem == null) return;

            // Lấy đối tượng user đã lưu
            var selectedUser = (dynamic)cbb_LIST_USER.SelectedItem;
            string encryptedUsername = selectedUser.EncryptedName; // Tên mã hóa để gửi xuống DB
            string displayName = selectedUser.DisplayName; // Tên hiển thị


            string username = selectedUser.EncryptedName;  // DÙNG TÊN MÃ HÓA
            List<string> selectedOptions = GetSelectedOptions();

            if (selectedOptions.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một hoạt động để giám sát!");
                return;
            }

            // Kiểm tra quyền
            if (!isSysUser)
            {
                MessageBox.Show("Chỉ user có quyền ADMIN/SYS mới có thể thiết lập giám sát!");
                return;
            }

            try
            {
                int successCount = 0;
                List<string> failedOptions = new List<string>();

                foreach (string option in selectedOptions)
                {
                    try
                    {
                        // GỌI STORED PROCEDURE ORACLE: pro_create_audit
                        using (OracleCommand cmd = new OracleCommand("sp_create_audit", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Thêm parameter 1: p_statement
                            OracleParameter p1 = new OracleParameter("p_statement", OracleDbType.Varchar2);
                            p1.Value = option;
                            cmd.Parameters.Add(p1);

                            // Thêm parameter 2: p_username
                            OracleParameter p2 = new OracleParameter("p_username", OracleDbType.Varchar2);
                            p2.Value = username;
                            cmd.Parameters.Add(p2);

                            // Thực thi stored procedure
                            cmd.ExecuteNonQuery();
                            successCount++;

                            Console.WriteLine($"Đã gọi pro_create_audit('{option}', '{username}')");
                        }
                    }
                    catch (OracleException oraEx)
                    {
                        // Log lỗi Oracle
                        string errorMsg = $"Lỗi Oracle {oraEx.Number}: {oraEx.Message}";
                        failedOptions.Add($"{option} - {errorMsg}");
                        Console.WriteLine(errorMsg);
                    }
                }

                // Hiển thị kết quả
                ShowResultMessage("Áp dụng", username, selectedOptions.Count, successCount, failedOptions);

                // Refresh trạng thái audit
                LoadUserAuditStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi áp dụng audit: {ex.Message}", "Lỗi",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnX_Click(object sender, EventArgs e)
        {
            if (cbb_LIST_USER.SelectedItem == null) return;

            // Lấy đối tượng user đã lưu
            var selectedUser = (dynamic)cbb_LIST_USER.SelectedItem;
            string encryptedUsername = selectedUser.EncryptedName; // Tên mã hóa để gửi xuống DB
            string displayName = selectedUser.DisplayName; // Tên hiển thị

            string username = selectedUser.EncryptedName;  // DÙNG TÊN MÃ HÓA
            List<string> selectedOptions = GetSelectedOptions();

            if (selectedOptions.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một hoạt động để xóa giám sát!");
                return;
            }

            // Kiểm tra quyền
            if (!isSysUser)
            {
                MessageBox.Show("Chỉ user có quyền ADMIN/SYS mới có thể xóa giám sát!");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa giám sát {selectedOptions.Count} hoạt động cho user {username}?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                int successCount = 0;
                List<string> failedOptions = new List<string>();

                foreach (string option in selectedOptions)
                {
                    try
                    {
                        // GỌI STORED PROCEDURE ORACLE: pro_drop_audit
                        using (OracleCommand cmd = new OracleCommand("sp_drop_audit", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Thêm parameter 1: p_statement
                            OracleParameter p1 = new OracleParameter("p_statement", OracleDbType.Varchar2);
                            p1.Value = option;
                            cmd.Parameters.Add(p1);

                            // Thêm parameter 2: p_username
                            OracleParameter p2 = new OracleParameter("p_username", OracleDbType.Varchar2);
                            p2.Value = username;
                            cmd.Parameters.Add(p2);

                            // Thực thi stored procedure
                            cmd.ExecuteNonQuery();
                            successCount++;

                            Console.WriteLine($"Đã gọi sp_drop_audit('{option}', '{username}')");
                        }
                    }
                    catch (OracleException oraEx)
                    {
                        // Log lỗi Oracle
                        string errorMsg = $"Lỗi Oracle {oraEx.Number}: {oraEx.Message}";
                        failedOptions.Add($"{option} - {errorMsg}");
                        Console.WriteLine(errorMsg);
                    }
                }

                // Hiển thị kết quả
                ShowResultMessage("Xóa", username, selectedOptions.Count, successCount, failedOptions);

                // Refresh trạng thái audit
                LoadUserAuditStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa audit: {ex.Message}", "Lỗi",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            var selectedUser = (dynamic)cbb_LIST_USER.SelectedItem;
            string username = selectedUser.EncryptedName;

            string tableName = clbTable.Items[e.Index].ToString();
            string actions = GetSelectedObjectActions();

            if (string.IsNullOrEmpty(actions))
            {
                MessageBox.Show("Vui lòng chọn ít nhất 1 quyền (SELECT / INSERT / UPDATE / DELETE)");
                e.NewValue = e.CurrentValue;
                return;
            }

            try
            {
                if (e.NewValue == CheckState.Checked)
                {
                    using (OracleCommand cmd = new OracleCommand("SP_CREATE_OBJECT_AUDIT", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("p_table_name", OracleDbType.Varchar2).Value = tableName;
                        cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username;
                        cmd.Parameters.Add("p_actions", OracleDbType.Varchar2).Value = actions;
                        cmd.ExecuteNonQuery();
                    }
                }
                else if (e.NewValue == CheckState.Unchecked)
                {
                    using (OracleCommand cmd = new OracleCommand("SP_DROP_OBJECT_AUDIT", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("p_table_name", OracleDbType.Varchar2).Value = tableName;
                        cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username;
                        cmd.Parameters.Add("p_actions", OracleDbType.Varchar2).Value = actions;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show($"Lỗi Oracle: {ex.Message}");
                e.NewValue = e.CurrentValue;
            }
        }

    }
}
