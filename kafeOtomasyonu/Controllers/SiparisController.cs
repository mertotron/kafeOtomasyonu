using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using kafeOtomasyonu.Models;
using kafeOtomasyonu.Database;


namespace kafeOtomasyonu.Controllers
{
    internal class SiparisController
    {
        public static List<UrunModel> UrunleriGetir()
        {
            List<UrunModel> urunler = new List<UrunModel>();

            using (SqlConnection conn = Db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT UrunID, UrunAdi, Fiyat FROM Urunler", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    UrunModel urun = new UrunModel
                    {
                        ID = reader.GetInt32(0),
                        UrunAdi = reader.GetString(1),
                        Fiyat = reader.GetDecimal(2)
                    };
                    urunler.Add(urun);
                }
            }

            return urunler;
        }
    }
}
