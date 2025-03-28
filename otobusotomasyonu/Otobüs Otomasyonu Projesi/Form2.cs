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
    public partial class Form2 : Form
    {
        private string kullaniciadi;
        public Form2(string kadi)
        {
            InitializeComponent();
            kullaniciadi = kadi;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            SqlConnection sql = new SqlConnection("Server=DESKTOP-OL9B8A7;Database=otobusotomasyon;integrated security=true");
            sql.Open();
            SqlCommand cmd = new SqlCommand("select adi,soyadi from kullanicilar where kullaniciadi = @kadi1;", sql);
            cmd.Parameters.AddWithValue("@kadi1", kullaniciadi);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read()) {
                string ad = reader["adi"].ToString();
                string soyad = reader["soyadi"].ToString();
                label1.Text = $"Hoşgeldiniz {ad} {soyad}"; 
            }
        }

        private void buttonAracEkle_Click(object sender, EventArgs e)
        {
            using (SqlConnection bag = new SqlConnection("Server=DESKTOP-OL9B8A7;Database=otobusotomasyon;integrated security=true"))
            {
                try
                {
                    bag.Open();
                    string query = @"
                    SELECT 
                    o.OtobusID, 
                    o.PlakaNo, 
                    o.Kapasite, 
                    m.MarkaAdi AS Markası, 
                    mo.ModelAdi AS Modeli
                    FROM otobus o
                    JOIN OtobusMarka m ON o.MarkaID = m.MarkaID
                    JOIN OtobusModel mo ON o.ModelID = mo.ModelID";
                    SqlDataAdapter da = new SqlDataAdapter(query, bag);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    dataGridView1.Columns[0].HeaderText = "OtobusID";
                    dataGridView1.Columns[1].HeaderText = "Plakası";
                    dataGridView1.Columns[2].HeaderText = "Kapasite";
                    dataGridView1.Columns[3].HeaderText = "Markası";
                    dataGridView1.Columns[4].HeaderText = "Modeli";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Servera bağlanırken bir hata oluştu:" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void button2_MouseHover_1(object sender, EventArgs e)
        {
            toolTip1.Show("Ayarlar", button2);
        }

        private void buttonSefer_Click(object sender, EventArgs e)
        {
            using (SqlConnection bag = new SqlConnection("Server=DESKTOP-OL9B8A7;Database=otobusotomasyon;integrated security=true"))
            {
                try
                {
                    bag.Open();
                    string query = @"
                    SELECT 
                    s.SeferID,
                    s.KalkisSaati,
                    s.VarisSaati,
                    d1.DurakAdi AS KalkisYeri,
                    il1.İlAdi AS KalkisIli,
                    d2.DurakAdi AS VarisYeri,
                    il2.İlAdi AS VarisIli,
                    o.PlakaNo AS OtobusPlaka,
                    m.MarkaAdi AS OtobusMarkasi,
                    mo.ModelAdi AS OtobusModeli,
                    p.AdiSoyadi AS SoforAdi
                    FROM Sefer s
                    JOIN DurakKayit dk1 ON dk1.SeferID = s.SeferID AND dk1.Türü = 'Kalkış'
                    JOIN Durak d1 ON d1.DurakID = dk1.DurakID
                    JOIN İlce ilce1 ON ilce1.İlceID = d1.İlceID
                    JOIN İl il1 ON il1.İlID = ilce1.İlID
                    JOIN DurakKayit dk2 ON dk2.SeferID = s.SeferID AND dk2.Türü = 'Varış'
                    JOIN Durak d2 ON d2.DurakID = dk2.DurakID
                    JOIN İlce ilce2 ON ilce2.İlceID = d2.İlceID
                    JOIN İl il2 ON il2.İlID = ilce2.İlID
                    JOIN Otobus o ON o.OtobusID = s.OtobusID
                    JOIN OtobusMarka m ON o.MarkaID = m.MarkaID
                    JOIN OtobusModel mo ON o.ModelID = mo.ModelID
                    JOIN Personel p ON p.PersonelID = s.PersonelID;";
                    SqlDataAdapter da = new SqlDataAdapter(query, bag);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    dataGridView1.Columns[0].HeaderText = "Sefer Numarası";
                    dataGridView1.Columns[1].HeaderText = "Kalkış Saati";
                    dataGridView1.Columns[2].HeaderText = "Varış Saati";
                    dataGridView1.Columns[3].HeaderText = "Kalkış Otogarı";
                    dataGridView1.Columns[4].HeaderText = "Kalkış İli";
                    dataGridView1.Columns[5].HeaderText = "Varış Otogarı";
                    dataGridView1.Columns[6].HeaderText = "Varış İli";
                    dataGridView1.Columns[7].HeaderText = "Araç Plakası";
                    dataGridView1.Columns[8].HeaderText = "Araç Markası";
                    dataGridView1.Columns[9].HeaderText = "Araç Modeli";
                    dataGridView1.Columns[10].HeaderText = "Personel Adı Soyadı";
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Servera bağlanırken bir hata oluştu:" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection bag = new SqlConnection("Server=DESKTOP-OL9B8A7;Database=otobusotomasyon;integrated security=true"))
            {
                try
                {
                    bag.Open();
                    string query = @"
                    SELECT 
                    b.BiletID, 
                    b.KoltukNo, 
                    b.Fiyat, 
                    s.SeferID AS SeferNo, 
                    y.AdıSoyadi AS YolcuAdSoyad
                    FROM bilet b
                    JOIN Sefer S ON s.SeferID = b.SeferID
                    JOIN yolcu y ON y.YolcuID = b.yolcuID";
                    SqlDataAdapter da = new SqlDataAdapter(query, bag);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    dataGridView1.Columns[0].HeaderText = "Bilet Numarası";
                    dataGridView1.Columns[1].HeaderText = "Koltuk Numarası";
                    dataGridView1.Columns[2].HeaderText = "Fiyatı";
                    dataGridView1.Columns[3].HeaderText = "Sefer Numarası";
                    dataGridView1.Columns[4].HeaderText = "Yolcu Adı Soyadı";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Servera bağlanırken bir hata oluştu:" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonYolcu_Click(object sender, EventArgs e)
        {
            using (SqlConnection bag = new SqlConnection("Server=DESKTOP-OL9B8A7;Database=otobusotomasyon;integrated security=true"))
            {
                try
                {
                    bag.Open();
                    string query = @"
                    SELECT 
                    y.YolcuID, 
                    y.AdıSoyadi, 
                    y.TelefonNumarasi, 
                    y.Eposta, 
                    y.KimlikNo, 
                    b.BiletId AS BiletNumarasi
                    FROM yolcu y
                    JOIN Bilet B ON b.BiletId = y.BiletId";
                    SqlDataAdapter da = new SqlDataAdapter(query, bag);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    dataGridView1.Columns[0].HeaderText = "Yolcu Numarası";
                    dataGridView1.Columns[1].HeaderText = "Adı Soyadı";
                    dataGridView1.Columns[2].HeaderText = "Telefon Numarası";
                    dataGridView1.Columns[3].HeaderText = "E-Posta";
                    dataGridView1.Columns[4].HeaderText = "T.C. Kimlik No";
                    dataGridView1.Columns[5].HeaderText = "Bilet Numarası";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Servera bağlanırken bir hata oluştu:" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonPersonel_Click(object sender, EventArgs e)
        {
            using (SqlConnection bag = new SqlConnection("Server=DESKTOP-OL9B8A7;Database=otobusotomasyon;integrated security=true"))
            {
                try
                {
                    bag.Open();
                    string query = @"
                    SELECT 
                    p.PersonelID,
                    p.AdiSoyadi,
                    p.TelefonNumarasi,
                    g.GorevAdi AS GorevAdı
                    FROM personel p
                    JOIN Gorev G ON g.GorevId = p.GorevId;";
                    SqlDataAdapter da = new SqlDataAdapter(query, bag);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    dataGridView1.Columns[0].HeaderText = "Personel No";
                    dataGridView1.Columns[1].HeaderText = "Adı Soyadı";
                    dataGridView1.Columns[2].HeaderText = "Telefon Numarası";
                    dataGridView1.Columns[3].HeaderText = "Görevi";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Servera bağlanırken bir hata oluştu:" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonOtogar_Click(object sender, EventArgs e)
        {
            using (SqlConnection bag = new SqlConnection("Server=DESKTOP-OL9B8A7;Database=otobusotomasyon;integrated security=true"))
            {
                try
                {
                    bag.Open();
                    string query = @"
                    SELECT 
                    d.DurakId,
                    d.DurakAdi,
                    d.Adres,
                    s.İlAdi AS SehirAdi,
                    i.İlceAdi AS İlceAdi
                    FROM Durak D
                    JOIN İl S ON s.İlID = d.İlID
                    JOIN İlce i ON i.İlceID = d.İlceID;";
                    SqlDataAdapter da = new SqlDataAdapter(query, bag);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    dataGridView1.Columns[0].HeaderText = "Otogar No";
                    dataGridView1.Columns[1].HeaderText = "Otogar Adı";
                    dataGridView1.Columns[2].HeaderText = "Otogar Adresi";
                    dataGridView1.Columns[3].HeaderText = "İli";
                    dataGridView1.Columns[4].HeaderText = "İlçesi";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Servera bağlanırken bir hata oluştu:" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonBakim_Click(object sender, EventArgs e)
        {
            using (SqlConnection bag = new SqlConnection("Server=DESKTOP-OL9B8A7;Database=otobusotomasyon;integrated security=true"))
            {
                try
                {
                    bag.Open();
                    string query = @"
                    SELECT 
                    b.BakimId,
                    b.BakimTarihi,
                    b.BakimTuru,
                    b.YapilanIslemler,
                    b.Maliyet,
                    p.AdiSoyadi AS PersonelAdiSoyadi,
                    o.PlakaNo AS PlakaNo
                    FROM Bakim B
                    JOIN Personel P ON p.PersonelID = b.PersonelID
                    JOIN Otobus o ON o.OtobusID = b.OtobusID;";
                    SqlDataAdapter da = new SqlDataAdapter(query, bag);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    dataGridView1.Columns[0].HeaderText = "Bakım No";
                    dataGridView1.Columns[1].HeaderText = "Bakım Tarihi";
                    dataGridView1.Columns[2].HeaderText = "Bakım Türü";
                    dataGridView1.Columns[3].HeaderText = "Yapılan İşlemler";
                    dataGridView1.Columns[4].HeaderText = "Maliyeti";
                    dataGridView1.Columns[5].HeaderText = "Görevli Personel";
                    dataGridView1.Columns[6].HeaderText = "Otobüs Plaka No";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SQL Servera bağlanırken bir hata oluştu:" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonOtobusEkle_Click(object sender, EventArgs e)
        {

        }
    }
}
