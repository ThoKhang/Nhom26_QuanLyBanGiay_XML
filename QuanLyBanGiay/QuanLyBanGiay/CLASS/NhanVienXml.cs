using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyBanGiay.CLASS
{
    internal class NhanVienXml
    {
        private readonly string _xmlPath;
        public DataTable Table { get; private set; }

        public NhanVienXml()
        {
            _xmlPath = Path.Combine(Application.StartupPath, "NhanVien.xml");
            Load();
        }

        public void Load()
        {
            DataSet ds = new DataSet();

            if (File.Exists(_xmlPath))
            {
                ds.ReadXml(_xmlPath);

                if (ds.Tables.Count > 0)
                {
                    Table = ds.Tables[0];
                }
                else
                {
                    CreateSchema();
                }
            }
            else
            {
                CreateSchema();
            }
        }

        private void CreateSchema()
        {
            Table = new DataTable("NhanVien");
            Table.Columns.Add("MaNhanVien", typeof(string));
            Table.Columns.Add("HoTen", typeof(string));
            Table.Columns.Add("NgaySinh", typeof(string));   // lưu dạng yyyy-MM-dd
            Table.Columns.Add("GioiTinh", typeof(string));
            Table.Columns.Add("DiaChi", typeof(string));
            Table.Columns.Add("DienThoai", typeof(string));
            Table.Columns.Add("TrangThai", typeof(string));
        }

        public void Save()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(Table.Copy());
            ds.WriteXml(_xmlPath, XmlWriteMode.WriteSchema);
        }

        public bool Exists(string maNV)
        {
            return Table.AsEnumerable()
                        .Any(r => r["MaNhanVien"].ToString().Trim()
                                   .Equals(maNV.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public DataRow FindByMa(string maNV)
        {
            return Table.AsEnumerable()
                        .FirstOrDefault(r => r["MaNhanVien"].ToString().Trim()
                                              .Equals(maNV.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public void AddNhanVien(string maNV, string hoTen, DateTime ngaySinh,
                                string gioiTinh, string diaChi, string dienThoai,
                                string trangThai)
        {
            DataRow row = Table.NewRow();
            row["MaNhanVien"] = maNV.Trim();
            row["HoTen"] = hoTen.Trim();
            row["NgaySinh"] = ngaySinh.ToString("yyyy-MM-dd");
            row["GioiTinh"] = gioiTinh.Trim();
            row["DiaChi"] = diaChi.Trim();
            row["DienThoai"] = dienThoai.Trim();
            row["TrangThai"] = trangThai.Trim();

            Table.Rows.Add(row);
            Save();
        }

        public void UpdateNhanVien(string maNV, string hoTen, DateTime ngaySinh,
                                   string gioiTinh, string diaChi, string dienThoai,
                                   string trangThai)
        {
            DataRow row = FindByMa(maNV);
            if (row == null) return;

            row["HoTen"] = hoTen.Trim();
            row["NgaySinh"] = ngaySinh.ToString("yyyy-MM-dd");
            row["GioiTinh"] = gioiTinh.Trim();
            row["DiaChi"] = diaChi.Trim();
            row["DienThoai"] = dienThoai.Trim();
            row["TrangThai"] = trangThai.Trim();

            Save();
        }

        public void DeleteNhanVien(string maNV)
        {
            DataRow row = FindByMa(maNV);
            if (row == null) return;

            Table.Rows.Remove(row);
            Save();
        }

        public string GetXmlPath()
        {
            return _xmlPath;
        }
    }
}
