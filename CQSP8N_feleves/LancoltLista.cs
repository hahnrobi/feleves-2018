using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CQSP8N_feleves
{

    class LancoltLista<T> : IEnumerable<T>, IEnumerator<T> where T : IComparable
    {
        class ListaElem
        {
            public ListaElem kovetkezo { get; set; } //Link a következő elemre
            public T tartalom { get; set; } //T tartalom
        }

        private int hossz; //Lista elemeinek száma
        public int Hossz { get { return hossz; } } //publikus tag

        private ListaElem fej; // FEJ //

        private ListaElem bejaroMutato; //IENUMERABLE

        public LancoltLista()
        {
            hossz = 0;
        }

        //=============================
        //  HOZZÁADÁS METÓDUSOK ELEJE
        //=============================
        /// <summary> 
        /// Lista elejére szúr be elemet
        /// </summary>
        public void HozzaadElejere(T tartalom)
        {
            ListaElem ez = new ListaElem() { kovetkezo = fej, tartalom = tartalom }; //Új elem létrehozása, következő elemhez a fej
            fej = ez; //Fej lecserélése az új elemmel, hogy ez legyen az első
            hossz++;
        }
        /// <summary> 
        /// Lista végére szúr be elemet
        /// </summary>
        public void HozzaadVegere(T tartalom)
        {
            ListaElem uj = new ListaElem() { kovetkezo = null, tartalom = tartalom }; //Hozzáadni kívánt listaelem
            if (fej != null)
            {
                ListaElem aktualis = fej; //Éppen vizsgált listaelem. Elsőnek a fej-t kapja meg
                while (aktualis.kovetkezo != null) //Amíg nincs egy olyan elem a listában, aminél a következő elem címe null
                    aktualis = aktualis.kovetkezo; //Lépegetés
                aktualis.kovetkezo = uj; //Végéhez hozzáfűzés
            }
            else
            {
                fej = uj;
            }
            hossz++;
        }

        //=============================
        //  HOZZÁADÁS METÓDUSOK VÉGE
        //=============================


        //  TÖRLÉS
        //=============================
        /// <summary> 
        /// Lista tartalmának megfelelő elemet átadva, azt az elemet kitörli a listából
        /// </summary>
        public void Torles(T tartalom)
        {
            if (fej != null)
            {
                ListaElem aktualis = fej;
                ListaElem elozo = null;

                while (aktualis != null && (aktualis.tartalom as IComparable).CompareTo(tartalom) != 0)
                {
                    elozo = aktualis;
                    aktualis = aktualis.kovetkezo;
                }
                if (aktualis != null)
                {
                    if (elozo == null)
                    {
                        fej = aktualis.kovetkezo;
                    }
                    else
                    {
                        elozo.kovetkezo = aktualis.kovetkezo;
                    }
                    aktualis = null;
                    hossz--;
                }
            }
        }
        /// <summary> 
        /// Grafikusan kilistázza a lista elemeit a konzolra
        /// </summary>
        public void Kilistaz()
        {
            ListaElem ez = fej;
            while (ez != null)
            {
                Console.WriteLine(" >" + ez.tartalom);
                ez = ez.kovetkezo;
            }
        }
        /// <summary> 
        /// Lista N-edik elemét adja vissza. !Indexelés 0-tól!
        /// </summary>
        public T ElemItt(int n)
        {
            if(n>=this.Hossz)
            {
                throw new ListaKiindexelesException() { Msg = "Nincs ennyi elem a listában" };
            }
            int szamlalo = 0;
            ListaElem vizsgalt = fej;
            while (szamlalo != n) //Amíg nincs meg
            {
                szamlalo++;
                if (vizsgalt.kovetkezo != null) //Hogy megálljunk, ha nincs már több elem
                {
                    vizsgalt = vizsgalt.kovetkezo; //Lépegetés
                }
            }
            return vizsgalt.tartalom;
        }
        #region IENUMERABLE
        //=============================
        //       IENUMERABLE
        //=============================
        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)this;
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region IENUMERATOR
        //=============================
        //       IENUMERATOR
        //=============================
        public T Current { get { return bejaroMutato.tartalom; } }
        public void Dispose() { }
        object System.Collections.IEnumerator.Current
        {
            get { throw new NotImplementedException(); }
        }
        public bool MoveNext() //VÉGIGLÉPEGETÉS
        {
            if (bejaroMutato == null)
            {
                //KEZDÉS
                bejaroMutato = fej;
                return true;
            }
            else if (bejaroMutato.kovetkezo != null)
            {
                //N.
                bejaroMutato = bejaroMutato.kovetkezo;
                return true;
            }
            else
            {
                //VÉGE
                this.Reset();
                return false;
            }
        }
        public void Reset()
        {
            bejaroMutato = null;
        }
        #endregion
    }

    //==========================================================
    //              LÁNCOLT LISTA KULCCSAL
    //==========================================================

    class LancoltLista<K, T> : IEnumerator<T>, IEnumerable<T> where K : IComparable
    {
        class ListaElem
        {
            public ListaElem kovetkezo { get; set; } //Link a következő listaelemre
            public T tartalom { get; set; } //T tartalom
            public K kulcs { get; set; } // K Comparable kulcs
        }
        private int hossz; //Listában lévő elemek száma
        public int Hossz { get { return hossz; } } // publikus csak írható tag

        private ListaElem fej; // FEJ //

        ListaElem bejaroMutato; //IENUMERABLE


        public LancoltLista()
        {
            hossz = 0; //Az üres lista 0 elemű
        }
        public void Torles(T tartalom)
        {
            if (fej != null)
            {
                ListaElem aktualis = fej;
                ListaElem elozo = null;
                
                while (aktualis != null && (aktualis.tartalom as IComparable).CompareTo(tartalom) != 0)
                {
                    elozo = aktualis;
                    aktualis = aktualis.kovetkezo;
                }
                if(aktualis != null)
                {
                    if(elozo == null)
                    {
                        fej = aktualis.kovetkezo;
                    }else
                    {
                        elozo.kovetkezo = aktualis.kovetkezo;
                    }
                    aktualis = null;
                    hossz--;
                }
            }
        }
        /// <summary> 
        /// Rendezett listához hozzáadás K kulccsal és T tartalommal
        /// </summary>
        public void Hozzaad(K kulcs, T tartalom)
        {
            hossz++; //Új elemünk lesz.
            ListaElem uj = new ListaElem() { kulcs = kulcs, tartalom = tartalom, kovetkezo = null };
            if (fej == null) // Ha a fej üres
            {
                uj.kovetkezo = null;
                fej = uj; //FONTOS A SORREND!!!
            }
            else
            {
                if (fej.kulcs.CompareTo(kulcs) > 0) //Ha a hozzáadandó kisebb, mint a legelső
                {
                    uj.kovetkezo = fej;
                    fej = uj; //FONTOS A SORREND!!!
                }
                else
                {
                    ListaElem p = fej;
                    ListaElem e = null;
                    while (p != null && p.kulcs.CompareTo(kulcs) < 0) //Végiglépegetünk
                    {
                        e = p;
                        p = p.kovetkezo;
                    }
                    if (p == null)
                    {
                        uj.kovetkezo = null; //Ha a végére értünk és a hozzáadandó a legnagyobb
                        e.kovetkezo = uj;
                    }
                    else
                    {
                        uj.kovetkezo = p;
                        e.kovetkezo = uj;
                    }
                }
            }
        }

        /// <summary> 
        /// Grafikusan megjeleníti a lista elemeit a konzolon.
        /// </summary>
        public void Kilistaz()
        {
            if (fej == null)
            {
                throw new UresListaException() { Msg = "A kilistázni kívánt láncolt lista üres." };
            }
            ListaElem ez = fej;
            while (ez != null)
            {
                Console.WriteLine(" >" + ez.tartalom);
                ez = ez.kovetkezo;
            }
        }
        /// <summary> 
        /// A lista n-edik elemét adja vissza. !Indexelés 0-tól!
        /// </summary>
        public T ElemItt(int n)
        {
            int szamlalo = 0;
            ListaElem vizsgalt = fej;
            while (szamlalo != n)
            {
                szamlalo++;
                if (vizsgalt.kovetkezo != null)
                {
                    vizsgalt = vizsgalt.kovetkezo;
                }
                else
                {
                }
            }
            return vizsgalt.tartalom;
        }
        #region IENUMERABLE
        //=============================
        //       IENUMERABLE
        //=============================
        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)this;
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region IENUMERATOR
        //=============================
        //       IENUMERATOR
        //=============================
        public T Current { get {
                    return bejaroMutato.tartalom;
            } }
        public void Dispose() { }
        object System.Collections.IEnumerator.Current
        {
            get { throw new NotImplementedException(); }
        }
        public bool MoveNext() //VÉGIGLÉPEGETÉS
        {
            if (bejaroMutato == null)
            {
                //KEZDÉS
                bejaroMutato = fej;
                if(fej == null)
                {
                    this.Reset();
                    return false;
                }
                return true;
            }
            else if (bejaroMutato.kovetkezo != null)
            {
                //N.
                bejaroMutato = bejaroMutato.kovetkezo;
                return true;
            }
            else
            {
                //VÉGE
                this.Reset();
                return false;
            }
        }
        public void Reset()
        {
            bejaroMutato = null;
        }
        #endregion
    }

}
