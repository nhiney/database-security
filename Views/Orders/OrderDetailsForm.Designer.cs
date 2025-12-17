namespace DA_N6.views.orders
{
    partial class OrderDetailForm
    {
        private System.ComponentModel.IContainer components = null;

        // Khai báo Controls
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblOrderId;
        private System.Windows.Forms.Button btnSignAndSend; // Nút Ký số
        private System.Windows.Forms.DataGridView dgvDetail;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnSignAndSend = new System.Windows.Forms.Button();
            this.lblOrderId = new System.Windows.Forms.Label();
            this.dgvDetail = new System.Windows.Forms.DataGridView();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).BeginInit();
            this.SuspendLayout();

            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelTop.Controls.Add(this.btnSignAndSend);
            this.panelTop.Controls.Add(this.lblOrderId);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(800, 70);
            this.panelTop.TabIndex = 0;

            // 
            // lblOrderId
            // 
            this.lblOrderId.AutoSize = true;
            this.lblOrderId.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblOrderId.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblOrderId.Location = new System.Drawing.Point(20, 20);
            this.lblOrderId.Name = "lblOrderId";
            this.lblOrderId.Size = new System.Drawing.Size(173, 25);
            this.lblOrderId.TabIndex = 0;
            this.lblOrderId.Text = "Chi tiết đơn hàng:";

            // 
            // btnSignAndSend (Nút Ký số)
            // 
            this.btnSignAndSend.BackColor = System.Drawing.Color.SeaGreen;
            this.btnSignAndSend.FlatAppearance.BorderSize = 0;
            this.btnSignAndSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSignAndSend.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSignAndSend.ForeColor = System.Drawing.Color.White;
            this.btnSignAndSend.Location = new System.Drawing.Point(580, 15);
            this.btnSignAndSend.Name = "btnSignAndSend";
            this.btnSignAndSend.Size = new System.Drawing.Size(200, 40);
            this.btnSignAndSend.TabIndex = 1;
            this.btnSignAndSend.Text = "✍ Ký Số && Gửi Mail";
            this.btnSignAndSend.UseVisualStyleBackColor = false;
            this.btnSignAndSend.Click += new System.EventHandler(this.btnSignAndSend_Click);

            // 
            // dgvDetail
            // 
            this.dgvDetail.AllowUserToAddRows = false;
            this.dgvDetail.AllowUserToDeleteRows = false;
            this.dgvDetail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDetail.BackgroundColor = System.Drawing.Color.White;
            this.dgvDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetail.Location = new System.Drawing.Point(0, 70);
            this.dgvDetail.Name = "dgvDetail";
            this.dgvDetail.ReadOnly = true;
            this.dgvDetail.RowHeadersVisible = false;
            this.dgvDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetail.Size = new System.Drawing.Size(800, 380);
            this.dgvDetail.TabIndex = 1;

            // 
            // OrderDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvDetail);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Name = "OrderDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chi Tiết Đơn Hàng";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).EndInit();
            this.ResumeLayout(false);
        }
    }
}