using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CQSP8N_feleves
{
    class HallgatoKezeloModul
    {
        static Random r = new Random(); //Random példány a NeptunKód generáláshoz
        public LancoltLista<Tankor> Tankorok { get; set; }

        public HallgatoKezeloModul()
        {
            Tankorok = new LancoltLista<Tankor>();
        }
        public static Targy[] ImportTargyak(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            int mennyiseg = 0;
            while(!sr.EndOfStream)
            {
                sr.ReadLine();
                mennyiseg++;
            } //Tömbben való tároláshoz a tantárgyak száma
            sr = new StreamReader(filename);
            Targy[] targyak = new Targy[mennyiseg];

            mennyiseg = 0;
            while (!sr.EndOfStream)
            {//És felvétel a tömbbe
                string[] sor = sr.ReadLine().Split(',');
                targyak[mennyiseg++] = new Targy() { Megnevezes = sor[0], Kreditertek = int.Parse(sor[1]), Nehezseg = int.Parse(sor[2]) };
            }
            return targyak;
        }


        public void Kiiratas(Hallgato hallgato)
        {//Megkeressük a hallgatót és elsütjük az eseményt
            foreach (Tankor tk in Tankorok)
            {
                foreach (Hallgato  h in tk.Hallgatok)
                {
                    if(h == hallgato)
                    {
                        h.OnKiiratkozva();
                        UresTankorEllenorzese(tk);
                    }
                }
            }
        }
        public void TankorokRendezese()
        {
            //Tankörök bepakolása egy rendezett láncolt listába, majd a rendezett láncolt lista átmásolása egy normál láncolt listába.
            LancoltLista<int, Tankor> rendezettTankorok = new LancoltLista<int, Tankor>();
            foreach (Tankor tk in Tankorok)
            {
                rendezettTankorok.Hozzaad(tk.getRendezoSzam(), tk);
            }
            LancoltLista<Tankor> UjTankorok = new LancoltLista<Tankor>();
            foreach (Tankor tk in rendezettTankorok)
            {
                UjTankorok.HozzaadVegere(tk);
            }
            Tankorok = UjTankorok;
        }
        public void TankorTantargyBeosztas(ref Tankor tk, Targy[] orak)
        {
            //BTS

            Hallgato[] Elemek = new Hallgato[tk.Hallgatok.Hossz]; //HALLGATÓK

            int szintek = tk.Hallgatok.Hossz - 1; //HALLGATÓK SZÁMA

            bool van = false; //MEGOLDÁS VAN? egyenlőre nincs

            LancoltLista<int, Hallgato> HallgatokRendezett = new LancoltLista<int, Hallgato>();

            foreach (Hallgato h in tk.Hallgatok)
                HallgatokRendezett.Hozzaad(h.Teljesitmeny(), h); //Teljesítményük szerint bepakoljuk az embereket egy listába

            int i = 0;
            foreach (Hallgato h in HallgatokRendezett)
            {
                Elemek[i++] = h; //Hallgatók felvétele az Elemek tömbbe.
            }

            int teljesitmeny_atlag = 1000; //1000-nél kisebb a teljesítmény, ezért fentről haladunk lefelé

            BTS(0, ref van, szintek, ref Elemek, ref teljesitmeny_atlag);

            //BTS után felvehetjük a megfelelő mennyiségű órát
            for (int j = 0; j < Elemek.Length; j++)
            {
                for (int k = 0; k < orak.Length; k++)
                {
                    if(Elemek[j].OrakSzum() < teljesitmeny_atlag)
                    {
                        Elemek[j].Lefoglalas(orak[k]);
                    }
                }
            }

            //Console.WriteLine(teljesitmeny_atlag); //JUST DEBUG
        }
        private static int Ft(Hallgato h, int atlag)
        {
            if (!h.VanMegKapacitas())
            {
                return -1;
            }
            else
            {
                if (h.Teljesitmeny() * 0.95 <= atlag)
                {
                    return -1;
                }
                else if (h.Teljesitmeny() * 0.25 >= atlag)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
        private static bool Fk(Hallgato[] h, int atlag, ref int valtoztatas)
        {
            bool jo = true;
            for (int i = 0; i < h.Length; i++)
            {
                int Ft_ = Ft(h[i], atlag);
                if (Ft_ != 0)
                {
                    jo = false;
                    valtoztatas = Ft_;
                    //Console.WriteLine(h[i] + " nem jó"); //DEBUG
                }
            }
            return jo;
        }
        private static void BTS(int szint, ref bool van, int szintek, ref Hallgato[] Elemek, ref int atlag)
        {
            int valtoztatas = 0;
                while (!van)
                {
                    int Ft_ = Ft(Elemek[szint], atlag);
                    if (Ft_ == 0)
                    {
                        if (Fk(Elemek, atlag, ref valtoztatas))
                        {
                            if (szint == szintek)
                                van = true;
                            else
                                BTS(szint + 1, ref van, szintek, ref Elemek, ref atlag);
                        }
                    }
                    else
                    valtoztatas = Ft_;
                //Console.WriteLine(atlag + " " + szint); //DEBUG
                atlag = atlag + valtoztatas;
                valtoztatas = 0;
                }
        }
        public LancoltLista<Hallgato> ImportHallgatok(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            LancoltLista<Hallgato> hallgatok = new LancoltLista<Hallgato>();
            while (!sr.EndOfStream)
            {
                string[] sor = sr.ReadLine().Split(',');
                
                for (int i = 0; i < this.Tankorok.Hossz; i++)
                {
                    //Hallgató importálásakor, a fájlban megadott azonosítójú Tankörhöz felvesszük
                    if (this.Tankorok.ElemItt(i).Azonosito == int.Parse(sor[5])) 
                    {
                        Hallgato hallgato = new Hallgato(sor[0], sor[1], sor[2], int.Parse(sor[3]), int.Parse(sor[4]));
                        this.Tankorok.ElemItt(i).Hallgatok.Hozzaad(hallgato.GetKulcs(), hallgato);
                    }
                }
                
            }
            return hallgatok;
        }
        public void ImportTankorok(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            LancoltLista<Tankor> tankorok = new LancoltLista<Tankor>();
            while (!sr.EndOfStream)
            {
                Tankorok.HozzaadVegere(new Tankor() { Azonosito = int.Parse(sr.ReadLine()) });
            }
        }
        private void UresTankorEllenorzese(Tankor tk)
        {
            if(tk.Hallgatok.Hossz == 0)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(tk.Azonosito + ". tankör üressé vált, ezért törlésre került.\n");
                Console.ResetColor();
                this.Tankorok.Torles(tk);
            }
        }
        public static string IDGen(Nem n)
        {
            int szamertek = r.Next(100, 1000);
            char[] betuk = "QWERTZUIOPASDFGHJKLYXCVBNM".ToCharArray();
            string nem = "";
            if (n == Nem.Nő)
                nem = "N";
            else
                nem = "F";
            return nem + "-" + szamertek + betuk[r.Next(0, betuk.Length)] + betuk[r.Next(0, betuk.Length)] + betuk[r.Next(0, betuk.Length)];
        }
    }
}
