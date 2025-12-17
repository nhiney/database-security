using DA_N6.Database;
using DA_N6.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using Oracle.ManagedDataAccess.Types;
using System.IO;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;



namespace DA_N6.views.orders
{
    public partial class CartForm : Form
    {
        private int _userId;

        public CartForm(int userId)
        {
            InitializeComponent();
            _userId = userId;

            this.Load += CartForm_Load;
        }

        private void CartForm_Load(object sender, EventArgs e)
        {
            // Đảm bảo Grid nằm trên cùng, không bị Panel che khuất
            dgvCart.BringToFront();
            LoadCart();
        }

        private void LoadCart()
        {
            // 1. Reset lưới hoàn toàn
            dgvCart.DataSource = null;
            dgvCart.Rows.Clear();
            dgvCart.Columns.Clear(); // <--- QUAN TRỌNG: Xóa cột rác do Designer tạo

            // 2. Cấu hình tự động tạo cột
            dgvCart.AutoGenerateColumns = true;

            // 3. Dùng BindingSource để gói dữ liệu (Fix lỗi Grid không hiện List)
            var bindingSource = new BindingSource();
            bindingSource.DataSource = ShoppingCart.Items;
            dgvCart.DataSource = bindingSource;

            // 4. Tính tổng tiền
            decimal total = ShoppingCart.GetGrandTotal();
            lblTotal.Text = "Tổng tiền: " + total.ToString("N0") + " VNĐ";

            // 5. Định dạng cột 
            if (dgvCart.Columns.Count > 0)
            {
                if (dgvCart.Columns["ProductId"] != null) dgvCart.Columns["ProductId"].HeaderText = "Mã SP";
                if (dgvCart.Columns["ProductName"] != null) dgvCart.Columns["ProductName"].HeaderText = "Tên Sản Phẩm";
                if (dgvCart.Columns["Quantity"] != null) dgvCart.Columns["Quantity"].HeaderText = "Số Lượng";
                if (dgvCart.Columns["Price"] != null) dgvCart.Columns["Price"].HeaderText = "Đơn Giá";
                if (dgvCart.Columns["Total"] != null) dgvCart.Columns["Total"].HeaderText = "Thành Tiền";

                // Định dạng số
                if (dgvCart.Columns["Price"] != null) dgvCart.Columns["Price"].DefaultCellStyle.Format = "N0";
                if (dgvCart.Columns["Total"] != null) dgvCart.Columns["Total"].DefaultCellStyle.Format = "N0";

                // Căn chỉnh độ rộng
                dgvCart.Columns["ProductName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            // Refresh lại giao diện
            dgvCart.Refresh();
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            // Kiểm tra giỏ hàng có sản phẩm không
            if (ShoppingCart.Items.Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống. Vui lòng thêm sản phẩm vào giỏ hàng!", "Thông báo",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra thông tin bắt buộc
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên người nhận!", "Thông báo",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ giao hàng!", "Thông báo",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddress.Focus();
                return;
            }

            if (cboPayment.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn phương thức thanh toán!", "Thông báo",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboPayment.Focus();
                return;
            }

            // Nếu muốn hiển thị tất cả lỗi cùng lúc (tùy chọn)
            // var errors = new List<string>();
            // if (string.IsNullOrWhiteSpace(txtName.Text)) errors.Add("Tên người nhận");
            // if (string.IsNullOrWhiteSpace(txtPhone.Text)) errors.Add("Số điện thoại");
            // if (string.IsNullOrWhiteSpace(txtAddress.Text)) errors.Add("Địa chỉ giao hàng");
            // if (cboPayment.SelectedItem == null) errors.Add("Phương thức thanh toán");
            // 
            // if (errors.Count > 0)
            // {
            //     MessageBox.Show("Vui lòng điền đầy đủ thông tin:\n" + string.Join("\n", errors.Select(e => $"• {e}")), 
            //                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //     return;
            // }

            // Kiểm tra định dạng số điện thoại (tùy chọn)
            if (!IsValidPhoneNumber(txtPhone.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Vui lòng kiểm tra lại.", "Thông báo",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                txtPhone.SelectAll();
                return;
            }

            // Hiển thị xác nhận trước khi thanh toán (tùy chọn)
            DialogResult confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn đặt hàng?\n\n" +
                $"Tên: {txtName.Text}\n" +
                $"SĐT: {txtPhone.Text}\n" +
                $"Địa chỉ: {txtAddress.Text}\n" +
                $"Phương thức: {cboPayment.SelectedItem}\n" +
                $"Tổng tiền: {ShoppingCart.GetGrandTotal():N0} VNĐ",
                "Xác nhận đặt hàng",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            // Phần code thanh toán hiện tại của bạn...
            OracleConnection conn = DataBase.Get_Connect();
            if (conn.State != ConnectionState.Open) conn.Open();

            OracleTransaction transaction = conn.BeginTransaction();

            try
            {
                // TẠO ORDER 
                OracleCommand cmdHeader = new OracleCommand("NAM_DOAN.P_CREATE_ORDER", conn);
                cmdHeader.CommandType = CommandType.StoredProcedure;
                cmdHeader.Transaction = transaction;

                // Các tham số 
                cmdHeader.Parameters.Add("p_user_id", OracleDbType.Int32).Value = _userId;
                cmdHeader.Parameters.Add("p_contact_name", OracleDbType.NVarchar2).Value = txtName.Text;
                cmdHeader.Parameters.Add("p_phone", OracleDbType.Varchar2).Value = txtPhone.Text;
                cmdHeader.Parameters.Add("p_address", OracleDbType.NVarchar2).Value = txtAddress.Text;

                string selectedMethod = cboPayment.SelectedItem.ToString();
                cmdHeader.Parameters.Add("p_payment_method", OracleDbType.NVarchar2).Value = selectedMethod;

                // Tham số Output ID
                var outIdParam = new OracleParameter("p_order_id", OracleDbType.Int32, ParameterDirection.Output);
                cmdHeader.Parameters.Add(outIdParam);

                cmdHeader.ExecuteNonQuery();

                // Xử lý lấy ID trả về
                int newOrderId;
                if (outIdParam.Value is OracleDecimal oraDec)
                    newOrderId = oraDec.ToInt32();
                else
                    newOrderId = Convert.ToInt32(outIdParam.Value);

                // TẠO ORDER DETAILS
                foreach (var item in ShoppingCart.Items)
                {
                    OracleCommand cmdDetail = new OracleCommand("NAM_DOAN.P_ADD_ORDER_DETAIL", conn);
                    cmdDetail.CommandType = CommandType.StoredProcedure;
                    cmdDetail.Transaction = transaction;

                    cmdDetail.Parameters.Add("p_order_id", OracleDbType.Int32).Value = newOrderId;
                    cmdDetail.Parameters.Add("p_product_id", OracleDbType.Int32).Value = item.ProductId;
                    cmdDetail.Parameters.Add("p_quantity", OracleDbType.Int32).Value = item.Quantity;

                    cmdDetail.ExecuteNonQuery();
                }

                transaction.Commit();

                // ====== 1. TẠO FILE PDF ======
                string pdfPath = GenerateOrderPdf(newOrderId);

                // ====== 2. MÃ HÓA PDF QUA ORACLE ======
                byte[] encryptedData = EncryptPDF_Oracle(pdfPath, conn);

                // ====== 3. LƯU FILE .ENC ======
                string encPath = SaveEncryptedFile(encryptedData, newOrderId);

                // ====== 4. GIẢI MÃ LẠI FILE .ENC ======
                byte[] decryptedData = DecryptPDF_Oracle(encryptedData, conn);

                // ====== 5. LƯU PDF ĐÃ GIẢI MÃ ======
                string decryptedPdfPath = SaveDecryptedFile(decryptedData, newOrderId);

                MessageBox.Show($"Đặt hàng thành công! Mã đơn: {newOrderId}\nPhương thức: {selectedMethod}",
                               "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShoppingCart.Clear();
                this.Close();
            }
            catch (Exception ex)
            {
                try { transaction.Rollback(); } catch { }
                MessageBox.Show("Lỗi thanh toán: " + ex.Message, "Lỗi",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        // Hàm kiểm tra số điện thoại (tùy chọn)
        private bool IsValidPhoneNumber(string phone)
        {
            // Chỉ cho phép số, dấu +, khoảng trắng
            if (string.IsNullOrWhiteSpace(phone)) return false;

            // Loại bỏ khoảng trắng và dấu +
            string cleanPhone = phone.Replace(" ", "").Replace("+", "");

            // Kiểm tra chỉ chứa số và có độ dài hợp lý (10-11 số)
            if (!long.TryParse(cleanPhone, out _)) return false;

            return cleanPhone.Length >= 10 && cleanPhone.Length <= 11;
        }
        private string GenerateOrderPdf(int orderId)
        {
            string folder = Path.Combine(Application.StartupPath, "OrdersPDF");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string filePath = Path.Combine(folder, $"Order_{orderId}.pdf");

            Document doc = new Document(PageSize.A4);
            PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            doc.Open();

            // ======== TIÊU ĐỀ ========
            var titleFont = FontFactory.GetFont("Arial", "20", Font.Bold);
            doc.Add(new Paragraph("HOÁ ĐƠN ĐẶT HÀNG", titleFont));
            doc.Add(new Paragraph(" "));

            // ======== THÔNG TIN KHÁCH ========
            var infoFont = FontFactory.GetFont("Arial", "12", Font.Bold);
            doc.Add(new Paragraph($"Mã đơn: {orderId}", infoFont));
            doc.Add(new Paragraph($"Tên: {txtName.Text}", infoFont));
            doc.Add(new Paragraph($"SĐT: {txtPhone.Text}", infoFont));
            doc.Add(new Paragraph($"Địa chỉ: {txtAddress.Text}", infoFont));
            doc.Add(new Paragraph($"Phương thức: {cboPayment.SelectedItem}", infoFont));
            doc.Add(new Paragraph(" "));

            // ======== DANH SÁCH SẢN PHẨM ========
            PdfPTable table = new PdfPTable(4);
            table.AddCell("Tên SP");
            table.AddCell("SL");
            table.AddCell("Giá");
            table.AddCell("Thành tiền");

            foreach (var item in ShoppingCart.Items)
            {
                table.AddCell(item.ProductName);
                table.AddCell(item.Quantity.ToString());
                table.AddCell(item.Price.ToString("N0"));
                table.AddCell(item.Total.ToString("N0"));
            }

            doc.Add(table);

            doc.Close();
            return filePath;
        }
        private byte[] EncryptPDF_Oracle(string pdfFilePath, OracleConnection conn)
        {
            byte[] input = File.ReadAllBytes(pdfFilePath);

            OracleCommand cmd = new OracleCommand("NAM_DOAN.DES_PKG.encrypt_file", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // Input BLOB
            OracleParameter pInput = new OracleParameter("p_input", OracleDbType.Blob);
            pInput.Direction = ParameterDirection.Input;
            pInput.Value = input;
            cmd.Parameters.Add(pInput);

            // KEY DES (8 ký tự)
            cmd.Parameters.Add("p_key", OracleDbType.Varchar2).Value = "12345678";

            // Output BLOB
            OracleParameter pOutput = new OracleParameter("p_output", OracleDbType.Blob);
            pOutput.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(pOutput);

            cmd.ExecuteNonQuery();

            OracleBlob encryptedBlob = (OracleBlob)pOutput.Value;
            byte[] encryptedBytes = new byte[encryptedBlob.Length];
            encryptedBlob.Read(encryptedBytes, 0, encryptedBytes.Length);

            return encryptedBytes;
        }
        private byte[] DecryptPDF_Oracle(byte[] encryptedData, OracleConnection conn)
        {
            OracleCommand cmd = new OracleCommand("NAM_DOAN.DES_PKG.decrypt_file", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // Input encrypted BLOB
            OracleParameter pInput = new OracleParameter("p_input", OracleDbType.Blob);
            pInput.Direction = ParameterDirection.Input;
            pInput.Value = encryptedData;
            cmd.Parameters.Add(pInput);

            // Key DES
            cmd.Parameters.Add("p_key", OracleDbType.Varchar2).Value = "12345678";

            // Output decrypted BLOB
            OracleParameter pOutput = new OracleParameter("p_output", OracleDbType.Blob);
            pOutput.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(pOutput);

            cmd.ExecuteNonQuery();

            OracleBlob decryptedBlob = (OracleBlob)pOutput.Value;
            byte[] decryptedBytes = new byte[decryptedBlob.Length];
            decryptedBlob.Read(decryptedBytes, 0, decryptedBytes.Length);

            return decryptedBytes;
        }

        private string SaveEncryptedFile(byte[] data, int orderId)
        {
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "OrdersPDF_DES");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string encryptedPath = Path.Combine(path, $"Order_{orderId}.enc");

            File.WriteAllBytes(encryptedPath, data);
            return encryptedPath;
        }
        private string SaveDecryptedFile(byte[] data, int orderId)
        {
            string folderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "OrdersPDF_DES");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string pdfPath = Path.Combine(folderPath, $"Order_{orderId}_decrypted.pdf");

            File.WriteAllBytes(pdfPath, data);

            return pdfPath;
        }

    }
}
