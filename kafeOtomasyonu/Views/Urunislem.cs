using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using kafeOtomasyonu.Database;
using kafeOtomasyonu.Controllers;
using System.Data.SqlClient;

namespace kafeOtomasyonu.Views
{
    public partial class Urunislem : Form
    {
        UrunController urunController = new UrunController();
        public Urunislem()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string isim = textBox1.Text;
            decimal fiyat = Convert.ToDecimal(textBox2.Text);
            urunController.urunAdd(isim,fiyat);
            dataGridRefresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int urunID = Convert.ToInt32(textBox3.Text);
            urunController.urunDelete(urunID);
            dataGridRefresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int urunID = Convert.ToInt32(textBox4.Text);
            decimal fiyat = Convert.ToDecimal(textBox5.Text);
            urunController.urunUpdate(urunID,fiyat);
            dataGridRefresh();
        }
        private void dataGridRefresh()
        {
            using (SqlConnection conn = Db.GetConnection())
            {
                conn.Close();
                conn.Open();
                string sqlQuery = "SELECT * FROM Urunler";

                SqlDataAdapter da = new SqlDataAdapter(sqlQuery, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void Urunislem_Load(object sender, EventArgs e)
        {
            dataGridRefresh();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Başlık satırı değilse
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                textBox3.Text = selectedRow.Cells[0].Value.ToString();
                textBox4.Text = selectedRow.Cells[0].Value.ToString();
            }
        }
    }
}
