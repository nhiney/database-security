using System;
using System.Drawing;
using System.Windows.Forms;

namespace DA_N6
{
    partial class Register
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
            this.btnClose = new Label(); 
            this.lblTitle = new Label();
            
            // Username
            this.panelUser = new Panel();
            this.lblUserIcon = new Label();
            this.txtUsername = new TextBox();
            
            // Password
            this.panelPass = new Panel();
            this.lblPassIcon = new Label();
            this.txtPassword = new TextBox();
            
            // Email
            this.panelEmail = new Panel();
            this.lblEmailIcon = new Label();
            this.txtEmail = new TextBox();
            
            // Address
            this.panelAddr = new Panel();
            this.lblAddrIcon = new Label();
            this.txtAddress = new TextBox();
            
            this.btnRegister = new Button();
            this.linkLogin = new LinkLabel();

            // Form Setup
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(240, 242, 245); 
            this.ClientSize = new Size(600, 800); // Increased Size
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Register";

            // 
            // panelCard
            // 
            this.panelCard.Size = new Size(500, 700); // Increased Size
            this.panelCard.Location = new Point(50, 50);
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
            this.btnClose.Location = new Point(460, 10);
            this.btnClose.Cursor = Cursors.Hand;
            this.btnClose.Click += new EventHandler(this.BtnClose_Click);
            this.panelCard.Controls.Add(this.btnClose);

            // 
            // lblTitle
            // 
            this.lblTitle.Text = "Tạo Tài Khoản";
            this.lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold); // Larger Font
            this.lblTitle.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblTitle.AutoSize = false;
            this.lblTitle.Size = new Size(500, 60);
            this.lblTitle.Location = new Point(0, 40);
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.panelCard.Controls.Add(this.lblTitle);

            // 
            // panelUser
            // 
            this.panelUser.Size = new Size(400, 55); // Larger Panel
            this.panelUser.Location = new Point(50, 130);
            this.panelUser.BackColor = Color.White;
            this.panelUser.BorderStyle = BorderStyle.FixedSingle; 
            this.panelCard.Controls.Add(this.panelUser);

            // lblUserIcon - Remove Text as per Login form changes
            this.lblUserIcon.Text = ""; 
            this.lblUserIcon.Font = new Font("Segoe UI", 12F);
            this.lblUserIcon.ForeColor = Color.Gray;
            this.lblUserIcon.AutoSize = true;
            this.lblUserIcon.Location = new Point(5, 15);
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
            this.panelPass.Location = new Point(50, 210);
            this.panelPass.BackColor = Color.White;
            this.panelPass.BorderStyle = BorderStyle.FixedSingle;
            this.panelCard.Controls.Add(this.panelPass);

            // lblPassIcon - Remove Text
            this.lblPassIcon.Text = "";
            this.lblPassIcon.Font = new Font("Segoe UI", 12F);
            this.lblPassIcon.ForeColor = Color.Gray;
            this.lblPassIcon.AutoSize = true;
            this.lblPassIcon.Location = new Point(5, 15);
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
            // panelEmail
            // 
            this.panelEmail.Size = new Size(400, 55); // Larger Panel
            this.panelEmail.Location = new Point(50, 290);
            this.panelEmail.BackColor = Color.White;
            this.panelEmail.BorderStyle = BorderStyle.FixedSingle;
            this.panelCard.Controls.Add(this.panelEmail);

            // lblEmailIcon - Remove Text (Consistency)
            this.lblEmailIcon.Text = "";
            this.lblEmailIcon.Font = new Font("Segoe UI", 12F);
            this.lblEmailIcon.ForeColor = Color.Gray;
            this.lblEmailIcon.AutoSize = true;
            this.lblEmailIcon.Location = new Point(5, 15);
            this.panelEmail.Controls.Add(this.lblEmailIcon);

            this.txtEmail.BorderStyle = BorderStyle.None;
            this.txtEmail.Font = new Font("Segoe UI", 14F); // Larger Font
            this.txtEmail.Location = new Point(40, 12);
            this.txtEmail.Size = new Size(340, 32);
            this.txtEmail.Text = "Nhập email";
            this.txtEmail.ForeColor = Color.Gray;
            this.txtEmail.Enter += new EventHandler(this.txtEmail_Enter);
            this.txtEmail.Leave += new EventHandler(this.txtEmail_Leave);
            this.panelEmail.Controls.Add(this.txtEmail);

            // 
            // panelAddr
            // 
            this.panelAddr.Size = new Size(400, 55); // Larger Panel
            this.panelAddr.Location = new Point(50, 370);
            this.panelAddr.BackColor = Color.White;
            this.panelAddr.BorderStyle = BorderStyle.FixedSingle;
            this.panelCard.Controls.Add(this.panelAddr);

            // lblAddrIcon - Remove Text (Consistency)
            this.lblAddrIcon.Text = "";
            this.lblAddrIcon.Font = new Font("Segoe UI", 12F);
            this.lblAddrIcon.ForeColor = Color.Gray;
            this.lblAddrIcon.AutoSize = true;
            this.lblAddrIcon.Location = new Point(5, 15);
            this.panelAddr.Controls.Add(this.lblAddrIcon);

            this.txtAddress.BorderStyle = BorderStyle.None;
            this.txtAddress.Font = new Font("Segoe UI", 14F); // Larger Font
            this.txtAddress.Location = new Point(40, 12);
            this.txtAddress.Size = new Size(340, 32);
            this.txtAddress.Text = "Nhập địa chỉ";
            this.txtAddress.ForeColor = Color.Gray;
            this.txtAddress.Enter += new EventHandler(this.txtAddress_Enter);
            this.txtAddress.Leave += new EventHandler(this.txtAddress_Leave);
            this.panelAddr.Controls.Add(this.txtAddress);

            // 
            // grpAdvanced (New)
            // 
            this.grpAdvanced = new GroupBox();
            this.grpAdvanced.Text = "Xác thực nâng cao (Tùy chọn)";
            this.grpAdvanced.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.grpAdvanced.Location = new Point(50, 450);
            this.grpAdvanced.Size = new Size(400, 200);
            this.panelCard.Controls.Add(this.grpAdvanced);

            // Face Area
            this.picFace = new PictureBox();
            this.picFace.BorderStyle = BorderStyle.FixedSingle;
            this.picFace.Location = new Point(20, 30);
            this.picFace.Size = new Size(120, 120);
            this.picFace.SizeMode = PictureBoxSizeMode.Zoom;
            this.grpAdvanced.Controls.Add(this.picFace);

            this.btnCaptureFace = new Button();
            this.btnCaptureFace.Text = "Chụp";
            this.btnCaptureFace.Font = new Font("Segoe UI", 9F);
            this.btnCaptureFace.Location = new Point(20, 155);
            this.btnCaptureFace.Size = new Size(60, 30); // Smaller width to fit both
            this.btnCaptureFace.Click += new EventHandler(this.btnCaptureFace_Click);
            this.grpAdvanced.Controls.Add(this.btnCaptureFace);

            // 
            // btnUploadFace (New)
            // 
            this.btnUploadFace = new Button();
            this.btnUploadFace.Text = "Tải Ảnh";
            this.btnUploadFace.Font = new Font("Segoe UI", 9F);
            this.btnUploadFace.Location = new Point(85, 155); // Next to Capture
            this.btnUploadFace.Size = new Size(55, 30);
            this.btnUploadFace.Click += new EventHandler(this.btnUploadFace_Click);
            this.grpAdvanced.Controls.Add(this.btnUploadFace);

            // QR Area
            this.picQR = new PictureBox();
            this.picQR.BorderStyle = BorderStyle.FixedSingle;
            this.picQR.Location = new Point(260, 30);
            this.picQR.Size = new Size(120, 120);
            this.picQR.SizeMode = PictureBoxSizeMode.Zoom;
            this.grpAdvanced.Controls.Add(this.picQR);

            this.btnGenerateQR = new Button();
            this.btnGenerateQR.Text = "Tạo QR";
            this.btnGenerateQR.Font = new Font("Segoe UI", 9F);
            this.btnGenerateQR.Location = new Point(260, 155);
            this.btnGenerateQR.Size = new Size(120, 30);
            this.btnGenerateQR.Click += new EventHandler(this.btnGenerateQR_Click);
            this.grpAdvanced.Controls.Add(this.btnGenerateQR);

            // 
            // btnRegister
            // 
            this.btnRegister.Text = "Đăng Ký";
            this.btnRegister.Font = new Font("Segoe UI", 14F, FontStyle.Bold); 
            this.btnRegister.ForeColor = Color.White;
            this.btnRegister.BackColor = Color.FromArgb(91, 155, 213); 
            this.btnRegister.FlatStyle = FlatStyle.Flat;
            this.btnRegister.FlatAppearance.BorderSize = 0;
            this.btnRegister.Size = new Size(400, 50); 
            this.btnRegister.Location = new Point(50, 670); // Moved Down
            this.btnRegister.Cursor = Cursors.Hand;
            this.btnRegister.Click += new EventHandler(this.btnRegister_Click);
            this.panelCard.Controls.Add(this.btnRegister);

            // 
            // linkLogin
            // 
            this.linkLogin.Text = "Đã có tài khoản? Đăng nhập";
            this.linkLogin.Font = new Font("Segoe UI", 10F); 
            this.linkLogin.LinkColor = Color.FromArgb(91, 155, 213);
            this.linkLogin.ActiveLinkColor = Color.FromArgb(0, 122, 255);
            this.linkLogin.AutoSize = true;
            this.linkLogin.Location = new Point(130, 740); // Moved Down
            this.linkLogin.Cursor = Cursors.Hand;
            this.linkLogin.LinkClicked += new LinkLabelLinkClickedEventHandler(this.btnLogin_Click); 
            this.panelCard.Controls.Add(this.linkLogin);

            this.ResumeLayout(false);
        }

        #endregion

        private Panel panelCard;
        private Label btnClose;
        private Label lblTitle;
        
        private Panel panelUser;
        private Label lblUserIcon;
        private TextBox txtUsername;
        
        private Panel panelPass;
        private Label lblPassIcon;
        private TextBox txtPassword;

        private Panel panelEmail;
        private Label lblEmailIcon;
        private TextBox txtEmail;

        private Panel panelAddr;
        private Label lblAddrIcon;
        private TextBox txtAddress;

        private GroupBox grpAdvanced;
        private PictureBox picFace;
        private Button btnCaptureFace;
        private PictureBox picQR;
        private Button btnGenerateQR;

        private Button btnRegister;
        private LinkLabel linkLogin;
        private Button btnUploadFace; // New Button
    }
}
