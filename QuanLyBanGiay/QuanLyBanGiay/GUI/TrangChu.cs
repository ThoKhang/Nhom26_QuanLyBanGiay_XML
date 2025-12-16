using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

using QuanLyBanGiay.CLASS;

namespace QuanLyBanGiay.GUI
{
    public partial class TrangChu : Form
    {
        private SqlXmlConverter _converter = new SqlXmlConverter();

        public TrangChu()
        {
            InitializeComponent();

            // GÁN THÔNG TIN Ở ĐÂY – sẽ luôn chạy khi form được tạo
            label1.Text = "Xin chào: " + QuanLyBanGiay.CLASS.DangNhap.TenNguoiDung;
            label5.Text = QuanLyBanGiay.CLASS.DangNhap.Quyen;
            label6.Text = QuanLyBanGiay.CLASS.DangNhap.TenDangNhapHienTai;
            label7.Text = QuanLyBanGiay.CLASS.DangNhap.TenNguoiDung;

            // Nếu muốn “mờ/khóa” menu admin thì bật dòng này:
            ApDungPhanQuyenMenu();
        }

        private void TrangChu_Load(object sender, EventArgs e)
        {
        }

        // =========================
        // PHÂN QUYỀN
        // =========================
        private bool IsAdmin()
        {
            // admin trong TaiKhoan.xml thường là "Admin" / "admin"
            string q = QuanLyBanGiay.CLASS.DangNhap.Quyen?.Trim();
            return string.Equals(q, "admin", StringComparison.OrdinalIgnoreCase);
        }

        private bool CheckAdminPermission()
        {
            if (IsAdmin()) return true;

            MessageBox.Show("Bạn không có quyền để truy cập.",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
            return false;
        }

        // Không phải admin thì chỉ được:
        // đổi mật khẩu, đăng xuất, thoát, bán giày, chấm công, trợ giúp
        private void ApDungPhanQuyenMenu()
        {
            if (IsAdmin()) return;

            ToolStripItem[] allow =
            {
                đổiMậtKhẩuToolStripMenuItem,
                đăngXuấtTàiKhoảnToolStripMenuItem,
                thoátChươngTrìnhToolStripMenuItem,
                bánGiàyToolStripMenuItem,
                xácNhậnNhânViênĐiLàmToolStripMenuItem,
                trợGiúpToolStripMenuItem
            };

            foreach (ToolStripItem item in menuStrip1.Items)
                SetEnabledRecursive(item, allow);
        }

        private void SetEnabledRecursive(ToolStripItem item, ToolStripItem[] allow)
        {
            bool enabled = allow.Contains(item);

            if (item is ToolStripMenuItem mi && mi.DropDownItems.Count > 0)
            {
                foreach (ToolStripItem child in mi.DropDownItems)
                    SetEnabledRecursive(child, allow);

                if (!enabled)
                    enabled = mi.DropDownItems.Cast<ToolStripItem>().Any(x => x.Enabled);
            }

            item.Enabled = enabled;

            // Nếu muốn ẨN hẳn thay vì mờ:
            // item.Visible = enabled;
        }

        // =========================
        // MENU EVENTS
        // =========================
        private void đổiMậtKhẩuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoiMatKhau frm = new DoiMatKhau();
            frm.ShowDialog();
        }

        private void đăngXuấtTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            QuanLyBanGiay.GUI.DangNhap frm = new QuanLyBanGiay.GUI.DangNhap();
            frm.Show();
        }

        private void thoátChươngTrìnhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void quảnLýNhânViênToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!CheckAdminPermission()) return;

            QuanLyNhanVien frm = new QuanLyNhanVien();
            frm.ShowDialog();
        }

        private void quảnLýTàiKhoảnNhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckAdminPermission()) return;

            QuanLyTaiKhoanNhanVien frm = new QuanLyTaiKhoanNhanVien();
            frm.ShowDialog();
        }

        private void quảnLýMặtHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckAdminPermission()) return;

            QuanLyMatHang frm = new QuanLyMatHang();
            frm.ShowDialog();
        }

        private void quảnLýNhậpHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckAdminPermission()) return;

            QuanLyPhieuNhap frm = new QuanLyPhieuNhap();
            frm.ShowDialog();
        }

        private void quảnLýNhàCungCấpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckAdminPermission()) return;

            QuanLyNhaCungCap frm = new QuanLyNhaCungCap();
            frm.ShowDialog();
        }

        private void bánGiàyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BanHang frm = new BanHang();
            frm.ShowDialog();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void xácNhậnNhânViênĐiLàmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(QuanLyBanGiay.CLASS.DangNhap.TenDangNhapHienTai) ||
                string.IsNullOrEmpty(QuanLyBanGiay.CLASS.DangNhap.TenNguoiDung) ||
                string.IsNullOrEmpty(QuanLyBanGiay.CLASS.DangNhap.MaNhanVien))
            {
                MessageBox.Show("Chưa có nhân viên nào đăng nhập, không thể xác nhận đi làm.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            string taiKhoan = QuanLyBanGiay.CLASS.DangNhap.TenDangNhapHienTai;
            string hoTen = QuanLyBanGiay.CLASS.DangNhap.TenNguoiDung;
            string maNV = QuanLyBanGiay.CLASS.DangNhap.MaNhanVien;

            ChamCongXml chamCong = new ChamCongXml();
            chamCong.ThemChamCong(maNV, "DiLam");

            MessageBox.Show(
                $"Tài khoản \"{taiKhoan}\" với họ tên \"{hoTen}\" đã xác nhận đi làm.",
                "Xác nhận đi làm",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void trợGiúpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "https://learn.microsoft.com/vi-vn/visualstudio/ide/create-csharp-winform-visual-studio?view=vs-2022";

            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở trang trợ giúp: " + ex.Message,
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void báoCáoThốngKêToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckAdminPermission()) return;

            ThongKe frm = new ThongKe();
            frm.ShowDialog();
        }

        private void sQLSangXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckAdminPermission()) return;

            _converter.SqlToXml_All();
        }

        private void xMLSangSQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckAdminPermission()) return;

            _converter.XmlToSql_All();
        }
    }
}
