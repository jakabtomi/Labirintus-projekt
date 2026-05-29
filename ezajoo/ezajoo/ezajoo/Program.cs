using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace ezajoo
{
    class program
    {
        static char[,] palya;
        static bool[,] bejart;
        static bool[,] megvan;

        static int sorok;
        static int oszlopok;
        static int jatekosSor;
        static int jatekosOszlop;
        static int termekSzama;
        static int megtalaltTermek;

        static bool magyar = true;
        static bool fedett = false;
        static string palyaNev = "minta.txt";
        static string mentesNev = "minta.SAV";
        static DateTime vegeIdo;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;

            nyelvValasztas();
            palyaBekeres();

            palyaNev = fajlMegkeresese(palyaNev);

            if (!File.Exists(palyaNev))
            {
                Console.WriteLine(szoveg("nincs ilyen pályafájl", "map file not found"));
                Console.ReadKey(true);
                return;
            }

            mentesNev = Path.ChangeExtension(palyaNev, ".SAV");
            palyaBetoltes();
            ujJatek();

            if (File.Exists(mentesNev) && igenNem(szoveg("van mentés, betöltöd?", "save found, load it?")))
            {
                mentesBetoltes();
            }
            else
            {
                fedett = igenNem(szoveg("fedett térkép legyen?", "hidden map?"));
                vegeIdo = DateTime.Now.AddSeconds(idoBekeres());
            }

            jatek();
        }

        static void nyelvValasztas()
        {
            Console.Write("nyelv / language (m/e): ");
            string valasz = Console.ReadLine();

            if (valasz != null && valasz.ToLower() == "e")
            {
                magyar = false;
            }
        }

        static void palyaBekeres()
        {
            Console.Write(szoveg("pályafájl neve (enter = minta.txt): ", "map file name (enter = minta.txt): "));
            string valasz = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(valasz))
            {
                palyaNev = valasz.Trim();
            }
        }

        static string fajlMegkeresese(string fajlNev)
        {
            if (File.Exists(fajlNev))
            {
                return fajlNev;
            }

            string futtatasHelye = AppContext.BaseDirectory;
            string exeMappa = Path.Combine(futtatasHelye, fajlNev);

            if (File.Exists(exeMappa))
            {
                return exeMappa;
            }

            string projektMappa = Path.GetFullPath(Path.Combine(futtatasHelye, "..", "..", "..", fajlNev));

            if (File.Exists(projektMappa))
            {
                return projektMappa;
            }

            return fajlNev;
        }

        static int idoBekeres()
        {
            Console.Write(szoveg("idő másodpercben (enter = 180): ", "time in seconds (enter = 180): "));
            string valasz = Console.ReadLine();
            int ido;

            if (int.TryParse(valasz, out ido) && ido > 0)
            {
                return ido;
            }

            return 180;
        }

        // a pályát betöltöm egy kétdimenziós tömbbe
        static void palyaBetoltes()
        {
            string[] fajlSorai = File.ReadAllLines(palyaNev, Encoding.UTF8);
            sorok = fajlSorai.Length;
            oszlopok = 0;

            for (int i = 0; i < fajlSorai.Length; i++)
            {
                if (fajlSorai[i].Length > oszlopok)
                {
                    oszlopok = fajlSorai[i].Length;
                }
            }

            palya = new char[sorok, oszlopok];

            for (int s = 0; s < sorok; s++)
            {
                for (int o = 0; o < oszlopok; o++)
                {
                    if (o < fajlSorai[s].Length)
                    {
                        palya[s, o] = fajlSorai[s][o];
                    }
                    else
                    {
                        palya[s, o] = '.';
                    }
                }
            }
        }

        static void ujJatek()
        {
            bejart = new bool[sorok, oszlopok];
            megvan = new bool[sorok, oszlopok];
            termekSzama = 0;
            megtalaltTermek = 0;

            for (int s = 0; s < sorok; s++)
            {
                for (int o = 0; o < oszlopok; o++)
                {
                    if (palya[s, o] == '█')
                    {
                        termekSzama++;
                    }
                }
            }

            kezdoPontKereses();
            bejart[jatekosSor, jatekosOszlop] = true;
        }

        // az első kijárattól indul a játékos
        static void kezdoPontKereses()
        {
            for (int s = 0; s < sorok; s++)
            {
                for (int o = 0; o < oszlopok; o++)
                {
                    if (kijarat(s, o))
                    {
                        jatekosSor = s;
                        jatekosOszlop = o;
                        return;
                    }
                }
            }
        }

        static void jatek()
        {
            bool fut = true;

            while (fut)
            {
                if (DateTime.Now >= vegeIdo)
                {
                    rajzol();
                    Console.WriteLine(szoveg("lejárt az idő, bennragadtál", "time is over, you are trapped"));
                    break;
                }

                rajzol();

                while (!Console.KeyAvailable && DateTime.Now < vegeIdo)
                {
                    Thread.Sleep(200);
                    rajzol();
                }

                if (DateTime.Now >= vegeIdo)
                {
                    continue;
                }

                ConsoleKeyInfo gomb = Console.ReadKey(true);
                char betu = char.ToLower(gomb.KeyChar);

                if (gomb.Key == ConsoleKey.Escape)
                {
                    fut = false;
                }
                else if (betu == 'm')
                {
                    mentes();
                }
                else if (betu == 'w')
                {
                    fut = lepes('f');
                }
                else if (betu == 's')
                {
                    fut = lepes('l');
                }
                else if (betu == 'a')
                {
                    fut = lepes('b');
                }
                else if (betu == 'd')
                {
                    fut = lepes('j');
                }
            }

            Console.WriteLine(szoveg("nyomj egy gombot a kilépéshez", "press a key to exit"));
            Console.ReadKey(true);
        }

        static void rajzol()
        {
            Console.Clear();

            for (int s = 0; s < sorok; s++)
            {
                for (int o = 0; o < oszlopok; o++)
                {
                    if (s == jatekosSor && o == jatekosOszlop)
                    {
                        Console.Write('@');
                    }
                    else if (fedett && !bejart[s, o])
                    {
                        Console.Write(' ');
                    }
                    else
                    {
                        Console.Write(palya[s, o]);
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine(szoveg("termek: ", "rooms: ") + megtalaltTermek + "/" + termekSzama);
            Console.WriteLine(szoveg("idő: ", "time: ") + hatralevoIdo() + " mp");
            Console.WriteLine(szoveg("mehetsz: ", "you can go: ") + lehetosegek());
            Console.WriteLine(szoveg("w/a/s/d mozgás, m mentés, esc kilépés", "w/a/s/d move, m save, esc quit"));
        }

        static int hatralevoIdo()
        {
            int ido = (int)(vegeIdo - DateTime.Now).TotalSeconds;

            if (ido < 0)
            {
                return 0;
            }

            return ido;
        }

        static string lehetosegek()
        {
            string szoveg = "";

            if (lehetLepni('f') || kiLehetMenni('f')) szoveg += "W ";
            if (lehetLepni('b') || kiLehetMenni('b')) szoveg += "A ";
            if (lehetLepni('l') || kiLehetMenni('l')) szoveg += "S ";
            if (lehetLepni('j') || kiLehetMenni('j')) szoveg += "D ";

            if (szoveg == "")
            {
                return "-";
            }

            return szoveg;
        }

        // egy lépésnél először csak ellenőrzök
        static bool lepes(char irany)
        {
            int ujSor = jatekosSor;
            int ujOszlop = jatekosOszlop;

            if (irany == 'f') ujSor--;
            if (irany == 'l') ujSor++;
            if (irany == 'b') ujOszlop--;
            if (irany == 'j') ujOszlop++;

            if (ujSor < 0 || ujSor >= sorok || ujOszlop < 0 || ujOszlop >= oszlopok)
            {
                return kilepes(irany);
            }

            if (lehetLepni(irany))
            {
                jatekosSor = ujSor;
                jatekosOszlop = ujOszlop;
                bejart[jatekosSor, jatekosOszlop] = true;
                teremNezes();
            }

            return true;
        }

        static bool lehetLepni(char irany)
        {
            int ujSor = jatekosSor;
            int ujOszlop = jatekosOszlop;

            if (irany == 'f') ujSor--;
            if (irany == 'l') ujSor++;
            if (irany == 'b') ujOszlop--;
            if (irany == 'j') ujOszlop++;

            if (ujSor < 0 || ujSor >= sorok || ujOszlop < 0 || ujOszlop >= oszlopok)
            {
                return false;
            }

            char innen = palya[jatekosSor, jatekosOszlop];
            char oda = palya[ujSor, ujOszlop];

            return vanUt(innen, irany) && vanUt(oda, ellentetes(irany));
        }

        static bool vanUt(char jel, char irany)
        {
            if (jel == '█') return true;
            if (jel == '═' && (irany == 'b' || irany == 'j')) return true;
            if (jel == '║' && (irany == 'f' || irany == 'l')) return true;
            if (jel == '╔' && (irany == 'j' || irany == 'l')) return true;
            if (jel == '╗' && (irany == 'b' || irany == 'l')) return true;
            if (jel == '╚' && (irany == 'f' || irany == 'j')) return true;
            if (jel == '╝' && (irany == 'f' || irany == 'b')) return true;
            if (jel == '╦' && (irany == 'b' || irany == 'j' || irany == 'l')) return true;
            if (jel == '╩' && (irany == 'b' || irany == 'j' || irany == 'f')) return true;
            if (jel == '╠' && (irany == 'f' || irany == 'l' || irany == 'j')) return true;
            if (jel == '╣' && (irany == 'f' || irany == 'l' || irany == 'b')) return true;
            if (jel == '╬') return true;

            return false;
        }

        static char ellentetes(char irany)
        {
            if (irany == 'f') return 'l';
            if (irany == 'l') return 'f';
            if (irany == 'b') return 'j';
            return 'b';
        }

        static bool kijarat(int s, int o)
        {
            if (s == 0 && vanUt(palya[s, o], 'f')) return true;
            if (s == sorok - 1 && vanUt(palya[s, o], 'l')) return true;
            if (o == 0 && vanUt(palya[s, o], 'b')) return true;
            if (o == oszlopok - 1 && vanUt(palya[s, o], 'j')) return true;

            return false;
        }

        static bool kiLehetMenni(char irany)
        {
            if (irany == 'f' && jatekosSor == 0) return vanUt(palya[jatekosSor, jatekosOszlop], 'f');
            if (irany == 'l' && jatekosSor == sorok - 1) return vanUt(palya[jatekosSor, jatekosOszlop], 'l');
            if (irany == 'b' && jatekosOszlop == 0) return vanUt(palya[jatekosSor, jatekosOszlop], 'b');
            if (irany == 'j' && jatekosOszlop == oszlopok - 1) return vanUt(palya[jatekosSor, jatekosOszlop], 'j');

            return false;
        }

        static bool kilepes(char irany)
        {
            if (!kiLehetMenni(irany))
            {
                return true;
            }

            if (megtalaltTermek == termekSzama)
            {
                Console.Clear();
                Console.WriteLine(szoveg("sikerült kijutni", "you escaped"));
                return false;
            }

            if (igenNem(szoveg("még nincs meg minden terem, biztos kimész?", "not all rooms are found, leave anyway?")))
            {
                Console.Clear();
                Console.WriteLine(szoveg("kimentél, de nem találtál meg minden termet", "you left, but not all rooms were found"));
                return false;
            }

            return true;
        }

        static void teremNezes()
        {
            if (palya[jatekosSor, jatekosOszlop] == '█' && !megvan[jatekosSor, jatekosOszlop])
            {
                megvan[jatekosSor, jatekosOszlop] = true;
                megtalaltTermek++;
            }
        }

        // a mentés csak a fontos adatokat írja ki
        static void mentes()
        {
            List<string> adatok = new List<string>();

            adatok.Add(jatekosSor.ToString());
            adatok.Add(jatekosOszlop.ToString());
            adatok.Add(hatralevoIdo().ToString());
            adatok.Add(fedett.ToString());

            for (int s = 0; s < sorok; s++)
            {
                for (int o = 0; o < oszlopok; o++)
                {
                    if (bejart[s, o])
                    {
                        adatok.Add("B;" + s + ";" + o);
                    }

                    if (megvan[s, o])
                    {
                        adatok.Add("T;" + s + ";" + o);
                    }
                }
            }

            File.WriteAllLines(mentesNev, adatok, Encoding.UTF8);
        }

        static void mentesBetoltes()
        {
            string[] adatok = File.ReadAllLines(mentesNev, Encoding.UTF8);
            int ido;

            int.TryParse(adatok[0], out jatekosSor);
            int.TryParse(adatok[1], out jatekosOszlop);
            int.TryParse(adatok[2], out ido);
            bool.TryParse(adatok[3], out fedett);

            vegeIdo = DateTime.Now.AddSeconds(ido);
            bejart = new bool[sorok, oszlopok];
            megvan = new bool[sorok, oszlopok];
            megtalaltTermek = 0;

            for (int i = 4; i < adatok.Length; i++)
            {
                string[] darab = adatok[i].Split(';');

                if (darab.Length == 3)
                {
                    int s;
                    int o;
                    int.TryParse(darab[1], out s);
                    int.TryParse(darab[2], out o);

                    if (s >= 0 && s < sorok && o >= 0 && o < oszlopok)
                    {
                        if (darab[0] == "B")
                        {
                            bejart[s, o] = true;
                        }

                        if (darab[0] == "T")
                        {
                            megvan[s, o] = true;
                            megtalaltTermek++;
                        }
                    }
                }
            }

            bejart[jatekosSor, jatekosOszlop] = true;
        }

        static bool igenNem(string kerdes)
        {
            Console.Write(kerdes + " (i/n): ");
            string valasz = Console.ReadLine();

            if (valasz == null)
            {
                return false;
            }

            valasz = valasz.ToLower();
            return valasz == "i" || valasz == "y";
        }

        static string szoveg(string magyarSzoveg, string angolSzoveg)
        {
            if (magyar)
            {
                return magyarSzoveg;
            }

            return angolSzoveg;
        }
    }
}
