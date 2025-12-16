using System;
using System.Windows.Forms;

namespace QuanLyBanGiay.GUI
{
    public partial class DangNhap : Form
    {
        // Tạo đối tượng class xử lý đăng nhập
        QuanLyBanGiay.CLASS.DangNhap dn = new QuanLyBanGiay.CLASS.DangNhap();

        public DangNhap()
        {
            InitializeComponent();
            label2.Text = "Vui lòng đăng nhập !";
            label2.ForeColor = System.Drawing.Color.Blue;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tk = textBox1.Text.Trim();  // tài khoản
            string mk = textBox2.Text.Trim();  // mật khẩu

            if (tk == "" || mk == "")
            {
                label2.Text = "Vui lòng nhập đầy đủ tài khoản và mật khẩu";
                label2.ForeColor = Color.Red;
                return;
            }

            // gọi class kiểm tra đăng nhập
            if (dn.KiemTraDangNhap(tk, mk))
            {
                label2.Text = ""; // xóa thông báo lỗi

                // Mở form Trang chủ
                TrangChu frm = new TrangChu();
                frm.Show();

                this.Hide(); // ẩn form đăng nhập
            }
            else
            {
                label2.Text = "Tài khoản hoặc mật khẩu không đúng";
                label2.ForeColor = Color.Red;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
