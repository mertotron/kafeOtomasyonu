using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using kafeOtomasyonu.Controllers;
using kafeOtomasyonu.Views;
using Newtonsoft.Json;
using System.Net.Http;
using kafeOtomasyonu.Models;
using kafeOtomasyonu.Helpers;

namespace kafeOtomasyonu.Views
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string kullaniciAdi = textBox1.Text;
                string sifre = textBox2.Text;
                if (LoginController.GirisYap(kullaniciAdi, sifre))
                {
                    //burayı yeniden yap 2. iç fonksiyon serial kontrolü yapıp bunu sunucu üzerinen kontrol edecek

                    UserModel aktifKullanici = Session.AktifKullanici;
                    int userId = aktifKullanici.Id;
                    string userName = aktifKullanici.KullaniciAdi;
                    string userPassword = aktifKullanici.Sifre;
                    MessageBox.Show(userName + " - " + userId + " - " + userPassword);
                    SerialController serialController = new SerialController();
                    if (await serialController.CheckSerialKey(userName, userPassword, userId) == true)
                    {
                        // Giriş başarılı
                        MessageBox.Show("Giriş başarılı!");
                        this.Hide();
                        MainForm mainForm = new MainForm();
                        mainForm.Show();
                        UserModel userModel = new UserModel();
                    }
                    else
                    {
                        MessageBox.Show("Seri anahtarınız veya kullanıcı bilgilerinizle ilgili bir sorun var");
                    }


                }
                else
                {
                    // Giriş başarısız
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı.");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private async void LoginForm_Load(object sender, EventArgs e)
        {
            SerialController serialController = new SerialController();
            

            // arka planuı kaldır
            button1.BackColor = Color.Transparent;
            button2.BackColor = Color.Transparent;
            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bu Modül Henüz Aktif Değil");
        }
    }
}
