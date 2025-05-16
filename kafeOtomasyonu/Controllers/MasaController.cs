using kafeOtomasyonu.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace kafeOtomasyonu.Controllers
{
    internal class MasaController
    {
        public void masaAdd(string masaAd)
        {
            using (SqlConnection conn = Db.GetConnection())
            {
                conn.Close();
                conn.Open();
                string sqlQuey = "INSERT INTO Masalar (MasaAdi, Durum) VALUES (@masaAd, 'Boş')";
                using (SqlCommand cmd = new SqlCommand(sqlQuey, conn))
                {
                    cmd.Parameters.AddWithValue("@masaAd", masaAd);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void masaDelete(int masaID)
        {
            using (SqlConnection conn = Db.GetConnection())
            {
                conn.Close();
                conn.Open();
                string sqlQuey = "DELETE FROM Masalar WHERE Id = @masaId";
                using (SqlCommand cmd = new SqlCommand(sqlQuey, conn))
                {
                    cmd.Parameters.AddWithValue("@masaId", masaID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
