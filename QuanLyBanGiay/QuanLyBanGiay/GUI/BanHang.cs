using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using QuanLyBanGiay.CLASS;

namespace QuanLyBanGiay.GUI
{
    public partial class BanHang : Form
    {
        private BanHangXml _banHang;
        private PhieuMuaXml _phieuMua;

        public BanHang()
        {
            InitializeComponent();

            _banHang = new BanHangXml();
            _phieuMua = new PhieuMuaXml();

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = _banHang.Table;
        }

        // BUTTON6: TÌM KIẾM GIÀY THEO MÃ
        private void button6_Click(object sender, EventArgs e)
        {
            string maGiay = textBox3.Text.Trim();

            if (string.IsNullOrEmpty(maGiay))
            {
                MessageBox.Show("Vui lòng nhập mã giày cần tìm.",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRow row = _banHang.FindByMaGiay(maGiay);

            if (row == null)
            {
                MessageBox.Show("Không tìm thấy giày với mã: " + maGiay,
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Đổ dữ liệu lên các textbox
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

        // BUTTON1: MUA NGAY
        private void button1_Click(object sender, EventArgs e)
        {
            string maGiay = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(maGiay))
            {
                MessageBox.Show("Vui lòng tìm giày trước khi mua (mã giày trống).",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy số lượng tồn
            if (!int.TryParse(textBox5.Text.Trim(), out int soLuongTon))
            {
                MessageBox.Show("Số lượng tồn không hợp lệ.",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // textBox11: SỐ LƯỢNG MUA
            if (!int.TryParse(textBox11.Text.Trim(), out int soLuongMua) || soLuongMua <= 0)
            {
                MessageBox.Show("Vui lòng nhập số lượng mua hợp lệ (> 0).",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (soLuongMua > soLuongTon)
            {
                MessageBox.Show("Số lượng mua vượt quá số lượng tồn.",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // XÁC NHẬN THANH TOÁN
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn mua {soLuongMua} đôi giày mã {maGiay} không?",
                "Xác nhận mua hàng",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.No)
            {
                return;
            }

            // TRỪ SỐ LƯỢNG TỒN
            int soLuongConLai = soLuongTon - soLuongMua;

            bool ok = _banHang.UpdateSoLuong(maGiay, soLuongConLai);

            if (!ok)
            {
                MessageBox.Show("Không cập nhật được số lượng trong file Giay.xml.",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // GHI PHIEUMUA.XML
            if (!int.TryParse(textBox10.Text.Trim(), out int donGiaBan))
            {
                MessageBox.Show("Đơn giá bán không hợp lệ, nhưng số lượng tồn đã được cập nhật.",
                                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                int thanhTien = soLuongMua * donGiaBan;
                _phieuMua.ThemPhieuMua(maGiay, soLuongMua, donGiaBan, thanhTien);
            }

            // Cập nhật lại textbox số lượng tồn
            textBox5.Text = soLuongConLai.ToString();

            // Refresh lại DataGridView
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _banHang.Table;

            MessageBox.Show("Mua hàng thành công!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // BUTTON5: XEM FILE PHIEUMUA.XML
        private void button5_Click(object sender, EventArgs e)
        {
            string path = _phieuMua.GetPath();
            try
            {
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không mở được file PhieuMua.xml: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // CLICK LÊN DÒNG TRONG DATAGRIDVIEW → ĐỔ LÊN TEXTBOX
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // click header thì bỏ qua

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            textBox1.Text = row.Cells["MaGiay"].Value?.ToString();
            textBox2.Text = row.Cells["TenGiay"].Value?.ToString();
            textBox7.Text = row.Cells["Loai"].Value?.ToString();
            textBox6.Text = row.Cells["Size"].Value?.ToString();
            textBox4.Text = row.Cells["Mau"].Value?.ToString();
            textBox5.Text = row.Cells["SoLuongTon"].Value?.ToString();
            textBox8.Text = row.Cells["DonGiaNhap"].Value?.ToString();
            textBox10.Text = row.Cells["DonGiaBan"].Value?.ToString();
            textBox9.Text = row.Cells["MaNhaCungCap"].Value?.ToString();
        }
    }
}
