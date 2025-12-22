using System.Windows.Forms;

namespace DA_N6.views.devices
{
    partial class DevicesForm
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvDevices;
        private Button btnLogoutSelected;
        private Button btnLogoutAll;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvDevices = new System.Windows.Forms.DataGridView();
            this.btnLogoutSelected = new System.Windows.Forms.Button();
            this.btnLogoutAll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevices)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvDevices
            // 
            this.dgvDevices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDevices.ColumnHeadersHeight = 29;
            this.dgvDevices.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvDevices.Location = new System.Drawing.Point(0, 0);
            this.dgvDevices.Name = "dgvDevices";
            this.dgvDevices.RowHeadersWidth = 51;
            this.dgvDevices.Size = new System.Drawing.Size(700, 350);
            this.dgvDevices.TabIndex = 0;
            // 
            // btnLogoutSelected
            // 
            this.btnLogoutSelected.Location = new System.Drawing.Point(30, 370);
            this.btnLogoutSelected.Name = "btnLogoutSelected";
            this.btnLogoutSelected.Size = new System.Drawing.Size(220, 23);
            this.btnLogoutSelected.TabIndex = 1;
            this.btnLogoutSelected.Text = "Đăng xuất thiết bị chọn";
            this.btnLogoutSelected.Click += new System.EventHandler(this.btnLogoutSelected_Click_1);
            // 
            // btnLogoutAll
            // 
            this.btnLogoutAll.Location = new System.Drawing.Point(270, 370);
            this.btnLogoutAll.Name = "btnLogoutAll";
            this.btnLogoutAll.Size = new System.Drawing.Size(220, 23);
            this.btnLogoutAll.TabIndex = 2;
            this.btnLogoutAll.Text = "Đăng xuất tất cả thiết bị";
            // 
            // DevicesForm
            // 
            this.ClientSize = new System.Drawing.Size(700, 500);
            this.Controls.Add(this.dgvDevices);
            this.Controls.Add(this.btnLogoutSelected);
            this.Controls.Add(this.btnLogoutAll);
            this.Name = "DevicesForm";
            this.Text = "Quản lý thiết bị";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevices)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
