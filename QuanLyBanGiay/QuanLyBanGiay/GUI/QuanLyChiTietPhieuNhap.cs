using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using QuanLyBanGiay.CLASS;

namespace QuanLyBanGiay.GUI
{
    public partial class QuanLyChiTietPhieuNhap : Form
    {
        private ChiTietPhieuNhapXml _ctpn;

        public QuanLyChiTietPhieuNhap()
        {
            InitializeComponent();

            _ctpn = new ChiTietPhieuNhapXml();

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = _ctpn.Table;
        }

        // ===== VALIDATION =====
        private bool ValidateInput(out string error, out int soLuong, out int donGia)
        {
            error = "";
            soLuong = 0;
            donGia = 0;

            if (string.IsNullOrEmpty(textBox1.Text.Trim()))
            {
                error = "Mã phiếu nhập không được để trống.";
                return false;
            }

            if (string.IsNullOrEmpty(textBox2.Text.Trim()))
            {
                error = "Mã giày không được để trống.";
                return false;
            }

            if (!int.TryParse(textBox7.Text.Trim(), out soLuong) || soLuong <= 0)
            {
                error = "Số lượng phải là số nguyên > 0.";
                return false;
            }

            if (!int.TryParse(textBox4.Text.Trim(), out donGia) || donGia <= 0)
            {
                error = "Đơn giá nhập phải là số nguyên > 0.";
                return false;
            }

            return true;
        }

        // ====== AUTO CALC THANH TIEN ======
        private void UpdateThanhTien()
        {
            int sl, dg;

            if (int.TryParse(textBox7.Text, out sl) &&
                int.TryParse(textBox4.Text, out dg))
            {
                textBox5.Text = (sl * dg).ToString();
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e) => UpdateThanhTien();
        private void textBox4_TextChanged(object sender, EventArgs e) => UpdateThanhTien();

        // ===== BUTTON1: THÊM =====


        // ===== BUTTON2: SỬA =====


        // ===== BUTTON3: XÓA =====
        private void button3_Click(object sender, EventArgs e)
        {
            string maPN = textBox1.Text.Trim();
            string maGiay = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(maPN) || string.IsNullOrEmpty(maGiay))
            {
                MessageBox.Show("Vui lòng chọn mã phiếu nhập và mã giày để xóa.");
                return;
            }

            if (!_ctpn.Exists(maPN, maGiay))
            {
                MessageBox.Show("Không tìm thấy chi tiết để xóa.");
                return;
            }

            if (MessageBox.Show("Bạn muốn xóa chi tiết này?", "Xóa",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _ctpn.Delete(maPN, maGiay);
                MessageBox.Show("Xóa thành công.");
                Reload();
            }
        }

        // ====== LOAD LAI ======
        private void Reload()
        {
            _ctpn.Load();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _ctpn.Table;
        }

        // ===== BUTTON4: HIỂN THỊ =====


        // ===== BUTTON5: XEM XML =====


        // ===== BUTTON6: TÌM KIẾM THEO MÃ PHIẾU NHẬP =====
        private void button6_Click(object sender, EventArgs e)
        {
            string maPN = textBox3.Text.Trim();

            if (string.IsNullOrEmpty(maPN))
            {
                MessageBox.Show("Vui lòng nhập mã phiếu nhập cần tìm.");
                return;
            }

            // Lấy danh sách chi tiết phiếu nhập theo mã
            DataTable kq = _ctpn.FindByMaPhieuNhap(maPN);

            if (kq == null || kq.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy chi tiết phiếu nhập.");
                return;
            }

            // Gán kết quả lên DataGridView
            dataGridView1.DataSource = kq;

            // ====== ĐƯA DỮ LIỆU LÊN TEXTBOX ======
            DataRow row = kq.Rows[0];  // lấy dòng đầu tiên

            textBox1.Text = row["MaPhieuNhap"].ToString();
            textBox2.Text = row["MaGiay"].ToString();
            textBox7.Text = row["SoLuong"].ToString();
            textBox4.Text = row["DonGiaNhap"].ToString();
            textBox5.Text = row["ThanhTien"].ToString();

            // ====== TÔ ĐẬM DÒNG TRONG DATAGRIDVIEW ======
            dataGridView1.ClearSelection();

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows[0].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];

                // Cuộn tới dòng đầu
                dataGridView1.FirstDisplayedScrollingRowIndex = 0;
            }
        }


        // ===== CLICK LÊN DÒNG =====
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow r = dataGridView1.Rows[e.RowIndex];

            textBox1.Text = r.Cells["MaPhieuNhap"].Value.ToString();
            textBox2.Text = r.Cells["MaGiay"].Value.ToString();
            textBox7.Text = r.Cells["SoLuong"].Value.ToString();
            textBox4.Text = r.Cells["DonGiaNhap"].Value.ToString();
            textBox5.Text = r.Cells["ThanhTien"].Value.ToString();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            int sl, dg;
            string error;

            if (!ValidateInput(out error, out sl, out dg))
            {
                MessageBox.Show(error);
                return;
            }

            string maPN = textBox1.Text.Trim();
            string maGiay = textBox2.Text.Trim();

            if (!_ctpn.Exists(maPN, maGiay))
            {
                MessageBox.Show("Không tìm thấy chi tiết để sửa.");
                return;
            }

            _ctpn.Update(maPN, maGiay, sl, dg);

            MessageBox.Show("Sửa chi tiết phiếu nhập thành công.");
            Reload();

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int sl, dg;
            string error;

            if (!ValidateInput(out error, out sl, out dg))
            {
                MessageBox.Show(error);
                return;
            }

            string maPN = textBox1.Text.Trim();
            string maGiay = textBox2.Text.Trim();

            if (_ctpn.Exists(maPN, maGiay))
            {
                MessageBox.Show("Chi tiết này đã tồn tại trong phiếu nhập.");
                return;
            }

            _ctpn.Add(maPN, maGiay, sl, dg);

            MessageBox.Show("Thêm chi tiết phiếu nhập thành công.");
            Reload();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Reload();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            string path = _ctpn.GetXmlPath();
            if (File.Exists(path))
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
        }

        private void QuanLyChiTietPhieuNhap_Load(object sender, EventArgs e)
        {

        }
    }
}
