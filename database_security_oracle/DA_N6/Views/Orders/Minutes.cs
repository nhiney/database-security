using System;
using System.Windows.Forms;

namespace DA_N6.Views.Orders
{
    public partial class Minutes : Form
    {
        public int InputMinutes { get; private set; }

        public Minutes()
        {
            InitializeComponent();
            InputMinutes = 0;
        }

        private void InputMinutes_Load(object sender, EventArgs e)
        {
            txtMinutes.Focus();
            txtMinutes.SelectAll();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtMinutes.Text, out int minutes) || minutes <= 0)
            {
                MessageBox.Show("Vui lòng nhập số phút hợp lệ (số nguyên dương)!",
                              "Lỗi nhập liệu",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                txtMinutes.Focus();
                txtMinutes.SelectAll();
                return;
            }

            InputMinutes = minutes;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtMinutes_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép nhập số và control keys
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            // Cho phép Enter để xác nhận
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnOK_Click(sender, e);
            }
        }
    }
}