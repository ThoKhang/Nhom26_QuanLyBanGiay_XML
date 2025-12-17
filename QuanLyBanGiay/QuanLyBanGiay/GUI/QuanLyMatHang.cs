using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using QuanLyBanGiay.CLASS;

namespace QuanLyBanGiay.GUI
{
    public partial class QuanLyMatHang : Form
    {
        private SanPhamXml _sp;

        public QuanLyMatHang()
        {
            InitializeComponent();

            _sp = new SanPhamXml();

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = _sp.Table;
        }

        // ====== VALIDATE ======
        private bool ValidateInput(out string message,
                                   out int size,
                                   out int soLuongTon,
                                   out int donGiaNhap,
                                   out int donGiaBan)
        {
            message = "";
            size = soLuongTon = donGiaNhap = donGiaBan = 0;

            string maGiay = textBox1.Text.Trim();
            string tenGiay = textBox2.Text.Trim();
            string loai = textBox7.Text.Trim();
            string sSize = textBox6.Text.Trim();
            string mau = textBox4.Text.Trim();
            string sSoLuong = textBox5.Text.Trim();
            string sDonGiaNhap = textBox8.Text.Trim();
            string sDonGiaBan = textBox10.Text.Trim();
            string maNcc = textBox9.Text.Trim();

            if (string.IsNullOrEmpty(maGiay))
            {
                message = "Mã giày không được để trống.";
                return false;
            }
            if (string.IsNullOrEmpty(tenGiay))
            {
                message = "Tên giày không được để trống.";
                return false;
            }
            if (string.IsNullOrEmpty(loai))
            {
                message = "Loại giày không được để trống.";
                return false;
            }
            if (!int.TryParse(sSize, out size) || size <= 0)
            {
                message = "Size phải là số nguyên dương.";
                return false;
            }
            if (string.IsNullOrEmpty(mau))
            {
                message = "Màu không được để trống.";
                return false;
            }
            if (!int.TryParse(sSoLuong, out soLuongTon) || soLuongTon < 0)
            {
                message = "Số lượng tồn phải là số nguyên >= 0.";
                return false;
            }
            if (!int.TryParse(sDonGiaNhap, out donGiaNhap) || donGiaNhap <= 0)
            {
                message = "Đơn giá nhập phải là số nguyên dương.";
                return false;
            }
            if (!int.TryParse(sDonGiaBan, out donGiaBan) || donGiaBan <= 0)
            {
                message = "Đơn giá bán phải là số nguyên dương.";
                return false;
            }
            if (donGiaBan < donGiaNhap)
            {
                message = "Đơn giá bán phải lớn hơn hoặc bằng đơn giá nhập.";
                return false;
            }
            if (string.IsNullOrEmpty(maNcc))
            {
                message = "Mã nhà cung cấp không được để trống.";
                return false;
            }

            return true;
        }

        private void LoadRowToControls(DataRow row)
        {
            if (row == null) return;

            textBox1.Text = row["MaGiay"].ToString();
            textBox2.Text = row["TenGiay"].ToString();
            textBox7.Text = row["Loai"].ToString();
            textBox6.Text = row["Size"].ToString();
            textBox4.Text = row["Mau"].ToString();
            textBox5.Text = row["SoLuongTon"].ToString();
            textBox8.Text = row["DonGiaNhap"].ToString();
            textBox10.Text = row["DonGiaBan"].ToString();
            textBox9.Text = row["MaNhaCungCap"].ToString();
        }

        // ====== BUTTON1: THÊM ======
        private void button1_Click_1(object sender, EventArgs e)
        {
            int size, soLuongTon, donGiaNhap, donGiaBan;
            string error;

            if (!ValidateInput(out error, out size, out soLuongTon, out donGiaNhap, out donGiaBan))
            {
                MessageBox.Show(error, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maGiay = textBox1.Text.Trim();

            if (_sp.Exists(maGiay))
            {
                MessageBox.Show("Mã giày đã tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _sp.AddSanPham(
                maGiay,
                textBox2.Text.Trim(),
                textBox7.Text.Trim(),
                size,
                textBox4.Text.Trim(),
                soLuongTon,
                donGiaNhap,
                donGiaBan,
                textBox9.Text.Trim()
            );

            MessageBox.Show("Thêm mặt hàng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _sp.Table;
        }

        // ====== BUTTON2: SỬA ======
        private void button2_Click_1(object sender, EventArgs e)
        {
            int size, soLuongTon, donGiaNhap, donGiaBan;
            string error;

            if (!ValidateInput(out error, out size, out soLuongTon, out donGiaNhap, out donGiaBan))
            {
                MessageBox.Show(error, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maGiay = textBox1.Text.Trim();

            if (!_sp.Exists(maGiay))
            {
                MessageBox.Show("Không tìm thấy mã giày để sửa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _sp.UpdateSanPham(
                maGiay,
                textBox2.Text.Trim(),
                textBox7.Text.Trim(),
                size,
                textBox4.Text.Trim(),
                soLuongTon,
                donGiaNhap,
                donGiaBan,
                textBox9.Text.Trim()
            );

            MessageBox.Show("Sửa mặt hàng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _sp.Table;
        }

        // ====== BUTTON3: XÓA ======
        private void button3_Click_1(object sender, EventArgs e)
        {
            string maGiay = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(maGiay))
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập mã giày cần xóa.",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_sp.Exists(maGiay))
            {
                MessageBox.Show("Không tìm thấy mã giày cần xóa.",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa mặt hàng " + maGiay + " ?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _sp.DeleteSanPham(maGiay);
                MessageBox.Show("Xóa mặt hàng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = _sp.Table;
            }
        }

        // ====== BUTTON4: HIỂN THỊ / NẠP LẠI ======
        private void button4_Click_1(object sender, EventArgs e)
        {
            _sp.Load();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _sp.Table;
        }

        // ====== BUTTON5: MỞ FILE XML (CŨ – COMMENT) ======
        /*
        private void button5_Click(object sender, EventArgs e)
        {
            string path = _sp.GetXmlPath();

            if (!File.Exists(path))
            {
                MessageBox.Show("Không tìm thấy file SanPham.xml", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
        }
        */

        // ====== BUTTON5: PREVIEW BẰNG XSLT ======
        private void button5_Click(object sender, EventArgs e)
        {
            PreviewSanPhamBangXSLT();
        }

        // ====== XSLT PREVIEW (MINH HỌA – KHÔNG ẢNH HƯỞNG LOGIC) ======
        private void PreviewSanPhamBangXSLT()
        {
            try
            {
                string xmlPath = Path.Combine(Application.StartupPath, "Giay.xml");
                string xslPath = Path.Combine(Application.StartupPath, "Giay.xsl");
                string htmlPath = Path.Combine(Application.StartupPath, "Giay_Preview.html");

                if (!File.Exists(xslPath))
                {
                    MessageBox.Show("Chưa có file Giay.xsl để preview.",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                XslCompiledTransform xslt = new XslCompiledTransform();
                xslt.Load(xslPath);
                xslt.Transform(xmlPath, htmlPath);

                Process.Start(new ProcessStartInfo(htmlPath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi preview XSLT: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====== BUTTON6: TÌM KIẾM ======
        private void button6_Click(object sender, EventArgs e)
        {
            string maTim = textBox3.Text.Trim();

            if (string.IsNullOrEmpty(maTim))
            {
                MessageBox.Show("Vui lòng nhập mã giày cần tìm.",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRow row = _sp.FindByMa(maTim);

            if (row == null)
            {
                MessageBox.Show("Không tìm thấy giày với mã: " + maTim,
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            LoadRowToControls(row);

            foreach (DataGridViewRow dgRow in dataGridView1.Rows)
            {
                if (dgRow.Cells["MaGiay"].Value != null &&
                    dgRow.Cells["MaGiay"].Value.ToString().Trim()
                        .Equals(maTim, StringComparison.OrdinalIgnoreCase))
                {
                    dgRow.Selected = true;
                    dataGridView1.CurrentCell = dgRow.Cells[0];
                    break;
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow dgRow = dataGridView1.Rows[e.RowIndex];
            if (dgRow.DataBoundItem is DataRowView drv)
            {
                LoadRowToControls(drv.Row);
            }
        }

        private void QuanLyMatHang_Load(object sender, EventArgs e)
        {
        }
    }
}
