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
using kafeOtomasyonu.Models;
using kafeOtomasyonu.Helpers;
using System.Data.SqlClient;

namespace kafeOtomasyonu.Views
{
    public partial class SiparisForm : Form
    {
        private List<SiparisKalemi> sepet = new List<SiparisKalemi>();
        private int masaID;
        public SiparisForm(int masaID)
        {
            InitializeComponent();
            this.masaID = masaID;
        }

        private void SiparisForm_Load(object sender, EventArgs e)
        {
            label1.Text = $"Masa {masaID}";

            List<UrunModel> urnList = SiparisController.UrunleriGetir();
            lstUrunler.DisplayMember = "UrunAdi";
            lstUrunler.ValueMember = "ID";
            lstUrunler.DataSource = urnList;
            listBox2.Items.Add($"URUN ADİ   -   ADET   -   TUTAR");


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lstUrunler.SelectedItem is UrunModel secilenUrun)
            {
                int adet = (int)numericUpDown1.Value;

                if (adet > 0)
                {
                    SiparisKalemi kalem = new SiparisKalemi
                    {
                        UrunID = secilenUrun.ID,
                        UrunAdi = secilenUrun.UrunAdi,
                        Fiyat = secilenUrun.Fiyat,
                        Adet = adet
                    };

                    sepet.Add(kalem);
                    listBox2.Items.Add(kalem);

                    numericUpDown1.Value = 1; // Adeti sıfırla
                    lstUrunler.ClearSelected(); // Ürün seçimini sıfırla
                }
                else
                {
                    MessageBox.Show("Lütfen adet seçin!", "Uyarı");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (sepet.Count == 0)
            {
                MessageBox.Show("Sepet boş, sipariş kaydedilemez.", "Uyarı");
                return;
            }

            decimal toplamTutar = sepet.Sum(k => k.Fiyat * k.Adet);
            int kullaniciId = Session.AktifKullanici?.Id ?? 1; // Kullanıcı boşsa 1 olsun (Admin gibi)

            int yeniSiparisId = 0;

            using (var conn = Db.GetConnection()) // MSSQL bağlantı
            {
                conn.Close();
                conn.Open();
                var transaction = conn.BeginTransaction();

                try
                {
                    // 1. Sipariş kaydı + yeni ID'yi alma
                    string siparisQuery = @"
                INSERT INTO Siparisler (MasaId, KullaniciId, Tarih, ToplamTutar, Durum)
                OUTPUT INSERTED.Id
                VALUES (@MasaId, @KullaniciId, @Tarih, @ToplamTutar, @Durum)";

                    using (var cmd = new SqlCommand(siparisQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@MasaId", masaID);
                        cmd.Parameters.AddWithValue("@KullaniciId", kullaniciId);
                        cmd.Parameters.AddWithValue("@Tarih", DateTime.Now);
                        cmd.Parameters.AddWithValue("@ToplamTutar", toplamTutar);
                        cmd.Parameters.AddWithValue("@Durum", "Ödendi"); // Direkt ödenmiş

                        yeniSiparisId = (int)cmd.ExecuteScalar(); // Yeni sipariş ID
                    }

                    // 2. SiparisDetay kayıtları
                    foreach (var kalem in sepet)
                    {
                        string detayQuery = @"
                    INSERT INTO SiparisDetay (SiparisId, UrunId, Adet, Fiyat)
                    VALUES (@SiparisId, @UrunId, @Adet, @Fiyat);";

                        using (var cmd = new SqlCommand(detayQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@SiparisId", yeniSiparisId);
                            cmd.Parameters.AddWithValue("@UrunId", kalem.UrunID);
                            cmd.Parameters.AddWithValue("@Adet", kalem.Adet);
                            cmd.Parameters.AddWithValue("@Fiyat", kalem.Fiyat);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    // 3. Masa boşaltılıyor
                    string masaUpdateQuery = "UPDATE Masalar SET Durum = 'Boş' WHERE Id = @MasaId";
                    using (var cmd = new SqlCommand(masaUpdateQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@MasaId", masaID);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    MessageBox.Show("Sipariş başarıyla kaydedildi!", "Bilgi");

                    // Sepeti ve listeyi temizle
                    sepet.Clear();
                    listBox2.Items.Clear();
                    listBox2.Items.Add("URUN ADİ   -   ADET   -   TUTAR");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Sipariş kaydedilemedi: " + ex.Message, "Hata");
                }
            }
        }


        private void btnSil_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                listBox2.Items.Remove(listBox2.SelectedItem); 
            }
            else
            {
                MessageBox.Show("Silinecek ürün seçilmedi!", "Uyarı");
            }
        }

    }
}
