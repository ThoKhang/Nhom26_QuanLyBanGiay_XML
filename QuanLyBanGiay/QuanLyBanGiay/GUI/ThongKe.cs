using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using QuanLyBanGiay.CLASS;

namespace QuanLyBanGiay.GUI
{
    public partial class ThongKe : Form
    {
        private ThongKeService _tk;

        public ThongKe()
        {
            InitializeComponent();

            _tk = new ThongKeService();
            _tk.TinhTatCa();

            // =========================
            // GÁN LÊN LABEL (LOGIC GỐC)
            // =========================
            label11.Text = _tk.TongTaiKhoan.ToString();                  // tổng số tài khoản
            label12.Text = _tk.TongNhanVien.ToString();                  // tổng nhân viên
            label13.Text = _tk.TongNhanVienDangLam.ToString();           // tổng nhân viên đang làm việc
            label14.Text = _tk.TongMatHangDangBan.ToString();            // tổng mặt hàng đang bán
            label15.Text = _tk.TongPhieuHangDaBan.ToString();            // tổng phiếu hàng đã bán

            label16.Text = _tk.TongTienDaBan.ToString("N0") + " VND";    // tổng tiền bán
            label17.Text = _tk.TongNhaCungCap.ToString();                // tổng nhà cung cấp
            label18.Text = _tk.TongMatHangDaCungCap.ToString();          // tổng mặt hàng đã cung cấp
            label19.Text = _tk.TongTienDaMua.ToString("N0") + " VND";    // tổng tiền mua
        }

        // =========================
        // XSLT PREVIEW (MINH HỌA)
        // =========================

        /*
         ❌ KHÔNG ẢNH HƯỞNG LOGIC THỐNG KÊ
         ❌ CHỈ DÙNG ĐỂ DEMO XML + XSLT
        */
        private void PreviewThongKeBangXSLT()
        {
            try
            {
                string xmlPath = Path.Combine(Application.StartupPath, "ThongKe.xml");
                string xslPath = Path.Combine(Application.StartupPath, "ThongKe.xsl");
                string htmlPath = Path.Combine(Application.StartupPath, "ThongKe_Preview.html");

                if (!File.Exists(xslPath))
                {
                    MessageBox.Show("Chưa có file ThongKe.xsl để preview.",
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
         👉 Nếu cần demo:
         - Có thể gắn PreviewThongKeBangXSLT() vào 1 nút ẩn / menu debug
         - Hiện tại KHÔNG GỌI → không ảnh hưởng chương trình
        */

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void ThongKe_Load(object sender, EventArgs e)
        {
            // nếu muốn refresh khi load thì có thể gọi lại _tk.TinhTatCa()
        }

        private void ThongKe_Load_1(object sender, EventArgs e)
        {
        }
    }
}
