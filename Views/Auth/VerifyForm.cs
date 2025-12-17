using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace DA_N6.views
{
    public partial class VerifyForm : Form
    {
        public VerifyForm()
        {
            InitializeComponent();
        }

        // 1. Chọn File Hóa đơn (.txt)
        private void btnBrowseData_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PDF Files|*.pdf|All Files|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtDataPath.Text = ofd.FileName;
            }
        }

        // 2. Chọn File Chữ ký (.sig)
        private void btnBrowseSig_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Signature Files|*.sig|All Files|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtSigPath.Text = ofd.FileName;
            }
        }

        // 3. Chọn File Public Key (.xml)
        private void btnBrowseKey_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML Key|*.xml|All Files|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtKeyPath.Text = ofd.FileName;
            }
        }

        // 4. Nút KIỂM TRA
        private void btnVerify_Click(object sender, EventArgs e)
        {
            // Reset kết quả
            lblResult.Text = "Đang kiểm tra...";
            lblResult.ForeColor = Color.Black;

            // Kiểm tra xem đã chọn đủ file chưa
            if (string.IsNullOrEmpty(txtDataPath.Text) ||
                string.IsNullOrEmpty(txtSigPath.Text) ||
                string.IsNullOrEmpty(txtKeyPath.Text))
            {
                MessageBox.Show("Vui lòng chọn đủ 3 file (Hóa đơn, Chữ ký, Khóa)!", "Thiếu thông tin");
                return;
            }

            try
            {
                // ReadAllBytes rồi chuyển sang Base64 
                byte[] fileBytes = File.ReadAllBytes(txtDataPath.Text);
                string dataContent = Convert.ToBase64String(fileBytes); // Chuyển PDF sang chuỗi để verify

                string signatureBase64 = File.ReadAllText(txtSigPath.Text);
                string publicKeyXml = File.ReadAllText(txtKeyPath.Text);

                // Gọi hàm VerifyData (Hàm này vẫn nhận string, nên ta truyền chuỗi Base64 của PDF vào)
                bool isValid = DA_N6.Utils.DigitalSignatureHelper.VerifyData(dataContent, signatureBase64, publicKeyXml);

                if (isValid)
                {
                    lblResult.Text = "File PDF toàn vẹn.";
                    lblResult.ForeColor = Color.Green;
                    MessageBox.Show("Xác thực thành công!", "Thông báo");
                }
                else
                {
                    lblResult.Text = "CẢNH BÁO! File PDF đã bị chỉnh sửa.";
                    lblResult.ForeColor = Color.Red;
                    MessageBox.Show("Chữ ký không khớp!", "Cảnh báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}