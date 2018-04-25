using System;
using System.Collections.Generic;
using System.Text;

namespace CQSP8N_feleves
{
    sealed class Targy : IComparable
    {
        public string Megnevezes { get; set; }
        public int Kreditertek { get; set; }
        public int Nehezseg { get; set; }

        public override string ToString()
        {
            return string.Format("> {0}\tKreditérték: {1}", Megnevezes, Kreditertek);
        }

        public int CompareTo(object obj)
        {
            if ((obj as Targy).Megnevezes == this.Megnevezes)
                return 0;
            else
            return -1;
        }
    }
}
