using System;
using System.Drawing;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using DA_N6.Database;
using DA_N6.Utils;
using System.IO;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using ZXing;
using System.Data;

namespace DA_N6
{
    public partial class Register : Form
    {
        private VideoCapture capture;
        private Timer timerCapture;
        private Bitmap capturedFaceBitmap;
        private string generatedQRCode;
        private CascadeClassifier faceCascade;

        public Register()
        {
            InitializeComponent();
            CenterToScreen();

            try
            {
                faceCascade = new CascadeClassifier("haarcascade_frontalface_default.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tìm thấy file nhận diện khuôn mặt (xml)!\n" + ex.Message);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string newUser = txtUsername.Text.Trim();
            string newPass = txtPassword.Text.Trim();
            string email = txtEmail.Text.Trim();
            string address = txtAddress.Text.Trim();

            if (string.IsNullOrEmpty(newUser) || string.IsNullOrEmpty(newPass))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên tài khoản và mật khẩu!");
                return;
            }

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập email!");
                return;
            }

            // Mã hóa tầng 1 (client)
            string encryptedUserLv1 = EncryptUtils.EncryptMultiplicative(newUser, 7);
            string encryptedPassLv1 = EncryptUtils.EncryptMultiplicative(newPass, 7);

            try
            {
                // ---- DÙNG CHUẨN USER NHƯ ĐOẠN 1 ----
                DataBase.Set_DataBase("127.0.0.1", "1521", "ORCL", "NAM_DOAN", "NAM_DOAN");

                if (!DataBase.Connect())
                {
                    MessageBox.Show("Không thể kết nối tới user NAM_DOAN!");
                    return;
                }

                // 1. REGISTER STANDARD USER
                DatabaseHelper.ExecuteProcedure(
                    "NAM_DOAN.ENCRYPT_PKG.P_REGISTER_USER",
                    new OracleParameter("p_username", OracleDbType.Varchar2, encryptedUserLv1, ParameterDirection.Input),
                    new OracleParameter("p_password", OracleDbType.Varchar2, encryptedPassLv1, ParameterDirection.Input),
                    new OracleParameter("p_email", OracleDbType.Varchar2, email, ParameterDirection.Input),
                    new OracleParameter("p_addr", OracleDbType.NVarchar2, address, ParameterDirection.Input)
                );

                // 2. SAVE BIOMETRICS (If available)
                bool bioSuccess = true;
                string bioMsg = "";

                // Face
                if (capturedFaceBitmap != null)
                {
                    try
                    {
                        // 1. Get Encrypted Username (Lv2)
                        object encUserLv2 = DatabaseHelper.ExecuteFunction(
                            "NAM_DOAN.ENCRYPT_PKG.F_ENCRYPT_DB",
                            OracleDbType.NVarchar2,
                            new OracleParameter("p_input", OracleDbType.NVarchar2, encryptedUserLv1, ParameterDirection.Input),
                            new OracleParameter("p_key", OracleDbType.Int32, 9, ParameterDirection.Input)
                        );
                        string finalUser = encUserLv2?.ToString(); 

                        // 2. Resize Image & Convert
                        // Specify System.Drawing.Size explicitly to avoid OpenCvSharp ambiguity
                        Bitmap resized = new Bitmap(capturedFaceBitmap, 200, 200);
                        byte[] faceBytes = ImageToByteArray(resized);
 
                        // 3. Save to DB
                         DatabaseHelper.ExecuteProcedure(
                            "NAM_DOAN.AUTH_EXT_PKG.P_UPDATE_FACE_IMG",
                            new OracleParameter("p_username", OracleDbType.Varchar2, finalUser, ParameterDirection.Input),
                            new OracleParameter("p_img", OracleDbType.Blob) { Value = faceBytes }
                        );
                        bioMsg += "+ Face ID ";
                    }
                    catch (Exception ex) { bioSuccess = false; bioMsg += "(Face Error: " + ex.Message + ") "; }
                }

                // QR
                if (!string.IsNullOrEmpty(generatedQRCode))
                {
                    try
                    {
                        // Same logic: use finalUser
                         object encUserLv2 = DatabaseHelper.ExecuteFunction(
                            "NAM_DOAN.ENCRYPT_PKG.F_ENCRYPT_DB",
                            OracleDbType.NVarchar2,
                            new OracleParameter("p_input", OracleDbType.NVarchar2, encryptedUserLv1, ParameterDirection.Input),
                            new OracleParameter("p_key", OracleDbType.Int32, 9, ParameterDirection.Input)
                        );
                        string finalUser = encUserLv2?.ToString();

                        DatabaseHelper.ExecuteProcedure(
                            "NAM_DOAN.AUTH_EXT_PKG.P_UPDATE_QR_CODE",
                            new OracleParameter("p_username", OracleDbType.Varchar2, finalUser, ParameterDirection.Input),
                            new OracleParameter("p_qr", OracleDbType.Varchar2, generatedQRCode, ParameterDirection.Input)
                        );
                         bioMsg += "+ QR Code ";
                    }
                     catch (Exception ex) { bioSuccess = false; bioMsg += "(QR Error: " + ex.Message + ") "; }
                }

                MessageBox.Show($"Đăng ký tài khoản {newUser} thành công!\n{bioMsg}\nBạn có thể đăng nhập ngay bây giờ.");

                DataBase.Disconnect();

                StopCamera(); // Ensure camera off
                this.Hide();
                new Login(true).ShowDialog(); // Auto-start Face Scan
                this.Close();
            }
            catch (OracleException ex)
            {
                string msg = ex.Message;
                if (msg.Contains("ORA-20012"))
                    MessageBox.Show("Tên người dùng đã tồn tại!");
                else if (msg.Contains("ORA-20010"))
                    MessageBox.Show("Lỗi khi tạo tài khoản trong Oracle!");
                else
                    MessageBox.Show("Lỗi Oracle: " + msg);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đăng ký: " + ex.Message);
            }
        }

        // ===================================
        // BIOMETRIC HANDLERS
        // ===================================

        private void btnCaptureFace_Click(object sender, EventArgs e)
        {
            if (capture == null)
            {
                StartCamera();
                btnCaptureFace.Text = "Chụp";
            }
            else
            {
                // Capture current frame
                if (picFace.Image != null)
                {
                    Bitmap currentFrame = (Bitmap)picFace.Image.Clone();
                    
                    // Detect and Crop
                    Bitmap croppedFace = DetectAndCropFace(currentFrame);
                    
                    if (croppedFace != null)
                    {
                        capturedFaceBitmap = croppedFace;
                        picFace.Image = capturedFaceBitmap; // Update UI with cropped face
                        StopCamera();
                        btnCaptureFace.Text = "Chụp Lại";
                        MessageBox.Show("Đã chụp và nhận diện được khuôn mặt!");
                    }
                    else
                    {
                         MessageBox.Show("Không tìm thấy khuôn mặt nào trong khung hình! Vui lòng thử lại.");
                    }
                    
                    currentFrame.Dispose();
                }
            }
        }

        private void btnUploadFace_Click(object sender, EventArgs e)
        {
            // If camera is running, stop it first
            if (capture != null)
            {
                StopCamera();
                btnCaptureFace.Text = "Chụp Lại";
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            ofd.Title = "Chọn ảnh khuôn mặt";
            
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Load image
                    Bitmap loadedImage = new Bitmap(ofd.FileName);
                    picFace.Image = loadedImage;
                    capturedFaceBitmap = (Bitmap)loadedImage.Clone();
                    MessageBox.Show("Đã tải ảnh lên!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải ảnh: " + ex.Message);
                }
            }
        }

        private void btnGenerateQR_Click(object sender, EventArgs e)
        {
            // Simple QR Generation: UserID + Random UUID to ensure uniqueness
            // Since we don't have UserID yet, we use Random UUID.
            // On Login, we verify if this unique code exists in USERS table.
            string token = Guid.NewGuid().ToString();
            generatedQRCode = token;

            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 300,
                    Width = 300
                }
            };
            var result = writer.Write(token);
            picQR.Image = result;
            MessageBox.Show("Đã tạo mã QR! Hãy lưu lại hoặc chụp ảnh mã này để đăng nhập.");
        }

        // ===================================
        // CAMERA LOGIC
        // ===================================

        private void StartCamera()
        {
            if (capture == null)
            {
                try
                {
                    capture = new VideoCapture(0);
                }
                catch
                {
                    MessageBox.Show("Không tìm thấy camera!");
                    return;
                }
            }
            if (timerCapture == null)
            {
                timerCapture = new Timer();
                timerCapture.Interval = 30;
                timerCapture.Tick += TimerCapture_Tick;
            }
            timerCapture.Start();
        }

        private void StopCamera()
        {
            timerCapture?.Stop();
            capture?.Dispose();
            capture = null;
        }

        private void TimerCapture_Tick(object sender, EventArgs e)
        {
            if (capture != null && capture.IsOpened())
            {
                using (Mat frame = new Mat())
                {
                    capture.Read(frame);
                    if (!frame.Empty())
                    {
                        var bmp = BitmapConverter.ToBitmap(frame);
                        picFace.Image?.Dispose();
                        picFace.Image = bmp;
                    }
                }
            }
        }

        private byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private Bitmap DetectAndCropFace(Bitmap original)
        {
            if (faceCascade == null || original == null) return null;

            try
            {
                using (Mat mat = BitmapConverter.ToMat(original))
                using (Mat gray = new Mat())
                {
                    Cv2.CvtColor(mat, gray, ColorConversionCodes.BGR2GRAY);
                    Cv2.EqualizeHist(gray, gray);

                    var faces = faceCascade.DetectMultiScale(
                        image: gray,
                        scaleFactor: 1.1,
                        minNeighbors: 5,
                        flags: HaarDetectionTypes.ScaleImage,
                        minSize: new OpenCvSharp.Size(30, 30)
                    );

                    if (faces.Length > 0)
                    {
                        // Get largest face
                        Rect largestFace = faces[0];
                        foreach (var face in faces)
                        {
                            if (face.Width * face.Height > largestFace.Width * largestFace.Height)
                                largestFace = face;
                        }

                        // Crop
                        Mat faceMat = new Mat(mat, largestFace);
                        return BitmapConverter.ToBitmap(faceMat);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Face Detect Error: " + ex.Message);
            }
            return null;
        }

        // ===================================
        // UI HELPERS (Unchanged)
        // ===================================

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Login().ShowDialog();
            this.Close();
        }
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void PanelCard_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            int radius = 20;
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(panel.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(panel.Width - radius, panel.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, panel.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            panel.Region = new Region(path);
            using (Pen pen = new Pen(Color.FromArgb(230, 230, 230), 1))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }
        private void txtUsername_Enter(object sender, EventArgs e)
        {
            if (txtUsername.Text == "Nhập tên đăng nhập") { txtUsername.Text = ""; txtUsername.ForeColor = Color.Black; }
        }
        private void txtUsername_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text)) { txtUsername.Text = "Nhập tên đăng nhập"; txtUsername.ForeColor = Color.Gray; }
        }
        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Nhập mật khẩu") { txtPassword.Text = ""; txtPassword.ForeColor = Color.Black; txtPassword.UseSystemPasswordChar = true; }
        }
        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text)) { txtPassword.Text = "Nhập mật khẩu"; txtPassword.ForeColor = Color.Gray; txtPassword.UseSystemPasswordChar = false; }
        }
        private void txtEmail_Enter(object sender, EventArgs e)
        {
            if (txtEmail.Text == "Nhập email") { txtEmail.Text = ""; txtEmail.ForeColor = Color.Black; }
        }
        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text)) { txtEmail.Text = "Nhập email"; txtEmail.ForeColor = Color.Gray; }
        }
        private void txtAddress_Enter(object sender, EventArgs e)
        {
            if (txtAddress.Text == "Nhập địa chỉ") { txtAddress.Text = ""; txtAddress.ForeColor = Color.Black; }
        }
        private void txtAddress_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddress.Text)) { txtAddress.Text = "Nhập địa chỉ"; txtAddress.ForeColor = Color.Gray; }
        }
    }
}
