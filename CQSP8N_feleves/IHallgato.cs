using System;
using System.Collections.Generic;
using System.Text;

namespace CQSP8N_feleves
{
    interface IHallgato
    {
        string Nev { get; }
        string NeptunKod { get; }

        int Fogyasztas(int ora);
        int Teljesitmeny();
        bool VanMegKapacitas();
        void Lefoglalas(Targy t);
        LancoltLista<Targy> Targyak { get; set; }

    }
}
