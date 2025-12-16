using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace QuanLyBanGiay.CLASS
{
    internal class PhieuMuaXml
    {
        private string _path = Path.Combine(Application.StartupPath, "PhieuMua.xml");

        public DataSet Data { get; private set; }
        public DataTable Table => Data.Tables[0];

        public PhieuMuaXml()
        {
            Load();
        }

        public void Load()
        {
            Data = new DataSet();

            if (File.Exists(_path))
            {
                Data.ReadXml(_path);
            }
            else
            {
                // Tạo cấu trúc bảng nếu chưa có file
                DataTable tb = new DataTable("PhieuMua");
                tb.Columns.Add("MaPhieuMua");
                tb.Columns.Add("MaGiay");
                tb.Columns.Add("SoLuongMua", typeof(int));
                tb.Columns.Add("DonGiaBan", typeof(int));
                tb.Columns.Add("ThanhTien", typeof(int));
                tb.Columns.Add("NgayMua");
                tb.Columns.Add("TenDangNhap");
                tb.Columns.Add("TenNguoiDung");

                Data.Tables.Add(tb);
                Save();
            }
        }

        public void Save()
        {
            Data.WriteXml(_path);
        }

        public string GetPath() => _path;

        private string TaoMaPhieuMoi()
        {
            if (Table.Rows.Count == 0)
                return "PM001";

            int maxSo = 0;

            foreach (DataRow r in Table.Rows)
            {
                string ma = r["MaPhieuMua"].ToString(); // ví dụ PM005
                if (ma.Length > 2 && int.TryParse(ma.Substring(2), out int so))
                {
                    if (so > maxSo) maxSo = so;
                }
            }

            int soMoi = maxSo + 1;
            return "PM" + soMoi.ToString("000");
        }

        public void ThemPhieuMua(string maGiay, int soLuongMua, int donGiaBan, int thanhTien)
        {
            string maPhieu = TaoMaPhieuMoi();
            string ngayMua = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string tenDangNhap = DangNhap.TenDangNhapHienTai;
            string tenNguoiDung = DangNhap.TenNguoiDung;

            DataRow row = Table.NewRow();
            row["MaPhieuMua"] = maPhieu;
            row["MaGiay"] = maGiay;
            row["SoLuongMua"] = soLuongMua;
            row["DonGiaBan"] = donGiaBan;
            row["ThanhTien"] = thanhTien;
            row["NgayMua"] = ngayMua;
            row["TenDangNhap"] = tenDangNhap;
            row["TenNguoiDung"] = tenNguoiDung;

            Table.Rows.Add(row);
            Save();
        }
    }
}
