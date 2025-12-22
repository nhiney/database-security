using DA_N6.Services;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;
using ZXing;

namespace DA_N6.Forms
{
    public partial class LoginSecure : Form
    {
        private VideoCapture capture;
        private Timer timer;

        // AES key / IV demo
        private byte[] aesKey = new byte[16];
        private byte[] aesIV = new byte[16];

        // RSA demo
        private RSAParameters rsaPrivate;
        private RSAParameters rsaPublic;

        public LoginSecure()
        {
            InitializeComponent();

            // Khởi tạo webcam
            capture = new VideoCapture(0);
            if (!capture.IsOpened())
            {
                MessageBox.Show("Không thể mở camera!");
                return;
            }

            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += Timer_Tick;

            // Sinh AES key và IV demo
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(aesKey);
                rng.GetBytes(aesIV);
            }

            // Sinh RSA key pair
            using (RSA rsa = RSA.Create(2048))
            {
                rsaPrivate = rsa.ExportParameters(true);
                rsaPublic = rsa.ExportParameters(false);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            using (Mat frame = new Mat())
            {
                capture.Read(frame);
                if (!frame.Empty())
                {
                    // Chuyển Mat -> Bitmap
                    Bitmap bitmap = BitmapConverter.ToBitmap(frame);
                    pictureBox1.Image?.Dispose();
                    pictureBox1.Image = bitmap;

                    // QR code detection
                    BarcodeReader reader = new BarcodeReader();
                    var result = reader.Decode(bitmap);
                    if (result != null)
                    {
                        txtQRCode.Text = result.Text;
                        timer.Stop();
                        MessageBox.Show("QR code đã quét: " + result.Text);
                    }
                }
            }
        }

        private void btnStartCamera_Click(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void btnFaceLogin_Click(object sender, EventArgs e)
        {
            using (Mat frame = new Mat())
            {
                capture.Read(frame);
                if (!frame.Empty() && FaceRecognitionService.ValidateFace(frame))
                {
                    MessageBox.Show("Xác thực khuôn mặt thành công!");
                }
                else
                {
                    MessageBox.Show("Không nhận diện được khuôn mặt!");
                }
            }
        }

        private void btnEncryptAES_Click(object sender, EventArgs e)
        {
            string text = txtInput.Text;
            byte[] cipher = SymmetricCryptoService.Encrypt(text, aesKey, aesIV);
            txtOutput.Text = Convert.ToBase64String(cipher);
        }

        private void btnDecryptAES_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] cipher = Convert.FromBase64String(txtOutput.Text);
                string plain = SymmetricCryptoService.Decrypt(cipher, aesKey, aesIV);
                MessageBox.Show("AES Decrypted: " + plain);
            }
            catch
            {
                MessageBox.Show("Giá trị đầu ra không hợp lệ!");
            }
        }

        private void btnEncryptRSA_Click(object sender, EventArgs e)
        {
            string text = txtInput.Text;
            byte[] cipher = AsymmetricCryptoService.Encrypt(text, rsaPublic);
            txtOutput.Text = Convert.ToBase64String(cipher);
        }

        private void btnDecryptRSA_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] cipher = Convert.FromBase64String(txtOutput.Text);
                string plain = AsymmetricCryptoService.Decrypt(cipher, rsaPrivate);
                MessageBox.Show("RSA Decrypted: " + plain);
            }
            catch
            {
                MessageBox.Show("Giá trị đầu ra không hợp lệ!");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            timer?.Stop();
            capture?.Release();
            capture?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
