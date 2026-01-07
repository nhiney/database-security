using DA_N6.Database;
using DA_N6.Utils;
using DA_N6.views.devices;
using DA_N6.views.orders;
using DA_N6.views.products;
using System;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using DA_N6.Views.Users;

namespace DA_N6
{
    public partial class Main : Form
    {
        private int _userId;
        private bool _isSysUser;
        private string _sid;
        private string _serial;
        private string _username;
        private Timer _sessionTimer;

        public Main(int userId, bool isSysUser, string sid, string serial, string username)
        {
            InitializeComponent();
            _userId = userId;
            _isSysUser = isSysUser;
            _sid = sid;       // Lưu SID của phiên hiện tại
            _serial = serial; // Lưu Serial của phiên hiện tại
            _username = username;

            lblUsername.Text = $"Xin chào: {_username}";

            InitializeSessionTimer();
        }

        private void InitializeSessionTimer()
        {
            _sessionTimer = new Timer();
            _sessionTimer.Interval = 5000; // Kiểm tra mỗi 5 giây
            _sessionTimer.Tick += SessionTimer_Tick;
            _sessionTimer.Start();
        }

        private void SessionTimer_Tick(object sender, EventArgs e)
        {
            CheckSessionStatus();
        }

        private void CheckSessionStatus()
        {
            if (_isSysUser)
            {
                return;
            }

            try
            {
                // Gọi hàm F_IS_SESSION_ACTIVE 
                object result = DatabaseHelper.ExecuteFunction(
                    "NAM_DOAN.F_IS_SESSION_ACTIVE",
                    OracleDbType.Decimal,
                    new OracleParameter("p_sid", OracleDbType.Varchar2) { Value = _sid },
                    new OracleParameter("p_serial", OracleDbType.Varchar2) { Value = _serial }
                );

                int isAlive = 0;

                // SỬA LỖI CASTING TẠI ĐÂY
                if (result != null)
                {
                    // Kiểm tra nếu kết quả là kiểu OracleDecimal (đặc thù của Oracle)
                    if (result is OracleDecimal oracleDecimal)
                    {
                        // Chuyển đổi an toàn từ OracleDecimal sang int
                        isAlive = oracleDecimal.IsNull ? 0 : oracleDecimal.ToInt32();
                    }
                    else
                    {
                        // Trường hợp trả về số thường
                        isAlive = Convert.ToInt32(result);
                    }
                }

                // Nếu kết quả = 0 (hoặc null) nghĩa là Session không tồn tại -> Logout
                if (isAlive == 0)
                {
                    PerformForceLogout("Tài khoản của bạn đã bị đăng xuất từ thiết bị khác.");
                }
            }
            catch (Exception ex)
            {
                // Bắt các lỗi liên quan đến việc Session bị Kill (mất kết nối đột ngột)
                string msg = ex.Message.ToLower();
                if (msg.Contains("ora-00028") ||
                    msg.Contains("session has been killed") ||
                    msg.Contains("end of file") ||
                    msg.Contains("not connected"))
                {
                    PerformForceLogout("Kết nối tới máy chủ đã bị ngắt.");
                }
                else
                {
                    // Log lỗi khác để debug (như lỗi casting cũ)
                    Console.WriteLine($"Lỗi check session: {ex.Message}");
                }
            }
        }

        // Hàm xử lý văng Form về màn hình Login
        private void PerformForceLogout(string reason)
        {
            _sessionTimer.Stop(); // Dừng timer

            MessageBox.Show(reason,
                            "Cảnh báo bảo mật",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

            // Ngắt kết nối phía Client
            try { DataBase.Disconnect(); } catch { }

            // Ẩn Form Main, hiện lại Login
            this.Hide();
            Login loginForm = new Login();
            loginForm.ShowDialog();
            this.Close();
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ProductsForm(_userId, _isSysUser));
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            OpenChildForm(new OrdersForm(_userId, _username, _isSysUser));
        }

        private void btnDevices_Click(object sender, EventArgs e)
        {
            OpenChildForm(new DevicesForm());
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra quyền TRƯỚC khi tạo form
            if (!_isSysUser) // Nếu không phải admin
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này.\nChỉ quản trị viên (SYS) mới được phép.",
                                "Cảnh báo bảo mật",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return; // Dừng lại, không làm gì nữa (ở lại Main)
            }

            // 2. Nếu là Admin thì mới mở form
            OpenChildForm(new UsersForm(_isSysUser));
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            // Mở form VerifyForm 
            OpenChildForm(new DA_N6.views.VerifyForm());
        }

        private void btnAuthorization_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra quyền TRƯỚC khi tạo form
            if (!_isSysUser) // Nếu không phải admin
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này.\nChỉ quản trị viên (SYS) mới được phép.",
                                "Cảnh báo bảo mật",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }
            // 2. Mở form
            OpenChildForm(new DA_N6.Views.Authorization.AuthorizationForm());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                _sessionTimer.Stop();

                // Gọi thủ tục DB để xóa session
                DatabaseHelper.ExecuteProcedure(
                    "NAM_DOAN.P_LOGOUT_CURRENT_DEVICE",
                    new OracleParameter("p_username", OracleDbType.Varchar2) { Value = _username }
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đăng xuất: " + ex.Message);
            }

            try { DataBase.Disconnect(); } catch { }

            Hide();
            new Login().ShowDialog();
            Close();
        }

        private void btnPolicy_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra quyền Admin
            if (!_isSysUser)
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này.\nChỉ quản trị viên mới được phép thay đổi chính sách bảo mật.",
                                "Cảnh báo bảo mật",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return; // Dừng lại, không mở form
            }

            // 2. Nếu là Admin thì mở form
            OpenChildForm(new DA_N6.views.SecurityPolicyForm());
        }

        private void btnAudit_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra quyền Admin
            if (!_isSysUser)
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này.\nChỉ quản trị viên (SYS) mới được phép.",
                                "Cảnh báo bảo mật",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            // 2. Mở form Audit
            OpenChildForm(new DA_N6.Views.Users.Audit(_isSysUser));
        }

        // Hàm hỗ trợ mở form con 
        private void OpenChildForm(Form childForm)
        {
            if (this.panelContent.Controls.Count > 0)
                this.panelContent.Controls.RemoveAt(0);

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.panelContent.Controls.Add(childForm);
            this.panelContent.Tag = childForm;
            childForm.Show();
        }
        


    }
}