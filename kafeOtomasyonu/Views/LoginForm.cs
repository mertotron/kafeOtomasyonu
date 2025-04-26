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

namespace kafeOtomasyonu.Views
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = textBox1.Text;
            string sifre = textBox2.Text;
            if (LoginController.GirisYap(kullaniciAdi, sifre))
            {
                // Giriş başarılı
                MessageBox.Show("Giriş başarılı!");
                this.Hide();
                MainForm mainForm = new MainForm();
                mainForm.Show();
            }
            else
            {
                // Giriş başarısız
                MessageBox.Show("Kullanıcı adı veya şifre hatalı.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // arka planuı kaldır
            button1.BackColor = Color.Transparent;
            button2.BackColor = Color.Transparent;
            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;

        }
    }
}
