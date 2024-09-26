using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using VeriYapilari;

namespace ders_takip_sistemi
{

    internal static class Program
    {
        public static HashTable dersler;
        public static OgrenciAVL ogrenciListe;
        public static OgretimGorevlisiAVL ogretmenListe;
        [STAThread]
        static void Main()
        {
            Console.WriteLine("deneme");
            dersler = new HashTable(12,20);
            ogrenciListe = new OgrenciAVL();
            ogretmenListe = new OgretimGorevlisiAVL();

            string dersAdi, gunId, dersTuru;
            int ortmenId, dersId, saatId, donemId, dersKredi;
            string connString = "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=12345;";
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();
            string sql = "SELECT * FROM ders";
            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                dersAdi = string.Join("", (string[])reader.GetValue(1));
                gunId = string.Join("", (string[])reader.GetValue(6));
                dersTuru = string.Join("", (string[])reader.GetValue(7));
                ortmenId = reader.GetInt32(0);
                dersId = reader.GetInt32(3);
                saatId = reader.GetInt32(4);
                donemId = reader.GetInt32(5);
                dersKredi = reader.GetInt32(2);
                dersler.Ekle(new Ders(dersAdi, gunId, dersTuru, ortmenId, dersId, saatId, donemId, dersKredi));
            }

            reader.Close();

            sql = "SELECT * FROM OgrenciBilgi";
            cmd = new NpgsqlCommand(sql, conn);
            reader = cmd.ExecuteReader();

            string fullAdi, bolumu, cinsiyet;
            int ogrenciId;
            while (reader.Read())
            {
                fullAdi = string.Join("", (string[])reader.GetValue(1));
                bolumu = string.Join("", (string[])reader.GetValue(2));
                cinsiyet = string.Join("", (string[])reader.GetValue(3));
                ogrenciId = reader.GetInt32(0);
                donemId = reader.GetInt32(4);
                ogrenciListe.InsertOgr(new Ogrenci(fullAdi, cinsiyet, ogrenciId, donemId, "", "", bolumu));
            }

            reader.Close();

            sql = "SELECT * FROM ogrencigiris";
            cmd = new NpgsqlCommand(sql, conn);
            reader = cmd.ExecuteReader();

            string kullaniciAdi, sifre;
            Ogrenci ogrenciTemp;
            while (reader.Read())
            {
                ogrenciId = reader.GetInt32(0);
                kullaniciAdi = reader.GetValue(1).ToString();
                sifre = reader.GetValue(2).ToString();
                ogrenciTemp = ogrenciListe.FindOgrenciID(ogrenciId).Ogrenci;
                ogrenciTemp.kullanici_adi = kullaniciAdi;
                ogrenciTemp.sifre=sifre;
            }

            reader.Close();

            sql = "SELECT * FROM ogretimgorevlisibilgi";
            cmd = new NpgsqlCommand(sql, conn);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                fullAdi = string.Join("", (string[])reader.GetValue(1));
                cinsiyet = string.Join("", (string[])reader.GetValue(2));
                ortmenId = reader.GetInt32(0);

                ogretmenListe.InsertGorevli(new OgretimGorevlisi(fullAdi,cinsiyet,"","",ortmenId));
            }

            reader.Close();

            sql = "SELECT * FROM ogretmengiris";
            cmd = new NpgsqlCommand(sql, conn);
            reader = cmd.ExecuteReader();

            OgretimGorevlisi ogretimTemp;
            while (reader.Read())
            {
                ortmenId = reader.GetInt32(0);
                kullaniciAdi = reader.GetValue(1).ToString();
                sifre = reader.GetValue(2).ToString();
                ogretimTemp = ogretmenListe.FindGorevliID(ortmenId).OgretimGorevlisi;
                ogretimTemp.kullanici_adi = kullaniciAdi;
                ogretimTemp.sifre = sifre;
            }
            reader.Close();
            conn.Close();

            Liste x;

            for (int i = 0; i < dersler.hkapasite; i++)
            {
                x = dersler.ListBul(i+10);
                x.ElemanYaz();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new loadingPage(dersler,ogrenciListe,ogretmenListe));

            
        }
    }
}
 