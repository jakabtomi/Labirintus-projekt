using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace labirintus_jatek
{
    internal class palya
    {
        public const int fel = 0;
        public const int jobb = 1;
        public const int le = 2;
        public const int bal = 3;

        public char[,] terkep;
        public int sorok;
        public int oszlopok;
        public string fajlnev;
        public List<pozicio> termek = new List<pozicio>();

        static int[] sorLepes = new int[] { -1, 0, 1, 0 };
        static int[] oszlopLepes = new int[] { 0, 1, 0, -1 };

        public palya(string fajlnev)
        {
            this.fajlnev = fajlnev;
            string[] fajlSorok = File.ReadAllLines(fajlnev, Encoding.UTF8);
            sorok = fajlSorok.Length;
            oszlopok = 0;

            for (int i = 0; i < fajlSorok.Length; i++)
            {
                if (fajlSorok[i].Length > oszlopok)
                {
                    oszlopok = fajlSorok[i].Length;
                }
            }

            terkep = new char[sorok, oszlopok];

            for (int i = 0; i < sorok; i++)
            {
                for (int j = 0; j < oszlopok; j++)
                {
                    if (j < fajlSorok[i].Length)
                    {
                        terkep[i, j] = fajlSorok[i][j];
                    }
                    else
                    {
                        terkep[i, j] = '.';
                    }

                    if (terkep[i, j] == '█')
                    {
                        termek.Add(new pozicio(i, j));
                    }
                }
            }
        }

        public static string fajlHelye(string fajlnev)
        {
            if (File.Exists(fajlnev))
            {
                return fajlnev;
            }

            string masikHely = Path.Combine(AppContext.BaseDirectory, fajlnev);

            if (File.Exists(masikHely))
            {
                return masikHely;
            }

            return fajlnev;
        }

        public bool palyanBelul(int sor, int oszlop)
        {
            return sor >= 0 && sor < sorok && oszlop >= 0 && oszlop < oszlopok;
        }

        public char karakter(int sor, int oszlop)
        {
            if (!palyanBelul(sor, oszlop))
            {
                return ' ';
            }

            return terkep[sor, oszlop];
        }

        public int ujSor(int sor, int irany)
        {
            return sor + sorLepes[irany];
        }

        public int ujOszlop(int oszlop, int irany)
        {
            return oszlop + oszlopLepes[irany];
        }

        public bool terem(int sor, int oszlop)
        {
            return palyanBelul(sor, oszlop) && terkep[sor, oszlop] == '█';
        }

        public bool jarat(char karakter)
        {
            return karakter == '║' || karakter == '═' || karakter == '╔' || karakter == '╗' ||
                   karakter == '╝' || karakter == '╚' || karakter == '╦' || karakter == '╩' ||
                   karakter == '╠' || karakter == '╣' || karakter == '╬';
        }

        public bool csatlakozik(char karakter, int irany)
        {
            if (karakter == '║')
            {
                return irany == fel || irany == le;
            }

            if (karakter == '═')
            {
                return irany == jobb || irany == bal;
            }

            if (karakter == '╔')
            {
                return irany == jobb || irany == le;
            }

            if (karakter == '╗')
            {
                return irany == bal || irany == le;
            }

            if (karakter == '╝')
            {
                return irany == fel || irany == bal;
            }

            if (karakter == '╚')
            {
                return irany == fel || irany == jobb;
            }

            if (karakter == '╦')
            {
                return irany == jobb || irany == le || irany == bal;
            }

            if (karakter == '╩')
            {
                return irany == fel || irany == jobb || irany == bal;
            }

            if (karakter == '╠')
            {
                return irany == fel || irany == jobb || irany == le;
            }

            if (karakter == '╣')
            {
                return irany == fel || irany == le || irany == bal;
            }

            if (karakter == '╬')
            {
                return true;
            }

            return false;
        }

        public int ellenkezo(int irany)
        {
            return (irany + 2) % 4;
        }

        public bool lehetLepni(int sor, int oszlop, int irany)
        {
            int kovSor = ujSor(sor, irany);
            int kovOszlop = ujOszlop(oszlop, irany);

            if (!palyanBelul(kovSor, kovOszlop))
            {
                return false;
            }

            char most = karakter(sor, oszlop);
            char kov = karakter(kovSor, kovOszlop);

            if (kov == '.')
            {
                return false;
            }

            if (most == '█')
            {
                return csatlakozik(kov, ellenkezo(irany));
            }

            if (kov == '█')
            {
                return csatlakozik(most, irany);
            }

            return csatlakozik(most, irany) && csatlakozik(kov, ellenkezo(irany));
        }

        public bool kijarat(int sor, int oszlop, int irany)
        {
            int kovSor = ujSor(sor, irany);
            int kovOszlop = ujOszlop(oszlop, irany);

            if (palyanBelul(kovSor, kovOszlop))
            {
                return false;
            }

            return csatlakozik(karakter(sor, oszlop), irany);
        }

        public List<int> lehetosegek(int sor, int oszlop)
        {
            List<int> lista = new List<int>();

            for (int i = 0; i < 4; i++)
            {
                if (lehetLepni(sor, oszlop, i) || kijarat(sor, oszlop, i))
                {
                    lista.Add(i);
                }
            }

            return lista;
        }

        public pozicio kezdohely()
        {
            for (int i = 0; i < sorok; i++)
            {
                for (int j = 0; j < oszlopok; j++)
                {
                    if (bejarat(i, j))
                    {
                        return new pozicio(i, j);
                    }
                }
            }

            return null;
        }

        public int kijaratokSzama()
        {
            int darab = 0;

            for (int i = 0; i < sorok; i++)
            {
                for (int j = 0; j < oszlopok; j++)
                {
                    if (bejarat(i, j))
                    {
                        darab++;
                    }
                }
            }

            return darab;
        }

        bool bejarat(int sor, int oszlop)
        {
            char karakter = this.karakter(sor, oszlop);

            if (!jarat(karakter))
            {
                return false;
            }

            if (sor == 0 && csatlakozik(karakter, fel))
            {
                return true;
            }

            if (sor == sorok - 1 && csatlakozik(karakter, le))
            {
                return true;
            }

            if (oszlop == 0 && csatlakozik(karakter, bal))
            {
                return true;
            }

            if (oszlop == oszlopok - 1 && csatlakozik(karakter, jobb))
            {
                return true;
            }

            return false;
        }
    }
}
