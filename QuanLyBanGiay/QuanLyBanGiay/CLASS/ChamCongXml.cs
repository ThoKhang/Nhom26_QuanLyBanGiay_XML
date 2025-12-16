using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyBanGiay.CLASS
{
    internal class ChamCongXml
    {
        private string _path = Path.Combine(Application.StartupPath, "ChamCong.xml");

        public DataSet Data { get; private set; }
        public DataTable Table => Data.Tables[0];

        public ChamCongXml()
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
                // Tạo cấu trúc file mới
                DataTable tb = new DataTable("ChamCong");
                tb.Columns.Add("MaNhanVien");
                tb.Columns.Add("Ngay");      // yyyy-MM-dd
                tb.Columns.Add("TrangThai"); // DiLam, NghiPhep, NghiKhongPhep
                tb.Columns.Add("GioVao");
                tb.Columns.Add("GioRa");

                Data.Tables.Add(tb);
                Save();
            }
        }

        public void Save()
        {
            Data.WriteXml(_path);
        }

        public string GetPath() => _path;

        public void ThemChamCong(string maNhanVien, string trangThai, DateTime? gioVao = null, DateTime? gioRa = null)
        {
            if (string.IsNullOrEmpty(maNhanVien))
            {
                MessageBox.Show("Không xác định được mã nhân viên để chấm công.",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime now = DateTime.Now;
            string ngay = now.ToString("yyyy-MM-dd");
            string strGioVao = (gioVao ?? now).ToString("HH:mm");
            string strGioRa = gioRa.HasValue ? gioRa.Value.ToString("HH:mm") : "";

            DataRow row = Table.NewRow();
            row["MaNhanVien"] = maNhanVien;
            row["Ngay"] = ngay;
            row["TrangThai"] = trangThai;  // "DiLam"
            row["GioVao"] = strGioVao;
            row["GioRa"] = strGioRa;

            Table.Rows.Add(row);
            Save();
        }
    }
}
