using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using QuanLyBanGiay.CLASS;

namespace QuanLyBanGiay.GUI
{
    public partial class QuanLyNhanVien : Form
    {
        private NhanVienXml _nv;

        public QuanLyNhanVien()
        {
            InitializeComponent();

            _nv = new NhanVienXml();

            // Đổ dữ liệu vào DataGridView
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = _nv.Table;

            // Đổ dữ liệu trạng thái vào ComboBox
            comboBox1.Items.Clear();
            comboBox1.Items.Add("DangLam");
            comboBox1.Items.Add("NghiPhep");
            comboBox1.Items.Add("NghiViec");
            comboBox1.SelectedIndex = 0;
        }

        // ====== HÀM HỖ TRỢ VALIDATE ======
        private bool ValidateInput(out string message)
        {
            message = "";

            string maNV = textBox1.Text.Trim();
            string hoTen = textBox2.Text.Trim();
            DateTime ngaySinh = dateTimePicker1.Value;
            string gioiTinh = textBox6.Text.Trim();
            string diaChi = textBox4.Text.Trim();
            string dienThoai = textBox5.Text.Trim();
            string trangThai = comboBox1.SelectedItem?.ToString() ?? "";

            if (string.IsNullOrEmpty(maNV))
            {
                message = "Mã nhân viên không được để trống.";
                return false;
            }
            if (string.IsNullOrEmpty(hoTen))
            {
                message = "Họ tên không được để trống.";
                return false;
            }
            if (string.IsNullOrEmpty(gioiTinh))
            {
                message = "Giới tính không được để trống.";
                return false;
            }
            if (string.IsNullOrEmpty(diaChi))
            {
                message = "Địa chỉ không được để trống.";
                return false;
            }
            if (string.IsNullOrEmpty(dienThoai))
            {
                message = "Điện thoại không được để trống.";
                return false;
            }
            if (!dienThoai.All(char.IsDigit))
            {
                message = "Điện thoại chỉ được chứa số.";
                return false;
            }
            if (string.IsNullOrEmpty(trangThai))
            {
                message = "Vui lòng chọn trạng thái.";
                return false;
            }

            // Ví dụ: nhân viên phải >= 18 tuổi
            if (ngaySinh > DateTime.Now.AddYears(-18))
            {
                message = "Nhân viên phải đủ 18 tuổi.";
                return false;
            }

            return true;
        }

        private void LoadRowToControls(DataRow row)
        {
            if (row == null) return;

            textBox1.Text = row["MaNhanVien"].ToString();
            textBox2.Text = row["HoTen"].ToString();

            DateTime ngaySinh;
            if (DateTime.TryParse(row["NgaySinh"].ToString(), out ngaySinh))
                dateTimePicker1.Value = ngaySinh;
            else
                dateTimePicker1.Value = DateTime.Now;

            textBox6.Text = row["GioiTinh"].ToString();
            textBox4.Text = row["DiaChi"].ToString();
            textBox5.Text = row["DienThoai"].ToString();

            string trangThai = row["TrangThai"].ToString();
            if (comboBox1.Items.Contains(trangThai))
                comboBox1.SelectedItem = trangThai;
        }

        // ====== BUTTON1: THÊM ======
        private void button1_Click(object sender, EventArgs e)
        {
            string error;
            if (!ValidateInput(out error))
            {
                MessageBox.Show(error, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maNV = textBox1.Text.Trim();

            if (_nv.Exists(maNV))
            {
                MessageBox.Show("Mã nhân viên đã tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _nv.AddNhanVien(
                maNV,
                textBox2.Text.Trim(),
                dateTimePicker1.Value,
                textBox6.Text.Trim(),
                textBox4.Text.Trim(),
                textBox5.Text.Trim(),
                comboBox1.SelectedItem.ToString()
            );

            MessageBox.Show("Thêm nhân viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _nv.Table;
        }

        // ====== BUTTON2: SỬA ======
        private void button2_Click(object sender, EventArgs e)
        {
            string error;
            if (!ValidateInput(out error))
            {
                MessageBox.Show(error, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maNV = textBox1.Text.Trim();

            if (!_nv.Exists(maNV))
            {
                MessageBox.Show("Không tìm thấy mã nhân viên để sửa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _nv.UpdateNhanVien(
                maNV,
                textBox2.Text.Trim(),
                dateTimePicker1.Value,
                textBox6.Text.Trim(),
                textBox4.Text.Trim(),
                textBox5.Text.Trim(),
                comboBox1.SelectedItem.ToString()
            );

            MessageBox.Show("Sửa thông tin nhân viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _nv.Table;
        }

        // ====== BUTTON3: XÓA ======
        private void button3_Click(object sender, EventArgs e)
        {
            string maNV = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(maNV))
            {
                MessageBox.Show("Vui lòng chọn nhân viên hoặc nhập mã nhân viên để xóa.",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_nv.Exists(maNV))
            {
                MessageBox.Show("Không tìm thấy mã nhân viên cần xóa.",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa nhân viên " + maNV + " ?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _nv.DeleteNhanVien(maNV);
                MessageBox.Show("Xóa nhân viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = _nv.Table;
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Nếu click vào header thì bỏ qua
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            // Ở đây mình dùng theo thứ tự cột trong DataTable:
            // 0: MaNhanVien, 1: HoTen, 2: NgaySinh, 3: GioiTinh, 4: DiaChi, 5: DienThoai, 6: TrangThai

            textBox1.Text = row.Cells[0].Value?.ToString(); // MaNhanVien
            textBox2.Text = row.Cells[1].Value?.ToString(); // HoTen

            // NgaySinh
            DateTime ns;
            if (DateTime.TryParse(row.Cells[2].Value?.ToString(), out ns))
                dateTimePicker1.Value = ns;
            else
                dateTimePicker1.Value = DateTime.Now;

            textBox6.Text = row.Cells[3].Value?.ToString(); // GioiTinh
            textBox4.Text = row.Cells[4].Value?.ToString(); // DiaChi
            textBox5.Text = row.Cells[5].Value?.ToString(); // DienThoai

            string trangThai = row.Cells[6].Value?.ToString(); // TrangThai
            if (!string.IsNullOrEmpty(trangThai) && comboBox1.Items.Contains(trangThai))
            {
                comboBox1.SelectedItem = trangThai;
            }
        }


        // ====== BUTTON4: HIỆN LẠI / NẠP LẠI DỮ LIỆU ======
        private void button4_Click(object sender, EventArgs e)
        {
            _nv.Load();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _nv.Table;
        }

        // ====== BUTTON5: MỞ FILE XML ======
        private void button5_Click(object sender, EventArgs e)
        {
            string path = _nv.GetXmlPath();

            if (!File.Exists(path))
            {
                MessageBox.Show("Không tìm thấy file NhanVien.xml", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Mở file bằng chương trình mặc định (Notepad, VS Code, ...)
            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
        }

        // ====== BUTTON6: TÌM KIẾM THEO MÃ NHÂN VIÊN ======
        private void button6_Click(object sender, EventArgs e)
        {
            string maTim = textBox3.Text.Trim();

            if (string.IsNullOrEmpty(maTim))
            {
                MessageBox.Show("Vui lòng nhập mã nhân viên cần tìm.",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRow row = _nv.FindByMa(maTim);

            if (row == null)
            {
                MessageBox.Show("Không tìm thấy nhân viên với mã: " + maTim,
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            LoadRowToControls(row);

            // Tô đậm dòng trên DataGridView
            foreach (DataGridViewRow dgRow in dataGridView1.Rows)
            {
                if (dgRow.Cells["MaNhanVien"].Value != null &&
                    dgRow.Cells["MaNhanVien"].Value.ToString().Trim().Equals(maTim, StringComparison.OrdinalIgnoreCase))
                {
                    dgRow.Selected = true;
                    dataGridView1.CurrentCell = dgRow.Cells[0];
                    break;
                }
            }
        }

        // ====== CLICK LÊN DÒNG TRONG DATAGRIDVIEW → ĐỔ LÊN TEXTBOX ======
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // click vào header thì bỏ

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            textBox1.Text = row.Cells["MaNhanVien"].Value?.ToString();
            textBox2.Text = row.Cells["HoTen"].Value?.ToString();

            DateTime ns;
            if (DateTime.TryParse(row.Cells["NgaySinh"].Value?.ToString(), out ns))
                dateTimePicker1.Value = ns;
            else
                dateTimePicker1.Value = DateTime.Now;

            textBox6.Text = row.Cells["GioiTinh"].Value?.ToString();
            textBox4.Text = row.Cells["DiaChi"].Value?.ToString();
            textBox5.Text = row.Cells["DienThoai"].Value?.ToString();

            string trangThai = row.Cells["TrangThai"].Value?.ToString();
            if (!string.IsNullOrEmpty(trangThai) && comboBox1.Items.Contains(trangThai))
            {
                comboBox1.SelectedItem = trangThai;
            }
        }

        private void QuanLyNhanVien_Load(object sender, EventArgs e)
        {

        }
    }
}
