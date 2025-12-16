using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using QuanLyBanGiay.CLASS;

namespace QuanLyBanGiay.GUI
{
    public partial class QuanLyPhieuNhap : Form
    {
        private PhieuNhapXml _pn;

        public QuanLyPhieuNhap()
        {
            InitializeComponent();

            _pn = new PhieuNhapXml();

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = _pn.Table;
        }

        // ========== VALIDATION ==========
        private bool ValidateInput(out string message, out int tongTien)
        {
            message = "";
            tongTien = 0;

            if (string.IsNullOrEmpty(textBox1.Text.Trim()))
            {
                message = "Mã phiếu nhập không được để trống.";
                return false;
            }

            if (string.IsNullOrEmpty(textBox7.Text.Trim()))
            {
                message = "Mã nhân viên không được để trống.";
                return false;
            }

            if (string.IsNullOrEmpty(textBox6.Text.Trim()))
            {
                message = "Mã nhà cung cấp không được để trống.";
                return false;
            }

            if (!int.TryParse(textBox4.Text.Trim(), out tongTien) || tongTien < 0)
            {
                message = "Tổng tiền phải là số nguyên >= 0.";
                return false;
            }

            return true;
        }

        // ========== BUTTON1: THÊM ==========


        // ========== BUTTON2: SỬA ==========


        // ========== BUTTON3: XÓA ==========
        private void button3_Click(object sender, EventArgs e)
        {
            string maPN = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(maPN))
            {
                MessageBox.Show("Vui lòng nhập mã phiếu nhập.");
                return;
            }

            if (!_pn.Exists(maPN))
            {
                MessageBox.Show("Không tìm thấy phiếu nhập để xóa.");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xóa", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _pn.Delete(maPN);
                MessageBox.Show("Xóa thành công.");
                Reload();
            }
        }

        // ========== LOAD DATA ==========
        private void Reload()
        {
            _pn.Load();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _pn.Table;
        }

        // ========== BUTTON4: HIỂN THỊ ==========


        // ========== BUTTON5: XEM XML ==========

        // ========== BUTTON6: TÌM KIẾM ==========
        private void button6_Click(object sender, EventArgs e)
        {
            string maTim = textBox3.Text.Trim();

            if (string.IsNullOrEmpty(maTim))
            {
                MessageBox.Show("Vui lòng nhập mã phiếu nhập cần tìm.");
                return;
            }

            DataRow row = _pn.FindByMa(maTim);

            if (row == null)
            {
                MessageBox.Show("Không tìm thấy phiếu nhập.");
                return;
            }

            // Đổ dữ liệu lên các control
            textBox1.Text = row["MaPhieuNhap"].ToString();
            DateTime ngay;
            if (DateTime.TryParse(row["NgayNhap"].ToString(), out ngay))
                dateTimePicker1.Value = ngay;

            textBox7.Text = row["MaNhanVien"].ToString();
            textBox6.Text = row["MaNhaCungCap"].ToString();
            textBox4.Text = row["TongTien"].ToString();

            // ====== TÔ ĐẬM DÒNG TRONG DATAGRIDVIEW ======
            dataGridView1.ClearSelection();

            foreach (DataGridViewRow dgRow in dataGridView1.Rows)
            {
                if (dgRow.Cells["MaPhieuNhap"].Value != null &&
                    dgRow.Cells["MaPhieuNhap"].Value.ToString().Trim()
                        .Equals(maTim, StringComparison.OrdinalIgnoreCase))
                {
                    dgRow.Selected = true;
                    dataGridView1.CurrentCell = dgRow.Cells[0]; // focus vào cột đầu
                    dataGridView1.FirstDisplayedScrollingRowIndex = dgRow.Index; // cuộn tới dòng đó
                    break;
                }
            }
        }


        // ========== CLICK ROW ==========
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow r = dataGridView1.Rows[e.RowIndex];

            textBox1.Text = r.Cells["MaPhieuNhap"].Value.ToString();
            dateTimePicker1.Value = DateTime.Parse(r.Cells["NgayNhap"].Value.ToString());
            textBox7.Text = r.Cells["MaNhanVien"].Value.ToString();
            textBox6.Text = r.Cells["MaNhaCungCap"].Value.ToString();
            textBox4.Text = r.Cells["TongTien"].Value.ToString();
        }

        // ========== BUTTON7: MỞ CHI TIẾT PHIẾU NHẬP ==========
        private void button7_Click_1(object sender, EventArgs e)
        {
            QuanLyChiTietPhieuNhap frm = new QuanLyChiTietPhieuNhap();
            frm.ShowDialog();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            string path = _pn.GetXmlPath();
            if (File.Exists(path))
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int tongTien;
            string error;

            if (!ValidateInput(out error, out tongTien))
            {
                MessageBox.Show(error);
                return;
            }

            string maPN = textBox1.Text.Trim();

            if (_pn.Exists(maPN))
            {
                MessageBox.Show("Mã phiếu nhập đã tồn tại.");
                return;
            }

            _pn.Add(
                maPN,
                dateTimePicker1.Value.ToString("yyyy-MM-dd"),
                textBox7.Text.Trim(),
                textBox6.Text.Trim(),
                tongTien
            );

            MessageBox.Show("Thêm phiếu nhập thành công.");
            Reload();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int tongTien;
            string error;

            if (!ValidateInput(out error, out tongTien))
            {
                MessageBox.Show(error);
                return;
            }

            string maPN = textBox1.Text.Trim();

            if (!_pn.Exists(maPN))
            {
                MessageBox.Show("Không tìm thấy phiếu nhập để sửa.");
                return;
            }

            _pn.Update(
                maPN,
                dateTimePicker1.Value.ToString("yyyy-MM-dd"),
                textBox7.Text.Trim(),
                textBox6.Text.Trim(),
                tongTien
            );

            MessageBox.Show("Sửa phiếu nhập thành công.");
            Reload();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Reload();
        }
    }
}
