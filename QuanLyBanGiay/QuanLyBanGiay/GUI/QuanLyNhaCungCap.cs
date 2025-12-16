using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using QuanLyBanGiay.CLASS;

namespace QuanLyBanGiay.GUI
{
    public partial class QuanLyNhaCungCap : Form
    {
        private NhaCungCapXml _ncc;

        public QuanLyNhaCungCap()
        {
            InitializeComponent();
            _ncc = new NhaCungCapXml();

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = _ncc.Table;
        }

        // Validate ràng buộc nhập liệu
        private bool ValidateInput(out string msg)
        {
            msg = "";

            if (string.IsNullOrWhiteSpace(textBox1.Text))
            { msg = "Mã nhà cung cấp không được để trống."; return false; }

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            { msg = "Tên nhà cung cấp không được để trống."; return false; }

            if (string.IsNullOrWhiteSpace(textBox4.Text))
            { msg = "Địa chỉ không được để trống."; return false; }

            if (string.IsNullOrWhiteSpace(textBox5.Text))
            { msg = "Điện thoại không được để trống."; return false; }

            if (!textBox5.Text.All(char.IsDigit))
            { msg = "Điện thoại chỉ được chứa số."; return false; }

            return true;
        }

        private void LoadRow(DataRow row)
        {
            textBox1.Text = row["MaNhaCungCap"].ToString();
            textBox2.Text = row["TenNhaCungCap"].ToString();
            textBox4.Text = row["DiaChi"].ToString();
            textBox5.Text = row["DienThoai"].ToString();
        }

        // BUTTON1: Thêm
        private void button1_Click(object sender, EventArgs e)
        {
            string msg;
            if (!ValidateInput(out msg))
            {
                MessageBox.Show(msg);
                return;
            }

            string ma = textBox1.Text.Trim();

            if (_ncc.Exists(ma))
            {
                MessageBox.Show("Mã nhà cung cấp đã tồn tại!");
                return;
            }

            _ncc.Add(ma,
                     textBox2.Text.Trim(),
                     textBox4.Text.Trim(),
                     textBox5.Text.Trim());

            MessageBox.Show("Thêm thành công!");
            dataGridView1.DataSource = _ncc.Table;
        }

        // BUTTON2: Sửa
        private void button2_Click(object sender, EventArgs e)
        {
            string msg;
            if (!ValidateInput(out msg))
            {
                MessageBox.Show(msg);
                return;
            }

            string ma = textBox1.Text.Trim();

            if (!_ncc.Exists(ma))
            {
                MessageBox.Show("Không tìm thấy mã nhà cung cấp để sửa!");
                return;
            }

            _ncc.Update(ma,
                        textBox2.Text.Trim(),
                        textBox4.Text.Trim(),
                        textBox5.Text.Trim());

            MessageBox.Show("Sửa thành công!");
            dataGridView1.DataSource = _ncc.Table;
        }

        // BUTTON3: Xóa
        private void button3_Click(object sender, EventArgs e)
        {
            string ma = textBox1.Text.Trim();

            if (!_ncc.Exists(ma))
            {
                MessageBox.Show("Không tìm thấy mã nhà cung cấp để xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _ncc.Delete(ma);
                MessageBox.Show("Xóa thành công!");
                dataGridView1.DataSource = _ncc.Table;
            }
        }

        // BUTTON4: Hiển thị lại
        private void button4_Click(object sender, EventArgs e)
        {
            _ncc.Load();
            dataGridView1.DataSource = _ncc.Table;
        }

        // BUTTON5: Mở file xml
        private void button5_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo(_ncc.GetPath()) { UseShellExecute = true });
        }

        // BUTTON6: Tìm kiếm
        private void button6_Click(object sender, EventArgs e)
        {
            string ma = textBox3.Text.Trim();
            DataRow row = _ncc.Find(ma);

            if (row == null)
            {
                MessageBox.Show("Không tìm thấy nhà cung cấp!");
                return;
            }

            LoadRow(row);

            // Tô sáng dòng
            foreach (DataGridViewRow dg in dataGridView1.Rows)
            {
                if (dg.Cells["MaNhaCungCap"].Value != null &&
                    dg.Cells["MaNhaCungCap"].Value.ToString() == ma)
                {
                    dg.Selected = true;
                    dataGridView1.CurrentCell = dg.Cells[0];
                    break;
                }
            }
        }

        // Click lên DataGridView → hiện vào textbox
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataRow row = ((DataRowView)dataGridView1.Rows[e.RowIndex].DataBoundItem).Row;
                LoadRow(row);
            }
        }
    }
}
