using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Columns.Add("colNama", "Nama");
            dataGridView1.Columns.Add("colNilai", "Nilai");
            dataGridView1.Columns.Add("colGrade", "Grade");
            dataGridView1.Columns.Add("colNo", "Nomor");

            dataGridView1.Columns["colNilai"].Width = 60;
            dataGridView1.Columns["colGrade"].Width = 60;

            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nama = textBox1.Text;
            string nilaiText = textBox2.Text;

            if (string.IsNullOrWhiteSpace(nama) || string.IsNullOrWhiteSpace(nilaiText))
            {
                MessageBox.Show("Nama dan nilai harus diisi.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(nilaiText, out int nilai))
            {
                MessageBox.Show("Nilai harus berupa angka.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (nilai < 0 || nilai > 100)
            {
                MessageBox.Show("Nilai harus antara 0 hingga 100.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hitung grade
            string grade;
            if (nilai > 85)
                grade = "A";
            else if (nilai > 70)
                grade = "B";
            else if (nilai > 60)
                grade = "C";
            else if (nilai > 40)
                grade = "D";
            else
                grade = "F";

            int no = dataGridView1.Rows.Count + 1;
            dataGridView1.Rows.Add(no, nama, nilai, grade);

            textBox1.Clear();
            textBox2.Clear();
            textBox1.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Belum ada data.");
                return;
            }

            double total = 0;
            int jumlah = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["colNilai"].Value != null &&
                    double.TryParse(row.Cells["colNilai"].Value.ToString(), out double nilai))
                {
                    total += nilai;
                    jumlah++;
                }
            }

            double rata = total / jumlah;
            MessageBox.Show($"Rata-rata nilai: {rata:F2}");
        }

        private string HitungGrade(double nilai)
        {
            if (nilai > 85)
                return "A";
            else if (nilai > 70)
                return "B";
            else if (nilai > 60)
                return "C";
            else if (nilai > 40)
                return "D";
            else
                return "h";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV File|*.csv";
            saveFileDialog.Title = "Save Data";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                {
                    // Tulis header
                    sw.WriteLine("No,Nama,Nilai,Grade");

                    // Tulis setiap baris
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            string no = row.Cells[0].Value?.ToString();
                            string nama = row.Cells[1].Value?.ToString();
                            string nilai = row.Cells[2].Value?.ToString();
                            string grade = row.Cells[3].Value?.ToString();

                            sw.WriteLine($"{no},{nama},{nilai},{grade}");
                        }
                    }
                }

                MessageBox.Show("Data berhasil disimpan!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV File|*.csv";
            openFileDialog.Title = "Open Data";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                dataGridView1.Rows.Clear();

                using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                {
                    string line;
                    bool skipHeader = true;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (skipHeader)
                        {
                            skipHeader = false;
                            continue;
                        }

                        string[] parts = line.Split(',');
                        if (parts.Length == 4)
                        {
                            dataGridView1.Rows.Add(parts[0], parts[1], parts[2], parts[3]);
                        }
                    }
                }

                MessageBox.Show("Data berhasil dibuka!");
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Belum ada data yang bisa direset.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult hasil = MessageBox.Show(
                "Apakah Anda yakin ingin menghapus semua data?",
                "Konfirmasi Reset",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (hasil == DialogResult.Yes)
            {
                dataGridView1.Rows.Clear();
                MessageBox.Show("Data berhasil dihapus.", "Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult hasil = MessageBox.Show(
           "Yakin ingin keluar dari aplikasi?",
           "Konfirmasi Keluar",
           MessageBoxButtons.YesNo,
           MessageBoxIcon.Question);

            if (hasil == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Belum ada data yang bisa ditampilkan.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Ambil semua data ke list
            var dataList = new List<(string nama, int nilai)>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[1].Value != null && row.Cells[2].Value != null)
                {
                    string nama = row.Cells[1].Value.ToString();
                    int nilai = Convert.ToInt32(row.Cells[2].Value);
                    dataList.Add((nama, nilai));
                }
            }

            // Urutkan dari nilai tertinggi
            var top10 = dataList.OrderByDescending(d => d.nilai).Take(10).ToList();

            // Buat isi pesan
            string hasil = "Top 10 Ranking:\n";
            for (int i = 0; i < top10.Count; i++)
            {
                hasil += $"{i + 1}. {top10[i].nama} - {top10[i].nilai}\n";
            }

            MessageBox.Show(hasil, "10 Besar Nilai Tertinggi");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count == 0)
    {
                MessageBox.Show("Belum ada data siswa.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Ambil KKM dari TextBox
            if (!int.TryParse(textBox3.Text, out int nilaiKKM) || nilaiKKM < 0 || nilaiKKM > 100)
            {
                MessageBox.Show("Nilai KKM tidak valid. Harus angka 0 - 100.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<string> daftarRemidi = new List<string>();
            int nomor = 1;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[1].Value != null && row.Cells[2].Value != null)
                {
                    string nama = row.Cells[1].Value.ToString();
                    int nilai = Convert.ToInt32(row.Cells[2].Value);

                    if (nilai < nilaiKKM)
                    {
                        daftarRemidi.Add($"{nomor++}. {nama} - {nilai}");
                    }
                }
            }

            if (daftarRemidi.Count == 0)
            {
                MessageBox.Show("Semua siswa memenuhi KKM. Tidak ada yang perlu remidi.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string hasil = "Daftar Siswa yang Harus Remidi:\n" + string.Join("\n", daftarRemidi);
                MessageBox.Show(hasil, "Remidi");
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
