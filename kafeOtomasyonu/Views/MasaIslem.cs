using kafeOtomasyonu.Controllers;
using kafeOtomasyonu.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace kafeOtomasyonu.Models
{
    public partial class MasaIslem : Form
    {
        public MasaIslem()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string masaAd = textBox1.Text;
            MasaController masaController = new MasaController();
            masaController.masaAdd(masaAd);
            dataGridRefresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MasaController masaController = new MasaController();
            int masaID = Convert.ToInt32(textBox3.Text);
            masaController.masaDelete(masaID);
            dataGridRefresh();
        }
        private void dataGridRefresh()
        {
            using (SqlConnection conn = Db.GetConnection())
            {
                conn.Close();
                conn.Open();
                string sqlQuery = "SELECT * FROM Masalar";

                SqlDataAdapter da = new SqlDataAdapter(sqlQuery, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void MasaIslem_Load(object sender, EventArgs e)
        {
            dataGridRefresh();
        }
    }
}
