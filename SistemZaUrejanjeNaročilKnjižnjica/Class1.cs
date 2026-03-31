using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemZaUrejanjeNaročilKnjižnjica
{
    /// <summary>
    /// Vmesnik samo za artikle za prodajo
    /// </summary>
    public interface IProdajni
    {
        decimal Cena { get; }
    }
    /// <summary>
    /// Vmesnik za izdelke, ki se lahko tiskajo. Pove ali ima artikel tisk
    /// in vrne ceno izdelka
    /// </summary>
    public interface IPrintable
    {
        bool ImaPrint { get; }
        decimal CenaPrinta { get; }
    }

    /// <summary>
    /// Razred z konstantami za velikost in debelino
    /// </summary>
    public static class Konstante
    {
        public static readonly string[] Velikosti =
        {
            "XS", "S", "M", "L", "XL", "XXL"
        };

        public static readonly string[] Debeline =
        {
            "Lahka", "Srednja", "Debela"
        };
    }
    /// <summary>
    /// Abstraktni razred za vse artikle, ki implementira vmesnik IProdajni
    /// </summary>
    public abstract class Artikel : IProdajni
    {
        public decimal Cena
        {
            get { return izracunCene(); }
        }

        private string imeArtikla;
        private decimal osnovnaCena;
        private string kategorija;
        private static int stevecID = 1000;

        public const decimal DDV = 0.22m;
        public readonly DateTime Ustvarjeno;

        public int IDIzdelka { get; }

        public string ImeArtikla
        {
            get { return imeArtikla; }
            set { imeArtikla = value; }
        }

        public decimal OsnovnaCena
        {
            get { return osnovnaCena; }
            set
            {
                if (value <= 0)
                {
                    Console.WriteLine("Cena mora biti večja od 0");
                    return;
                }

                osnovnaCena = value;
            }
        }

        public string Kategorija
        {
            get { return kategorija; }
            protected set { kategorija = value; }
        }

        /// <summary>
        /// Konstruktor da ustvarimo artikel
        /// </summary>
        /// <param name="ime">Ime artikla</param>
        /// <param name="cena">Osnovna cena</param>
        /// <param name="kategorija">Kategorija</param>

        public Artikel(string ime, decimal cena, string kategorija)
        {
            IDIzdelka = ++stevecID;
            Ustvarjeno = DateTime.Now;

            ImeArtikla = ime;
            OsnovnaCena = cena;
            Kategorija = kategorija;
        }

        /// <summary>
        /// Izracun cene
        /// </summary>
        /// <returns></returns>
        public abstract decimal izracunCene();

        //virtualna metoda
        /// <summary>
        /// Vrne vse podrobnosti izdelka
        /// </summary>
        /// <returns>Podrobnosti izdelka</returns>
        public virtual string VrniPodrobnosti()
        {
            return $"#{IDIzdelka}, {ImeArtikla}, kategorija: {Kategorija}, cena: {izracunCene():0.00}€";
        }

        public override string ToString()
        {
            return VrniPodrobnosti();
        }
    }

    /// <summary>
    /// Razred za kartko majico, ki zopet implementira IProdajni
    /// </summary>
    public class KratkaMajica : Artikel, IPrintable
    {
        private string barva;
        private string velikost;

        public string Barva
        {
            get { return barva; }
            set { barva = value; }
        }

        public string Velikost
        {
            get { return velikost; }
            set
            {
                if (!Konstante.Velikosti.Contains(value))
                {
                    Console.WriteLine("Napačna velikost");
                    return;
                }

                velikost = value;
            }
        }

        public bool SprednjiPrint { get; set; }
        public bool ZadnjiPrint { get; set; }

        public static readonly decimal DopSpredaj = 3.50m;
        public static readonly decimal DopZadaj = 4.00m;

        public bool ImaPrint
        {
            get
            {
                return SprednjiPrint || ZadnjiPrint;
            }
        }

        public decimal CenaPrinta
        {
            get
            {
                decimal cenaPrinta = 0m;

                if (SprednjiPrint)
                    cenaPrinta += DopSpredaj;

                if (ZadnjiPrint)
                    cenaPrinta += DopZadaj;

                return cenaPrinta;
            }
        }
        /// <summary>
        /// Konstruktor ki ustvari kratko majico
        /// </summary>
        /// <param name="ime">Ime</param>
        /// <param name="cena">Cena kratke majice</param>
        /// <param name="barva">Barva majice</param>
        /// <param name="velikost">Velikost majice</param>
        /// <param name="spredaj">Ali je tisk spredaj</param>
        /// <param name="zadaj">Ali je tisk zadaj</param>
        public KratkaMajica(string ime, decimal cena, string barva, string velikost, bool spredaj, bool zadaj)
            : base(ime, cena, "Kratka majica")
        {
            Barva = barva;
            Velikost = velikost;
            SprednjiPrint = spredaj;
            ZadnjiPrint = zadaj;
        }
        /// <summary>
        /// Izračuna ceno
        /// </summary>
        /// <returns>Končna cena</returns>
        public override decimal izracunCene()
        {
            decimal cena = OsnovnaCena;

            if (SprednjiPrint)
                cena += DopSpredaj;

            if (ZadnjiPrint)
                cena += DopZadaj;

            return cena;
        }

        /// <summary>
        /// Vrne vse podrobnosti kratke majice
        /// </summary>
        /// <returns>Podrobnosti kratke majice</returns>
        public override string VrniPodrobnosti()
        {
            return $"#{IDIzdelka}, {ImeArtikla}, barva: {Barva}, velikost: {Velikost}, print: {ImaPrint}, cena: {izracunCene():0.00}€";
        }
    }
    /// <summary>
    /// Razred za pulover
    /// </summary>
    public class Hoodie : Artikel
    {
        private string barva;
        private string debelina;

        public string Barva
        {
            get { return barva; }
            set { barva = value; }
        }

        public string Debelina
        {
            get { return debelina; }
            set
            {
                if (!Konstante.Debeline.Contains(value))
                {
                    Console.WriteLine("Napačna debelina");
                    return;
                }

                debelina = value;
            }
        }

        public bool Zadrga { get; set; }

        public static readonly decimal DopZadrga = 5.00m;
        public static readonly decimal DopDebel = 6.00m;

        /// <summary>
        /// Konstruktor ki ustvari pulover
        /// </summary>
        /// <param name="ime">Ime</param>
        /// <param name="cena">Cena puloverja</param>
        /// <param name="barva">Barva puloverja</param>
        /// <param name="debelina">Debelina puloverja</param>
        /// <param name="zadrga">Ali ima zadrgo ali ne</param>
        public Hoodie(string ime, decimal cena, string barva, string debelina, bool zadrga)
            : base(ime, cena, "Hoodie")
        {
            Barva = barva;
            Debelina = debelina;
            Zadrga = zadrga;
        }
        /// <summary>
        /// Izracuna ceno puloverja
        /// </summary>
        /// <returns>Končna cena</returns>
        public override decimal izracunCene()
        {
            decimal cena = OsnovnaCena;

            if (Zadrga)
                cena += DopZadrga;

            if (Debelina == "Debela")
                cena += DopDebel;

            return cena;
        }

        /// <summary>
        /// Vrne podrobnosti puloverja
        /// </summary>
        /// <returns>Podrobnosti puloverja</returns>
        public override string VrniPodrobnosti()
        {
            return $"#{IDIzdelka}, {ImeArtikla}, barva: {Barva}, debelina: {Debelina}, zadrga: {Zadrga}, cena: {izracunCene():0.00}€";
        }
    }
    /// <summary>
    /// Razred za kupca
    /// </summary>
    public class Kupec
    {
        private string ime;
        private string priimek;
        private string telSt;
        private string email;

        public string Ime
        {
            get { return ime; }
            set { ime = value; }
        }

        public string Priimek
        {
            get { return priimek; }
            set { priimek = value; }
        }

        public string TelSt
        {
            get { return telSt; }
            set
            {
                if (value.Length != 9)
                {
                    Console.WriteLine("Telefonska številka mora imeti 9 številk");
                    return;
                }

                foreach (char c in value)
                {
                    if (!char.IsDigit(c))
                    {
                        Console.WriteLine("Telefonska številka sme vsebovati samo številke");
                        return;
                    }
                }

                telSt = value;
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                if (!value.Contains("@"))
                {
                    Console.WriteLine("Napačen email");
                    return;
                }

                email = value;
            }
        }
        /// <summary>
        /// Ustvarimo kupca
        /// </summary>
        /// <param name="ime">Ime kupca</param>
        /// <param name="priimek">Priimek kupca</param>
        /// <param name="telst">Telefonska številka kupca</param>
        /// <param name="email">Email kupca</param>
        public Kupec(string ime, string priimek, string telst, string email)
        {
            Ime = ime;
            Priimek = priimek;
            TelSt = telst;
            Email = email;
        }

        public override string ToString()
        {
            return $"Ime: {Ime}, priimek: {Priimek}, telefonska številka: {TelSt}, email: {Email}";
        }
    }
    /// <summary>
    /// Razred za eno postavko
    /// </summary>
    public class NaročiloIzdelka
    {
        private int količina;

        public Artikel Artikel { get; }
        public Kupec Kupec { get; set; }

        public int Količina
        {
            get { return količina; }
            private set
            {
                if (value <= 0)
                {
                    Console.WriteLine("Količina mora biti več od 0");
                    return;
                }

                količina = value;
            }
        }

        public decimal KoncnaCena
        {
            get { return Artikel.izracunCene() * Količina; }
        }
        /// <summary>
        /// Konstrukor ki naredi eno postavko
        /// </summary>
        /// <param name="artikel">Artikel</param>
        /// <param name="količina">Količina</param>
        public NaročiloIzdelka(Artikel artikel, int količina)
        {
            Artikel = artikel;
            Količina = količina;
        }

        /// <summary>
        /// Operator da sešteje dve postavki
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static decimal operator +(NaročiloIzdelka a, NaročiloIzdelka b)
        {
            return a.KoncnaCena + b.KoncnaCena;
        }
        public static decimal operator +(decimal cena, NaročiloIzdelka n)
        {
            if (n == null)
                return cena;

            return cena + n.KoncnaCena;
        }

        public override string ToString()
        {
            return $"{Artikel}, količina: {Količina}, skupaj: {KoncnaCena:0.00}€";
        }
        /// <summary>
        /// Destruktor
        /// </summary>
        ~NaročiloIzdelka()
        {
            Console.WriteLine($"Artikel {Artikel.IDIzdelka} je bil odstranjen");
        }
    }
    /// <summary>
    /// Razred za upravljanje naročila
    /// </summary>
    public class Naročilo
    {
        private List<NaročiloIzdelka> postavka = new List<NaročiloIzdelka>();
        /// <summary>
        /// Delegat za postavko ki smo jo dodali
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="postavka"></param>
        public delegate void PostavkaHandler(object sender, NaročiloIzdelka postavka);
        /// <summary>
        /// Delegat za spremembo cene
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cena"></param>
        public delegate void CenaHandler(object sender, decimal cena);
        /// <summary>
        /// Dogodek ob dodaji nove postavke
        /// </summary>
        public event PostavkaHandler PostavkaDodana;
        /// <summary>
        /// Dogodek ob spremembi cene
        /// </summary>
        public event CenaHandler CenaSpremenjena;

        public NaročiloIzdelka this[int i]
        {
            get { return postavka[i]; }
        }

        public int stPos
        {
            get { return postavka.Count; }
        }

        /// <summary>
        /// Doda novo postavko
        /// </summary>
        /// <param name="n"></param>
        public void Dodaj(NaročiloIzdelka n)
        {
            postavka.Add(n);
            OnPostavkaDodana(n);
            OnCenaSpremenjena(SkupnaCena);
        }
        /// <summary>
        /// Sproži dogodek ko se postavka doda
        /// </summary>
        /// <param name="p"></param>
        protected virtual void OnPostavkaDodana(NaročiloIzdelka p)
        {
            if (PostavkaDodana != null)
                PostavkaDodana(this, p);
        }
        /// <summary>
        /// Sproži dogodek ko se cena spremeni
        /// </summary>
        /// <param name="cena"></param>
        protected virtual void OnCenaSpremenjena(decimal cena)
        {
            if (CenaSpremenjena != null)
                CenaSpremenjena(this, cena);
        }

        public decimal SkupnaCena
        {
            get
            {
                decimal skupaj = 0m;

                foreach (NaročiloIzdelka n in postavka)
                {
                    skupaj += n.KoncnaCena;
                }

                return skupaj;
            }
        }
    }
}