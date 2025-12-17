using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using DA_N6.Database;
using DA_N6.Utils;
using DA_N6.views;
using DA_N6.Repositories;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using ZXing;

namespace DA_N6
{
    public partial class Login : Form
    {
        private UserRepository userRepository = new UserRepository();
        private VideoCapture capture;
        private Timer timerCapture;
        private bool isScanningFace = false;
        private bool isScanningQR = false;
        private CascadeClassifier faceCascade;

        public Login(bool autoStartFace = false)
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

            if (autoStartFace)
            {
                this.Load += (s, e) => btnFaceLogin_Click(null, null);
            }

            // AUTO-REPAIR DATABASE (Fix Missing Package Error)
            try
            {
                // Ensure connection parameters are set (using standard local dev settings)
                DataBase.Set_DataBase("127.0.0.1", "1521", "ORCL", "NAM_DOAN", "NAM_DOAN");
                if (DataBase.Connect())
                {
                    CheckAndFixDatabase(); // Call the fix
                    DataBase.Disconnect();
                }
            }
            catch (Exception ex)
            {
                // Show error clearly so user knows why DB repair failed
                MessageBox.Show("Auto-Repair DB failed: " + ex.Message);
            }
        }

        private void CheckAndFixDatabase()
        {
            try
            {
                // 1. Check Columns
                string checkCol = "SELECT COUNT(*) FROM USER_TAB_COLS WHERE TABLE_NAME = 'USERS' AND COLUMN_NAME = 'FACE_IMG'";
                if (Convert.ToInt32(DatabaseHelper.ExecuteScalar(checkCol)) == 0)
                {
                    DatabaseHelper.ExecuteSQL("ALTER TABLE USERS ADD (FACE_IMG BLOB)");
                }
                
                checkCol = "SELECT COUNT(*) FROM USER_TAB_COLS WHERE TABLE_NAME = 'USERS' AND COLUMN_NAME = 'QR_CODE'";
                if (Convert.ToInt32(DatabaseHelper.ExecuteScalar(checkCol)) == 0)
                {
                    DatabaseHelper.ExecuteSQL("ALTER TABLE USERS ADD (QR_CODE VARCHAR2(255))");
                }

                // 2. Check Package Validity
                string checkPkg = "SELECT COUNT(*) FROM USER_OBJECTS WHERE OBJECT_NAME = 'AUTH_EXT_PKG' AND STATUS = 'VALID'";
                int validCount = Convert.ToInt32(DatabaseHelper.ExecuteScalar(checkPkg));

                if (validCount < 2) // Need both PACKAGE and PACKAGE BODY valid
                {
                    // Re-create Package Spec
                    string pkgSpec = @"
                        CREATE OR REPLACE PACKAGE NAM_DOAN.AUTH_EXT_PKG AS
                            PROCEDURE P_UPDATE_FACE_IMG(p_username IN VARCHAR2, p_img IN BLOB);
                            PROCEDURE P_UPDATE_QR_CODE(p_username IN VARCHAR2, p_qr IN VARCHAR2);
                            FUNCTION F_GET_USER_BY_QR(p_qr IN VARCHAR2) RETURN VARCHAR2;
                            PROCEDURE P_GET_ALL_FACES(p_cursor OUT SYS_REFCURSOR);
                            PROCEDURE P_GET_USER_CREDENTIALS(p_username IN VARCHAR2, p_enc_user OUT VARCHAR2, p_enc_pass OUT VARCHAR2);
                        END AUTH_EXT_PKG;";
                    DatabaseHelper.ExecuteSQL(pkgSpec);

                    // Re-create Package Body
                    string pkgBody = @"
                        CREATE OR REPLACE PACKAGE BODY NAM_DOAN.AUTH_EXT_PKG AS
                            PROCEDURE P_UPDATE_FACE_IMG(p_username IN VARCHAR2, p_img IN BLOB) IS
                            BEGIN
                                UPDATE USERS SET FACE_IMG = p_img WHERE USER_NAME = p_username;
                                COMMIT;
                            END P_UPDATE_FACE_IMG;

                            PROCEDURE P_UPDATE_QR_CODE(p_username IN VARCHAR2, p_qr IN VARCHAR2) IS
                            BEGIN
                                UPDATE USERS SET QR_CODE = p_qr WHERE USER_NAME = p_username;
                                COMMIT;
                            END P_UPDATE_QR_CODE;

                            FUNCTION F_GET_USER_BY_QR(p_qr IN VARCHAR2) RETURN VARCHAR2 IS
                                v_username VARCHAR2(100);
                            BEGIN
                                SELECT USER_NAME INTO v_username FROM USERS WHERE QR_CODE = p_qr;
                                RETURN v_username;
                            EXCEPTION WHEN NO_DATA_FOUND THEN RETURN NULL;
                            END F_GET_USER_BY_QR;

                            PROCEDURE P_GET_ALL_FACES(p_cursor OUT SYS_REFCURSOR) IS
                            BEGIN
                                OPEN p_cursor FOR SELECT USER_NAME, FACE_IMG FROM USERS WHERE FACE_IMG IS NOT NULL;
                            END P_GET_ALL_FACES;

                            PROCEDURE P_GET_USER_CREDENTIALS(p_username IN VARCHAR2, p_enc_user OUT VARCHAR2, p_enc_pass OUT VARCHAR2) IS
                            BEGIN
                                SELECT USER_NAME, PASSWORD INTO p_enc_user, p_enc_pass FROM USERS WHERE USER_NAME = p_username;
                            EXCEPTION WHEN NO_DATA_FOUND THEN p_enc_user := NULL; p_enc_pass := NULL;
                            END P_GET_USER_CREDENTIALS;
                        END AUTH_EXT_PKG;";
                    DatabaseHelper.ExecuteSQL(pkgBody);
                    
                    // Grant
                    try { DatabaseHelper.ExecuteSQL("GRANT EXECUTE ON NAM_DOAN.AUTH_EXT_PKG TO ROLE_USERS"); } catch {}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Cài đặt Database Tự động: " + ex.Message);
            }
        }

        private bool Check_TextBox(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || username == "Nhập tên đăng nhập" || 
                string.IsNullOrWhiteSpace(password) || password == "Nhập mật khẩu")
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu!");
                return false;
            }
            return true;
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            string host = "127.0.0.1";
            // string host = "26.74.206.69";
            string port = "1521";
            string sid = "ORCL";
            // string sid = "FREEPDB1";
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (!Check_TextBox(username, password))
                return;

            try
            {
                bool isSysUser = false;
                int userId = -1;
                string sidValue = "";
                string serialValue = "";

                // Login
                if (username.Equals("SYS", StringComparison.OrdinalIgnoreCase) ||
                    username.Equals("NAM_DOAN", StringComparison.OrdinalIgnoreCase))
                {
                    DataBase.Set_DataBase(host, port, sid, username, password);
                    if (!DataBase.Connect(true))
                    {
                        MessageBox.Show("Không thể kết nối bằng tài khoản quản trị!");
                        return;
                    }

                    isSysUser = true;
                    userId = 0;
                }
                else
                {
                    // Mã hóa tầng 1 tại ứng dụng
                    string encryptedUserLv1 = EncryptUtils.EncryptMultiplicative(username, 7);
                    string encryptedPassLv1 = EncryptUtils.EncryptMultiplicative(password, 7);

                    // Mã hóa tầng 2 bằng NAM_DOAN
                    DataBase.Set_DataBase("127.0.0.1", "1521", "ORCL", "NAM_DOAN", "NAM_DOAN");
                    if (!DataBase.Connect())
                    {
                        MessageBox.Show("Không thể kết nối tới user NAM_DOAN!");
                        return;
                    }

                    object encUserLv2 = DatabaseHelper.ExecuteFunction(
                        "NAM_DOAN.ENCRYPT_PKG.F_ENCRYPT_DB",
                        OracleDbType.NVarchar2,
                        new OracleParameter("p_input", OracleDbType.NVarchar2, encryptedUserLv1, ParameterDirection.Input),
                        new OracleParameter("p_key", OracleDbType.Int32, 9, ParameterDirection.Input)
                    );

                    object encPassLv2 = DatabaseHelper.ExecuteFunction(
                        "NAM_DOAN.ENCRYPT_PKG.F_ENCRYPT_DB",
                        OracleDbType.NVarchar2,
                        new OracleParameter("p_input", OracleDbType.NVarchar2, encryptedPassLv1, ParameterDirection.Input),
                        new OracleParameter("p_key", OracleDbType.Int32, 9, ParameterDirection.Input)
                    );

                    string finalUser = encUserLv2?.ToString();
                    string finalPass = encPassLv2?.ToString();

                    DataBase.Disconnect();

                    // Login user thật
                    DataBase.Set_DataBase(host, port, sid, finalUser, finalPass);
                    DataBase.Connect(true);

                    Console.WriteLine($"Đã đăng nhập bằng user mã hóa: {finalUser}");

                    // Lấy SID bằng hàm F_GET_SESSION_SID
                    object sidObj = DatabaseHelper.ExecuteFunction(
                        "NAM_DOAN.F_GET_SESSION_SID",
                        OracleDbType.Decimal,
                        new OracleParameter("p_username", OracleDbType.Varchar2, finalUser, ParameterDirection.Input)
                    );

                    object serialObj = DatabaseHelper.ExecuteFunction(
                        "NAM_DOAN.F_GET_SESSION_SERIAL",
                        OracleDbType.Decimal,
                        new OracleParameter("p_username", OracleDbType.Varchar2, finalUser, ParameterDirection.Input)
                    );

                    sidValue = sidObj?.ToString() ?? "";
                    serialValue = serialObj?.ToString() ?? "";

                    // Lấy userId trong bảng USERS (qua NAM_DOAN)
                    DataBase.Set_DataBase("127.0.0.1", "1521", "ORCL", "NAM_DOAN", "NAM_DOAN");
                    DataBase.Connect();

                    object encUserForId = DatabaseHelper.ExecuteFunction(
                        "NAM_DOAN.ENCRYPT_PKG.F_ENCRYPT_DB",
                        OracleDbType.NVarchar2,
                        new OracleParameter("p_input", OracleDbType.NVarchar2, encryptedUserLv1, ParameterDirection.Input),
                        new OracleParameter("p_key", OracleDbType.Int32, 9, ParameterDirection.Input)
                    );

                    string finalUserLv2 = encUserForId?.ToString();

                    // 1. KIỂM TRA TRẠNG THÁI KHÓA
                    object lockResult = DatabaseHelper.ExecuteFunction(
                        "NAM_DOAN.F_CHECK_IS_LOCKED",
                        OracleDbType.Decimal,
                        new OracleParameter("p_username", OracleDbType.Varchar2, finalUserLv2, ParameterDirection.Input)
                    );

                    if (lockResult != null)
                    {
                        // Chuyển đổi kết quả về int (1: Locked, 0: Active)
                        int isLocked = 0;
                        if (lockResult is OracleDecimal oraDec)
                            isLocked = oraDec.ToInt32();
                        else
                            isLocked = Convert.ToInt32(lockResult);

                        if (isLocked == 1)
                        {
                            MessageBox.Show("Tài khoản của bạn đã bị KHÓA!\nVui lòng liên hệ Quản trị viên.",
                                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            DataBase.Disconnect(); // Ngắt kết nối admin
                            return; // Dừng đăng nhập
                        }
                    }

                    object result = DatabaseHelper.ExecuteFunction(
                        "NAM_DOAN.F_GET_USER_ID_BY_NAME",
                        OracleDbType.Decimal,
                        new OracleParameter("p_username", OracleDbType.Varchar2, finalUserLv2, ParameterDirection.Input)
                    );

                    if (result != null && result != DBNull.Value)
                    {
                        userId = ((OracleDecimal)result).ToInt32();
                    }

                    DataBase.Disconnect();

                    if (userId == -1)
                    {
                        MessageBox.Show("User không tồn tại trong hệ thống!");
                        return;
                    }

                    // Giữ lại kết nối user thật cho Main form
                    DataBase.Set_DataBase(host, port, sid, finalUser, finalPass);
                    DataBase.Connect(true);
                }

                // Đăng nhập thành công
                MessageBox.Show("Đăng nhập thành công!");
                this.Hide();

                Main mainForm = new Main(userId, isSysUser, sidValue, serialValue, username);
                mainForm.ShowDialog();

                this.Close();
            }
            catch (OracleException ex)
            {
                // Bắt các mã lỗi Oracle cụ thể để thông báo rõ ràng
                switch (ex.Number)
                {
                    case 1017: // ORA-01017: invalid username/password; logon denied
                        MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;

                    case 28000: // ORA-28000: the account is locked
                        MessageBox.Show("Tài khoản đã bị KHÓA do đăng nhập sai quá số lần quy định (Profile Limit).\nVui lòng liên hệ Admin để mở khóa.",
                                        "Tài khoản bị khóa", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        break;

                    case 28001: // ORA-28001: the password has expired
                        MessageBox.Show("Mật khẩu của bạn đã HẾT HẠN (Password Life Time).\nVui lòng đổi mật khẩu mới.",
                                        "Mật khẩu hết hạn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Ở đây có thể mở Form đổi mật khẩu nếu muốn
                        break;

                    case 28002: // ORA-28002: the password will expire within ... days
                        // Đây là cảnh báo (Grace Time), vẫn đăng nhập được nhưng cần báo cho user biết
                        MessageBox.Show($"Cảnh báo: Mật khẩu sắp hết hạn!\n{ex.Message}", "Cảnh báo bảo mật", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        // Vẫn cho vào Main Form (Copy đoạn code mở Main vào đây hoặc dùng cờ flag)
                        // Tuy nhiên để đơn giản, ta chỉ hiện thông báo, user bấm OK rồi đăng nhập lại sau
                        break;

                    case 12541:
                        MessageBox.Show("Không thể kết nối đến máy chủ Oracle (Lỗi Listener)!", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;

                    case 12154: // ORA-12154: TNS:could not resolve the connect identifier specified
                        MessageBox.Show("Sai thông tin kết nối (SID hoặc Host không đúng)!", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;

                    default:
                        MessageBox.Show($"Lỗi Oracle ({ex.Number}): {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
        }


        private void PerformFinalLogin(string finalUser, string finalPass, string originalUsername)
        {
            try
            {
                string host = "127.0.0.1";
                string port = "1521";
                string sid = "ORCL";

                // 1. Kết nối user thật
                DataBase.Set_DataBase(host, port, sid, finalUser, finalPass);
                if (!DataBase.Connect(true))
                {
                    MessageBox.Show("Không thể đăng nhập bằng tài khoản người dùng (Sai mật khoản hoặc lỗi kết nối)!");
                    timerCapture.Start(); // Resume Camera if failed
                    return;
                }

                // 2. Lấy thông tin Session (SID, Serial)
                object sidObj = DatabaseHelper.ExecuteFunction(
                    "NAM_DOAN.F_GET_SESSION_SID",
                    OracleDbType.Decimal,
                    new OracleParameter("p_username", OracleDbType.Varchar2, finalUser, ParameterDirection.Input)
                );

                object serialObj = DatabaseHelper.ExecuteFunction(
                    "NAM_DOAN.F_GET_SESSION_SERIAL",
                    OracleDbType.Decimal,
                    new OracleParameter("p_username", OracleDbType.Varchar2, finalUser, ParameterDirection.Input)
                );

                string sidValue = sidObj?.ToString() ?? "";
                string serialValue = serialObj?.ToString() ?? "";

                // 3. Lấy UserID (Cần quyền NAM_DOAN để truy vấn bảng USERS, hoặc user hiện tại có quyền SELECT)
                // Do user thường chỉ thấy View của mình, nên ta dùng tạm kết nối NAM_DOAN để lấy ID an toàn
                DataBase.Disconnect(); // Disconnect user session momentarily

                DataBase.Set_DataBase("127.0.0.1", "1521", "ORCL", "NAM_DOAN", "NAM_DOAN");
                if (DataBase.Connect())
                {
                    // Helper to safely convert Oracle types
                    int SafeInt(object obj)
                    {
                        if (obj == null || obj == DBNull.Value) return 0;
                        if (obj is OracleDecimal decimalVal) return decimalVal.ToInt32();
                        return Convert.ToInt32(obj);
                    }

                    // 3.1 Check Lock Status
                    // Note: originalUsername might be the display name, BUT we need the ENCRYPTED name (finalUser) 
                    // to check in USERS table because P_REGISTER_USER stores encrypted username.
                    // HOWEVER, if we are coming from QR/Face, 'originalUsername' passed here is actually the PlainText name (for display in Main).
                    // The DB stores Encrypted names. 
                    // Logic Issue: We need the Encrypted Username to query F_GET_USER_ID_BY_NAME and F_CHECK_IS_LOCKED.
                    // 'finalUser' IS the encrypted username used for Oracle Login. So use 'finalUser'.
                    
                    object lockResult = DatabaseHelper.ExecuteFunction(
                        "NAM_DOAN.F_CHECK_IS_LOCKED",
                        OracleDbType.Decimal,
                        new OracleParameter("p_username", OracleDbType.Varchar2, finalUser, ParameterDirection.Input)
                    );

                    int isLocked = SafeInt(lockResult);
                    if (isLocked == 1)
                    {
                        MessageBox.Show("Tài khoản đang bị khóa tạm thời!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; 
                    }

                    // 4. Lấy UserID từ bảng App Users
                    // We can use the 'finalUser' (encrypted) to lookup ID via a procedure/function 
                    // stored in NAM_DOAN.
                    // Need a function: F_GET_USER_ID_BY_NAME(enc_username)
                    
                    object result = DatabaseHelper.ExecuteFunction(
                        "NAM_DOAN.F_GET_USER_ID_BY_NAME",
                        OracleDbType.Decimal,
                        new OracleParameter("p_username", OracleDbType.Varchar2, finalUser, ParameterDirection.Input)
                    );

                    int userId = -1;
                    if (result != null && result != DBNull.Value)
                        userId = SafeInt(result); // FIXED: Safe casting

                    DataBase.Disconnect();

                    if (userId == -1)
                    {
                        MessageBox.Show("Không tìm thấy User ID trong hệ thống!");
                        timerCapture.Start();
                        return;
                    }

                    // 4. Re-Login User Session for Main Form
                    DataBase.Set_DataBase(host, port, sid, finalUser, finalPass);
                    DataBase.Connect(true);

                    // Success
                    StopCamera(); // Stop camera before changing form
                    MessageBox.Show("Đăng nhập thành công!");
                    this.Hide();

                    Main mainForm = new Main(userId, false, sidValue, serialValue, finalUser); // Pass finalUser (Encrypted) or original? Main uses it for display usually.
                    mainForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lỗi kết nối Server kiểm tra thông tin.");
                    timerCapture.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi quy trình đăng nhập: " + ex.Message);
                timerCapture.Start();
            }
        }

        private void VerifyQRCode(string qrCode)
        {
             try
            {
                // Connect as Admin to lookup QR
                DataBase.Set_DataBase("127.0.0.1", "1521", "ORCL", "NAM_DOAN", "NAM_DOAN");
                if (DataBase.Connect())
                {
                    string username = userRepository.GetUserByQR(qrCode);
                    // DEBUG REMOVED

                    if (!string.IsNullOrEmpty(username)) // This 'username' is the Encrypted Username stored in DB
                    {
                        string encUser, encPass;
                        userRepository.GetUserCredentials(username, out encUser, out encPass);
                        DataBase.Disconnect();

                        if (!string.IsNullOrEmpty(encUser) && !string.IsNullOrEmpty(encPass))
                        {
                            // encUser is the Oracle Username (Encrypted 2 layers)
                            // encPass is the Oracle Password (Encrypted 2 layers)
                            PerformFinalLogin(encUser, encPass, username); 
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy thông tin định danh (Credentials) cho user này.");
                            timerCapture.Start();
                        }
                    }
                    else
                    {
                        MessageBox.Show("QR Code không tồn tại trong hệ thống.");
                        DataBase.Disconnect();
                        timerCapture.Start();
                    }
                }
                else
                {
                    MessageBox.Show("Lỗi kết nối Server.");
                    timerCapture.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi QR Verify: " + ex.Message);
                timerCapture.Start();
            }
        }

        // Helper Class for Cached Faces
        private class FaceData
        {
            public string UserEncoded { get; set; }
            public Mat Histogram { get; set; }
        }

        private List<FaceData> cachedFaces = new List<FaceData>();
        private bool isProcessingFrame = false;

        private void LoadAllFaces()
        {
            try
            {
                cachedFaces.Clear();
                DataBase.Set_DataBase("127.0.0.1", "1521", "ORCL", "NAM_DOAN", "NAM_DOAN");
                if (DataBase.Connect())
                {
                    DataTable dt = userRepository.GetAllFaceImages();
                    DataBase.Disconnect();

                    if (dt == null) return;

                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["FACE_IMG"] == DBNull.Value) continue;

                        try 
                        {
                            byte[] blob = null;
                            if (row["FACE_IMG"] is byte[] b) blob = b;
                            else if (row["FACE_IMG"] is OracleBlob oraBlob) { blob = oraBlob.Value; }

                            if (blob != null && blob.Length > 0)
                            {
                                using (var ms = new MemoryStream(blob))
                                using (Bitmap originalBmp = new Bitmap(ms))
                                {
                                    // Try to crop face (in case it's a full image from legacy registration)
                                    // If already cropped (small), DetectAndCrop might fail or return same.
                                    // Optimization: Only try cropping if image is large? 
                                    // Let's just try detecting.
                                    Bitmap faceBmp = DetectAndCropFace(originalBmp);
                                    if (faceBmp == null) faceBmp = new Bitmap(originalBmp); // Use original if no face detected (fallback)

                                    using (faceBmp)
                                    using (Mat mat = BitmapConverter.ToMat(faceBmp))
                                    using (Mat hsv = new Mat())
                                    {
                                        if (mat.Empty()) continue;
                                        Cv2.CvtColor(mat, hsv, ColorConversionCodes.BGR2HSV);
                                        
                                        Mat hist = new Mat();
                                        int[] histSize = { 50, 60 };
                                        Rangef[] ranges = { new Rangef(0, 180), new Rangef(0, 256) };
                                        int[] channels = { 0, 1 };
                                        Cv2.CalcHist(new Mat[] { hsv }, channels, null, hist, 2, histSize, ranges);
                                        Cv2.Normalize(hist, hist, 0, 1, NormTypes.MinMax);
                                        
                                        cachedFaces.Add(new FaceData 
                                        { 
                                            UserEncoded = row["USER_NAME"].ToString(), 
                                            Histogram = hist 
                                        });
                                    }
                                }
                            }
                        }
                        catch { /* Ignore individual bad images */ }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu khuôn mặt: " + ex.Message);
            }
        }

        private void VerifyFace(Bitmap inputImage)
        {
            if (isProcessingFrame) return;
            isProcessingFrame = true;

            try
            {
                // Ensure faces are loaded (Fallback)
                if (cachedFaces.Count == 0) LoadAllFaces();

                if (cachedFaces.Count == 0)
                {
                    MessageBox.Show("Chưa có khuôn mặt nào được đăng ký trong hệ thống!");
                    timerCapture.Start();
                    isProcessingFrame = false;
                    return;
                }

                // DETECT FACE IN INPUT
                using (Bitmap inputBmpRaw = new Bitmap(inputImage))
                {
                    Bitmap detectedFaceBmp = DetectAndCropFace(inputBmpRaw);

                    if (detectedFaceBmp == null)
                    {
                        lblStatus.Text = "Không tìm thấy khuôn mặt...";
                        MessageBox.Show("Không tìm thấy khuôn mặt trong khung hình! Vui lòng thử lại.");
                        timerCapture.Start();
                        isProcessingFrame = false;
                        return;
                    }

                    using (detectedFaceBmp) // Dispose automatically
                    using (Bitmap resizedInput = new Bitmap(detectedFaceBmp, 200, 200)) // Standardize size
                    using (Mat inputMat = BitmapConverter.ToMat(resizedInput))
                    using (Mat inputHist = new Mat())
                    using (Mat inputHsv = new Mat())
                    {
                        Cv2.CvtColor(inputMat, inputHsv, ColorConversionCodes.BGR2HSV);
                        
                        int[] histSize = { 50, 60 };
                        Rangef[] ranges = { new Rangef(0, 180), new Rangef(0, 256) };
                        int[] channels = { 0, 1 };
                        
                        Cv2.CalcHist(new Mat[] { inputHsv }, channels, null, inputHist, 2, histSize, ranges);
                        Cv2.Normalize(inputHist, inputHist, 0, 1, NormTypes.MinMax);

                        // Compare with Cache
                        string matchedUserEncoded = null;
                        double maxScore = -1;

                        foreach (var face in cachedFaces)
                        {
                            double score = Cv2.CompareHist(inputHist, face.Histogram, HistCompMethods.Correl);
                            if (score > maxScore)
                            {
                                maxScore = score;
                                matchedUserEncoded = face.UserEncoded;
                            }
                        }

                        // Threshold - tuned for Face-to-Face comparison
                        double threshold = 0.7; 
                        if (maxScore > threshold && !string.IsNullOrEmpty(matchedUserEncoded))
                        {
                            // Success logic
                            // ... existing success logic ...
                            LoginSuccess(matchedUserEncoded, maxScore); // Extracted method call or duplicated code logic
                        }
                        else
                        {
                            lblStatus.Text = $"Không khớp: {maxScore:F2} / {threshold}";
                            MessageBox.Show($"Không nhận diện đúng người dùng.\nScore: {maxScore:F2} (Cần > {threshold}).");
                            timerCapture.Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xử lý ảnh: " + ex.Message);
                timerCapture.Start();
            }
            finally
            {
                isProcessingFrame = false;
            }
        }

        private void btn_dn_reigster_Click(object sender, EventArgs e)
        {
            this.Hide();
            Register regForm = new Register();
            regForm.ShowDialog();
            this.Show();
        }

        // ===================================
        // UI HELPERS (Matching Register Form)
        // ===================================

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
            panel.Region = new System.Drawing.Region(path);

            using (Pen pen = new Pen(Color.FromArgb(230, 230, 230), 1))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        // Username
        private void txtUsername_Enter(object sender, EventArgs e)
        {
            if (txtUsername.Text == "Nhập tên đăng nhập")
            {
                txtUsername.Text = "";
                txtUsername.ForeColor = Color.Black;
            }
        }
        private void txtUsername_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Text = "Nhập tên đăng nhập";
                txtUsername.ForeColor = Color.Gray;
            }
        }

        // Password
        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Nhập mật khẩu")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black;
                if (!chkShowPass.Checked)
                    txtPassword.UseSystemPasswordChar = true;
            }
        }
        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = "Nhập mật khẩu";
                txtPassword.ForeColor = Color.Gray;
                txtPassword.UseSystemPasswordChar = false;
            }
        }

        // Checkbox
        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text != "Nhập mật khẩu")
            {
                txtPassword.UseSystemPasswordChar = !chkShowPass.Checked;
            }
        }

        // Link Register
        private void LinkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.btn_dn_reigster_Click(sender, e);
        }

        // ============================================
        // CAMERA LOGIC (Face / QR)
        // ============================================

        private void btnFaceLogin_Click(object sender, EventArgs e)
        {
            // Load DB Faces into Memory Cache BEFORE starting camera
            if (cachedFaces.Count == 0)
            {
                LoadAllFaces();
            }

            StartCamera();
            isScanningFace = true;
            isScanningQR = false;
            lblStatus.Text = "Đưa khuôn mặt vào camera...";
            
            panelCard.Visible = false;
            panelCamera.Visible = true;
        }

        private void btnQRLogin_Click(object sender, EventArgs e)
        {
            StartCamera();
            isScanningFace = false;
            isScanningQR = true;
            lblStatus.Text = "Đưa mã QR vào camera...";
            
            panelCard.Visible = false;
            panelCamera.Visible = true;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            StopCamera();
            panelCamera.Visible = false;
            panelCard.Visible = true;
        }
        
        private void btnCapture_Click(object sender, EventArgs e)
        {
            if (isScanningFace && picCamera.Image != null)
            {
                timerCapture.Stop();
                lblStatus.Text = "Đang xác thực khuôn mặt...";
                
                Bitmap currentFace = (Bitmap)picCamera.Image.Clone();
                VerifyFace(currentFace);
            }
        }

        private void StartCamera()
        {
            if (capture == null)
            {
                try {
                capture = new VideoCapture(0);
                } catch { 
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
            picCamera.Image = null;
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
                        picCamera.Image?.Dispose();
                        picCamera.Image = (Bitmap)bmp.Clone();

                        if (isScanningQR)
                        {
                            ScanQR(bmp);
                        }
                    }
                }
            }
        }
        
        private void ScanQR(Bitmap bitmap)
        {
            try
            {
                BarcodeReader reader = new BarcodeReader();
                Result result = reader.Decode(bitmap);
                if (result != null)
                {
                    timerCapture.Stop();
                    string decoded = result.Text;
                    lblStatus.Text = "Đã tìm thấy QR! Đang kiểm tra...";
                    VerifyQRCode(decoded);
                }
            }
            catch { }
        }

        private void LoginSuccess(string matchedUserEncoded, double maxScore)
        {
            DataBase.Disconnect();
            DataBase.Set_DataBase("127.0.0.1", "1521", "ORCL", "NAM_DOAN", "NAM_DOAN");
            
            if (DataBase.Connect())
            {
                string encUser, encPass;
                userRepository.GetUserCredentials(matchedUserEncoded, out encUser, out encPass);
                DataBase.Disconnect();
                
                if (!string.IsNullOrEmpty(encUser))
                {
                    MessageBox.Show($"Nhận diện thành công! (Score: {maxScore:F2})");
                    PerformFinalLogin(encUser, encPass, matchedUserEncoded);
                }
                else
                {
                    MessageBox.Show("Lỗi xác thực: Không tìm thấy thông tin đăng nhập.");
                    timerCapture.Start();
                }
            }
            else
            {
                MessageBox.Show("Lỗi kết nối Server.");
                timerCapture.Start();
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


    }
}
