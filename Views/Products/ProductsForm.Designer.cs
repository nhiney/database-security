namespace DA_N6.views.products
{
    partial class ProductsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelRight;

        // Controls
        private System.Windows.Forms.TextBox txtId, txtName, txtCategory, txtQuantity, txtPrice;
        private System.Windows.Forms.Label label1, label2, label3, label4, label5;
        private System.Windows.Forms.Button btnAdd, btnUpdate, btnDelete;
        public System.Windows.Forms.Button btnAddToCart;
        public System.Windows.Forms.Button btnViewCart;
        private System.Windows.Forms.DataGridView dgv;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.dgv = new System.Windows.Forms.DataGridView();

            this.txtId = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtCategory = new System.Windows.Forms.TextBox();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.txtPrice = new System.Windows.Forms.TextBox();

            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAddToCart = new System.Windows.Forms.Button();
            this.btnViewCart = new System.Windows.Forms.Button();

            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();

            // 
            // panelLeft
            // 
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Width = 360; // Mở rộng thêm chút để thoáng
            this.panelLeft.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelLeft.Padding = new System.Windows.Forms.Padding(10);

            // 
            // panelRight
            // 
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Controls.Add(this.dgv);

            // 
            // dgv
            // 
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.BackgroundColor = System.Drawing.Color.White;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellClick);

            // =================================================================
            // CẤU HÌNH VỊ TRÍ CỐ ĐỊNH (KHÔNG TÍNH TOÁN)
            // =================================================================

            // 1. Nút Xem Giỏ Hàng (Trên cùng)
            this.btnViewCart.Text = "🛒 XEM GIỎ HÀNG";
            this.btnViewCart.Location = new System.Drawing.Point(20, 20);
            this.btnViewCart.Size = new System.Drawing.Size(320, 45);
            this.btnViewCart.BackColor = System.Drawing.Color.Teal;
            this.btnViewCart.ForeColor = System.Drawing.Color.White;
            this.btnViewCart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewCart.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnViewCart.Click += new System.EventHandler(this.btnViewCart_Click);

            // 2. Dòng ID (Y = 90)
            this.label1.Text = "ID:";
            this.label1.Location = new System.Drawing.Point(20, 94); // Label căn chỉnh thấp hơn chút để giữa dòng
            this.label1.AutoSize = true;

            this.txtId.Location = new System.Drawing.Point(100, 90);
            this.txtId.Size = new System.Drawing.Size(240, 23);
            this.txtId.ReadOnly = true;

            // 3. Dòng Tên SP (Y = 140) - Cách dòng trên 50px
            this.label2.Text = "Tên SP:";
            this.label2.Location = new System.Drawing.Point(20, 144);
            this.label2.AutoSize = true;

            this.txtName.Location = new System.Drawing.Point(100, 140);
            this.txtName.Size = new System.Drawing.Size(240, 23);

            // 4. Dòng Loại (Y = 190)
            this.label3.Text = "Loại (ID):";
            this.label3.Location = new System.Drawing.Point(20, 194);
            this.label3.AutoSize = true;

            this.txtCategory.Location = new System.Drawing.Point(100, 190);
            this.txtCategory.Size = new System.Drawing.Size(240, 23);

            // 5. Dòng Số lượng (Y = 240)
            this.label4.Text = "SL Tồn:";
            this.label4.Location = new System.Drawing.Point(20, 244);
            this.label4.AutoSize = true;

            this.txtQuantity.Location = new System.Drawing.Point(100, 240);
            this.txtQuantity.Size = new System.Drawing.Size(240, 23);

            // 6. Dòng Giá (Y = 290)
            this.label5.Text = "Giá:";
            this.label5.Location = new System.Drawing.Point(20, 294);
            this.label5.AutoSize = true;

            this.txtPrice.Location = new System.Drawing.Point(100, 290);
            this.txtPrice.Size = new System.Drawing.Size(240, 23);

            // 7. Khu vực Nút bấm (Y = 350)

            // --- Nút Admin (Thêm/Sửa/Xóa) ---
            this.btnAdd.Text = "Thêm";
            this.btnAdd.Location = new System.Drawing.Point(20, 350);
            this.btnAdd.Size = new System.Drawing.Size(100, 40);
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            this.btnUpdate.Text = "Sửa";
            this.btnUpdate.Location = new System.Drawing.Point(130, 350);
            this.btnUpdate.Size = new System.Drawing.Size(100, 40);
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);

            this.btnDelete.Text = "Xóa";
            this.btnDelete.Location = new System.Drawing.Point(240, 350);
            this.btnDelete.Size = new System.Drawing.Size(100, 40);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            // --- Nút User (Thêm vào giỏ) - Đè lên vị trí nút Admin ---
            this.btnAddToCart.Text = "THÊM VÀO GIỎ";
            this.btnAddToCart.Location = new System.Drawing.Point(20, 350);
            this.btnAddToCart.Size = new System.Drawing.Size(320, 50); // To và dài bằng cả hàng
            this.btnAddToCart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0))))); // Màu cam
            this.btnAddToCart.ForeColor = System.Drawing.Color.White;
            this.btnAddToCart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddToCart.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnAddToCart.Click += new System.EventHandler(this.BtnAddToCart_Click);

            // Thêm controls vào Panel Left
            this.panelLeft.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.btnViewCart,
                this.label1, this.txtId,
                this.label2, this.txtName,
                this.label3, this.txtCategory,
                this.label4, this.txtQuantity,
                this.label5, this.txtPrice,
                this.btnAdd, this.btnUpdate, this.btnDelete,
                this.btnAddToCart
            });

            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelLeft);

            this.Text = "Sản phẩm";
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
        }
    }
}