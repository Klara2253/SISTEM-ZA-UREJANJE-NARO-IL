using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SistemZaUrejanjeNaročilKnjižnjica;

namespace Sistem_za_urejanje_naročil_App
{
    public partial class Form1 : Form
    {
        Kupec aktivniKupec;
        List<NaročiloIzdelka> narocilo = new List<NaročiloIzdelka>();
        public Form1()
        {
            InitializeComponent();
            comboBoxVelikostMajice.Items.AddRange(Konstante.Velikosti);
            comboBoxDebelina.Items.AddRange(Konstante.Debeline);
        }

        private void OsveziNarocilo()
        {
            Artikli.Items.Clear();

            decimal skupaj = 0m;

            foreach (NaročiloIzdelka x in narocilo)
            {
                Artikli.Items.Add(x);
                skupaj += x;
            }
            labelIzpisKoncneCene.Text = skupaj.ToString("0.00") + "€";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
            if(aktivniKupec == null)
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

            narocilo.Add(n);

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

            narocilo.Add(n);

            OsveziNarocilo();

            textBoxBarvaHoodie.Clear();
            checkBoxZadrga.Checked = false;
            comboBoxDebelina.SelectedIndex = 0;
        }
    }
}
