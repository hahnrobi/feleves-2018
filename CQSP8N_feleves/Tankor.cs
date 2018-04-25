using System;
using System.Collections.Generic;
using System.Text;

namespace CQSP8N_feleves
{
    class Tankor : IComparable
    {
        public int Azonosito { get; set; }
        public LancoltLista<int, Hallgato> Hallgatok { get; set; }

        public Tankor()
        {
            Hallgatok = new LancoltLista<int, Hallgato>();
        }
        public int getRendezoSzam()
        {
            if(Hallgatok.Hossz >= 4)
            {
                return Hallgatok.ElemItt(4).GetKulcs();
            }else
            {
                return Hallgatok.ElemItt(Hallgatok.Hossz - 1).GetKulcs();
            }
        }
        public void OnKiiratkozva(object obj, EventArgs args)
        {
            this.Hallgatok.Torles(obj as Hallgato);
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.WriteLine((obj as Hallgato).Nev + " kiiratkozott");
            Console.ResetColor();
        }

        public int CompareTo(object obj)
        {
            if((obj as Tankor).Azonosito == this.Azonosito)
            {
                return 0;
            }else
            {
                return -1;
            }
        }
    }
}
