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

namespace kafeOtomasyonu.Views
{
    public partial class MuhasebePaneli : Form
    {

        public MuhasebePaneli()
        {
            InitializeComponent();
        }


        private void MuhasebePaneli_Load(object sender, EventArgs e)
        {
            // VERİLERİ DATA GRİD VİEWE ÇEKEN SORGU
            //SELECT
            //u.UrunAdi,
            //sd.Adet,
            //sd.Fiyat,
            //(sd.Adet * sd.Fiyat) AS ToplamFiyat,
            //s.Tarih
            //FROM SiparisDetay sd
            //INNER JOIN Siparisler s ON sd.SiparisId = s.Id
            //INNER JOIN Urunler u ON sd.UrunId = u.UrunID
            //WHERE s.Tarih >= @BaslangicTarihi AND s.Tarih <= @BitisTarihi



            //buton kodları 
            //private void btnGunluk_Click(object sender, EventArgs e)
            //{
            //    DateTime bugun = DateTime.Today;
            //    RaporuGetir(bugun, bugun.AddDays(1).AddTicks(-1)); // 00:00 - 23:59
            //}

            //private void btnHaftalik_Click(object sender, EventArgs e)
            //{
            //    DateTime baslangic = DateTime.Today.AddDays(-7);
            //    DateTime bitis = DateTime.Now;
            //    RaporuGetir(baslangic, bitis);
            //}

            //private void btnAylik_Click(object sender, EventArgs e)
            //{
            //    DateTime baslangic = DateTime.Today.AddDays(-30);
            //    DateTime bitis = DateTime.Now;
            //    RaporuGetir(baslangic, bitis);
            //}


        }
        //exele data grid view 1 deki veirleri aktar 
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
    }
}
