using System.Drawing;
using System.Windows.Forms;

namespace DA_N6
{
    partial class Main
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnVerify;
        private Panel panelMenu;
        private Panel panelContent;
        private Button btnProducts;
        private Button btnOrders;
        private Button btnDevices;
        private Button btnLogout;
        private Button btnUsers;
        private Button btnAuthorization;
        private Label lblUsername;
        private Button btnPolicy;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelMenu = new System.Windows.Forms.Panel();
            this.btnVerify = new System.Windows.Forms.Button();
            this.btnDevices = new System.Windows.Forms.Button();
            this.btnOrders = new System.Windows.Forms.Button();
            this.btnProducts = new System.Windows.Forms.Button();
            this.lblUsername = new System.Windows.Forms.Label();
            this.btnUsers = new System.Windows.Forms.Button();
            this.btnAuthorization = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.panelContent = new System.Windows.Forms.Panel();
            this.btnPolicy = new System.Windows.Forms.Button();
            this.panelMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMenu
            // 
            this.panelMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));
            this.panelMenu.Controls.Add(this.btnVerify);
            this.panelMenu.Controls.Add(this.btnDevices);
            this.panelMenu.Controls.Add(this.btnOrders);
            this.panelMenu.Controls.Add(this.btnProducts);
            this.panelMenu.Controls.Add(this.lblUsername);
            this.panelMenu.Controls.Add(this.btnAuthorization);
            this.panelMenu.Controls.Add(this.btnUsers);
            this.panelMenu.Controls.Add(this.btnPolicy);
            this.panelMenu.Controls.Add(this.btnLogout);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMenu.Location = new System.Drawing.Point(0, 0);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Padding = new System.Windows.Forms.Padding(0, 15, 0, 10);
            this.panelMenu.Size = new System.Drawing.Size(220, 650);
            this.panelMenu.TabIndex = 1;
            // 
            // btnPolicy
            // 
            this.btnPolicy.Location = new System.Drawing.Point(0, 389); 
            this.btnPolicy.Name = "btnPolicy";
            this.btnPolicy.Size = new System.Drawing.Size(220, 53);
            this.btnPolicy.TabIndex = 7;
            this.btnPolicy.Text = "Quản lý Policy";
            this.btnPolicy.UseVisualStyleBackColor = true;
            this.btnPolicy.Click += new System.EventHandler(this.btnPolicy_Click); // Gán sự kiện

            // 
            // btnVerify
            // 
            this.btnVerify.Location = new System.Drawing.Point(0, 283);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(220, 53);
            this.btnVerify.TabIndex = 6;
            this.btnVerify.Text = "Kiểm tra Ký số";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // btnAuthorization
            // 
            this.btnAuthorization.Location = new System.Drawing.Point(0, 336);
            this.btnAuthorization.Name = "btnAuthorization";
            this.btnAuthorization.Size = new System.Drawing.Size(220, 53);
            this.btnAuthorization.TabIndex = 7;
            this.btnAuthorization.Text = "Phân quyền (Auth)";
            this.btnAuthorization.UseVisualStyleBackColor = true;
            this.btnAuthorization.Click += new System.EventHandler(this.btnAuthorization_Click);
            // 
            // btnDevices
            // 
            this.btnDevices.Location = new System.Drawing.Point(0, 177);
            this.btnDevices.Name = "btnDevices";
            this.btnDevices.Size = new System.Drawing.Size(220, 53);
            this.btnDevices.TabIndex = 0;
            this.btnDevices.Text = "Thiết bị";
            this.btnDevices.Click += new System.EventHandler(this.btnDevices_Click);
            // 
            // btnOrders
            // 
            this.btnOrders.Location = new System.Drawing.Point(0, 117);
            this.btnOrders.Name = "btnOrders";
            this.btnOrders.Size = new System.Drawing.Size(220, 64);
            this.btnOrders.TabIndex = 1;
            this.btnOrders.Text = "Đơn hàng";
            this.btnOrders.Click += new System.EventHandler(this.btnOrders_Click);
            // 
            // btnProducts
            // 
            this.btnProducts.Location = new System.Drawing.Point(0, 63);
            this.btnProducts.Name = "btnProducts";
            this.btnProducts.Size = new System.Drawing.Size(220, 58);
            this.btnProducts.TabIndex = 2;
            this.btnProducts.Text = "Sản phẩm";
            this.btnProducts.Click += new System.EventHandler(this.btnProducts_Click);
            // 
            // lblUsername
            // 
            this.lblUsername.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblUsername.Location = new System.Drawing.Point(0, 15);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(220, 60);
            this.lblUsername.TabIndex = 3;
            this.lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnUsers
            // 
            this.btnUsers.Location = new System.Drawing.Point(0, 230);
            this.btnUsers.Name = "btnUsers";
            this.btnUsers.Size = new System.Drawing.Size(220, 53);
            this.btnUsers.TabIndex = 5;
            this.btnUsers.Text = "Quản lý Users";
            this.btnUsers.UseVisualStyleBackColor = true;
            this.btnUsers.Click += new System.EventHandler(this.btnUsers_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(106)))), ((int)(((byte)(106)))));
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(0, 590);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(220, 50);
            this.btnLogout.TabIndex = 4;
            this.btnLogout.Text = "Đăng xuất";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.White;
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(220, 0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(880, 650);
            this.panelContent.TabIndex = 0;
            // 
            // Main
            // 
            this.ClientSize = new System.Drawing.Size(1100, 650);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelMenu);
            this.Name = "Main";
            this.Text = "Trang Chủ";
            this.Load += new System.EventHandler(this.SessionTimer_Tick);
            this.panelMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
