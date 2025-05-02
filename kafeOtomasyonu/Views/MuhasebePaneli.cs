using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using System.IO;

namespace kafeOtomasyonu.Views
{
    public partial class MuhasebePaneli : Form
    {

        public MuhasebePaneli()
        {
            InitializeComponent();
        }
        public void VeritabaniYedekAl(SqlConnection conn)
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                string databaseName = conn.Database;

                // Tarih formatı: yyyyMMdd_HHmmss
                string tarih = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                // Yedekleme dizini (projeyle aynı dizindeki sabit klasör)
                string yedekKlasoru = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"DatabaseBackup\");

                // Eğer klasör yoksa oluştur
                if (!Directory.Exists(yedekKlasoru))
                    Directory.CreateDirectory(yedekKlasoru);

                // Tam yedek dosya yolu
                string yedekDosyaYolu = Path.Combine(yedekKlasoru, $"veritabani_yedek_{tarih}.bak");

                // SQL backup komutu
                string backupQuery = $@"BACKUP DATABASE [{databaseName}] TO DISK = @yedekYolu WITH INIT, FORMAT";

                using (SqlCommand cmd = new SqlCommand(backupQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@yedekYolu", yedekDosyaYolu);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show($"Veritabanı yedeği alındı:\n{yedekDosyaYolu}", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yedek alma hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }


        private void MuhasebePaneli_Load(object sender, EventArgs e)
        {

        }
        //exele data grid view 1 deki veirleri aktar 
        public void raporGetir(DateTime baslangic, DateTime bitis)
        {
            using (SqlConnection conn = Database.Db.GetConnection())
            {
                string query = @"SELECT 
                            u.UrunAdi,
                            sd.Adet,
                            sd.Fiyat,
                            (sd.Adet * sd.Fiyat) AS ToplamFiyat,
                            s.Tarih
                         FROM SiparisDetay sd
                         INNER JOIN Siparisler s ON sd.SiparisId = s.Id
                         INNER JOIN Urunler u ON sd.UrunId = u.UrunID
                         WHERE s.Tarih BETWEEN @baslangic AND @bitis";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@baslangic", baslangic);
                    cmd.Parameters.AddWithValue("@bitis", bitis);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;
                }
            }
        }
        private void ExportToExcel(DataGridView dataGridView)
        {
            // Excel uygulamasını başlat
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Add();
            Excel.Worksheet worksheet = workbook.ActiveSheet;

            try
            {
                // Sütun başlıklarını yaz
                for (int i = 0; i < dataGridView.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dataGridView.Columns[i].HeaderText;
                }

                // Verileri yaz
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dataGridView.Rows[i].Cells[j].Value?.ToString();
                    }
                }

                // Excel dosyasını kaydet
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                    FileName = "DataGridViewExport.xlsx"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    workbook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Veriler Excel'e başarıyla aktarıldı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Excel uygulamasını kapat
                workbook.Close();
                excelApp.Quit();

                // COM nesnelerini serbest bırak
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DateTime bugun = DateTime.Today;
            raporGetir(bugun, bugun.AddDays(1).AddTicks(-1)); // 00:00 - 23:59
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime baslangic = DateTime.Today.AddDays(-7);
            DateTime bitis = DateTime.Now;
            raporGetir(baslangic, bitis);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime baslangic = DateTime.Today.AddDays(-30);
            DateTime bitis = DateTime.Now;
            raporGetir(baslangic, bitis);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ExportToExcel(dataGridView1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            VeritabaniYedekAl(kafeOtomasyonu.Database.Db.GetConnection());
        }
    }
}
