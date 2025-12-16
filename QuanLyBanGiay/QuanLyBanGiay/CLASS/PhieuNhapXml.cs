using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyBanGiay.CLASS
{
    internal class PhieuNhapXml
    {
        private readonly string _xmlPath;
        public DataTable Table { get; private set; }

        public PhieuNhapXml()
        {
            _xmlPath = Path.Combine(Application.StartupPath, "PhieuNhap.xml");
            Load();
        }

        public void Load()
        {
            DataSet ds = new DataSet();

            if (File.Exists(_xmlPath))
            {
                ds.ReadXml(_xmlPath);
                if (ds.Tables.Count > 0)
                    Table = ds.Tables["PhieuNhap"];
                else
                    CreateSchema();
            }
            else
            {
                CreateSchema();
            }
        }

        private void CreateSchema()
        {
            Table = new DataTable("PhieuNhap");
            Table.Columns.Add("MaPhieuNhap", typeof(string));
            Table.Columns.Add("NgayNhap", typeof(string));
            Table.Columns.Add("MaNhanVien", typeof(string));
            Table.Columns.Add("MaNhaCungCap", typeof(string));
            Table.Columns.Add("TongTien", typeof(int));
        }

        public void Save()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(Table.Copy());
            ds.WriteXml(_xmlPath, XmlWriteMode.WriteSchema);
        }

        public bool Exists(string maPN)
        {
            return Table.AsEnumerable()
                        .Any(r => r["MaPhieuNhap"].ToString().Trim()
                              .Equals(maPN.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public DataRow FindByMa(string maPN)
        {
            return Table.AsEnumerable()
                         .FirstOrDefault(r => r["MaPhieuNhap"].ToString().Trim()
                                           .Equals(maPN.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public void Add(string maPN, string ngayNhap, string maNV,
                        string maNCC, int tongTien)
        {
            DataRow row = Table.NewRow();
            row["MaPhieuNhap"] = maPN.Trim();
            row["NgayNhap"] = ngayNhap.Trim();
            row["MaNhanVien"] = maNV.Trim();
            row["MaNhaCungCap"] = maNCC.Trim();
            row["TongTien"] = tongTien;

            Table.Rows.Add(row);
            Save();
        }

        public void Update(string maPN, string ngayNhap, string maNV,
                           string maNCC, int tongTien)
        {
            DataRow row = FindByMa(maPN);
            if (row == null) return;

            row["NgayNhap"] = ngayNhap.Trim();
            row["MaNhanVien"] = maNV.Trim();
            row["MaNhaCungCap"] = maNCC.Trim();
            row["TongTien"] = tongTien;

            Save();
        }

        public void Delete(string maPN)
        {
            DataRow row = FindByMa(maPN);
            if (row == null) return;

            Table.Rows.Remove(row);
            Save();
        }

        public string GetXmlPath() => _xmlPath;
    }
}
