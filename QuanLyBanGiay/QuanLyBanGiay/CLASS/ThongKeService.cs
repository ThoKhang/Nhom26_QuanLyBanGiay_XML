using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyBanGiay.CLASS
{
    internal class ThongKeService
    {
        public int TongTaiKhoan { get; private set; }
        public int TongNhanVien { get; private set; }
        public int TongNhanVienDangLam { get; private set; }
        public int TongMatHangDangBan { get; private set; }
        public int TongPhieuHangDaBan { get; private set; }
        public long TongTienDaBan { get; private set; }
        public int TongNhaCungCap { get; private set; }
        public int TongMatHangDaCungCap { get; private set; }
        public long TongTienDaMua { get; private set; }

        public void TinhTatCa()
        {
            try
            {
                // 1. Tài khoản
                DataTable tbTK = LoadTable("TaiKhoan.xml", "TaiKhoan");
                TongTaiKhoan = tbTK?.Rows.Count ?? 0;

                // 2. Nhân viên
                DataTable tbNV = LoadTable("NhanVien.xml", "NhanVien");
                TongNhanVien = tbNV?.Rows.Count ?? 0;

                if (tbNV != null)
                {
                    TongNhanVienDangLam = tbNV.AsEnumerable()
                        .Count(r => r["TrangThai"].ToString().Trim()
                            .Equals("DangLam", StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    TongNhanVienDangLam = 0;
                }

                // 3. Mặt hàng đang bán (giày)
                // File Giay.xml, bảng SanPham (vì node là <SanPham>)
                DataTable tbGiay = LoadTable("Giay.xml", "SanPham");
                TongMatHangDangBan = tbGiay?.Rows.Count ?? 0;

                // 4. Phiếu hàng đã bán & tổng tiền đã bán (PhieuMua.xml)
                DataTable tbPM = LoadTable("PhieuMua.xml", "PhieuMua");
                TongPhieuHangDaBan = tbPM?.Rows.Count ?? 0;
                TongTienDaBan = 0;

                if (tbPM != null)
                {
                    foreach (DataRow r in tbPM.Rows)
                    {
                        if (long.TryParse(r["ThanhTien"]?.ToString(), out long tt))
                            TongTienDaBan += tt;
                    }
                }

                // 5. Nhà cung cấp
                DataTable tbNCC = LoadTable("NhaCungCap.xml", "NhaCungCap");
                TongNhaCungCap = tbNCC?.Rows.Count ?? 0;

                // 6. Mặt hàng đã cung cấp & tổng tiền đã mua
                // Lấy từ ChiTietPhieuNhap.xml: SoLuong + ThanhTien
                DataTable tbCTPN = LoadTable("ChiTietPhieuNhap.xml", "ChiTietPhieuNhap");
                TongMatHangDaCungCap = 0;
                TongTienDaMua = 0;

                if (tbCTPN != null)
                {
                    foreach (DataRow r in tbCTPN.Rows)
                    {
                        if (int.TryParse(r["SoLuong"]?.ToString(), out int sl))
                            TongMatHangDaCungCap += sl;

                        if (long.TryParse(r["ThanhTien"]?.ToString(), out long ttMua))
                            TongTienDaMua += ttMua;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tính thống kê: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable LoadTable(string fileName, string tableName)
        {
            string path = Path.Combine(Application.StartupPath, fileName);

            if (!File.Exists(path))
                return null;

            DataSet ds = new DataSet();
            ds.ReadXml(path);

            if (!string.IsNullOrEmpty(tableName) && ds.Tables.Contains(tableName))
                return ds.Tables[tableName];

            if (ds.Tables.Count > 0)
                return ds.Tables[0];

            return null;
        }
    }
}
