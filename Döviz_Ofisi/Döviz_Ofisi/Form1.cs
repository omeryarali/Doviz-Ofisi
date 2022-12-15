using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Data.SqlClient;

namespace Döviz_Ofisi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti=new SqlConnection(@"Data Source=DESKTOP-7KPL2B4\MSSQLSERVERRRR;Initial Catalog=DbDovizOfisi;Integrated Security=True");
        private void Form1_Load(object sender, EventArgs e)
        {
            string bugun = "https://www.tcmb.gov.tr/kurlar/today.xml";
            var xmldosya=new XmlDocument();
            xmldosya.Load(bugun);

            string dolaralis = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteBuying").InnerXml;
            LblDolarAlis.Text = dolaralis;
            string dolarsatis=xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml;
            LblDolarSatis.Text = dolarsatis;

            string euroalis = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteBuying").InnerXml;
            LblEuroAlis.Text = euroalis;
            string eurosatis = xmldosya.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteSelling").InnerXml;
            LblEuroSatis.Text = eurosatis;

        }

        private void BtnDolarAl_Click(object sender, EventArgs e)
        {
            TxtKur.Text = LblDolarAlis.Text;
        }

        private void BtnDolarSat_Click(object sender, EventArgs e)
        {
            TxtKur.Text = LblDolarSatis.Text;
        }

        private void BtnEuroAl_Click(object sender, EventArgs e)
        {
            TxtKur.Text = LblEuroAlis.Text;
        }

        private void BtnEuroSat_Click(object sender, EventArgs e)
        {
            TxtKur.Text = LblEuroSatis.Text;
        }

        private void BtnSatisyap_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut1 = new SqlCommand("insert into TBLDoviz (TL) VALUES (@P1) ",baglanti);
            
            double kur, miktar, tutar;
            kur=Convert.ToDouble(TxtKur.Text);
            miktar=Convert.ToDouble(TxtMiktar.Text);
            tutar = kur * miktar;
            TxtTutar.Text = tutar.ToString();
            if(RdDolarAlis.Checked)
            {
                komut1.Parameters.AddWithValue("@P1",float.Parse(tutar.ToString()));
                komut1.ExecuteNonQuery();
                baglanti.Close();
            }
            else if(RdEuroAlis.Checked)
            {
                komut1.Parameters.AddWithValue("@P1", float.Parse(tutar.ToString()));
                komut1.ExecuteNonQuery();
                baglanti.Close();

            }


        }

        private void TxtKur_TextChanged(object sender, EventArgs e)
        {
            TxtKur.Text = TxtKur.Text.Replace(".",",");
        }
        private void BtnSatisYap2_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut1 = new SqlCommand("insert into TBLDoviz (DOLAR) values (@P1)",baglanti);
            SqlCommand komut2 = new SqlCommand("insert into TBLDoviz (EURO) values (@P2)", baglanti);
            double kur=Convert.ToDouble(TxtKur.Text);
            int miktar = Convert.ToInt32(TxtMiktar.Text);
            int tutar = Convert.ToInt32(miktar / kur);
            TxtTutar.Text=tutar.ToString();
            double kalan=0;
            kalan = miktar%kur;
            TxtKalan.Text=kalan.ToString();
            if(RdDolarSatis.Checked)
            {
                komut1.Parameters.AddWithValue("@P1", float.Parse(tutar.ToString()));
                komut1.ExecuteNonQuery();
                baglanti.Close();
            }
            else if(RdEuroSatis.Checked)
            {
                komut2.Parameters.AddWithValue("@P2", float.Parse(tutar.ToString()));
                komut2.ExecuteNonQuery();
                baglanti.Close();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            TxtKur.Text=LblDolarAlis.Text;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            TxtKur.Text=LblDolarSatis.Text;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            TxtKur.Text=LblEuroAlis.Text;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            TxtKur.Text=LblEuroSatis.Text;
        }
    }
}
//sql tabanına ekle 3 sütün tl euro dolar diye dönüştükce yazdır sqle
