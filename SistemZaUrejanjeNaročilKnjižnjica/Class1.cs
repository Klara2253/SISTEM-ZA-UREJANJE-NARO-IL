using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SistemZaUrejanjeNaročilKnjižnjica
{
    //vmesniki
    public interface IProdajni
    {
        decimal Cena { get; }
    }

    public interface IPrintable
    {
        bool ImaPrint { get; }
        decimal CenaPrinta { get; }
    }



    //razred konstant ki jih nikoli ne spremenim
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

        public int IDIzdelka
        {
            get;
        }

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
                if (value <= 0) Console.WriteLine("Cena mora biti večja od 0");
                osnovnaCena = value;
            }
        }

        public string Kategorija
        {
            get { return kategorija; }
            protected set { kategorija = value; }
        }

        public Artikel(string ime, decimal cena, string kategorija)
        {
            IDIzdelka = ++stevecID;
            Ustvarjeno = DateTime.Now;

            ImeArtikla = ime;
            OsnovnaCena = cena;
            Kategorija = kategorija;
        }

        public abstract decimal izracunCene();


        public override string ToString()
        {
            return ($"#{IDIzdelka}, {ImeArtikla}, {izracunCene().ToString("0.00")}€");
        }
    }

    public class KratkaMajica : Artikel, IPrintable
    {
        public bool ImaPrint
        {
            get
            {
                if (SprednjiPrint == true || ZadnjiPrint == true)
                { return true; }
                else return false;
            }
        }

        public decimal CenaPrinta
        {
            get
            {
                decimal cenaPrinta = 0m;

                if (SprednjiPrint == true)
                {
                    cenaPrinta = cenaPrinta + DopSpredaj;
                }

                if (ZadnjiPrint == true)
                {
                    cenaPrinta = cenaPrinta + DopZadaj;
                }

                return cenaPrinta;
            }
        }


        private string barva;
        private string velikost;

        public string Barva
        {
            get { return barva; }
            set
            {
                barva = value;
            }
        }

        public string Velikost
        {
            get { return velikost; }
            set
            {
                if (!Konstante.Velikosti.Contains(value)) Console.WriteLine("Napačna velikost");
                velikost = value;
            }
        }

        public bool SprednjiPrint { get; set; }
        public bool ZadnjiPrint { get; set; }

        public static readonly decimal DopSpredaj = 3.50m;
        public static readonly decimal DopZadaj = 4.00m;

        public KratkaMajica(string ime, decimal cena, string barva, string velikost, bool spredaj, bool zadaj) : base(ime, cena, "Kratka Majica")
        {
            Barva = barva;
            Velikost = velikost;
            SprednjiPrint = spredaj;
            ZadnjiPrint = zadaj;
        }

        public override decimal izracunCene()
        {
            decimal cena = OsnovnaCena;

            if (SprednjiPrint) cena += DopSpredaj;
            if (ZadnjiPrint) cena += DopZadaj;
            return cena;
        }
    }

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
                if (!Konstante.Debeline.Contains(value)) Console.WriteLine("Napačna debelina");
                debelina = value;
            }
        }

        public bool Zadrga { get; set; }

        public static readonly decimal DopZadrga = 5.00m;
        public static readonly decimal DopDebel = 6.00m;

        public Hoodie(string ime, decimal cena, string barva, string debelina, bool zadrga) : base(ime, cena, "Hoodie")
        {
            Barva = barva;
            Debelina = debelina;
            Zadrga = zadrga;
        }

        public override decimal izracunCene()
        {
            decimal cena = OsnovnaCena;

            if (Zadrga) cena += DopZadrga;
            if (Debelina == "Debela") cena += DopDebel;
            return cena;
        }

    }

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
                if (value.Length != 9) return;
                foreach (char c in value)
                {
                    if (!char.IsDigit(c))
                        return;
                }
                telSt = value;
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                if (!value.Contains("@")) Console.WriteLine("Napačen email");
                email = value;
            }
        }

        public Kupec(string ime, string priimek, string telst, string email)
        {
            Ime = ime;
            Priimek = priimek;
            TelSt = telst;
            Email = email;
        }

        public override string ToString()
        {
            return ($"Ime: {Ime}, priimek: {Priimek}, telefonska številka: {TelSt}, email: {Email}");
        }
    }

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
                if (value <= 0) Console.WriteLine("Količina mora biti več od 0");
                količina = value;
            }
        }

        public decimal KoncnaCena
        {
            get { return Artikel.izracunCene() * Količina; }
        }

        public NaročiloIzdelka(Artikel artikel, int količina)
        {
            Artikel = artikel;
            Količina = količina;
        }

        //Preoblaganje operatorjev da vidimo ali gre za isti izdelek ali ne (zato gledamo ID)
        //z return new ustvarim nov objekt da lahko količini seštejem

        //operator za ceno
        public static decimal operator +(NaročiloIzdelka a, NaročiloIzdelka b)
        {
            return a.KoncnaCena + b.KoncnaCena;
        }
        public static decimal operator +(decimal cena, NaročiloIzdelka n)
        {
            if (n is null) return cena;
            return cena + n.KoncnaCena;
        }

        public override string ToString()
        {
            return ($"{Artikel}, {Količina} in {KoncnaCena.ToString("0.00")}€");
        }

        ~NaročiloIzdelka()
        {
            Console.WriteLine($"Artikel {Artikel.IDIzdelka} je bil odstranjen");
        }
    }



    //indekser
    public class Naročilo
    {
        private List<NaročiloIzdelka> postavka = new List<NaročiloIzdelka>();

        public NaročiloIzdelka this[int i]
        {
            get { return postavka[i]; }
        }

        public int stPos
        {
            get { return postavka.Count; }
        }

        public void Dodaj(NaročiloIzdelka n)
        {
            postavka.Add(n);
        }

        public decimal SkupnaCena
        {
            get
            {
                decimal skupaj = 0m;
                foreach (NaročiloIzdelka n in postavka)
                    skupaj += n.KoncnaCena;

                return skupaj;
            }
        }
    }
}

