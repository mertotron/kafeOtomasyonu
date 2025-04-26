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
using System.Data.SqlClient;

namespace kafeOtomasyonu.Views
{
    public partial class MasaForm : Form
    {
        public MasaForm()
        {
            InitializeComponent();
            MasaButonlariOlustur();
        }
        private void MasaButonlariOlustur()
        {
            int x = 20, y = 20;
            int butonGenislik = 150;
            int butonYukseklik = 100;
            int padding = 20;

            // SQL bağlantısı
            using (SqlConnection conn = Database.Db.GetConnection())
            {
                conn.Close();
                conn.Open();
                string query = "SELECT Id, MasaAdi FROM Masalar";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int masaID = reader.GetInt32(0);
                            string masaAdi = reader.GetString(1);

                            Button masaBtn = new Button();
                            masaBtn.Text = masaAdi;
                            masaBtn.Name = $"btnMasa{masaID}";
                            masaBtn.Size = new Size(butonGenislik, butonYukseklik);
                            masaBtn.Location = new Point(x, y);
                            masaBtn.Tag = masaID;
                            masaBtn.Click += MasaButon_Click;

                            // Panel'e ekle (this.Controls değil!)
                            panel1.Controls.Add(masaBtn);

                            x += butonGenislik + padding;

                            // Panelin genişliğini aşarsa alt satıra geç
                            if (x + butonGenislik + padding > panel1.Width)
                            {
                                x = padding;
                                y += butonYukseklik + padding;
                            }
                        }
                    }
                }
            }
        }


        private void MasaButon_Click(object sender, EventArgs e)
        {
            Button tiklananMasa = sender as Button;
            int masaID = (int)tiklananMasa.Tag;

            SiparisForm siparisForm = new SiparisForm(masaID);
            siparisForm.Show(); // Modal olarak aç
        }

        private void MasaForm_Load(object sender, EventArgs e)
        {
            panel1.Dock = DockStyle.Fill;

        }
    }
}
