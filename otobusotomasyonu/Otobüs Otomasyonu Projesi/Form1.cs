using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Otobüs_Otomasyonu_Projesi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Oturum Açma Butonu
        private async void button1_Click(object sender, EventArgs e)
        {
            oturumekrani();
        }
        async void oturumekrani()
        {
            using (SqlConnection bag = new SqlConnection("Server=DESKTOP-OL9B8A7;Database=otobusotomasyon;integrated security=true"))
            {
                try
                {
                    string kadi = tbKullaniciAdi.Text.ToString();
                    string sifre = tbSifre.Text.ToString();

                    bag.Open();
                    SqlCommand cmd = new SqlCommand("select count(*) from kullanicilar where kullaniciadi = @tb1 and sifre = @tb2;", bag);

                    cmd.Parameters.AddWithValue("@tb1", kadi);
                    cmd.Parameters.AddWithValue("@tb2", sifre);

                    object sonuc = cmd.ExecuteScalar();
                    int login = 0;

                    if (sonuc != DBNull.Value)
                    {
                        if (int.TryParse(sonuc.ToString(), out login))
                        {

                        }
                        else
                        {
                            login = 0;
                        }
                    }

                    //Login Sonuç Kontrolü
                    if (login != 0)
                    {
                        labelOturumAcma.ForeColor = Color.Green;
                        labelOturumAcma.Text = "Giriş Başarılı!";
                        await Task.Delay(1000);
                        Form2 form2 = new Form2(kadi);
                        form2.Show();
                        this.Hide();
                        bag.Close();
                    }

                    else
                    {
                        labelOturumAcma.ForeColor = Color.Red;
                        labelOturumAcma.Text = "Giriş Başarısız.";
                        tbKullaniciAdi.Text = "";
                        tbSifre.Text = "";
                        await Task.Delay(5000);
                        labelOturumAcma.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir Hata Oluştu" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Enter tuşuna basıldığında "Oturum Aç" düğmesine basılmış gibi davranan kod
            protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                button1.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}

