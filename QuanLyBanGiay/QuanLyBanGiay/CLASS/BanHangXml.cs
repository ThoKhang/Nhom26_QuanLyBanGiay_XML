using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyBanGiay.CLASS
{
    internal class BanHangXml
    {
        // Giả sử file giày của bạn tên là Giay.xml
        private readonly string _path = Path.Combine(Application.StartupPath, "Giay.xml");

        public DataSet Data { get; private set; }
        public DataTable Table => Data.Tables[0];

        public BanHangXml()
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
                // Nếu chưa có thì tạo bảng rỗng đúng cấu trúc
                DataTable tb = new DataTable("SanPham");
                tb.Columns.Add("MaGiay");
                tb.Columns.Add("TenGiay");
                tb.Columns.Add("Loai");
                tb.Columns.Add("Size");
                tb.Columns.Add("Mau");
                tb.Columns.Add("SoLuongTon");
                tb.Columns.Add("DonGiaNhap");
                tb.Columns.Add("DonGiaBan");
                tb.Columns.Add("MaNhaCungCap");

                Data.Tables.Add(tb);
                Save();
            }
        }

        public void Save()
        {
            Data.WriteXml(_path);
        }

        public string GetPath() => _path;

        public DataRow FindByMaGiay(string maGiay)
        {
            return Table.AsEnumerable()
                .FirstOrDefault(r =>
                    r["MaGiay"].ToString().Equals(maGiay, StringComparison.OrdinalIgnoreCase));
        }

        public bool UpdateSoLuong(string maGiay, int newSoLuong)
        {
            foreach (DataRow row in Table.Rows)
            {
                if (row["MaGiay"].ToString().Equals(maGiay, StringComparison.OrdinalIgnoreCase))
                {
                    row["SoLuongTon"] = newSoLuong;
                    Save();
                    return true;
                }
            }
            return false;
        }
    }
}
