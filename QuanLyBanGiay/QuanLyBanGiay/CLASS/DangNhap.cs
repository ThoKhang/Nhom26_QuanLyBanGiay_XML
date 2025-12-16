using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace QuanLyBanGiay.CLASS
{
    public class DangNhap
    {
        public static string Quyen { get; private set; }
        public static string TenDangNhapHienTai { get; private set; }
        public static string TenNguoiDung { get; private set; }
        public static string MaNhanVien { get; private set; }

        public bool KiemTraDangNhap(string tenDangNhap, string matKhau)
        {
            try
            {
                string path = Path.Combine(Application.StartupPath, "TaiKhoan.xml");
                DataSet ds = new DataSet();
                ds.ReadXml(path);
                DataTable tb = ds.Tables[0];

                foreach (DataRow row in tb.Rows)
                {
                    if (row["TenDangNhap"].ToString().Trim() == tenDangNhap &&
                        row["MatKhau"].ToString().Trim() == matKhau)
                    {
                        TenDangNhapHienTai = tenDangNhap;
                        Quyen = row["Quyen"].ToString().Trim();
                        MaNhanVien = row["MaNhanVien"].ToString().Trim();

                        LayTenNguoiDungTuNhanVien(MaNhanVien);
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private static void LayTenNguoiDungTuNhanVien(string maNV)
        {
            string nvPath = Path.Combine(Application.StartupPath, "NhanVien.xml");
            DataSet ds = new DataSet();
            ds.ReadXml(nvPath);
            DataTable tb = ds.Tables[0];

            foreach (DataRow row in tb.Rows)
            {
                if (row["MaNhanVien"].ToString().Trim() == maNV)
                {
                    TenNguoiDung = row["HoTen"].ToString().Trim();
                    return;
                }
            }

            TenNguoiDung = "Không có tên nhân viên";
        }
    }
}
