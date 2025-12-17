namespace DA_N6.Forms
{
    partial class LoginSecure
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtQRCode;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnStartCamera;
        private System.Windows.Forms.Button btnFaceLogin;
        private System.Windows.Forms.Button btnEncryptAES;
        private System.Windows.Forms.Button btnDecryptAES;
        private System.Windows.Forms.Button btnEncryptRSA;
        private System.Windows.Forms.Button btnDecryptRSA;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtQRCode = new System.Windows.Forms.TextBox();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnStartCamera = new System.Windows.Forms.Button();
            this.btnFaceLogin = new System.Windows.Forms.Button();
            this.btnEncryptAES = new System.Windows.Forms.Button();
            this.btnDecryptAES = new System.Windows.Forms.Button();
            this.btnEncryptRSA = new System.Windows.Forms.Button();
            this.btnDecryptRSA = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();

            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(20, 20);
            this.pictureBox1.Size = new System.Drawing.Size(320, 240);
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // 
            // txtQRCode
            // 
            this.txtQRCode.Location = new System.Drawing.Point(360, 20);
            this.txtQRCode.Size = new System.Drawing.Size(200, 22);
            this.txtQRCode.ReadOnly = true;

            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(360, 60);
            this.txtInput.Size = new System.Drawing.Size(200, 22);

            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(360, 100);
            this.txtOutput.Size = new System.Drawing.Size(200, 22);
            this.txtOutput.ReadOnly = true;

            // 
            // btnStartCamera
            // 
            this.btnStartCamera.Location = new System.Drawing.Point(20, 280);
            this.btnStartCamera.Size = new System.Drawing.Size(100, 30);
            this.btnStartCamera.Text = "Bắt Camera";
            this.btnStartCamera.Click += new System.EventHandler(this.btnStartCamera_Click);

            // 
            // btnFaceLogin
            // 
            this.btnFaceLogin.Location = new System.Drawing.Point(140, 280);
            this.btnFaceLogin.Size = new System.Drawing.Size(100, 30);
            this.btnFaceLogin.Text = "Login Face";
            this.btnFaceLogin.Click += new System.EventHandler(this.btnFaceLogin_Click);

            // 
            // btnEncryptAES
            // 
            this.btnEncryptAES.Location = new System.Drawing.Point(360, 140);
            this.btnEncryptAES.Size = new System.Drawing.Size(95, 30);
            this.btnEncryptAES.Text = "AES Encrypt";
            this.btnEncryptAES.Click += new System.EventHandler(this.btnEncryptAES_Click);

            // 
            // btnDecryptAES
            // 
            this.btnDecryptAES.Location = new System.Drawing.Point(465, 140);
            this.btnDecryptAES.Size = new System.Drawing.Size(95, 30);
            this.btnDecryptAES.Text = "AES Decrypt";
            this.btnDecryptAES.Click += new System.EventHandler(this.btnDecryptAES_Click);

            // 
            // btnEncryptRSA
            // 
            this.btnEncryptRSA.Location = new System.Drawing.Point(360, 180);
            this.btnEncryptRSA.Size = new System.Drawing.Size(95, 30);
            this.btnEncryptRSA.Text = "RSA Encrypt";
            this.btnEncryptRSA.Click += new System.EventHandler(this.btnEncryptRSA_Click);

            // 
            // btnDecryptRSA
            // 
            this.btnDecryptRSA.Location = new System.Drawing.Point(465, 180);
            this.btnDecryptRSA.Size = new System.Drawing.Size(95, 30);
            this.btnDecryptRSA.Text = "RSA Decrypt";
            this.btnDecryptRSA.Click += new System.EventHandler(this.btnDecryptRSA_Click);

            // 
            // LoginSecure Form
            // 
            this.ClientSize = new System.Drawing.Size(600, 350);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtQRCode);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnStartCamera);
            this.Controls.Add(this.btnFaceLogin);
            this.Controls.Add(this.btnEncryptAES);
            this.Controls.Add(this.btnDecryptAES);
            this.Controls.Add(this.btnEncryptRSA);
            this.Controls.Add(this.btnDecryptRSA);
            this.Text = "LoginSecure";

            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
