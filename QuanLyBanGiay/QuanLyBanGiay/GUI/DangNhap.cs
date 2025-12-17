using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;

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

        // =========================
        // BUTTON1: ĐĂNG NHẬP
        // =========================
        private void button1_Click(object sender, EventArgs e)
        {
            string tk = textBox1.Text.Trim();  // tài khoản
            string mk = textBox2.Text.Trim();  // mật khẩu

            if (tk == "" || mk == "")
            {
                label2.Text = "Vui lòng nhập đầy đủ tài khoản và mật khẩu";
                label2.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // gọi class kiểm tra đăng nhập (LOGIC GỐC – GIỮ NGUYÊN)
            if (dn.KiemTraDangNhap(tk, mk))
            {
                label2.Text = "";

                TrangChu frm = new TrangChu();
                frm.Show();

                this.Hide();
            }
            else
            {
                label2.Text = "Tài khoản hoặc mật khẩu không đúng";
                label2.ForeColor = System.Drawing.Color.Red;
            }
        }

        // =========================
        // BUTTON2: THOÁT
        // =========================
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // =========================
        // XSLT PREVIEW (MINH HỌA – KHÔNG ẢNH HƯỞNG ĐĂNG NHẬP)
        // =========================

        /*
        ❌ KHÔNG DÙNG TRONG LOGIC ĐĂNG NHẬP
        ❌ CHỈ DÙNG ĐỂ DEMO XML + XSLT TRONG ĐỒ ÁN
        */

        private void PreviewTaiKhoanBangXSLT()
        {
            try
            {
                string xmlPath = Path.Combine(Application.StartupPath, "TaiKhoan.xml");
                string xslPath = Path.Combine(Application.StartupPath, "TaiKhoan.xsl");
                string htmlPath = Path.Combine(Application.StartupPath, "TaiKhoan_Preview.html");

                if (!File.Exists(xslPath))
                {
                    MessageBox.Show("Chưa có file TaiKhoan.xsl để preview.",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                XslCompiledTransform xslt = new XslCompiledTransform();
                xslt.Load(xslPath);
                xslt.Transform(xmlPath, htmlPath);

                Process.Start(new ProcessStartInfo(htmlPath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi preview XSLT: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*
        👉 Nếu GIẢNG VIÊN hỏi XSLT dùng ở đâu:
        - Có thể gọi hàm PreviewTaiKhoanBangXSLT() từ 1 nút ẩn / menu debug
        - Hiện tại KHÔNG GỌI → KHÔNG ẢNH HƯỞNG CHƯƠNG TRÌNH
        */

        private void DangNhap_Load(object sender, EventArgs e)
        {
        }
    }
}
