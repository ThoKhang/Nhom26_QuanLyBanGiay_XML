using System;
using System.Windows.Forms;

namespace QuanLyBanGiay.GUI
{
    public partial class DoiMatKhau : Form
    {
        public DoiMatKhau()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string mkMoi = textBox1.Text.Trim();      // mật khẩu mới
            string mkNhapLai = textBox2.Text.Trim();  // nhập lại mật khẩu mới

            if (string.IsNullOrEmpty(mkMoi) || string.IsNullOrEmpty(mkNhapLai))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ mật khẩu mới và nhập lại.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            if (mkMoi != mkNhapLai)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            // Lấy tên đăng nhập hiện tại từ class DangNhap (trong CLASS, không phải form)
            string tenDangNhapHienTai = QuanLyBanGiay.CLASS.DangNhap.TenDangNhapHienTai;

            if (string.IsNullOrEmpty(tenDangNhapHienTai))
            {
                MessageBox.Show("Không xác định được tài khoản đang đăng nhập.",
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            // Gọi class TaiKhoan trong CLASS để đổi mật khẩu
            QuanLyBanGiay.CLASS.TaiKhoan tk = new QuanLyBanGiay.CLASS.TaiKhoan();
            string err;

            bool ok = tk.DoiMatKhau(tenDangNhapHienTai, mkMoi, out err);

            if (ok)
            {
                MessageBox.Show("Đổi mật khẩu thành công!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show(err,
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void DoiMatKhau_Load(object sender, EventArgs e)
        {

        }
    }
}
