using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Net.Http;
using kafeOtomasyonu.Database;
using Newtonsoft.Json;
using kafeOtomasyonu.Models;
using System.Windows.Forms;

namespace kafeOtomasyonu.Controllers
{
    public class SerialController
    {
        public async Task SyncSerialTableFromServerAsync()
        {
            List<SerialRecord> serverData = new List<SerialRecord>();

            // 1. SUNUCUDAN VERİ ÇEKİLİYOR
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("http://localhost:5000/api/serials");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    serverData = JsonConvert.DeserializeObject<List<SerialRecord>>(json);
                }
                else
                {
                    MessageBox.Show("Sunucudan veri alınamadı!");
                    return;
                }
            }

            // 2. YEREL VERİLERİ OKU
            List<SerialRecord> localData = new List<SerialRecord>();
            using (SqlConnection conn = Db.GetConnection())
            {
                conn.Close();
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT SerialNo, SerialDate, SerialStatus FROM SerialTable", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    localData.Add(new SerialRecord
                    {
                        SerialNo = reader["SerialNo"].ToString(),
                        SerialDate = Convert.ToDateTime(reader["SerialDate"]),
                        SerialStatus = reader["SerialStatus"].ToString()
                    });
                }
                reader.Close();

                // 3. EŞİTLEME: EKLE / GÜNCELLE
                foreach (var serverItem in serverData)
                {
                    var localMatch = localData.FirstOrDefault(x => x.SerialNo == serverItem.SerialNo);
                    if (localMatch == null)
                    {
                        // EKLE
                        SqlCommand insert = new SqlCommand(
                            "INSERT INTO SerialTable (SerialNo, SerialDate, SerialStatus) VALUES (@no, @date, @status)", conn);
                        insert.Parameters.AddWithValue("@no", serverItem.SerialNo);
                        insert.Parameters.AddWithValue("@date", serverItem.SerialDate);
                        insert.Parameters.AddWithValue("@status", serverItem.SerialStatus);
                        insert.ExecuteNonQuery();
                    }
                    else if (localMatch.SerialDate != serverItem.SerialDate || localMatch.SerialStatus != serverItem.SerialStatus)
                    {
                        // GÜNCELLE
                        SqlCommand update = new SqlCommand(
                            "UPDATE SerialTable SET SerialDate = @date, SerialStatus = @status WHERE SerialNo = @no", conn);
                        update.Parameters.AddWithValue("@no", serverItem.SerialNo);
                        update.Parameters.AddWithValue("@date", serverItem.SerialDate);
                        update.Parameters.AddWithValue("@status", serverItem.SerialStatus);
                        update.ExecuteNonQuery();
                    }
                }

                // 4. SUNUCUDA OLMAYANLARI SİL
                foreach (var localItem in localData)
                {
                    if (!serverData.Any(x => x.SerialNo == localItem.SerialNo))
                    {
                        SqlCommand delete = new SqlCommand(
                            "DELETE FROM SerialTable WHERE SerialNo = @no", conn);
                        delete.Parameters.AddWithValue("@no", localItem.SerialNo);
                        delete.ExecuteNonQuery();
                    }
                }
            }

            MessageBox.Show("Tablo başarıyla güncellendi.");
        }
        public bool CheckSerialKey()
        {
            using (SqlConnection conn = Db.GetConnection())
            {
                conn.Close();
                conn.Open();
                string sqlActiveCdKeyGet = "SELECT SerialNo FROM SerialTable WHERE SerialStatus = 'Aktif'";
                SqlCommand cmd = new SqlCommand(sqlActiveCdKeyGet, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    //burada kaldın
                }
            }
        }

    }
}
