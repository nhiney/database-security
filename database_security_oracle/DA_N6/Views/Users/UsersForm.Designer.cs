namespace DA_N6.Views.Users
{
    partial class UsersForm
    {
        private System.ComponentModel.IContainer components = null;

        // --- KHAI BÁO CONTROL ---
        private System.Windows.Forms.TabControl tabControlMain;

        // Tab 1: Quản lý User
        private System.Windows.Forms.TabPage tabPageUserList;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnLock;
        private System.Windows.Forms.Button btnUnlock;
        private System.Windows.Forms.Button btnDrop;
        private System.Windows.Forms.DataGridView dgvUsers;

        // Tab 2: Quản lý Storage (MỚI)
        private System.Windows.Forms.TabPage tabPageStorage;
        private System.Windows.Forms.GroupBox grpCreateTablespace;
        private System.Windows.Forms.Button btnCreateTablespace;
        private System.Windows.Forms.NumericUpDown numTbSize;
        private System.Windows.Forms.TextBox txtDataFile;
        private System.Windows.Forms.TextBox txtTbName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.GroupBox grpQuota;
        private System.Windows.Forms.Button btnGrantQuota;
        private System.Windows.Forms.NumericUpDown numQuotaSize;
        private System.Windows.Forms.TextBox txtQuotaTbName;
        private System.Windows.Forms.TextBox txtQuotaUsername;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageUserList = new System.Windows.Forms.TabPage();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnDrop = new System.Windows.Forms.Button();
            this.btnUnlock = new System.Windows.Forms.Button();
            this.btnLock = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tabPageStorage = new System.Windows.Forms.TabPage();
            this.grpQuota = new System.Windows.Forms.GroupBox();
            this.btnGrantQuota = new System.Windows.Forms.Button();
            this.numQuotaSize = new System.Windows.Forms.NumericUpDown();
            this.txtQuotaTbName = new System.Windows.Forms.TextBox();
            this.txtQuotaUsername = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.grpCreateTablespace = new System.Windows.Forms.GroupBox();
            this.btnCreateTablespace = new System.Windows.Forms.Button();
            this.numTbSize = new System.Windows.Forms.NumericUpDown();
            this.txtDataFile = new System.Windows.Forms.TextBox();
            this.txtTbName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControlMain.SuspendLayout();
            this.tabPageUserList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.panelTop.SuspendLayout();
            this.tabPageStorage.SuspendLayout();
            this.grpQuota.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQuotaSize)).BeginInit();
            this.grpCreateTablespace.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTbSize)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageUserList);
            this.tabControlMain.Controls.Add(this.tabPageStorage);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(800, 500);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageUserList
            // 
            this.tabPageUserList.Controls.Add(this.dgvUsers);
            this.tabPageUserList.Controls.Add(this.panelTop);
            this.tabPageUserList.Location = new System.Drawing.Point(4, 22);
            this.tabPageUserList.Name = "tabPageUserList";
            this.tabPageUserList.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUserList.Size = new System.Drawing.Size(792, 474);
            this.tabPageUserList.TabIndex = 0;
            this.tabPageUserList.Text = "Danh sách User";
            this.tabPageUserList.UseVisualStyleBackColor = true;
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsers.BackgroundColor = System.Drawing.Color.White;
            this.dgvUsers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUsers.Location = new System.Drawing.Point(3, 83);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(786, 388);
            this.dgvUsers.TabIndex = 1;
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelTop.Controls.Add(this.btnDrop);
            this.panelTop.Controls.Add(this.btnUnlock);
            this.panelTop.Controls.Add(this.btnLock);
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(3, 3);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(786, 80);
            this.panelTop.TabIndex = 0;
            // 
            // btnDrop
            // 
            this.btnDrop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
            this.btnDrop.FlatAppearance.BorderSize = 0;
            this.btnDrop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDrop.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDrop.ForeColor = System.Drawing.Color.White;
            this.btnDrop.Location = new System.Drawing.Point(229, 43);
            this.btnDrop.Name = "btnDrop";
            this.btnDrop.Size = new System.Drawing.Size(100, 30);
            this.btnDrop.TabIndex = 3;
            this.btnDrop.Text = "Xóa User";
            this.btnDrop.UseVisualStyleBackColor = false;
            this.btnDrop.Click += new System.EventHandler(this.btnDrop_Click);
            // 
            // btnUnlock
            // 
            this.btnUnlock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnUnlock.FlatAppearance.BorderSize = 0;
            this.btnUnlock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnlock.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnUnlock.ForeColor = System.Drawing.Color.White;
            this.btnUnlock.Location = new System.Drawing.Point(123, 43);
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new System.Drawing.Size(100, 30);
            this.btnUnlock.TabIndex = 2;
            this.btnUnlock.Text = "Mở Khóa TK";
            this.btnUnlock.UseVisualStyleBackColor = false;
            this.btnUnlock.Click += new System.EventHandler(this.btnUnlock_Click);
            // 
            // btnLock
            // 
            this.btnLock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnLock.FlatAppearance.BorderSize = 0;
            this.btnLock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLock.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLock.ForeColor = System.Drawing.Color.White;
            this.btnLock.Location = new System.Drawing.Point(17, 43);
            this.btnLock.Name = "btnLock";
            this.btnLock.Size = new System.Drawing.Size(100, 30);
            this.btnLock.TabIndex = 1;
            this.btnLock.Text = "Khóa TK";
            this.btnLock.UseVisualStyleBackColor = false;
            this.btnLock.Click += new System.EventHandler(this.btnLock_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(253, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Quản lý Người Dùng Oracle";
            // 
            // tabPageStorage
            // 
            this.tabPageStorage.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPageStorage.Controls.Add(this.grpQuota);
            this.tabPageStorage.Controls.Add(this.grpCreateTablespace);
            this.tabPageStorage.Location = new System.Drawing.Point(4, 22);
            this.tabPageStorage.Name = "tabPageStorage";
            this.tabPageStorage.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStorage.Size = new System.Drawing.Size(792, 474);
            this.tabPageStorage.TabIndex = 1;
            this.tabPageStorage.Text = "Quản lý Storage";
            // 
            // grpQuota
            // 
            this.grpQuota.BackColor = System.Drawing.Color.White;
            this.grpQuota.Controls.Add(this.btnGrantQuota);
            this.grpQuota.Controls.Add(this.numQuotaSize);
            this.grpQuota.Controls.Add(this.txtQuotaTbName);
            this.grpQuota.Controls.Add(this.txtQuotaUsername);
            this.grpQuota.Controls.Add(this.label4);
            this.grpQuota.Controls.Add(this.label5);
            this.grpQuota.Controls.Add(this.label6);
            this.grpQuota.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpQuota.Location = new System.Drawing.Point(400, 20);
            this.grpQuota.Name = "grpQuota";
            this.grpQuota.Size = new System.Drawing.Size(350, 250);
            this.grpQuota.TabIndex = 1;
            this.grpQuota.TabStop = false;
            this.grpQuota.Text = "2. Cấp Quota cho User";
            // 
            // btnGrantQuota
            // 
            this.btnGrantQuota.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnGrantQuota.FlatAppearance.BorderSize = 0;
            this.btnGrantQuota.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGrantQuota.ForeColor = System.Drawing.Color.White;
            this.btnGrantQuota.Location = new System.Drawing.Point(140, 160);
            this.btnGrantQuota.Name = "btnGrantQuota";
            this.btnGrantQuota.Size = new System.Drawing.Size(180, 35);
            this.btnGrantQuota.TabIndex = 7;
            this.btnGrantQuota.Text = "Cấp Quota";
            this.btnGrantQuota.UseVisualStyleBackColor = false;
            this.btnGrantQuota.Click += new System.EventHandler(this.btnGrantQuota_Click);
            // 
            // numQuotaSize
            // 
            this.numQuotaSize.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.numQuotaSize.Location = new System.Drawing.Point(140, 115);
            this.numQuotaSize.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.numQuotaSize.Name = "numQuotaSize";
            this.numQuotaSize.Size = new System.Drawing.Size(180, 23);
            this.numQuotaSize.TabIndex = 6;
            this.numQuotaSize.Value = new decimal(new int[] { 50, 0, 0, 0 });
            // 
            // txtQuotaTbName
            // 
            this.txtQuotaTbName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtQuotaTbName.Location = new System.Drawing.Point(140, 75);
            this.txtQuotaTbName.Name = "txtQuotaTbName";
            this.txtQuotaTbName.Size = new System.Drawing.Size(180, 23);
            this.txtQuotaTbName.TabIndex = 5;
            // 
            // txtQuotaUsername
            // 
            this.txtQuotaUsername.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtQuotaUsername.Location = new System.Drawing.Point(140, 35);
            this.txtQuotaUsername.Name = "txtQuotaUsername";
            this.txtQuotaUsername.Size = new System.Drawing.Size(180, 23);
            this.txtQuotaUsername.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label4.Location = new System.Drawing.Point(20, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "Dung lượng (M):";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label5.Location = new System.Drawing.Point(20, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "Tablespace:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label6.Location = new System.Drawing.Point(20, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 15);
            this.label6.TabIndex = 0;
            this.label6.Text = "Username:";
            // 
            // grpCreateTablespace
            // 
            this.grpCreateTablespace.BackColor = System.Drawing.Color.White;
            this.grpCreateTablespace.Controls.Add(this.btnCreateTablespace);
            this.grpCreateTablespace.Controls.Add(this.numTbSize);
            this.grpCreateTablespace.Controls.Add(this.txtDataFile);
            this.grpCreateTablespace.Controls.Add(this.txtTbName);
            this.grpCreateTablespace.Controls.Add(this.label3);
            this.grpCreateTablespace.Controls.Add(this.label2);
            this.grpCreateTablespace.Controls.Add(this.label1);
            this.grpCreateTablespace.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpCreateTablespace.Location = new System.Drawing.Point(20, 20);
            this.grpCreateTablespace.Name = "grpCreateTablespace";
            this.grpCreateTablespace.Size = new System.Drawing.Size(350, 250);
            this.grpCreateTablespace.TabIndex = 0;
            this.grpCreateTablespace.TabStop = false;
            this.grpCreateTablespace.Text = "1. Tạo Tablespace Mới";
            // 
            // btnCreateTablespace
            // 
            this.btnCreateTablespace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnCreateTablespace.FlatAppearance.BorderSize = 0;
            this.btnCreateTablespace.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateTablespace.ForeColor = System.Drawing.Color.White;
            this.btnCreateTablespace.Location = new System.Drawing.Point(140, 160);
            this.btnCreateTablespace.Name = "btnCreateTablespace";
            this.btnCreateTablespace.Size = new System.Drawing.Size(180, 35);
            this.btnCreateTablespace.TabIndex = 6;
            this.btnCreateTablespace.Text = "Tạo Tablespace";
            this.btnCreateTablespace.UseVisualStyleBackColor = false;
            this.btnCreateTablespace.Click += new System.EventHandler(this.btnCreateTablespace_Click);
            // 
            // numTbSize
            // 
            this.numTbSize.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.numTbSize.Location = new System.Drawing.Point(140, 115);
            this.numTbSize.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.numTbSize.Name = "numTbSize";
            this.numTbSize.Size = new System.Drawing.Size(180, 23);
            this.numTbSize.TabIndex = 5;
            this.numTbSize.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // txtDataFile
            // 
            this.txtDataFile.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtDataFile.Location = new System.Drawing.Point(140, 75);
            this.txtDataFile.Name = "txtDataFile";
            this.txtDataFile.Size = new System.Drawing.Size(180, 23);
            this.txtDataFile.TabIndex = 4;
            // 
            // txtTbName
            // 
            this.txtTbName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTbName.Location = new System.Drawing.Point(140, 35);
            this.txtTbName.Name = "txtTbName";
            this.txtTbName.Size = new System.Drawing.Size(180, 23);
            this.txtTbName.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label3.Location = new System.Drawing.Point(20, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Dung lượng (M):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label2.Location = new System.Drawing.Point(20, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "File data (.dbf):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label1.Location = new System.Drawing.Point(20, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tên Tablespace:";
            // 
            // UsersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.tabControlMain);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Name = "UsersForm";
            this.Text = "Quản lý Users";
            this.tabControlMain.ResumeLayout(false);
            this.tabPageUserList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.tabPageStorage.ResumeLayout(false);
            this.grpQuota.ResumeLayout(false);
            this.grpQuota.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numQuotaSize)).EndInit();
            this.grpCreateTablespace.ResumeLayout(false);
            this.grpCreateTablespace.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTbSize)).EndInit();
            this.ResumeLayout(false);

        }
    }
}