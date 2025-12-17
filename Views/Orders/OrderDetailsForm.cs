using DA_N6.Database;
using DA_N6.Repositories;
using DA_N6.Utils;   // Chứa ReadNumberHelper và DigitalSignatureHelper
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;

namespace DA_N6.views.orders
{
    public partial class OrderDetailForm : Form
    {
        private int _orderId;
        private bool _isSysUser;

        public OrderDetailForm(int orderId, bool isSysUser)
        {
            InitializeComponent();
            _orderId = orderId;
            _isSysUser = isSysUser;
            lblOrderId.Text = $"Chi tiết đơn hàng #{_orderId}";

            this.Load += OrderDetailForm_Load;
        }

        private void OrderDetailForm_Load(object sender, EventArgs e)
        {
            if (!_isSysUser)
            {
                btnSignAndSend.Visible = false;
            }
            LoadDetails();
        }

        // LOAD DỮ LIỆU
        private void LoadDetails()
        {
            try
            {
                var pReturn = new OracleParameter("rc", OracleDbType.RefCursor, ParameterDirection.ReturnValue);
                var pOrderId = new OracleParameter("p_order_id", OracleDbType.Int32) { Value = _orderId };

                DataTable dt = DatabaseHelper.ExecuteFunctionToTable("NAM_DOAN.F_GET_ORDER_DETAILS", pReturn, pOrderId);

                if (!dt.Columns.Contains("TOTAL"))
                {
                    dt.Columns.Add("TOTAL", typeof(decimal));
                }

                foreach (DataRow row in dt.Rows)
                {
                    decimal qty = Convert.ToDecimal(row["QUANTITY"]);
                    decimal price = Convert.ToDecimal(row["PRICE"]);
                    row["TOTAL"] = qty * price;
                }

                dgvDetail.DataSource = dt;

                // Định dạng hiển thị GridView
                if (dgvDetail.Columns.Count > 0)
                {
                    if (dgvDetail.Columns["PRODUCT_NAME"] != null) dgvDetail.Columns["PRODUCT_NAME"].HeaderText = "Tên Sản Phẩm";
                    if (dgvDetail.Columns["QUANTITY"] != null) dgvDetail.Columns["QUANTITY"].HeaderText = "Số Lượng";
                    if (dgvDetail.Columns["PRICE"] != null)
                    {
                        dgvDetail.Columns["PRICE"].HeaderText = "Đơn Giá";
                        dgvDetail.Columns["PRICE"].DefaultCellStyle.Format = "N0";
                    }
                    if (dgvDetail.Columns["TOTAL"] != null)
                    {
                        dgvDetail.Columns["TOTAL"].HeaderText = "Thành Tiền";
                        dgvDetail.Columns["TOTAL"].DefaultCellStyle.Format = "N0";
                    }
                    if (dgvDetail.Columns["ORDER_ID"] != null) dgvDetail.Columns["ORDER_ID"].Visible = false;
                    if (dgvDetail.Columns["PRODUCT_ID"] != null) dgvDetail.Columns["PRODUCT_ID"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết: " + ex.Message);
            }
        }

        // KÝ SỐ VÀ GỬI MAIL
        private void btnSignAndSend_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Tạo file PDF
                string pdfPath = $"Invoice_{_orderId}.pdf";
                ExportToPdf(pdfPath); // Gọi hàm xuất PDF mới

                // 2. Tạo Key
                string publicKey, privateKey;
                DigitalSignatureHelper.GenerateKeys(out publicKey, out privateKey);

                string keyPath = $"PublicKey_{_orderId}.xml";
                File.WriteAllText(keyPath, publicKey);

                // 3. Ký số trên file PDF
                byte[] pdfBytes = File.ReadAllBytes(pdfPath);
                string pdfBase64 = Convert.ToBase64String(pdfBytes);
                string signature = DigitalSignatureHelper.SignData(pdfBase64, privateKey);

                string sigPath = $"Signature_{_orderId}.sig";
                File.WriteAllText(sigPath, signature);

                // 4. Lưu vào Database
                // Sử dụng cách gọi trực tiếp nếu class OrderDetailRepository là static
                // Nếu không static thì dùng: new OrderDetailRepository().UpdateSignature(...)
                DA_N6.Repositories.OrderDetailRepository repo = new DA_N6.Repositories.OrderDetailRepository();
                repo.UpdateSignature(_orderId, signature);

                // 5. Gửi Email
                string customerEmail = "hoangnam27062004@gmail.com";
                SendEmail(customerEmail, pdfPath, sigPath, keyPath);

                MessageBox.Show("Đã xuất PDF, ký số và gửi mail thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSignAndSend.Enabled = false;
                btnSignAndSend.Text = "Đã ký số";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- HÀM TẠO PDF CHUYÊN NGHIỆP ---
        private void ExportToPdf(string savePath)
        {
            Document doc = new Document(PageSize.A4, 25, 25, 20, 20);
            try
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(savePath, FileMode.Create));
                doc.Open();

                // Font chữ
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.ttf");
                BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                var fCompany = new iTextSharp.text.Font(bf, 14, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                var fInfo = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                var fTitle = new iTextSharp.text.Font(bf, 18, iTextSharp.text.Font.BOLD, BaseColor.RED);
                var fHeaderTable = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                var fBodyTable = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                var fItalic = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.ITALIC, BaseColor.BLACK);
                var fSign = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

                // Header Table (Công ty)
                PdfPTable headerTable = new PdfPTable(2);
                headerTable.WidthPercentage = 100;
                headerTable.SetWidths(new float[] { 70f, 30f });

                PdfPCell cellLeft = new PdfPCell();
                cellLeft.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cellLeft.AddElement(new Paragraph("CÔNG TY CỔ PHẦN CÔNG NGHỆ NHOM06", fCompany));
                cellLeft.AddElement(new Paragraph("Mã số thuế: 0123456789", fInfo));
                cellLeft.AddElement(new Paragraph("Địa chỉ: 140 Lê Trọng Tấn", fInfo));
                cellLeft.AddElement(new Paragraph("Điện thoại: (012) 3456 7890 - Website: www.nhom06.vn", fInfo));
                headerTable.AddCell(cellLeft);

                PdfPCell cellRight = new PdfPCell();
                cellRight.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cellRight.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellRight.AddElement(new Paragraph($"Mẫu số: 01GTKT0/001", fInfo) { Alignment = Element.ALIGN_RIGHT });
                cellRight.AddElement(new Paragraph($"Ký hiệu: AB/25E", fInfo) { Alignment = Element.ALIGN_RIGHT });
                cellRight.AddElement(new Paragraph($"Số: {_orderId.ToString("D7")}", fInfo) { Alignment = Element.ALIGN_RIGHT });
                headerTable.AddCell(cellRight);

                doc.Add(headerTable);

                // Tiêu đề
                doc.Add(new Paragraph("\n"));
                Paragraph title = new Paragraph("HÓA ĐƠN", fTitle);
                title.Alignment = Element.ALIGN_CENTER;
                doc.Add(title);

                Paragraph date = new Paragraph($"Ngày {DateTime.Now.Day} tháng {DateTime.Now.Month} năm {DateTime.Now.Year}", fItalic);
                date.Alignment = Element.ALIGN_CENTER;
                doc.Add(date);
                doc.Add(new Paragraph("\n"));

                // Thông tin khách hàng
                doc.Add(new Paragraph($"Họ tên người mua hàng: Khách lẻ (Đơn #{_orderId})", fInfo));
                doc.Add(new Paragraph("Đơn vị: .......................................................................................................", fInfo));
                doc.Add(new Paragraph("Địa chỉ: .................................................................................................................", fInfo));
                doc.Add(new Paragraph("Hình thức thanh toán: TM/CK                         Mã số thuế: ...........................................", fInfo));
                doc.Add(new Paragraph("\n"));

                // Bảng chi tiết
                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 5f, 35f, 10f, 10f, 20f, 20f });

                // Header Bảng
                string[] headers = { "STT", "Tên hàng hóa, dịch vụ", "ĐVT", "Số lượng", "Đơn giá", "Thành tiền" };
                foreach (string h in headers)
                {
                    AddCellToTable(table, h, fHeaderTable, Element.ALIGN_CENTER);
                }

                // Dữ liệu từ GridView
                decimal totalGoods = 0;
                int stt = 1;

                foreach (DataGridViewRow row in dgvDetail.Rows)
                {
                    if (row.Cells["PRODUCT_NAME"].Value == null) continue;

                    string name = row.Cells["PRODUCT_NAME"].Value.ToString();
                    decimal qty = Convert.ToDecimal(row.Cells["QUANTITY"].Value);
                    decimal price = Convert.ToDecimal(row.Cells["PRICE"].Value);
                    decimal total = Convert.ToDecimal(row.Cells["TOTAL"].Value);
                    totalGoods += total;

                    AddCellToTable(table, stt++.ToString(), fBodyTable, Element.ALIGN_CENTER);
                    AddCellToTable(table, name, fBodyTable, Element.ALIGN_LEFT);
                    AddCellToTable(table, "Cái", fBodyTable, Element.ALIGN_CENTER);
                    AddCellToTable(table, qty.ToString("N0"), fBodyTable, Element.ALIGN_RIGHT);
                    AddCellToTable(table, price.ToString("N0"), fBodyTable, Element.ALIGN_RIGHT);
                    AddCellToTable(table, total.ToString("N0"), fBodyTable, Element.ALIGN_RIGHT);
                }

                // Dòng trống
                for (int k = 0; k < 3; k++)
                {
                    for (int i = 0; i < 6; i++) AddCellToTable(table, " ", fBodyTable, Element.ALIGN_CENTER);
                }

                // Tổng kết
                decimal vatRate = 0.1m;
                decimal vatAmount = totalGoods * vatRate;
                decimal grandTotal = totalGoods + vatAmount;

                // Cộng tiền hàng
                PdfPCell cellColspan = new PdfPCell(new Phrase("Cộng tiền hàng:", fHeaderTable));
                cellColspan.Colspan = 5;
                cellColspan.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellColspan.Padding = 5;
                table.AddCell(cellColspan);
                table.AddCell(new PdfPCell(new Phrase(totalGoods.ToString("N0"), fBodyTable)) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5 });

                // Thuế
                cellColspan = new PdfPCell(new Phrase("Tiền thuế GTGT (10%):", fHeaderTable));
                cellColspan.Colspan = 5;
                cellColspan.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellColspan.Padding = 5;
                table.AddCell(cellColspan);
                table.AddCell(new PdfPCell(new Phrase(vatAmount.ToString("N0"), fBodyTable)) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5 });

                // Tổng cộng
                cellColspan = new PdfPCell(new Phrase("Tổng tiền thanh toán:", fHeaderTable));
                cellColspan.Colspan = 5;
                cellColspan.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellColspan.Padding = 5;
                table.AddCell(cellColspan);
                table.AddCell(new PdfPCell(new Phrase(grandTotal.ToString("N0"), fHeaderTable)) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5 });

                doc.Add(table);

                // GỌI HÀM ĐỌC SỐ TỪ ReadNumberHelper 
                doc.Add(new Paragraph($"Số tiền viết bằng chữ: {ReadNumberHelper.ReadNumber(grandTotal)}", fItalic));
                doc.Add(new Paragraph("\n"));

                // Chữ ký
                PdfPTable signTable = new PdfPTable(3);
                signTable.WidthPercentage = 100;

                // Người mua
                PdfPCell cellBuyer = new PdfPCell();
                cellBuyer.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cellBuyer.HorizontalAlignment = Element.ALIGN_CENTER;
                cellBuyer.AddElement(new Paragraph("Người mua hàng", fSign) { Alignment = Element.ALIGN_CENTER });
                cellBuyer.AddElement(new Paragraph("(Ký, ghi rõ họ tên)", fItalic) { Alignment = Element.ALIGN_CENTER });
                signTable.AddCell(cellBuyer);

                // Người bán
                PdfPCell cellSeller = new PdfPCell();
                cellSeller.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cellSeller.HorizontalAlignment = Element.ALIGN_CENTER;
                cellSeller.AddElement(new Paragraph("Người bán hàng", fSign) { Alignment = Element.ALIGN_CENTER });
                cellSeller.AddElement(new Paragraph("(Ký, ghi rõ họ tên)", fItalic) { Alignment = Element.ALIGN_CENTER });
                signTable.AddCell(cellSeller);

                // Ký số
                PdfPCell cellDigital = new PdfPCell();
                cellDigital.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cellDigital.HorizontalAlignment = Element.ALIGN_CENTER;
                cellDigital.AddElement(new Paragraph("Thủ trưởng đơn vị", fSign) { Alignment = Element.ALIGN_CENTER });
                cellDigital.AddElement(new Paragraph("(Ký, đóng dấu)", fItalic) { Alignment = Element.ALIGN_CENTER });
                cellDigital.AddElement(new Paragraph("\n\n\n"));

                // Dấu ký số
                var fDigital = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD, BaseColor.RED);
                Paragraph pDigi = new Paragraph("Ký bởi: CÔNG TY CÔNG NGHỆ NHOM06\nNgày ký: " + DateTime.Now.ToString("dd/MM/yyyy"), fDigital);
                pDigi.Alignment = Element.ALIGN_CENTER;

                PdfPCell stampCell = new PdfPCell(pDigi);
                stampCell.BorderColor = BaseColor.RED;
                stampCell.BorderWidth = 2f;
                stampCell.HorizontalAlignment = Element.ALIGN_CENTER;
                stampCell.Padding = 5;

                PdfPTable stampTable = new PdfPTable(1);
                stampTable.WidthPercentage = 80;
                stampTable.AddCell(stampCell);

                cellDigital.AddElement(stampTable);
                signTable.AddCell(cellDigital);

                doc.Add(signTable);

                // Footer
                Paragraph footer = new Paragraph("\n(Cần kiểm tra, đối chiếu khi lập, giao, nhận hóa đơn)", fItalic);
                footer.Alignment = Element.ALIGN_CENTER;
                doc.Add(footer);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi tạo PDF: " + ex.Message);
            }
            finally
            {
                doc.Close();
            }
        }

        private void AddCellToTable(PdfPTable table, string text, iTextSharp.text.Font font, int alignment)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = alignment;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 5;
            table.AddCell(cell);
        }

        private void SendEmail(string toEmail, string invoicePath, string sigPath, string keyPath)
        {
            string fromEmail = "hoangnam27062004@gmail.com";
            string password = "pwlt hdwa vthq ahhm";

            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true,
            };

            var mail = new MailMessage();
            mail.From = new MailAddress(fromEmail);
            mail.To.Add(toEmail);
            mail.Subject = $"Hóa đơn điện tử #{_orderId} - Đã ký số";
            mail.Body = "Kính gửi quý khách,\n\nĐính kèm là hóa đơn mua hàng và chữ ký số xác thực.";

            mail.Attachments.Add(new Attachment(invoicePath));
            mail.Attachments.Add(new Attachment(sigPath));
            mail.Attachments.Add(new Attachment(keyPath));

            smtp.Send(mail);
        }
    }
}