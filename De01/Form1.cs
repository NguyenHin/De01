using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS;
using DAL.Enitities;

namespace De01
{
    public partial class Form1 : Form
    {
        
        private readonly SinhVienService sinhVienService = new SinhVienService();
        private readonly LopService lopService = new LopService();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                var listLops = lopService.GetAll();
                var listSV = sinhVienService.GetAll();
                FillLopCombobox(listLops);
                BindGrid(listSV);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        private void FillLopCombobox(List<Lop> listLops)
        {
            listLops.Insert(0, new Lop ()); 
            comboBox1.DataSource = listLops;
            comboBox1.DisplayMember = "TenLop";
            comboBox1.ValueMember = "MaLop";
        }
        private void BindGrid(List<SinhVien> listSV)
        {

            dataGridView1.Rows.Clear();
            foreach (var item in listSV)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = item.MaSV;
                dataGridView1.Rows[index].Cells[1].Value = item.HoTenSV;
                dataGridView1.Rows[index].Cells[2].Value = item.NgaySinh;
                dataGridView1.Rows[index].Cells[3].Value = item.Lop.TenLop;
            }

        }
        private void ResetForm()
        {
            txtMaSV.Clear();
            txtHoTen.Clear();
            dateTimePicker1.Value = DateTime.Now;
            comboBox1.SelectedIndex = 0;
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu hợp lệ
                if (string.IsNullOrWhiteSpace(txtMaSV.Text) || string.IsNullOrWhiteSpace(txtHoTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            

            // Tạo đối tượng SinhVien mới
            SinhVien sinhVien = new SinhVien
            {
                MaSV = txtMaSV.Text.Trim(),
                HoTenSV = txtHoTen.Text.Trim(),
                NgaySinh = dateTimePicker1.Value,
                MaLop = comboBox1.SelectedValue.ToString() // Lấy mã lớp từ combobox
            };
                // Gọi service để thêm sinh viên vào DB
                sinhVienService.Insert(sinhVien);

                // Refresh dữ liệu trên Grid
                var listSV = sinhVienService.GetAll();
                BindGrid(listSV);

                // Thông báo thành công
                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Xóa dữ liệu trên các control để chuẩn bị cho lần nhập tiếp theo
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra thông tin mã số sinh viên
            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Vui lòng nhập mã số sinh viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy mã sinh viên từ txtMaSV
            string maSV = txtMaSV.Text.Trim();

            // Hiện cảnh báo xác nhận
            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này không?",
                                                 "Xác nhận xóa!",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Warning);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    // Gọi service để xóa sinh viên
                    sinhVienService.Delete(maSV); 

                    // Tải lại dữ liệu
                    var listSV = sinhVienService.GetAll();
                    BindGrid(listSV);
                    ResetForm(); 

                    MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa dữ liệu: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (string.IsNullOrWhiteSpace(txtMaSV.Text) || string.IsNullOrWhiteSpace(txtHoTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maSV = txtMaSV.Text.Trim();

                SinhVien updatedSinhVien = new SinhVien
                {
                    MaSV = maSV,
                    HoTenSV = txtHoTen.Text.Trim(),
                    NgaySinh = dateTimePicker1.Value,
                    MaLop = comboBox1.SelectedValue.ToString() 
                };

                sinhVienService.Insert(updatedSinhVien); 

                var listSV = sinhVienService.GetAll();
                BindGrid(listSV);

                MessageBox.Show("Cập nhật sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            // Kiểm tra thông tin đầu vào
            if (string.IsNullOrWhiteSpace(txtTim.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sinh viên cần tìm!");
                return;
            }

            // Lấy tên sinh viên từ TextBox
            string tenSinhVien = txtTim.Text;

            // Tìm kiếm sinh viên theo tên
            var listSV = sinhVienService.FindByName(tenSinhVien);
            if (listSV.Count == 0)
            {
                MessageBox.Show("Không tìm thấy sinh viên nào với tên đã nhập.");
            }
            else
            {
                // Hiển thị danh sách sinh viên tìm thấy lên DataGridView
                BindGrid(listSV);
            }


        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            // Hiện cảnh báo xác nhận trước khi thoát
            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn thoát không?",
                                                 "Xác nhận thoát",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);
            // Nếu người dùng chọn "Yes", thì thoát
            if (confirmResult == DialogResult.Yes)
            {
                this.Close(); // Đóng form
            }
        }
    }
}
