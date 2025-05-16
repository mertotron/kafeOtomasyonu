using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using kafeOtomasyonu.Database;

namespace kafeOtomasyonu.Controllers
{
    internal class UrunController
    {
        public void urunAdd(string urunAdi, decimal urunFiyat)
        {
            using (SqlConnection conn = Db.GetConnection())
            {
                conn.Close();
                conn.Open();
                string sqlQuey = "INSERT INTO Urunler (UrunAdi, Fiyat) VALUES (@UrunAd, @urunFiyat)";
                using (SqlCommand cmd = new SqlCommand(sqlQuey,conn))
                {
                    cmd.Parameters.AddWithValue("@UrunAd",urunAdi);
                    cmd.Parameters.AddWithValue("@urunFiyat", urunFiyat);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void urunDelete(int urunID)
        {
            using (SqlConnection conn = Db.GetConnection())
            {
                conn.Close();
                conn.Open();
                string sqlQuey = "DELETE FROM Urunler WHERE UrunID = @urunID";
                using (SqlCommand cmd = new SqlCommand(sqlQuey, conn))
                {
                    cmd.Parameters.AddWithValue("@urunID", urunID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void urunUpdate(int urunID, decimal urunFiyat)
        {
            using (SqlConnection conn = Db.GetConnection())
            {
                conn.Close();
                conn.Open();
                string sqlQuey = "UPDATE Urunler SET Fiyat = @urunFiyat WHERE UrunID = @urunID";
                using (SqlCommand cmd = new SqlCommand(sqlQuey, conn))
                {
                    cmd.Parameters.AddWithValue("@urunFiyat", urunFiyat);
                    cmd.Parameters.AddWithValue("@urunID", urunID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
