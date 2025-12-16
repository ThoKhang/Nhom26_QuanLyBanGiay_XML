using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

            // Gán lên label
            label11.Text = _tk.TongTaiKhoan.ToString();                  // tổng số tài khoản
            label12.Text = _tk.TongNhanVien.ToString();                  // tổng nhân viên
            label13.Text = _tk.TongNhanVienDangLam.ToString();           // tổng nhân viên đang làm việc
            label14.Text = _tk.TongMatHangDangBan.ToString();            // tổng mặt hàng đang bán
            label15.Text = _tk.TongPhieuHangDaBan.ToString();            // tổng phiếu hàng đã bán

            // format tiền cho dễ nhìn (ngàn, triệu)
            label16.Text = _tk.TongTienDaBan.ToString("N0") + " VND";    // tổng số tiền đã bán được
            label17.Text = _tk.TongNhaCungCap.ToString();                // tổng nhà cung cấp
            label18.Text = _tk.TongMatHangDaCungCap.ToString();          // tổng mặt hàng đã cung cấp
            label19.Text = _tk.TongTienDaMua.ToString("N0") + " VND";    // tổng số tiền đã mua
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void ThongKe_Load(object sender, EventArgs e)
        {
            // nếu muốn refresh khi load thì có thể gọi lại _tk.TinhTatCa() ở đây
        }
    }
}
