using System;
using System.Drawing;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using DA_N6.Database;
using DA_N6.Utils;

namespace DA_N6
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
            CenterToScreen();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string newUser = txtUsername.Text.Trim();
            string newPass = txtPassword.Text.Trim();
            string email = txtEmail.Text.Trim();
            string address = txtAddress.Text.Trim();

            if (string.IsNullOrEmpty(newUser) || string.IsNullOrEmpty(newPass))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên tài khoản và mật khẩu!");
                return;
            }

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập email!");
                return;
            }

            // Mã hóa tầng 1 (client)
            string encryptedUserLv1 = EncryptUtils.EncryptMultiplicative(newUser, 7);
            string encryptedPassLv1 = EncryptUtils.EncryptMultiplicative(newPass, 7);

            try
            {
                // ---- DÙNG CHUẨN USER NHƯ ĐOẠN 1 ----
                DataBase.Set_DataBase("localhost", "1521", "PDBORCL", "NAM_DOAN", "NAM_DOAN");

                if (!DataBase.Connect())
                {
                    MessageBox.Show("Không thể kết nối tới user NAM_DOAN!");
                    return;
                }

                // ---- Gọi đúng package + đúng user ----
                DatabaseHelper.ExecuteProcedure(
                    "NAM_DOAN.ENCRYPT_PKG.P_REGISTER_USER",
                    new OracleParameter("p_username", OracleDbType.Varchar2, encryptedUserLv1, System.Data.ParameterDirection.Input),
                    new OracleParameter("p_password", OracleDbType.Varchar2, encryptedPassLv1, System.Data.ParameterDirection.Input),
                    new OracleParameter("p_email", OracleDbType.Varchar2, email, System.Data.ParameterDirection.Input),
                    new OracleParameter("p_addr", OracleDbType.NVarchar2, address, System.Data.ParameterDirection.Input)
                );

                MessageBox.Show($"Đăng ký tài khoản {newUser} thành công!\nBạn có thể đăng nhập ngay bây giờ.");

                DataBase.Disconnect();

                this.Hide();
                new Login().ShowDialog();
                this.Close();
            }
            catch (OracleException ex)
            {
                string msg = ex.Message;
                if (msg.Contains("ORA-20012"))
                    MessageBox.Show("Tên người dùng đã tồn tại!");
                else if (msg.Contains("ORA-20010"))
                    MessageBox.Show("Lỗi khi tạo tài khoản trong Oracle!");
                else
                    MessageBox.Show("Lỗi Oracle: " + msg);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đăng ký: " + ex.Message);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Login().ShowDialog();
            this.Close();
        }
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PanelCard_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            int radius = 20;
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(panel.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(panel.Width - radius, panel.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, panel.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            panel.Region = new Region(path);
            
            // Optional: Draw border
            using (Pen pen = new Pen(Color.FromArgb(230, 230, 230), 1))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        // ===================================
        // PLACEHOLDER LOGIC
        // ===================================

        // Username
        private void txtUsername_Enter(object sender, EventArgs e)
        {
            if (txtUsername.Text == "Nhập tên đăng nhập")
            {
                txtUsername.Text = "";
                txtUsername.ForeColor = Color.Black;
            }
        }
        private void txtUsername_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Text = "Nhập tên đăng nhập";
                txtUsername.ForeColor = Color.Gray;
            }
        }

        // Password
        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Nhập mật khẩu")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black;
                txtPassword.UseSystemPasswordChar = true;
            }
        }
        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = "Nhập mật khẩu";
                txtPassword.ForeColor = Color.Gray;
                txtPassword.UseSystemPasswordChar = false;
            }
        }

        // Email
        private void txtEmail_Enter(object sender, EventArgs e)
        {
            if (txtEmail.Text == "Nhập email")
            {
                txtEmail.Text = "";
                txtEmail.ForeColor = Color.Black;
            }
        }
        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtEmail.Text = "Nhập email";
                txtEmail.ForeColor = Color.Gray;
            }
        }

        // Address
        private void txtAddress_Enter(object sender, EventArgs e)
        {
            if (txtAddress.Text == "Nhập địa chỉ")
            {
                txtAddress.Text = "";
                txtAddress.ForeColor = Color.Black;
            }
        }
        private void txtAddress_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                txtAddress.Text = "Nhập địa chỉ";
                txtAddress.ForeColor = Color.Gray;
            }
        }
    }
}
