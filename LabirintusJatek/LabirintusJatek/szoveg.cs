using System;

namespace labirintus_jatek
{
    internal class szoveg
    {
        public static bool magyar = true;

        public static string t(string magyarSzoveg, string angolSzoveg)
        {
            if (magyar)
            {
                return magyarSzoveg;
            }

            return angolSzoveg;
        }

        public static bool igenNem(string magyarSzoveg, string angolSzoveg)
        {
            Console.Write(t(magyarSzoveg + " (i/n): ", angolSzoveg + " (y/n): "));

            while (true)
            {
                ConsoleKeyInfo gomb = Console.ReadKey(true);
                char valasz = char.ToLower(gomb.KeyChar);

                if (valasz == 'i' || valasz == 'y')
                {
                    Console.WriteLine(t("igen", "yes"));
                    return true;
                }

                if (valasz == 'n')
                {
                    Console.WriteLine(t("nem", "no"));
                    return false;
                }
            }
        }
    }
}
