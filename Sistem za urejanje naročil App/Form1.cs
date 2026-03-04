using System;
using System.Windows.Forms;
using SistemZaUrejanjeNaročilKnjižnjica;

namespace Sistem_za_urejanje_naročil_App
{
    public partial class Form1 : Form
    {
        Kupec aktivniKupec;

        //uporablja se razred Naročilo z indekserjem
        Naročilo narocilo = new Naročilo();

        public Form1()
        {
            InitializeComponent();
            comboBoxVelikostMajice.Items.AddRange(Konstante.Velikosti);
            comboBoxDebelina.Items.AddRange(Konstante.Debeline);

            narocilo.PostavkaDodana += Narocilo_PostavkaDodana;
            narocilo.CenaSpremenjena += Narocilo_CenaSpremenjena;
        }

        private void OsveziNarocilo()
        {
            Artikli.Items.Clear();

            labelIzpisKoncneCene.Text = narocilo.SkupnaCena.ToString("0.00") + "€";

            //indekser v praksi
            for (int i = 0; i < narocilo.stPos; i++)
            {
                NaročiloIzdelka x = narocilo[i];   //indekser
                Artikli.Items.Add(x);
                Artikel a = x.Artikel;
                decimal cena = a.izracunCene();
            }

            if (narocilo.stPos > 0)
            {
                var prva = narocilo[0];
            }
        }

        private void Narocilo_PostavkaDodana(object sender, NaročiloIzdelka p)
        {
            //ko se doda postavka jo dodamo v ListBox
            Artikli.Items.Add(p);
        }

        private void Narocilo_CenaSpremenjena(object sender, decimal cena)
        {
            //ko se spremeni cena posodobimo label
            labelIzpisKoncneCene.Text = cena.ToString("0.00") + "€";
        }

        private void buttonShraniKupca_Click(object sender, EventArgs e)
        {
            if (!textBoxEmail.Text.Contains("@"))
            {
                MessageBox.Show("Email mora vsebovati znak @");
                return;
            }

            aktivniKupec = new Kupec
                (
                    textBoxIme.Text,
                    textBoxPriimek.Text,
                    textBoxTelŠt.Text,
                    textBoxEmail.Text
                );

            labelIzpisAktivnegaKupca.Text = aktivniKupec.ToString();
        }

        private void buttonDodajMajico_Click(object sender, EventArgs e)
        {
            if (aktivniKupec == null)
            {
                MessageBox.Show("Ni aktivnega kupca");
                return;
            }

            KratkaMajica majica = new KratkaMajica
                (
                    "Kratka majica",
                    12m,
                    textBoxBarvaMajice.Text,
                    comboBoxVelikostMajice.Text,
                    checkBoxSprednjiPrt.Checked,
                    checkBoxZadnjiPrt.Checked
                );

            NaročiloIzdelka n = new NaročiloIzdelka(majica, 1);
            n.Kupec = aktivniKupec;

            narocilo.Dodaj(n);

            //vmesniki
            if (majica is IPrintable p && p.ImaPrint)
            {
                MessageBox.Show("Cena printa: " + p.CenaPrinta.ToString("0.00") + "€");
            }

            IProdajni prod = majica;
            decimal cenaArtikla = prod.Cena;

            OsveziNarocilo();

            textBoxBarvaMajice.Clear();
            checkBoxSprednjiPrt.Checked = false;
            checkBoxZadnjiPrt.Checked = false;
            comboBoxVelikostMajice.SelectedIndex = 0;
        }

        private void buttonDodajHoodie_Click(object sender, EventArgs e)
        {
            if (aktivniKupec == null)
            {
                MessageBox.Show("Ni aktivnega kupca");
                return;
            }

            Hoodie hudi = new Hoodie
                (
                    "Hoodie",
                    25m,
                    textBoxBarvaHoodie.Text,
                    comboBoxDebelina.Text,
                    checkBoxZadrga.Checked
                );

            NaročiloIzdelka n = new NaročiloIzdelka(hudi, 1);
            n.Kupec = aktivniKupec;

            narocilo.Dodaj(n);

            IProdajni prod = hudi;
            decimal cenaArtikla = prod.Cena;

            OsveziNarocilo();

            textBoxBarvaHoodie.Clear();
            checkBoxZadrga.Checked = false;
            comboBoxDebelina.SelectedIndex = 0;
        }
    }
}
