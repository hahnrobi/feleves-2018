using System;
using System.Collections.Generic;
using System.Text;

namespace CQSP8N_feleves
{
    public enum Nem
    {
        Nő,
        Férfi
    }
    class Hallgato : HallgatoKezeloModul, IHallgato, IComparable
    {
        private int teljesitmeny;
        private int fogyasztas;

        public delegate void KiiratkozottEventHandler(object obj, EventArgs args);
        public event KiiratkozottEventHandler Kiiratkozott;
        public string Nev { get; }
        public string NeptunKod { get; }
        public DateTime SzuletesiEv { get; }
        public Nem Nem { get; }
        public LancoltLista<Targy> Targyak { get; set; }


        //CTOR
        public Hallgato(string nev, string nem, string szuletesiEv, int fogyasztas, int teljesitmeny)
        {
            this.Nev = nev;
            if (nem == "férfi")
                this.Nem = Nem.Férfi;
            else
                this.Nem = Nem.Nő;
            NeptunKod = HallgatoKezeloModul.IDGen(Nem);
            this.SzuletesiEv = DateTime.Parse(szuletesiEv);
            this.fogyasztas = fogyasztas;
            this.teljesitmeny = teljesitmeny;
            Targyak = new LancoltLista<Targy>();
        }


        //METÓDUSOK
        public virtual void OnKiiratkozva()
        {
            if(Kiiratkozott != null)
            {
                Kiiratkozott(this, EventArgs.Empty);
            }
        }
        public int GetKulcs()
        {
            return int.Parse(NeptunKod[2].ToString() + NeptunKod[3].ToString() + NeptunKod[4].ToString());
        }
        public int Fogyasztas(int ora)
        {
            return ora * (int)fogyasztas;
        }

        public int Teljesitmeny()
        {
            return teljesitmeny;
        }

        public bool VanMegKapacitas()
        {
            int leterheltseg = 0;
            try
            {
                foreach (Targy targy in Targyak)
                {
                    leterheltseg += targy.Nehezseg;
                }
            }
            catch (Exception)
            {

            }
            if(leterheltseg > teljesitmeny)
                return false;
            else
               return true;
        }
        public void Lefoglalas(Targy t)
        {
           if(VanMegKapacitas())
            {
                this.Targyak.HozzaadElejere(t);
            }
        }
        public int OrakSzum()
        {
            int szum = 0;
            try
            {
                foreach (Targy ora in Targyak)
                {
                    szum += ora.Nehezseg;
                }
            }
            catch (Exception)
            {

            }
            return szum;
        }
        public override string ToString()
        {
            string kap = "";
            if(VanMegKapacitas())
            {
                kap = "Igen";
            }else
            {
                kap = "Nem";
            }
            return string.Format("> {0}, {1}\tTelj: {2}\tFogy: {4}\tTerhelhető?: {3}", Nev, NeptunKod, teljesitmeny, kap, Fogyasztas(1));
        }

        public int CompareTo(object obj)
        {
            if((obj as Hallgato).NeptunKod == this.NeptunKod)
            {
                return 0;
            }
            else { return -1; }
        }
    }
}
