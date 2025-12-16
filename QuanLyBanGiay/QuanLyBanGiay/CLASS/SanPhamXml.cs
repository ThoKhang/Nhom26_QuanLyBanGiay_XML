using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyBanGiay.CLASS
{
    internal class SanPhamXml
    {
        private readonly string _xmlPath;
        public DataTable Table { get; private set; }

        public SanPhamXml()
        {
            _xmlPath = Path.Combine(Application.StartupPath, "Giay.xml");
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
            Table = new DataTable("SanPham");
            Table.Columns.Add("MaGiay", typeof(string));
            Table.Columns.Add("TenGiay", typeof(string));
            Table.Columns.Add("Loai", typeof(string));
            Table.Columns.Add("Size", typeof(int));
            Table.Columns.Add("Mau", typeof(string));
            Table.Columns.Add("SoLuongTon", typeof(int));
            Table.Columns.Add("DonGiaNhap", typeof(int));
            Table.Columns.Add("DonGiaBan", typeof(int));
            Table.Columns.Add("MaNhaCungCap", typeof(string));
        }

        public void Save()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(Table.Copy());
            ds.WriteXml(_xmlPath, XmlWriteMode.WriteSchema);
        }

        public string GetXmlPath()
        {
            return _xmlPath;
        }

        // ===== HỖ TRỢ =====

        public bool Exists(string maGiay)
        {
            return Table.AsEnumerable()
                        .Any(r => r["MaGiay"].ToString().Trim()
                                   .Equals(maGiay.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public DataRow FindByMa(string maGiay)
        {
            return Table.AsEnumerable()
                        .FirstOrDefault(r => r["MaGiay"].ToString().Trim()
                                              .Equals(maGiay.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        // ===== CRUD =====

        public void AddSanPham(string maGiay, string tenGiay, string loai,
                               int size, string mau, int soLuongTon,
                               int donGiaNhap, int donGiaBan, string maNcc)
        {
            DataRow row = Table.NewRow();
            row["MaGiay"] = maGiay.Trim();
            row["TenGiay"] = tenGiay.Trim();
            row["Loai"] = loai.Trim();
            row["Size"] = size;
            row["Mau"] = mau.Trim();
            row["SoLuongTon"] = soLuongTon;
            row["DonGiaNhap"] = donGiaNhap;
            row["DonGiaBan"] = donGiaBan;
            row["MaNhaCungCap"] = maNcc.Trim();

            Table.Rows.Add(row);
            Save();
        }

        public void UpdateSanPham(string maGiay, string tenGiay, string loai,
                                  int size, string mau, int soLuongTon,
                                  int donGiaNhap, int donGiaBan, string maNcc)
        {
            DataRow row = FindByMa(maGiay);
            if (row == null) return;

            row["TenGiay"] = tenGiay.Trim();
            row["Loai"] = loai.Trim();
            row["Size"] = size;
            row["Mau"] = mau.Trim();
            row["SoLuongTon"] = soLuongTon;
            row["DonGiaNhap"] = donGiaNhap;
            row["DonGiaBan"] = donGiaBan;
            row["MaNhaCungCap"] = maNcc.Trim();

            Save();
        }

        public void DeleteSanPham(string maGiay)
        {
            DataRow row = FindByMa(maGiay);
            if (row == null) return;

            Table.Rows.Remove(row);
            Save();
        }
    }
}
