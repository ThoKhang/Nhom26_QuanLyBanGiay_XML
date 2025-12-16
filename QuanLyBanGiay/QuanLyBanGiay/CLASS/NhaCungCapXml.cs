using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace QuanLyBanGiay.CLASS
{
    internal class NhaCungCapXml
    {
        private string _path = Path.Combine(Application.StartupPath, "NhaCungCap.xml");

        public DataSet Data { get; private set; }
        public DataTable Table => Data.Tables[0];

        public NhaCungCapXml()
        {
            Load();
        }

        public void Load()
        {
            Data = new DataSet();

            if (File.Exists(_path))
                Data.ReadXml(_path);
            else
            {
                // Tạo bảng rỗng nếu file chưa tồn tại
                DataTable tb = new DataTable("NhaCungCap");

                tb.Columns.Add("MaNhaCungCap");
                tb.Columns.Add("TenNhaCungCap");
                tb.Columns.Add("DiaChi");
                tb.Columns.Add("DienThoai");

                Data.Tables.Add(tb);
                Save();
            }
        }

        public void Save()
        {
            Data.WriteXml(_path);
        }

        public string GetPath() => _path;

        public bool Exists(string ma)
        {
            return Table.AsEnumerable().Any(r =>
                r["MaNhaCungCap"].ToString().Equals(ma, StringComparison.OrdinalIgnoreCase));
        }

        public void Add(string ma, string ten, string diaChi, string dienThoai)
        {
            Table.Rows.Add(ma, ten, diaChi, dienThoai);
            Save();
        }

        public bool Update(string ma, string ten, string diaChi, string dienThoai)
        {
            foreach (DataRow row in Table.Rows)
            {
                if (row["MaNhaCungCap"].ToString() == ma)
                {
                    row["TenNhaCungCap"] = ten;
                    row["DiaChi"] = diaChi;
                    row["DienThoai"] = dienThoai;

                    Save();
                    return true;
                }
            }
            return false;
        }

        public bool Delete(string ma)
        {
            foreach (DataRow row in Table.Rows)
            {
                if (row["MaNhaCungCap"].ToString() == ma)
                {
                    Table.Rows.Remove(row);
                    Save();
                    return true;
                }
            }
            return false;
        }

        public DataRow Find(string ma)
        {
            return Table.AsEnumerable()
                .FirstOrDefault(r => r["MaNhaCungCap"].ToString().Equals(ma, StringComparison.OrdinalIgnoreCase));
        }
    }
}
