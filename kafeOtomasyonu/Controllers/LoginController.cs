using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using kafeOtomasyonu.Models;
using kafeOtomasyonu.Database;
using kafeOtomasyonu.Helpers;

namespace kafeOtomasyonu.Controllers
{
    internal class LoginController
    {
        public static bool GirisYap(string kullaniciAdi, string sifre)
        {
            using (SqlConnection conn = Database.Db.GetConnection())
            {
                string query = "SELECT * FROM Kullanicilar WHERE KullaniciAdi = @kullaniciAdi AND Sifre = @sifre";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                cmd.Parameters.AddWithValue("@sifre", sifre);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read()) 
                {
                    UserModel user = new UserModel
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        KullaniciAdi = reader["KullaniciAdi"].ToString(),
                        Sifre = reader["Sifre"].ToString(),
                        Yetki = reader["Yetki"].ToString()
                    };
                    Session.AktifKullanici = user;
                    return true; // Giriş başarılı
                }
            }
            return false; // Giriş başarısız
        }
    }
}
