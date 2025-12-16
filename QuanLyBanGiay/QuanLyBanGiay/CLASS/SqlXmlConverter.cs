using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace QuanLyBanGiay.CLASS
{
    internal class SqlXmlConverter
    {
        private string connectionString =
            @"Server=localhost;Database=QuanLyBanGiay;Trusted_Connection=True;TrustServerCertificate=True;";

        private string folder = Application.StartupPath;

        // Map tên bảng ↔ tên file XML
        // VD: bảng SanPham dùng file Giay.xml
        private string GetXmlFileNameForTable(string tableName)
        {
            switch (tableName)
            {
                case "SanPham":
                    return "Giay";          // Giay.xml
                default:
                    return tableName;      // NhanVien.xml, TaiKhoan.xml, ...
            }
        }

        /* =========================
         *  SQL  →  XML  (1 bảng)
         * ========================= */
        public void SqlToXml(string tableName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlDataAdapter da = new SqlDataAdapter($"SELECT * FROM {tableName}", conn);
                    DataSet ds = new DataSet();
                    da.Fill(ds, tableName);

                    string xmlName = GetXmlFileNameForTable(tableName);
                    string path = Path.Combine(folder, xmlName + ".xml");

                    ds.WriteXml(path, XmlWriteMode.IgnoreSchema);

                    MessageBox.Show($"Đã xuất: {xmlName}.xml", "Thành công");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi SQL → XML: " + ex.Message);
            }
        }

        /* =========================
         *  XML  →  SQL  (1 bảng, KHÔNG DELETE)
         *  (dùng nội bộ trong XmlToSql_All)
         * ========================= */
        private void InsertFromXml(string tableName)
        {
            string xmlName = GetXmlFileNameForTable(tableName);
            string xmlPath = Path.Combine(folder, xmlName + ".xml");

            if (!File.Exists(xmlPath))
            {
                MessageBox.Show($"Không tìm thấy file {xmlName}.xml !");
                return;
            }

            DataSet ds = new DataSet();
            ds.ReadXml(xmlPath);

            if (ds.Tables.Count == 0)
            {
                MessageBox.Show($"File {xmlName}.xml không có dữ liệu.");
                return;
            }

            DataTable tb = ds.Tables[0];

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                foreach (DataRow row in tb.Rows)
                {
                    string columns = string.Join(",", tb.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
                    string parameters = string.Join(",", tb.Columns.Cast<DataColumn>().Select(c => "@" + c.ColumnName));

                    string query = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        foreach (DataColumn col in tb.Columns)
                        {
                            object value = row[col.ColumnName] ?? DBNull.Value;
                            cmd.Parameters.AddWithValue("@" + col.ColumnName, value);
                        }

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        /* =========================
         *  XML  →  SQL  (toàn bộ)
         * ========================= */
        public void XmlToSql_All()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 1. XÓA THEO THỨ TỰ PHỤ THUỘC (tránh lỗi FK)
                    string[] deleteOrder =
                    {
                        "ChamCong",          // phụ thuộc NhanVien
                        "ChiTietPhieuNhap",  // phụ thuộc PhieuNhap, SanPham
                        "PhieuMua",          // phụ thuộc SanPham, TaiKhoan
                        "PhieuNhap",         // phụ thuộc NhanVien, NhaCungCap
                        "TaiKhoan",          // phụ thuộc NhanVien
                        "SanPham",           // phụ thuộc NhaCungCap
                        "NhaCungCap",
                        "NhanVien"
                    };

                    foreach (string t in deleteOrder)
                    {
                        using (SqlCommand cmd = new SqlCommand($"DELETE FROM {t}", conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                // 2. INSERT LẠI THEO THỨ TỰ ĐÚNG (bảng cha trước, con sau)
                string[] insertOrder =
                {
                    "NhanVien",
                    "NhaCungCap",
                    "SanPham",
                    "TaiKhoan",
                    "PhieuNhap",
                    "ChiTietPhieuNhap",
                    "PhieuMua",
                    "ChamCong"
                };

                foreach (string t in insertOrder)
                {
                    InsertFromXml(t);
                }

                MessageBox.Show("Đã nhập dữ liệu XML vào SQL (tất cả bảng) thành công!", "Thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi XML → SQL (All): " + ex.Message);
            }
        }

        /* =========================
         *  SQL  →  XML  (toàn bộ)
         * ========================= */
        public void SqlToXml_All()
        {
            try
            {
                string[] tables =
                {
                    "NhanVien",
                    "TaiKhoan",
                    "NhaCungCap",
                    "SanPham",
                    "PhieuNhap",
                    "ChiTietPhieuNhap",
                    "PhieuMua",
                    "ChamCong"
                };

                foreach (string t in tables)
                {
                    SqlToXml(t);
                }

                MessageBox.Show("Đã xuất tất cả bảng từ SQL ra XML!", "Thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi SQL → XML (All): " + ex.Message);
            }
        }
    }
}
