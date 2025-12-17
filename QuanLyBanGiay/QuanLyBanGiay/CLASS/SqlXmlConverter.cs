using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace QuanLyBanGiay.CLASS
{
    internal class SqlXmlConverter
    {
        private string connectionString =
            @"Server=localhost\SQLEXPRESS01;Database=QuanLyBanGiay;User Id=sa;Password=12345;TrustServerCertificate=True;";

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

                    // QUAN TRỌNG: Xuất kèm schema để đọc lại giữ đúng kiểu (Date/Time/Number...)
                    ds.WriteXml(path, XmlWriteMode.WriteSchema);

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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Lấy schema chuẩn từ SQL để biết kiểu dữ liệu thật
                DataTable sqlSchema = new DataTable(tableName);
                using (SqlDataAdapter daSchema = new SqlDataAdapter($"SELECT TOP 0 * FROM {tableName}", conn))
                {
                    daSchema.FillSchema(sqlSchema, SchemaType.Source);
                }

                string columns = string.Join(",", sqlSchema.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
                string parameters = string.Join(",", sqlSchema.Columns.Cast<DataColumn>().Select(c => "@" + c.ColumnName));
                string query = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";

                foreach (DataRow row in xmlTable.Rows)
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        foreach (DataColumn sqlCol in sqlSchema.Columns)
                        {
                            string colName = sqlCol.ColumnName;
                            SqlDbType dbType = GetSqlDbType(sqlCol.DataType);

                            object rawValue = DBNull.Value;

                            if (xmlTable.Columns.Contains(colName))
                                rawValue = row[colName];

                            // NULL / DBNull
                            if (rawValue == null || rawValue == DBNull.Value)
                            {
                                cmd.Parameters.Add("@" + colName, dbType).Value = DBNull.Value;
                                continue;
                            }

                            // Nếu là string thì xử lý rỗng + ép kiểu theo cột SQL
                            if (rawValue is string s)
                            {
                                s = s.Trim();

                                // chuỗi rỗng -> NULL (đặc biệt TIME/DATE)
                                if (string.IsNullOrEmpty(s))
                                {
                                    cmd.Parameters.Add("@" + colName, dbType).Value = DBNull.Value;
                                    continue;
                                }

                                object converted = ConvertToTargetType(s, sqlCol.DataType);

                                if (converted == null)
                                {
                                    // Dữ liệu bẩn -> báo rõ để bạn biết cột nào, giá trị nào
                                    throw new Exception($"Bảng {tableName} - Cột {colName} không convert được giá trị: '{s}'");
                                }

                                cmd.Parameters.Add("@" + colName, dbType).Value = converted;
                            }
                            else
                            {
                                // Đã đúng kiểu (DateTime/TimeSpan/int...)
                                cmd.Parameters.Add("@" + colName, dbType).Value = rawValue;
                            }
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

        // ===== Helpers =====

        private static object ConvertToTargetType(string s, Type targetType)
        {
            try
            {
                if (targetType == typeof(string))
                    return s;

                // DATE/DATETIME/DATETIME2
                if (targetType == typeof(DateTime))
                {
                    // nhận cả: 2025-12-15 và 2025-11-30T00:00:00+07:00
                    if (s.Contains("T") && (s.Contains("+") || s.EndsWith("Z")))
                        return DateTimeOffset.Parse(s, CultureInfo.InvariantCulture).DateTime;

                    // fallback
                    return DateTime.Parse(s, CultureInfo.InvariantCulture);
                }

                if (targetType == typeof(DateTimeOffset))
                    return DateTimeOffset.Parse(s, CultureInfo.InvariantCulture);

                // TIME
                if (targetType == typeof(TimeSpan))
                {
                    // format phổ biến của bạn: HH:mm
                    return TimeSpan.ParseExact(s, @"hh\:mm", CultureInfo.InvariantCulture);
                }

                if (targetType == typeof(int)) return int.Parse(s, CultureInfo.InvariantCulture);
                if (targetType == typeof(long)) return long.Parse(s, CultureInfo.InvariantCulture);
                if (targetType == typeof(decimal)) return decimal.Parse(s, CultureInfo.InvariantCulture);
                if (targetType == typeof(double)) return double.Parse(s, CultureInfo.InvariantCulture);
                if (targetType == typeof(bool)) return bool.Parse(s);

                // kiểu khác: thử convert chung
                return Convert.ChangeType(s, targetType, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

        private static SqlDbType GetSqlDbType(Type t)
        {
            if (t == typeof(string)) return SqlDbType.NVarChar;
            if (t == typeof(int)) return SqlDbType.Int;
            if (t == typeof(long)) return SqlDbType.BigInt;
            if (t == typeof(decimal)) return SqlDbType.Decimal;
            if (t == typeof(double)) return SqlDbType.Float;
            if (t == typeof(bool)) return SqlDbType.Bit;
            if (t == typeof(DateTime)) return SqlDbType.DateTime2;
            if (t == typeof(DateTimeOffset)) return SqlDbType.DateTimeOffset;
            if (t == typeof(TimeSpan)) return SqlDbType.Time;

            return SqlDbType.NVarChar;
        }
    }
}
