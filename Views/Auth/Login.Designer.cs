using System;
using System.Drawing;
using System.Windows.Forms;

namespace DA_N6
{
    partial class Login
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelCard = new Panel();
            this.lblTitle = new Label();
            this.btnClose = new Label();
            
            // Username Panel
            this.panelUser = new Panel();
            this.lblUserIcon = new Label();
            this.txtUsername = new TextBox();
            
            // Password Panel
            this.panelPass = new Panel();
            this.lblPassIcon = new Label();
            this.txtPassword = new TextBox();
            
            this.chkShowPass = new CheckBox();
            this.btnLogin = new Button();
            
            this.linkForgot = new LinkLabel();
            this.linkRegister = new LinkLabel();
            
            // Separator
            this.lblSeparator = new Label();
            
            // Optional Login Buttons
            this.btnFaceLogin = new Button();
            this.btnQRLogin = new Button();
            
            // Camera Panel
            this.panelCamera = new Panel();
            this.btnBack = new Button();
            this.lblStatus = new Label();
            this.picCamera = new PictureBox();
            this.btnCapture = new Button(); 

            // Form Setup
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(240, 242, 245); 
            this.ClientSize = new Size(600, 850); // Increased Size
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Login";

            // 
            // panelCard
            // 
            this.panelCard.Size = new Size(500, 700); // Increased Size
            this.panelCard.Location = new Point(50, 75);
            this.panelCard.BackColor = Color.White;
            this.panelCard.Paint += new PaintEventHandler(this.PanelCard_Paint);
            this.Controls.Add(this.panelCard);

            // 
            // btnClose (X)
            // 
            this.btnClose.Text = "✕";
            this.btnClose.Font = new Font("Segoe UI", 14F, FontStyle.Bold); // Larger Font
            this.btnClose.ForeColor = Color.Silver;
            this.btnClose.AutoSize = true;
            this.btnClose.Location = new Point(460, 10); // Adjusted Location
            this.btnClose.Cursor = Cursors.Hand;
            this.btnClose.Click += new EventHandler(this.BtnClose_Click);
            this.panelCard.Controls.Add(this.btnClose);

            // 
            // lblTitle
            // 
            this.lblTitle.Text = "Đăng Nhập";
            this.lblTitle.Font = new Font("Segoe UI", 28F, FontStyle.Bold); // Larger Font
            this.lblTitle.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblTitle.AutoSize = false;
            this.lblTitle.Size = new Size(500, 70); // Adjusted Size
            this.lblTitle.Location = new Point(0, 40);
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.panelCard.Controls.Add(this.lblTitle);
            
            // 
            // panelUser
            // 
            this.panelUser.Size = new Size(400, 55); // Larger Panel
            this.panelUser.Location = new Point(50, 140);
            this.panelUser.BackColor = Color.White;
            this.panelUser.BorderStyle = BorderStyle.FixedSingle;
            this.panelCard.Controls.Add(this.panelUser);

            this.lblUserIcon.ForeColor = Color.Gray;
            this.lblUserIcon.AutoSize = true;
            this.lblUserIcon.Location = new Point(10, 15);
            this.panelUser.Controls.Add(this.lblUserIcon);

            this.txtUsername.BorderStyle = BorderStyle.None;
            this.txtUsername.Font = new Font("Segoe UI", 14F); // Larger Font
            this.txtUsername.Location = new Point(40, 12);
            this.txtUsername.Size = new Size(340, 32);
            this.txtUsername.Text = "Nhập tên đăng nhập";
            this.txtUsername.ForeColor = Color.Gray;
            this.txtUsername.Enter += new EventHandler(this.txtUsername_Enter);
            this.txtUsername.Leave += new EventHandler(this.txtUsername_Leave);
            this.panelUser.Controls.Add(this.txtUsername);

            // 
            // panelPass
            // 
            this.panelPass.Size = new Size(400, 55); // Larger Panel
            this.panelPass.Location = new Point(50, 215);
            this.panelPass.BackColor = Color.White;
            this.panelPass.BorderStyle = BorderStyle.FixedSingle;
            this.panelCard.Controls.Add(this.panelPass);

            this.lblPassIcon.ForeColor = Color.Gray;
            this.lblPassIcon.AutoSize = true;
            this.lblPassIcon.Location = new Point(10, 15);
            this.panelPass.Controls.Add(this.lblPassIcon);

            this.txtPassword.BorderStyle = BorderStyle.None;
            this.txtPassword.Font = new Font("Segoe UI", 14F); // Larger Font
            this.txtPassword.Location = new Point(40, 12);
            this.txtPassword.Size = new Size(340, 32);
            this.txtPassword.Text = "Nhập mật khẩu";
            this.txtPassword.ForeColor = Color.Gray;
            this.txtPassword.UseSystemPasswordChar = false;
            this.txtPassword.Enter += new EventHandler(this.txtPassword_Enter);
            this.txtPassword.Leave += new EventHandler(this.txtPassword_Leave);
            this.panelPass.Controls.Add(this.txtPassword);

            // 
            // chkShowPass
            // 
            this.chkShowPass.Text = "Hiện mật khẩu";
            this.chkShowPass.Font = new Font("Segoe UI", 10F); // Larger Font
            this.chkShowPass.ForeColor = Color.FromArgb(64, 64, 64);
            this.chkShowPass.AutoSize = true;
            this.chkShowPass.Location = new Point(50, 285);
            this.chkShowPass.Cursor = Cursors.Hand;
            this.chkShowPass.CheckedChanged += new EventHandler(this.chkShowPass_CheckedChanged);
            this.panelCard.Controls.Add(this.chkShowPass);

            // 
            // btnLogin
            // 
            this.btnLogin.Text = "Đăng Nhập";
            this.btnLogin.Font = new Font("Segoe UI", 14F, FontStyle.Bold); // Larger Font
            this.btnLogin.BackColor = Color.FromArgb(91, 155, 213);
            this.btnLogin.ForeColor = Color.White;
            this.btnLogin.FlatStyle = FlatStyle.Flat;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.Size = new Size(400, 50); // Larger Button
            this.btnLogin.Location = new Point(50, 330);
            this.btnLogin.Cursor = Cursors.Hand;
            this.btnLogin.Click += new EventHandler(this.btn_login_Click);
            this.panelCard.Controls.Add(this.btnLogin);

            // 
            // linkForgot
            // 
            this.linkForgot.Text = "Quên mật khẩu?";
            this.linkForgot.Font = new Font("Segoe UI", 10F); // Larger Font
            this.linkForgot.LinkColor = Color.FromArgb(91, 155, 213);
            this.linkForgot.ActiveLinkColor = Color.FromArgb(0, 122, 255);
            this.linkForgot.AutoSize = true;
            this.linkForgot.Location = new Point(50, 395);
            this.linkForgot.Cursor = Cursors.Hand;
            this.panelCard.Controls.Add(this.linkForgot);

            // 
            // linkRegister
            // 
            this.linkRegister.Text = "Chưa có tài khoản? Đăng ký";
            this.linkRegister.Font = new Font("Segoe UI", 10F); // Larger Font
            this.linkRegister.LinkColor = Color.FromArgb(91, 155, 213);
            this.linkRegister.ActiveLinkColor = Color.FromArgb(0, 122, 255);
            this.linkRegister.AutoSize = true;
            this.linkRegister.Location = new Point(200, 395);
            this.linkRegister.Cursor = Cursors.Hand;
            this.linkRegister.LinkClicked += new LinkLabelLinkClickedEventHandler(this.LinkRegister_LinkClicked);
            this.panelCard.Controls.Add(this.linkRegister);

            // 
            // lblSeparator
            // 
            this.lblSeparator.Text = "---- Hoặc ----";
            this.lblSeparator.Font = new Font("Segoe UI", 11F); // Larger Font
            this.lblSeparator.ForeColor = Color.Gray;
            this.lblSeparator.AutoSize = false;
            this.lblSeparator.Size = new Size(500, 30);
            this.lblSeparator.Location = new Point(0, 435);
            this.lblSeparator.TextAlign = ContentAlignment.MiddleCenter;
            this.panelCard.Controls.Add(this.lblSeparator);

            // 
            // btnFaceLogin
            // 
            this.btnFaceLogin.Text = "😃 Xác thực khuôn mặt";
            this.btnFaceLogin.Font = new Font("Segoe UI", 11F); // Larger Font
            this.btnFaceLogin.BackColor = Color.White;
            this.btnFaceLogin.ForeColor = Color.FromArgb(64, 64, 64);
            this.btnFaceLogin.FlatStyle = FlatStyle.Flat;
            this.btnFaceLogin.FlatAppearance.BorderColor = Color.LightGray;
            this.btnFaceLogin.Size = new Size(400, 45); // Larger Button
            this.btnFaceLogin.Location = new Point(50, 480);
            this.btnFaceLogin.Cursor = Cursors.Hand;
            this.btnFaceLogin.Click += new EventHandler(this.btnFaceLogin_Click);
            this.panelCard.Controls.Add(this.btnFaceLogin);

            // 
            // btnQRLogin
            // 
            this.btnQRLogin.Text = "📷 Quét mã QR";
            this.btnQRLogin.Font = new Font("Segoe UI", 11F); // Larger Font
            this.btnQRLogin.BackColor = Color.White;
            this.btnQRLogin.ForeColor = Color.FromArgb(64, 64, 64);
            this.btnQRLogin.FlatStyle = FlatStyle.Flat;
            this.btnQRLogin.FlatAppearance.BorderColor = Color.LightGray;
            this.btnQRLogin.Size = new Size(400, 45); // Larger Button
            this.btnQRLogin.Location = new Point(50, 540);
            this.btnQRLogin.Cursor = Cursors.Hand;
            this.btnQRLogin.Click += new EventHandler(this.btnQRLogin_Click);
            this.panelCard.Controls.Add(this.btnQRLogin);

            // ============================================
            // CAMERA OVERLAY
            // ============================================
            this.panelCamera.Dock = DockStyle.Fill;
            this.panelCamera.Visible = false;
            this.panelCamera.BackColor = Color.FromArgb(240, 240, 240);
            
            this.btnBack.Text = "← Quay lại";
            this.btnBack.BackColor = Color.White;
            this.btnBack.ForeColor = Color.FromArgb(52, 73, 94);
            this.btnBack.FlatStyle = FlatStyle.Flat;
            this.btnBack.FlatAppearance.BorderColor = Color.FromArgb(220, 223, 230);
            this.btnBack.Size = new Size(120, 40); // Larger
            this.btnBack.Location = new Point(30, 30);
            this.btnBack.Cursor = Cursors.Hand;
            this.btnBack.Click += new EventHandler(this.btnBack_Click);

            this.lblStatus.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblStatus.Font = new Font("Segoe UI", 12F); // Larger
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new Point(160, 35);
            this.lblStatus.Text = "Khởi động camera...";
            
            this.picCamera.Location = new Point(0, 90);
            this.picCamera.Size = new Size(600, 600); // 600 width for form
            this.picCamera.SizeMode = PictureBoxSizeMode.CenterImage;
            this.picCamera.BackColor = Color.Black;

            this.btnCapture.Text = "Chụp";
            this.btnCapture.BackColor = Color.FromArgb(70, 150, 220);
            this.btnCapture.ForeColor = Color.White;
            this.btnCapture.FlatStyle = FlatStyle.Flat;
            this.btnCapture.FlatAppearance.BorderSize = 0;
            this.btnCapture.Font = new Font("Segoe UI", 14F, FontStyle.Bold); // Larger
            this.btnCapture.Size = new Size(180, 50); // Larger
            this.btnCapture.Location = new Point(210, 720);
            this.btnCapture.Cursor = Cursors.Hand;
            this.btnCapture.Click += new EventHandler(this.btnCapture_Click);

            this.panelCamera.Controls.Add(this.btnBack);
            this.panelCamera.Controls.Add(this.lblStatus);
            this.panelCamera.Controls.Add(this.picCamera);
            this.panelCamera.Controls.Add(this.btnCapture);

            this.Controls.Add(this.panelCamera); 
            
            this.ResumeLayout(false);
        }

        #endregion

        private Panel panelCard;
        private Label lblTitle;
        private Label btnClose;
        
        private Panel panelUser;
        private Label lblUserIcon;
        private TextBox txtUsername;
        
        private Panel panelPass;
        private Label lblPassIcon;
        private TextBox txtPassword;
        
        private CheckBox chkShowPass;
        private Button btnLogin;
        private LinkLabel linkForgot;
        private LinkLabel linkRegister;
        
        private Label lblSeparator;
        private Button btnFaceLogin;
        private Button btnQRLogin;
        
        private Panel panelCamera;
        private Button btnBack;
        private Label lblStatus;
        private PictureBox picCamera;
        private Button btnCapture;
    }
}
