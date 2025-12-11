using System.Windows.Forms;

namespace DA_N6.views.orders
{
    partial class OrdersForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelTop;
        private DataGridView dgvOrders;
        private Label lblTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelTop = new Panel();
            this.lblTitle = new Label();
            this.dgvOrders = new DataGridView();

            // panelTop
            this.panelTop.Dock = DockStyle.Top;
            this.panelTop.Height = 60;
            this.panelTop.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelTop.Controls.Add(lblTitle);

            // lblTitle
            this.lblTitle.Text = "Danh sách đơn hàng";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.AutoSize = true;
            this.lblTitle.Left = 20;
            this.lblTitle.Top = 15;

            // dgvOrders
            this.dgvOrders.Dock = DockStyle.Fill;
            this.dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOrders.CellDoubleClick += dgvOrders_CellDoubleClick;

            // OrdersForm
            this.Controls.Add(dgvOrders);
            this.Controls.Add(panelTop);
            this.Text = "Đơn hàng";
            this.ClientSize = new System.Drawing.Size(900, 550);
        }
    }
}
