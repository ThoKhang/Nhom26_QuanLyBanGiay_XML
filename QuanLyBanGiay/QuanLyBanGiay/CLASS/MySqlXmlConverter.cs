using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyBanGiay.CLASS
{
    internal class MySqlXmlConverter
    {
        // ====== CONNECTION MYSQL ======
        string connectionString ="Server=localhost;Port=3306;Database=QuanLyBanGiay;Uid=root;Pwd=12345;";

        private string folder = Application.StartupPath;

        // Map tên bảng ↔ tên file XML
        private string GetXmlFileNameForTable(string tableName)
        {
            switch (tableName)
            {
                case "SanPham":
                    return "Giay";
                default:
                    return tableName;
            }
        }

        /* =========================
         *  MYSQL  →  XML  (1 bảng)
         * ========================= */
        public void MySqlToXml(string tableName)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    MySqlDataAdapter da =
                        new MySqlDataAdapter($"SELECT * FROM {tableName}", conn);

                    DataSet ds = new DataSet();
                    da.Fill(ds, tableName);

                    string xmlName = GetXmlFileNameForTable(tableName);
                    string path = Path.Combine(folder, xmlName + ".xml");

                    ds.WriteXml(path, XmlWriteMode.WriteSchema);

                    MessageBox.Show($"Đã xuất MySQL → XML: {xmlName}.xml", "Thành công");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi MySQL → XML: " + ex.Message);
            }
        }

        /* =========================
         *  XML  →  MYSQL  (1 bảng)
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

            DataTable xmlTable = ds.Tables[0];

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Lấy schema chuẩn từ MySQL
                DataTable mysqlSchema = new DataTable(tableName);
                using (MySqlDataAdapter daSchema =
                       new MySqlDataAdapter($"SELECT * FROM {tableName} LIMIT 0", conn))
                {
                    daSchema.FillSchema(mysqlSchema, SchemaType.Source);
                }

                string columns = string.Join(",", mysqlSchema.Columns
                    .Cast<DataColumn>().Select(c => c.ColumnName));

                string parameters = string.Join(",", mysqlSchema.Columns
                    .Cast<DataColumn>().Select(c => "@" + c.ColumnName));

                string query =
                    $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";

                foreach (DataRow row in xmlTable.Rows)
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        foreach (DataColumn col in mysqlSchema.Columns)
                        {
                            string colName = col.ColumnName;
                            object value = DBNull.Value;

                            if (xmlTable.Columns.Contains(colName))
                                value = row[colName];

                            if (value == null || value == DBNull.Value)
                            {
                                cmd.Parameters.AddWithValue("@" + colName, DBNull.Value);
                                continue;
                            }

                            if (value is string s)
                            {
                                s = s.Trim();
                                if (string.IsNullOrEmpty(s))
                                {
                                    cmd.Parameters.AddWithValue("@" + colName, DBNull.Value);
                                    continue;
                                }

                                object converted =
                                    ConvertToTargetType(s, col.DataType);

                                if (converted == null)
                                    throw new Exception(
                                        $"MySQL {tableName}.{colName} không convert được '{s}'");

                                cmd.Parameters.AddWithValue("@" + colName, converted);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@" + colName, value);
                            }
                        }

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        /* =========================
         *  XML  →  MYSQL  (ALL)
         * ========================= */
        public void XmlToMySql_All()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string[] deleteOrder =
                    {
                        "ChamCong",
                        "ChiTietPhieuNhap",
                        "PhieuMua",
                        "PhieuNhap",
                        "TaiKhoan",
                        "SanPham",
                        "NhaCungCap",
                        "NhanVien"
                    };

                    foreach (string t in deleteOrder)
                    {
                        using (MySqlCommand cmd =
                               new MySqlCommand($"DELETE FROM {t}", conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

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

                MessageBox.Show("Đã nhập XML → MySQL (tất cả bảng) thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi XML → MySQL: " + ex.Message);
            }
        }

        /* =========================
         *  MYSQL  →  XML  (ALL)
         * ========================= */
        public void MySqlToXml_All()
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
                    MySqlToXml(t);
                }

                MessageBox.Show("Đã xuất tất cả bảng từ MySQL ra XML!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi MySQL → XML (All): " + ex.Message);
            }
        }

        // ===== Helpers =====

        private static object ConvertToTargetType(string s, Type targetType)
        {
            try
            {
                if (targetType == typeof(string)) return s;
                if (targetType == typeof(int)) return int.Parse(s);
                if (targetType == typeof(long)) return long.Parse(s);
                if (targetType == typeof(decimal)) return decimal.Parse(s, CultureInfo.InvariantCulture);
                if (targetType == typeof(double)) return double.Parse(s, CultureInfo.InvariantCulture);
                if (targetType == typeof(bool)) return bool.Parse(s);

                if (targetType == typeof(DateTime))
                    return DateTime.Parse(s, CultureInfo.InvariantCulture);

                if (targetType == typeof(TimeSpan))
                    return TimeSpan.Parse(s);

                return Convert.ChangeType(s, targetType);
            }
            catch
            {
                return null;
            }
        }
    }
}
