﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VeriYapilari;

namespace ders_takip_sistemi
{
    public partial class loadingPage : Form
    {
        public HashTable dersler;
        public OgrenciAVL ogrenciListe;
        public OgretimGorevlisiAVL ogretmenListe;

        public loadingPage(HashTable dersler, OgrenciAVL ogrenciListe, OgretimGorevlisiAVL ogretmenListe)
        {
            this.dersler = dersler;
            this.ogrenciListe = ogrenciListe;
            this.ogretmenListe = ogretmenListe;
            InitializeComponent();
        }

        private void loadingPage_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            panel2.Width +=3/2;

            if(panel2.Width > 599)
            {
                timer1.Stop();
                AnaSayfa ogrenciPage = new AnaSayfa(dersler, ogrenciListe, ogretmenListe);
                ogrenciPage.Show();
                this.Hide();
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
