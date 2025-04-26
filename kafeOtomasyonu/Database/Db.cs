using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kafeOtomasyonu.Database
{
    public static class Db
    {
        // Bağlantı cümlesi — kendi veritabanı adını ve SQL Server yolunu yaz!
        private static string connectionString = "Data Source=LAPTOP-AASUIVLU\\SQLEXPRESS;Initial Catalog=kafeOtomasyon;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        public static SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}
