namespace DA_N6.views
{
    partial class SecurityPolicyForm
    {
        private System.ComponentModel.IContainer components = null;

        // Khai báo Controls
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.GroupBox grpEdit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3; // Hướng dẫn
        private System.Windows.Forms.DataGridView dgvPolicies;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelRight = new System.Windows.Forms.Panel();
            this.grpEdit = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvPolicies = new System.Windows.Forms.DataGridView();
            this.panelRight.SuspendLayout();
            this.grpEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPolicies)).BeginInit();
            this.SuspendLayout();
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.grpEdit);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(500, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Padding = new System.Windows.Forms.Padding(10);
            this.panelRight.Size = new System.Drawing.Size(300, 450);
            this.panelRight.TabIndex = 0;
            // 
            // grpEdit
            // 
            this.grpEdit.Controls.Add(this.label3);
            this.grpEdit.Controls.Add(this.btnSave);
            this.grpEdit.Controls.Add(this.txtValue);
            this.grpEdit.Controls.Add(this.label2);
            this.grpEdit.Controls.Add(this.txtName);
            this.grpEdit.Controls.Add(this.label1);
            this.grpEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpEdit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpEdit.Location = new System.Drawing.Point(10, 10);
            this.grpEdit.Name = "grpEdit";
            this.grpEdit.Size = new System.Drawing.Size(280, 430);
            this.grpEdit.TabIndex = 0;
            this.grpEdit.TabStop = false;
            this.grpEdit.Text = "Điều chỉnh thông số";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.label3.ForeColor = System.Drawing.Color.Gray;
            this.label3.Location = new System.Drawing.Point(15, 230);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(250, 150);
            this.label3.TabIndex = 5;
            this.label3.Text = "Gợi ý giá trị:\n- UNLIMITED: Không giới hạn\n- DEFAULT: Theo mặc định\n- Số nguyên: " +
    "Giới hạn cụ thể (VD: 3 lần, 30 ngày...)";
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(0, 153);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(250, 40);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "LƯU THAY ĐỔI";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtValue
            // 
            this.txtValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtValue.Location = new System.Drawing.Point(15, 120);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(250, 27);
            this.txtValue.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label2.Location = new System.Drawing.Point(15, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Giá trị giới hạn:";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtName.Location = new System.Drawing.Point(15, 60);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(250, 27);
            this.txtName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label1.Location = new System.Drawing.Point(15, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tên chính sách:";
            // 
            // dgvPolicies
            // 
            this.dgvPolicies.AllowUserToAddRows = false;
            this.dgvPolicies.AllowUserToDeleteRows = false;
            this.dgvPolicies.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPolicies.BackgroundColor = System.Drawing.Color.White;
            this.dgvPolicies.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPolicies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPolicies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPolicies.Location = new System.Drawing.Point(0, 0);
            this.dgvPolicies.Name = "dgvPolicies";
            this.dgvPolicies.ReadOnly = true;
            this.dgvPolicies.RowHeadersVisible = false;
            this.dgvPolicies.RowHeadersWidth = 51;
            this.dgvPolicies.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPolicies.Size = new System.Drawing.Size(500, 450);
            this.dgvPolicies.TabIndex = 1;
            this.dgvPolicies.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPolicies_CellClick);
            // 
            // SecurityPolicyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvPolicies);
            this.Controls.Add(this.panelRight);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Name = "SecurityPolicyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý Chính sách Bảo mật (Oracle Profile)";
            this.panelRight.ResumeLayout(false);
            this.grpEdit.ResumeLayout(false);
            this.grpEdit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPolicies)).EndInit();
            this.ResumeLayout(false);

        }
    }
}