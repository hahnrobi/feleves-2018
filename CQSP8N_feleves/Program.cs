using System;

namespace CQSP8N_feleves
{
    class Program
    {

        static void Main(string[] args)
        {
            //=========== EZ A RÉSZ MINDENKÉPP SZÜKSÉGES A PROGRAM MŰKÖDÉSÉSHEZ ===========//
            //HallgatoKezeloModul példányosítása, innnen vezérelhető minden.
            HallgatoKezeloModul hk = new HallgatoKezeloModul();

            try
            {
                //Tankörök és hallgatók beolvasása, majd a megfelelő tankörhöz hozzáadása
                hk.ImportTankorok("tankorok.txt");
                hk.ImportHallgatok("hallgatok.txt");
            }catch (Exception e)
            {
                Console.WriteLine("Sikertelen volt a hallgatók és tankörök importálása");
                Console.WriteLine(e.Message);
            }
            Targy[] targyLista = null;
            try { 
                //Tantárgyak listája
                targyLista = HallgatoKezeloModul.ImportTargyak("tantargyak.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine("Sikertelen volt a tantárgyak importálása.");
                Console.WriteLine(e.Message);
            }
            //hk.TankorTantargyBeosztas(ref tka, targyLista);
            //=========== INNENTŐL MÁR TETSZÉS SZERINT HASZNÁLHATÓAK A DOLGOK ===========//


            //=========== FEATURE PROMO ===========//

            //Végigmegyünk a tankörökön én minden tankörbeli hallgató megkapja a tantárgyait
            foreach (Tankor tk in hk.Tankorok)
            {
                Tankor asd = tk;
                hk.TankorTantargyBeosztas(ref asd, targyLista);
            }

            //Kiiírjuk csak úgy a hallgatókat, hogy utána látszódjon, hogy működik a keresés. 
            foreach (Tankor tk in hk.Tankorok)
            {
                Console.WriteLine("{0} tankör hallgatói: (rendezési ID: {1})", tk.Azonosito, tk.getRendezoSzam());
                tk.Hallgatok.Kilistaz();
            }

            //Tetszőlegesen kiválasztunk egy hallgatót (jelen esetben ez az legelső tankör legelsőnek hallgatója), hogy később legyen egy hallgatónk aki kiíratkozhat.
            Hallgato rnd1 = hk.Tankorok.ElemItt(0).Hallgatok.ElemItt(0);

            //Tankörök rendezése a bennük található hallgatók neptun kódja szerint
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\n============= RENDEZÉS =============\n");
            Console.ResetColor();
            hk.TankorokRendezese(); //Tankör rendezés

            //Feliratkoztatunk minden hallgató kiiratkozott eseményére egy metódust, ami a tankörből fogja kitörölni és megvizsgálni, hogy üres lett-e e tankör
            foreach (Tankor tk in hk.Tankorok) 
                foreach (Hallgato h in tk.Hallgatok)
                    h.Kiiratkozott += tk.OnKiiratkozva;

            //Kiíratkoztatás a HallgatoKezeoModulon keresztül
            hk.Kiiratas(rnd1);
            //hk.Kiiratas(rnd2);
            //hk.Kiiratas(rnd3);

            //Végül kilistázzuk az összes hallgatót és a felvett tárgyait, immár sorba rendezett tankörökkel
            foreach (Tankor tk in hk.Tankorok)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("{0} tankör hallgatói: (rendezési ID: {1})\n", tk.Azonosito, tk.getRendezoSzam());
                Console.ResetColor();
                //tk.Hallgatok.Kilistaz();
                foreach (Hallgato h in tk.Hallgatok)
                {
                    Console.WriteLine(h);
                    h.Targyak.Kilistaz();
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

        }
    }
}
