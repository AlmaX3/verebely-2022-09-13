using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceShuttle {
    class Kuldetes
    {
        private string KuldetesKod;
        private DateOnly KilovesDatuma;
        private string UrsikloNev;
        private int HanyNap;
        private int HanyOra;
        private string LegiTamaszpontLandolasNev;
        private int Legenyseg;

        public Kuldetes(string _kuldetesKod, DateOnly _kilovesDatum, string _ursikloNev, int _hanyNap, int _hanyOra, string _legitamaszpontLandolasNeve, int _legenyseg)
        {
            KuldetesKod = _kuldetesKod;
            KilovesDatuma = _kilovesDatum;
            UrsikloNev = _ursikloNev;
            HanyNap = _hanyNap;
            HanyOra = _hanyOra;
            LegiTamaszpontLandolasNev = _legitamaszpontLandolasNeve;
            Legenyseg = _legenyseg;
        }

        public string getKuldetesKod() {
            return KuldetesKod;
        }

        public DateOnly getKilovesDatuma() { 
            return KilovesDatuma; 
        }

        public string getUrsikloNev() {
            return UrsikloNev;
        }
        public int getHanyNap()
        {
            return HanyNap;
        }
        public int getHanyOra()
        {
            return HanyOra;
        }
        public string getLegiTamaszpontLandolas()
        {
            return LegiTamaszpontLandolasNev;
        }
        public int getLegenyseg()
        {
            return Legenyseg;
        }

    }

    class Program {

        static List<Kuldetes> KuldetesLista = new List<Kuldetes>();
        static void Betoltes() {
            Console.WriteLine("2. Feladat: Betöltés");
            FileStream fs = new FileStream("kuldetesek.csv", FileMode.Open);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            
            while (!sr.EndOfStream)
            {
                string[] line = sr.ReadLine().Split(";");
                Kuldetes kuldetes = new Kuldetes(line[0], DateOnly.Parse(line[1]), line[2], int.Parse(line[3]), int.Parse(line[4]), line[5], int.Parse(line[6]));
                KuldetesLista.Add(kuldetes);
            }

            sr.Close();
            fs.Close();
        }
        private static void KuldetesSzama()
        {
            Console.WriteLine($"3. Feladat: Összesen {KuldetesLista.Count} alkalommal indítottak űrhajót.");
        }
        private static void OsszUtasokSzama()
        {
            Console.WriteLine($"4. Feladat: {KuldetesLista.Sum(x => x.getLegenyseg())} utas indult az űrbe összesen");
        }

        private static void OsszKevesebbMint5Utas()
        {
            Console.WriteLine($"5. Feladat: Összesen {KuldetesLista.Count(x =>x.getLegenyseg() < 5)} alkalommal küldtek kevesebb, mint 5 embert az űrbe");
        }
        private static void ColumbiaUtolsoLegenyege()
        {
            Kuldetes Columbia = KuldetesLista.Find(x => x.getLegiTamaszpontLandolas() == "nem landolt" && x.getUrsikloNev() == "Columbia");
            Console.WriteLine($"6. Feladat: {Columbia.getLegenyseg()} asztronauta volt Columbia fedélzetén annak utolsó útján");
        }
        private static void LeghosszabbKuldetes()
        {
            Kuldetes Leghosszabb = KuldetesLista.Find(x => x.getHanyOra() + x.getHanyNap()*24 == KuldetesLista.Max(x => x.getHanyNap() * 24 + x.getHanyOra()));
            Console.WriteLine($"7. Feladat: Leghosszabb ideig a {Leghosszabb.getUrsikloNev()} volt az űrben a {Leghosszabb.getKuldetesKod()} küldetés során.\nÖsszesen {Leghosszabb.getHanyNap()*24+Leghosszabb.getHanyOra()} órát volt távol a Földtől");
        }
        private static void EvbeliLekerdezes()
        {
            Console.Write("8. Feladat: Kérem adjon meg egy évszámot: ");
            int Evszam = int.Parse(Console.ReadLine());
            int KuldetesSzam = KuldetesLista.Count(x => x.getKilovesDatuma().Year == Evszam);
            if(KuldetesSzam > 0)
            {
                Console.WriteLine($"Ebben az évben {KuldetesSzam} küldetés volt.");
            } else
            {
                Console.WriteLine("Ebben az évben nem volt küldetés");
            }
        }
        private static void LandolasokHanySzazalekaKennedy()
        {
            double Szazalek = ((double)KuldetesLista.Count(x => x.getLegiTamaszpontLandolas() == "Kennedy") / (double)KuldetesLista.Count()) *100;
            Console.WriteLine($"A küldetések {Szazalek:0.00}%-a fejeződött be a Kennedy űrközpontban");
        }
        private static void OsszNapKulsoFajlba()
        {
            Dictionary<string, double> OsszNapok = new Dictionary<string, double>();
            KuldetesLista.ForEach(x => {
                if (!OsszNapok.ContainsKey(x.getUrsikloNev())) {
                    OsszNapok.Add(x.getUrsikloNev(), (double)(x.getHanyNap() + ((double)x.getHanyOra() / 24)));
                } else
                {
                    OsszNapok[x.getUrsikloNev()] = OsszNapok[x.getUrsikloNev()] + (double)(x.getHanyNap() + ((double)x.getHanyOra() / 24));
                }
            });
            FileStream fs = new FileStream("ursiklok.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);

            foreach (var x in OsszNapok)
            {
                sw.WriteLine($"{x.Key} {x.Value:0.00}");                
            }

            sw.Close();
            fs.Close();
        }

        static void Main() {
            Betoltes();
            KuldetesSzama();
            OsszUtasokSzama();
            OsszKevesebbMint5Utas();
            ColumbiaUtolsoLegenyege();
            LeghosszabbKuldetes();
            EvbeliLekerdezes();
            LandolasokHanySzazalekaKennedy();
            OsszNapKulsoFajlba();
        }

    }
}