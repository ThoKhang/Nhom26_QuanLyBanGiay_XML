using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using QuanLyBanGiay.CLASS;

namespace QuanLyBanGiay.GUI
{
    public partial class QuanLyTaiKhoanNhanVien : Form
    {
        private TaiKhoan _tk;

        public QuanLyTaiKhoanNhanVien()
        {
            InitializeComponent();

            _tk = new TaiKhoan();

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = _tk.Table;

            // Quyền mẫu
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Admin");
            comboBox1.Items.Add("NhanVien");
            comboBox1.Items.Add("KeToan");
            comboBox1.Items.Add("ThuKho");
            comboBox1.Items.Add("BanHang");
            comboBox1.SelectedIndex = 1; // NhanVien
        }

        // ==== HÀM KIỂM TRA NHẬP LIỆU ====
        private bool ValidateInput(out string message)
        {
            message = "";

            string maNV = textBox1.Text.Trim();
            string tenDN = textBox3.Text.Trim();
            string matKhau = textBox2.Text.Trim();
            string quyen = comboBox1.SelectedItem?.ToString() ?? "";

            if (string.IsNullOrEmpty(maNV))
            {
                message = "Mã nhân viên không được để trống.";
                return false;
            }
            if (string.IsNullOrEmpty(tenDN))
            {
                message = "Tên đăng nhập không được để trống.";
                return false;
            }
            if (string.IsNullOrEmpty(matKhau))
            {
                message = "Mật khẩu không được để trống.";
                return false;
            }
            if (string.IsNullOrEmpty(quyen))
            {
                message = "Vui lòng chọn quyền.";
                return false;
            }
            if (matKhau.Length < 3)
            {
                message = "Mật khẩu phải có ít nhất 3 ký tự.";
                return false;
            }

            return true;
        }

        // ==== BUTTON1: THÊM ====
        private void button1_Click(object sender, EventArgs e)
        {
            string error;
            if (!ValidateInput(out error))
            {
                MessageBox.Show(error, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maNV = textBox1.Text.Trim();
            string tenDN = textBox3.Text.Trim();

            if (_tk.ExistsByMaNV(maNV))
            {
                MessageBox.Show("Mã nhân viên này đã có tài khoản.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_tk.ExistsByTenDangNhap(tenDN))
            {
                MessageBox.Show("Tên đăng nhập này đã được sử dụng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _tk.AddTaiKhoan(
                maNV,
                tenDN,
                textBox2.Text.Trim(),
                comboBox1.SelectedItem.ToString()
            );

            MessageBox.Show("Thêm tài khoản thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _tk.Table;
        }

        // ==== BUTTON2: XÓA ====

        // ==== CLICK DÒNG TRONG DATAGRIDVIEW → ĐỔ LÊN TEXTBOX/COMBOBOX ====
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            textBox1.Text = row.Cells["MaNhanVien"].Value?.ToString();
            textBox3.Text = row.Cells["TenDangNhap"].Value?.ToString();
            textBox2.Text = row.Cells["MatKhau"].Value?.ToString();

            string quyen = row.Cells["Quyen"].Value?.ToString();
            if (comboBox1.Items.Contains(quyen))
                comboBox1.SelectedItem = quyen;
        }

        // ==== BUTTON3: LÀM MỚI (CLEAR + RELOAD) ====
        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;

            _tk.Load();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _tk.Table;
        }

        // ==== BUTTON4: THOÁT FORM ====
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string maNV = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(maNV))
            {
                MessageBox.Show("Vui lòng chọn tài khoản hoặc nhập mã nhân viên để xóa.",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_tk.ExistsByMaNV(maNV))
            {
                MessageBox.Show("Không tìm thấy tài khoản của mã nhân viên này.",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa tài khoản của nhân viên " + maNV + " ?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _tk.DeleteTaiKhoanByMaNV(maNV);
                MessageBox.Show("Xóa tài khoản thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = _tk.Table;
            }
        }

        private void QuanLyTaiKhoanNhanVien_Load(object sender, EventArgs e)
        {

        }
    }
}
