namespace DA_N6.views
{
    partial class VerifyForm
    {
        private System.ComponentModel.IContainer components = null;

        // Khai báo Controls
        private System.Windows.Forms.GroupBox grpInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDataPath;
        private System.Windows.Forms.Button btnBrowseData;

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSigPath;
        private System.Windows.Forms.Button btnBrowseSig;

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtKeyPath;
        private System.Windows.Forms.Button btnBrowseKey;

        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Label lblResult; // Hiển thị kết quả to rõ

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.grpInput = new System.Windows.Forms.GroupBox();
            this.btnBrowseKey = new System.Windows.Forms.Button();
            this.txtKeyPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBrowseSig = new System.Windows.Forms.Button();
            this.txtSigPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBrowseData = new System.Windows.Forms.Button();
            this.txtDataPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnVerify = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.grpInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpInput
            // 
            this.grpInput.Controls.Add(this.btnBrowseKey);
            this.grpInput.Controls.Add(this.txtKeyPath);
            this.grpInput.Controls.Add(this.label3);
            this.grpInput.Controls.Add(this.btnBrowseSig);
            this.grpInput.Controls.Add(this.txtSigPath);
            this.grpInput.Controls.Add(this.label2);
            this.grpInput.Controls.Add(this.btnBrowseData);
            this.grpInput.Controls.Add(this.txtDataPath);
            this.grpInput.Controls.Add(this.label1);
            this.grpInput.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.grpInput.Location = new System.Drawing.Point(20, 20);
            this.grpInput.Name = "grpInput";
            this.grpInput.Size = new System.Drawing.Size(755, 200);
            this.grpInput.TabIndex = 0;
            this.grpInput.TabStop = false;
            this.grpInput.Text = "Chọn các file cần kiểm tra";
            // 
            // btnBrowseKey
            // 
            this.btnBrowseKey.Location = new System.Drawing.Point(564, 135);
            this.btnBrowseKey.Name = "btnBrowseKey";
            this.btnBrowseKey.Size = new System.Drawing.Size(90, 30);
            this.btnBrowseKey.TabIndex = 0;
            this.btnBrowseKey.Text = "Chọn...";
            this.btnBrowseKey.Click += new System.EventHandler(this.btnBrowseKey_Click);
            // 
            // txtKeyPath
            // 
            this.txtKeyPath.Location = new System.Drawing.Point(204, 137);
            this.txtKeyPath.Name = "txtKeyPath";
            this.txtKeyPath.ReadOnly = true;
            this.txtKeyPath.Size = new System.Drawing.Size(350, 30);
            this.txtKeyPath.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "File Khóa Public:";
            // 
            // btnBrowseSig
            // 
            this.btnBrowseSig.Location = new System.Drawing.Point(564, 85);
            this.btnBrowseSig.Name = "btnBrowseSig";
            this.btnBrowseSig.Size = new System.Drawing.Size(90, 30);
            this.btnBrowseSig.TabIndex = 3;
            this.btnBrowseSig.Text = "Chọn...";
            this.btnBrowseSig.Click += new System.EventHandler(this.btnBrowseSig_Click);
            // 
            // txtSigPath
            // 
            this.txtSigPath.Location = new System.Drawing.Point(204, 87);
            this.txtSigPath.Name = "txtSigPath";
            this.txtSigPath.ReadOnly = true;
            this.txtSigPath.Size = new System.Drawing.Size(350, 30);
            this.txtSigPath.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = "File Chữ ký:";
            // 
            // btnBrowseData
            // 
            this.btnBrowseData.Location = new System.Drawing.Point(564, 35);
            this.btnBrowseData.Name = "btnBrowseData";
            this.btnBrowseData.Size = new System.Drawing.Size(90, 30);
            this.btnBrowseData.TabIndex = 6;
            this.btnBrowseData.Text = "Chọn...";
            this.btnBrowseData.Click += new System.EventHandler(this.btnBrowseData_Click);
            // 
            // txtDataPath
            // 
            this.txtDataPath.Location = new System.Drawing.Point(204, 37);
            this.txtDataPath.Name = "txtDataPath";
            this.txtDataPath.ReadOnly = true;
            this.txtDataPath.Size = new System.Drawing.Size(350, 30);
            this.txtDataPath.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "File Hóa đơn:";
            // 
            // btnVerify
            // 
            this.btnVerify.BackColor = System.Drawing.Color.SteelBlue;
            this.btnVerify.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnVerify.ForeColor = System.Drawing.Color.White;
            this.btnVerify.Location = new System.Drawing.Point(287, 244);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(240, 50);
            this.btnVerify.TabIndex = 3;
            this.btnVerify.Text = "KIỂM TRA XÁC THỰC";
            this.btnVerify.UseVisualStyleBackColor = false;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblResult.Location = new System.Drawing.Point(107, 314);
            this.lblResult.MinimumSize = new System.Drawing.Size(600, 0);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(600, 32);
            this.lblResult.TabIndex = 2;
            this.lblResult.Text = "Kết quả sẽ hiện ở đây...";
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // VerifyForm
            // 
            this.ClientSize = new System.Drawing.Size(851, 400);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btnVerify);
            this.Controls.Add(this.grpInput);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "VerifyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kiểm tra Chữ ký số (Verify)";
            this.grpInput.ResumeLayout(false);
            this.grpInput.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}