using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using LabirintusMethods;

namespace ezvegleges
{
    class program
    {
        static char[,] palya;
        static bool[,] bejart;
        static bool[,] megtalalt;

        static int sorok;
        static int oszlopok;
        static int jatekosSor;
        static int jatekosOszlop;
        static int osszesTerem;
        static int megtalaltTerem;

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

            // pálya ellenőrzése
            if (labirintus.IsInvalidElement(palya))
            {
                Console.WriteLine(szoveg("hibás karakter van a pályában", "wrong character in the map"));
                Console.ReadKey(true);
                return;
            }

            if (labirintus.GetSuitableEntrance(palya) == 0)
            {
                Console.WriteLine(szoveg("nincs kijárat a pályán", "there is no exit on the map"));
                Console.ReadKey(true);
                return;
            }

            if (labirintus.GetRoomNumber(palya) == 0)
            {
                Console.WriteLine(szoveg("nincs terem a pályán", "there is no room on the map"));
                Console.ReadKey(true);
                return;
            }

            if (labirintus.GetUnavailableElements(palya).Count > 0)
            {
                Console.WriteLine(szoveg("van elszigetelt járat a pályán", "there is an isolated path on the map"));
                Console.ReadKey(true);
                return;
            }

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
            if (File.Exists(fajlNev)) return fajlNev;

            string futtatasHelye = AppContext.BaseDirectory;
            string exeMappa = Path.Combine(futtatasHelye, fajlNev);
            if (File.Exists(exeMappa)) return exeMappa;

            string projektMappa = Path.GetFullPath(Path.Combine(futtatasHelye, "..", "..", "..", fajlNev));
            if (File.Exists(projektMappa)) return projektMappa;

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

        // pálya beolvasása tömbbe
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
            megtalalt = new bool[sorok, oszlopok];

            // termek megszámolása
            osszesTerem = labirintus.GetRoomNumber(palya);
            megtalaltTerem = 0;

            kezdoPontKereses();
            bejart[jatekosSor, jatekosOszlop] = true;
        }

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
            int kiirtIdo = -1;

            Console.Clear();
            rajzol();

            while (fut)
            {
                if (hatralevoIdo() <= 0)
                {
                    rajzol();
                    Console.WriteLine(szoveg("lejárt az idő, bennragadtál", "time is over, you are trapped"));
                    break;
                }

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo gomb = Console.ReadKey(true);
                    char betu = char.ToLower(gomb.KeyChar);

                    if (gomb.Key == ConsoleKey.Escape) fut = programbolKilepes();
                    else if (betu == 'm') mentes();
                    else if (betu == 'b') mentesBetoltes();
                    else if (betu == 'w') fut = lepes('f');
                    else if (betu == 's') fut = lepes('l');
                    else if (betu == 'a') fut = lepes('b');
                    else if (betu == 'd') fut = lepes('j');

                    if (fut) rajzol();
                }
                else
                {
                    if (kiirtIdo != hatralevoIdo())
                    {
                        kiirtIdo = hatralevoIdo();
                        rajzol();
                    }

                    Thread.Sleep(100);
                }
            }

            Console.WriteLine(szoveg("nyomj egy gombot a kilépéshez", "press a key to exit"));
            Console.ReadKey(true);
        }

        static bool programbolKilepes()
        {
            Console.SetCursorPosition(0, sorok + 7);
            sorKiir("");

            if (igenNem(szoveg("biztosan bezárod a játékot?", "are you sure you want to quit the game?")))
            {
                Console.Clear();
                return false;
            }

            Console.Clear();
            rajzol();
            return true;
        }

        static void rajzol()
        {
            Console.SetCursorPosition(0, 0);

            for (int s = 0; s < sorok; s++)
            {
                StringBuilder sor = new StringBuilder();

                for (int o = 0; o < oszlopok; o++)
                {
                    if (s == jatekosSor && o == jatekosOszlop)
                    {
                        sor.Append('@');
                    }
                    else if (fedett && !bejart[s, o])
                    {
                        sor.Append(' ');
                    }
                    else
                    {
                        sor.Append(palya[s, o]);
                    }
                }

                sorKiir(sor.ToString());
            }

            sorKiir("");
            sorKiir(szoveg("termek: ", "rooms: ") + megtalaltTerem + "/" + osszesTerem);
            sorKiir(szoveg("idő: ", "time: ") + hatralevoIdo() + " mp");
            sorKiir(szoveg("mehetsz: ", "you can go: ") + lehetosegek());
            sorKiir(szoveg("w/a/s/d mozgás, m mentés, b betöltés, esc kilépés", "w/a/s/d move, m save, b load, esc quit"));
            sorKiir("");
        }

        static void sorKiir(string szoveg)
        {
            int hossz = Console.BufferWidth - 1;

            if (hossz > 0 && szoveg.Length < hossz)
            {
                szoveg = szoveg.PadRight(hossz);
            }

            Console.WriteLine(szoveg);
        }

        static int hatralevoIdo()
        {
            int ido = (int)(vegeIdo - DateTime.Now).TotalSeconds;
            if (ido < 0) return 0;
            return ido;
        }

        static string lehetosegek()
        {
            string iranyok = "";

            if (lehetLepni('f') || kiLehetMenni('f')) iranyok += "W ";
            if (lehetLepni('b') || kiLehetMenni('b')) iranyok += "A ";
            if (lehetLepni('l') || kiLehetMenni('l')) iranyok += "S ";
            if (lehetLepni('j') || kiLehetMenni('j')) iranyok += "D ";

            if (iranyok == "") return "-";
            return iranyok;
        }

        // lépés ellenőrzése
        static bool lepes(char irany)
        {
            if (kiLehetMenni(irany))
            {
                return kilepes(irany);
            }

            int ujSor = jatekosSor;
            int ujOszlop = jatekosOszlop;

            if (irany == 'f') ujSor--;
            if (irany == 'l') ujSor++;
            if (irany == 'b') ujOszlop--;
            if (irany == 'j') ujOszlop++;

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
            char most = palya[jatekosSor, jatekosOszlop];

            if (most == '█') return false;
            if (!vanUt(most, irany)) return false;

            int s = jatekosSor;
            int o = jatekosOszlop;

            if (irany == 'f') s--;
            if (irany == 'l') s++;
            if (irany == 'b') o--;
            if (irany == 'j') o++;

            if (s < 0 || s >= sorok || o < 0 || o >= oszlopok)
            {
                return true;
            }

            if (palya[s, o] != '.')
            {
                return false;
            }

            while (s >= 0 && s < sorok && o >= 0 && o < oszlopok)
            {
                if (palya[s, o] != '.')
                {
                    return false;
                }

                if (irany == 'f') s--;
                if (irany == 'l') s++;
                if (irany == 'b') o--;
                if (irany == 'j') o++;
            }

            return true;
        }

        static bool kilepes(char irany)
        {
            if (!kiLehetMenni(irany)) return true;

            Console.SetCursorPosition(0, sorok + 7);
            sorKiir("");

            string kerdes = szoveg("biztosan kimész a labirintusból?", "are you sure you want to leave?");

            if (megtalaltTerem < osszesTerem)
            {
                kerdes = szoveg("még nincs meg minden terem. biztosan kimész?", "not all rooms are found. are you sure?");
            }

            if (!igenNem(kerdes))
            {
                Console.Clear();
                rajzol();
                return true;
            }

            Console.Clear();

            if (megtalaltTerem == osszesTerem)
            {
                Console.WriteLine(szoveg("sikerült kijutni", "you escaped"));
            }
            else
            {
                Console.WriteLine(szoveg("kimentél, de nem találtál meg minden termet", "you left, but not all rooms were found"));
            }

            return false;
        }

        static void teremNezes()
        {
            if (palya[jatekosSor, jatekosOszlop] == '█' && !megtalalt[jatekosSor, jatekosOszlop])
            {
                megtalalt[jatekosSor, jatekosOszlop] = true;
                megtalaltTerem++;
                Console.Beep();
            }
        }

        // állás mentése
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
                    if (bejart[s, o]) adatok.Add("B;" + s + ";" + o);
                    if (megtalalt[s, o]) adatok.Add("T;" + s + ";" + o);
                }
            }

            File.WriteAllLines(mentesNev, adatok, Encoding.UTF8);
        }

        static void mentesBetoltes()
        {
            if (!File.Exists(mentesNev))
            {
                Console.SetCursorPosition(0, sorok + 7);
                Console.WriteLine(szoveg("nincs mentés", "no save file"));
                Thread.Sleep(800);
                return;
            }

            string[] adatok = File.ReadAllLines(mentesNev, Encoding.UTF8);
            int ido;

            int.TryParse(adatok[0], out jatekosSor);
            int.TryParse(adatok[1], out jatekosOszlop);
            int.TryParse(adatok[2], out ido);
            bool.TryParse(adatok[3], out fedett);

            vegeIdo = DateTime.Now.AddSeconds(ido);
            bejart = new bool[sorok, oszlopok];
            megtalalt = new bool[sorok, oszlopok];
            megtalaltTerem = 0;

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
                        if (darab[0] == "B") bejart[s, o] = true;
                        if (darab[0] == "T")
                        {
                            megtalalt[s, o] = true;
                            megtalaltTerem++;
                        }
                    }
                }
            }

            bejart[jatekosSor, jatekosOszlop] = true;
        }

        static bool igenNem(string kerdes)
        {
            Console.Write(kerdes + " (i/n): ");
            ConsoleKeyInfo gomb = Console.ReadKey(true);
            Console.WriteLine(gomb.KeyChar);

            char valasz = char.ToLower(gomb.KeyChar);
            return valasz == 'i' || valasz == 'y';
        }

        static string szoveg(string magyarSzoveg, string angolSzoveg)
        {
            if (magyar) return magyarSzoveg;
            return angolSzoveg;
        }
    }
}
