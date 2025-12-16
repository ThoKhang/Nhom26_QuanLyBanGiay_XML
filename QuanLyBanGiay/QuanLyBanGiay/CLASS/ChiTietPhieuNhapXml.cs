using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyBanGiay.CLASS
{
    internal class ChiTietPhieuNhapXml
    {
        private readonly string _xmlPath;
        public DataTable Table { get; private set; }

        public ChiTietPhieuNhapXml()
        {
            _xmlPath = Path.Combine(Application.StartupPath, "ChiTietPhieuNhap.xml");
            Load();
        }

        public void Load()
        {
            DataSet ds = new DataSet();

            if (File.Exists(_xmlPath))
            {
                ds.ReadXml(_xmlPath);
                if (ds.Tables.Count > 0)
                    Table = ds.Tables["ChiTietPhieuNhap"];
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
            Table = new DataTable("ChiTietPhieuNhap");
            Table.Columns.Add("MaPhieuNhap", typeof(string));
            Table.Columns.Add("MaGiay", typeof(string));
            Table.Columns.Add("SoLuong", typeof(int));
            Table.Columns.Add("DonGiaNhap", typeof(int));
            Table.Columns.Add("ThanhTien", typeof(int));
        }

        public void Save()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(Table.Copy());
            ds.WriteXml(_xmlPath, XmlWriteMode.WriteSchema);
        }

        public bool Exists(string maPN, string maGiay)
        {
            return Table.AsEnumerable().Any(r =>
                   r["MaPhieuNhap"].ToString().Trim().Equals(maPN.Trim(), StringComparison.OrdinalIgnoreCase)
                   && r["MaGiay"].ToString().Trim().Equals(maGiay.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public DataRow Find(string maPN, string maGiay)
        {
            return Table.AsEnumerable()
                .FirstOrDefault(r =>
                    r["MaPhieuNhap"].ToString().Trim().Equals(maPN.Trim(), StringComparison.OrdinalIgnoreCase)
                    && r["MaGiay"].ToString().Trim().Equals(maGiay.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public void Add(string maPN, string maGiay, int soLuong, int donGiaNhap)
        {
            DataRow row = Table.NewRow();
            row["MaPhieuNhap"] = maPN.Trim();
            row["MaGiay"] = maGiay.Trim();
            row["SoLuong"] = soLuong;
            row["DonGiaNhap"] = donGiaNhap;
            row["ThanhTien"] = soLuong * donGiaNhap;

            Table.Rows.Add(row);
            Save();
        }

        public bool Update(string maPN, string maGiay, int soLuong, int donGiaNhap)
        {
            foreach (DataRow row in Table.Rows)
            {
                if (row["MaPhieuNhap"].ToString() == maPN &&
                    row["MaGiay"].ToString() == maGiay)
                {
                    row["SoLuong"] = soLuong;
                    row["DonGiaNhap"] = donGiaNhap;

                    // Thành tiền = SL * Đơn giá nhập
                    int thanhTien = soLuong * donGiaNhap;
                    row["ThanhTien"] = thanhTien;

                    Save();   // NHỚ LƯU XML
                    return true;
                }
            }

            return false;
        }


        public void Delete(string maPN, string maGiay)
        {
            DataRow row = Find(maPN, maGiay);
            if (row == null) return;

            Table.Rows.Remove(row);
            Save();
        }

        public string GetXmlPath() => _xmlPath;

        public DataTable FindByMaPhieuNhap(string maPN)
        {
            return Table.AsEnumerable()
                        .Where(r => r["MaPhieuNhap"].ToString().Trim()
                                    .Equals(maPN.Trim(), StringComparison.OrdinalIgnoreCase))
                        .CopyToDataTable();
        }
    }
}
