namespace DA_N6.Views.Users
{
    partial class Audit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvLoadLog = new System.Windows.Forms.DataGridView();
            this.clbTable = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnX = new System.Windows.Forms.Button();
            this.btnAD = new System.Windows.Forms.Button();
            this.cbUT = new System.Windows.Forms.CheckBox();
            this.cbST = new System.Windows.Forms.CheckBox();
            this.cbIT = new System.Windows.Forms.CheckBox();
            this.cbDT = new System.Windows.Forms.CheckBox();
            this.cbb_LIST_USER = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_Export = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoadLog)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.btn_Export);
            this.panel1.Controls.Add(this.dgvLoadLog);
            this.panel1.Controls.Add(this.clbTable);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnX);
            this.panel1.Controls.Add(this.btnAD);
            this.panel1.Controls.Add(this.cbUT);
            this.panel1.Controls.Add(this.cbST);
            this.panel1.Controls.Add(this.cbIT);
            this.panel1.Controls.Add(this.cbDT);
            this.panel1.Controls.Add(this.cbb_LIST_USER);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(809, 599);
            this.panel1.TabIndex = 3;
            // 
            // dgvLoadLog
            // 
            this.dgvLoadLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLoadLog.Location = new System.Drawing.Point(41, 376);
            this.dgvLoadLog.Name = "dgvLoadLog";
            this.dgvLoadLog.RowHeadersWidth = 51;
            this.dgvLoadLog.RowTemplate.Height = 24;
            this.dgvLoadLog.Size = new System.Drawing.Size(731, 153);
            this.dgvLoadLog.TabIndex = 18;
            // 
            // clbTable
            // 
            this.clbTable.FormattingEnabled = true;
            this.clbTable.Location = new System.Drawing.Point(122, 147);
            this.clbTable.Name = "clbTable";
            this.clbTable.Size = new System.Drawing.Size(250, 106);
            this.clbTable.TabIndex = 17;
            this.clbTable.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbTable_ItemCheck);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Window;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label1.Location = new System.Drawing.Point(12, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 32);
            this.label1.TabIndex = 16;
            this.label1.Text = "Table:";
            // 
            // btnX
            // 
            this.btnX.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnX.Location = new System.Drawing.Point(610, 290);
            this.btnX.Name = "btnX";
            this.btnX.Size = new System.Drawing.Size(138, 47);
            this.btnX.TabIndex = 15;
            this.btnX.Text = "Xóa";
            this.btnX.UseVisualStyleBackColor = true;
            this.btnX.Click += new System.EventHandler(this.btnX_Click);
            // 
            // btnAD
            // 
            this.btnAD.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAD.Location = new System.Drawing.Point(400, 290);
            this.btnAD.Name = "btnAD";
            this.btnAD.Size = new System.Drawing.Size(138, 47);
            this.btnAD.TabIndex = 14;
            this.btnAD.Text = "Áp dụng";
            this.btnAD.UseVisualStyleBackColor = true;
            this.btnAD.Click += new System.EventHandler(this.btnAD_Click);
            // 
            // cbUT
            // 
            this.cbUT.AutoSize = true;
            this.cbUT.Location = new System.Drawing.Point(520, 175);
            this.cbUT.Name = "cbUT";
            this.cbUT.Size = new System.Drawing.Size(131, 20);
            this.cbUT.TabIndex = 7;
            this.cbUT.Text = "UPDATE TABLE";
            this.cbUT.UseVisualStyleBackColor = true;
            // 
            // cbST
            // 
            this.cbST.AutoSize = true;
            this.cbST.Location = new System.Drawing.Point(520, 94);
            this.cbST.Name = "cbST";
            this.cbST.Size = new System.Drawing.Size(127, 20);
            this.cbST.TabIndex = 6;
            this.cbST.Text = "SELECT TABLE";
            this.cbST.UseVisualStyleBackColor = true;
            // 
            // cbIT
            // 
            this.cbIT.AutoSize = true;
            this.cbIT.Location = new System.Drawing.Point(520, 149);
            this.cbIT.Name = "cbIT";
            this.cbIT.Size = new System.Drawing.Size(125, 20);
            this.cbIT.TabIndex = 5;
            this.cbIT.Text = "INSERT TABLE";
            this.cbIT.UseVisualStyleBackColor = true;
            // 
            // cbDT
            // 
            this.cbDT.AutoSize = true;
            this.cbDT.Location = new System.Drawing.Point(520, 120);
            this.cbDT.Name = "cbDT";
            this.cbDT.Size = new System.Drawing.Size(128, 20);
            this.cbDT.TabIndex = 4;
            this.cbDT.Text = "DELETE TABLE";
            this.cbDT.UseVisualStyleBackColor = true;
            // 
            // cbb_LIST_USER
            // 
            this.cbb_LIST_USER.FormattingEnabled = true;
            this.cbb_LIST_USER.Location = new System.Drawing.Point(91, 65);
            this.cbb_LIST_USER.Name = "cbb_LIST_USER";
            this.cbb_LIST_USER.Size = new System.Drawing.Size(199, 24);
            this.cbb_LIST_USER.TabIndex = 2;
            this.cbb_LIST_USER.SelectedIndexChanged += new System.EventHandler(this.cbb_LIST_USER_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label8.Location = new System.Drawing.Point(12, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 32);
            this.label8.TabIndex = 1;
            this.label8.Text = "User:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label7.Location = new System.Drawing.Point(12, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(342, 32);
            this.label7.TabIndex = 0;
            this.label7.Text = "Giám sát Người Dùng Oracle";
            // 
            // btn_Export
            // 
            this.btn_Export.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Export.Location = new System.Drawing.Point(634, 539);
            this.btn_Export.Name = "btn_Export";
            this.btn_Export.Size = new System.Drawing.Size(138, 47);
            this.btn_Export.TabIndex = 19;
            this.btn_Export.Text = "Xuất";
            this.btn_Export.UseVisualStyleBackColor = true;
            this.btn_Export.Click += new System.EventHandler(this.btn_Export_Click);
            // 
            // Audit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 598);
            this.Controls.Add(this.panel1);
            this.Name = "Audit";
            this.Text = "Audit";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoadLog)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox cbUT;
        private System.Windows.Forms.CheckBox cbST;
        private System.Windows.Forms.CheckBox cbIT;
        private System.Windows.Forms.CheckBox cbDT;
        private System.Windows.Forms.ComboBox cbb_LIST_USER;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnX;
        private System.Windows.Forms.Button btnAD;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox clbTable;
        private System.Windows.Forms.DataGridView dgvLoadLog;
        private System.Windows.Forms.Button btn_Export;
    }
}