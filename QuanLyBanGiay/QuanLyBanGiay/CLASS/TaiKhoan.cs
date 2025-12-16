using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyBanGiay.CLASS
{
    internal class TaiKhoan
    {
        private readonly string _xmlPath;
        public DataTable Table { get; private set; }

        public TaiKhoan()
        {
            _xmlPath = Path.Combine(Application.StartupPath, "TaiKhoan.xml");
            Load();
        }

        // Đọc XML vào DataTable
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

        // Tạo cấu trúc bảng nếu chưa có file
        private void CreateSchema()
        {
            Table = new DataTable("TaiKhoan");
            Table.Columns.Add("MaNhanVien", typeof(string));
            Table.Columns.Add("TenDangNhap", typeof(string));
            Table.Columns.Add("MatKhau", typeof(string));
            Table.Columns.Add("Quyen", typeof(string));
        }

        // Ghi lại DataTable ra XML
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

        // ==== HÀM HỖ TRỢ ====
        public bool ExistsByMaNV(string maNV)
        {
            return Table.AsEnumerable()
                        .Any(r => r["MaNhanVien"].ToString().Trim()
                                   .Equals(maNV.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public bool ExistsByTenDangNhap(string tenDangNhap)
        {
            return Table.AsEnumerable()
                        .Any(r => r["TenDangNhap"].ToString().Trim()
                                   .Equals(tenDangNhap.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public DataRow FindByMaNV(string maNV)
        {
            return Table.AsEnumerable()
                        .FirstOrDefault(r => r["MaNhanVien"].ToString().Trim()
                                              .Equals(maNV.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        // ==== CRUD TÀI KHOẢN ====

        public void AddTaiKhoan(string maNV, string tenDangNhap, string matKhau, string quyen)
        {
            DataRow row = Table.NewRow();
            row["MaNhanVien"] = maNV.Trim();
            row["TenDangNhap"] = tenDangNhap.Trim();
            row["MatKhau"] = matKhau.Trim();
            row["Quyen"] = quyen.Trim();

            Table.Rows.Add(row);
            Save();
        }

        public void DeleteTaiKhoanByMaNV(string maNV)
        {
            DataRow row = FindByMaNV(maNV);
            if (row == null) return;

            Table.Rows.Remove(row);
            Save();
        }

        // ==== ĐỔI MẬT KHẨU (để form Đổi mật khẩu dùng tiếp) ====

        public bool DoiMatKhau(string tenDangNhap, string matKhauMoi, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                DataRow row = Table.AsEnumerable()
                                   .FirstOrDefault(r => r["TenDangNhap"].ToString().Trim()
                                             .Equals(tenDangNhap.Trim(), StringComparison.OrdinalIgnoreCase));

                if (row == null)
                {
                    errorMessage = "Không tìm thấy tài khoản cần đổi mật khẩu.";
                    return false;
                }

                row["MatKhau"] = matKhauMoi.Trim();
                Save();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "Lỗi khi đổi mật khẩu: " + ex.Message;
                return false;
            }
        }
    }
}
