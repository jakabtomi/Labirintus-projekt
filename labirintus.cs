using System;
using System.Collections.Generic;

namespace LabirintusMethods
{
    class labirintus
    {
        static readonly char[] pathChars =
        {
            '╬','═','╦','╩','║','╣','╠','╗','╝','╚','╔'
        };

        /// <summary>
        /// Megadja, hogy hány termet tartamaz a térkép
        /// </summary>
        /// <param name="map">Labirintus mátrixa</param>
        /// <returns>Termek száma</returns>
        static int GetRoomNumber(char[,] map)
        {
            int count = 0;

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == '█')
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// A kapott térkép széleit végignézve megállapítja, hogy hány kijárat van.
        /// </summary>
        /// <param name="map">Labirintus mátrixa</param>
        /// <returns>Az alkalmas kijáratok száma</returns>
        static int GetSuitableEntrance(char[,] map)
        {
            int count = 0;

            int rows = map.GetLength(0);
            int cols = map.GetLength(1);

            for (int j = 0; j < cols; j++)
            {
                if (IsPath(map[0, j])) count++;
                if (IsPath(map[rows - 1, j])) count++;
            }

            for (int i = 1; i < rows - 1; i++)
            {
                if (IsPath(map[i, 0])) count++;
                if (IsPath(map[i, cols - 1])) count++;
            }
            return count;
        }

        /// <summary>
        /// Megnézi, hogy van-e a térképen meg nem engedett karakter?
        /// </summary>
        /// <param name="map">Labirintus mátrixa</param>
        /// <returns>true - A térkép tartalmaz szabálytalan karaktert, false - nincs benne ilyen</returns>
        static bool IsInvalidElement(char[,] map)
        {
            char[] validChars =
            {
                '.', '█',
                '╬','═','╦','╩','║','╣','╠','╗','╝','╚','╔'
            };

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    bool valid = false;

                    foreach (char c in validChars)
                    {
                        if (map[i, j] == c)
                        {
                            valid = true;
                            break;
                        }
                    }
                    if (!valid)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Visszaadja azoknak a járatkaraktereknek a pozícióját, amelyekhez egyetlen szomszéd pozícióból sem lehet eljutni.
        /// </summary>
        /// <param name="map">Labirintus mátrixa</param>
        /// <returns>A pozíciók "sor_index:oszlop_index" formátumban szerepelnek a lista elemeiként</returns>
        static List<string> GetUnavailableElements(char[,] map)
        {
            List<string> unavailables = new List<string>();

            int rows = map.GetLength(0);
            int cols = map.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (IsPath(map[i, j]))
                    {
                        bool connected = false;

                        if (i > 0 && IsPath(map[i - 1, j]))
                            connected = true; 

                        if (i < rows - 1 && IsPath(map[i + 1, j]))
                            connected = true;

                        if (j > 0 && IsPath(map[i, j - 1]))
                            connected = true;

                        if (j < cols - 1 && IsPath(map[i, j + 1]))
                            connected = true;

                        if (!connected)
                        {
                            unavailables.Add($"{i}:{j}");
                        }
                    }
                }
            }
            return unavailables;
        }

        /// <summary>
        /// Labiritust generál a kapott pozíciókat tartalmazó lista alapján. A lista elemei egymáshoz kapcsolódó járatok pozíciói.
        /// </summary>
        /// <param name="positionsList">"sor_index:oszlop_index" formátumban az egymáshoz kapcsolódó járatok pozícióit tartalmazó lista </param>
        /// <returns>A létrehozott labirintus térképe</returns>
        static char[,] GenerateLabyrinth(List<string> positionsList)
        {
            int maxRow = 0;
            int maxCol = 0;

            foreach (string pos in positionsList)
            {
                string[] parts = pos.Split(':');

                int row = int.Parse(parts[0]);
                int col = int.Parse(parts[1]);

                if (row > maxRow) maxRow = row;
                if (col > maxCol) maxCol = col;
            }

            char[,] map = new char[maxRow + 1, maxCol + 1];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = '.';
                }
            }

            foreach (string pos in positionsList)
            {
                string[] parts = pos.Split(':');

                int row = int.Parse(parts[0]);
                int col = int.Parse(parts[1]);

                map[row, col] = '╬';
            }
            return map;
        }

        /// <summary>
        /// jarat karakter?
        /// </summary>
        // sajat segedmetodus 
        static bool IsPath(char c)
        {
            foreach (char p in pathChars)
            {
                if (c == p)
                    return true;
            }
            return false;
        }

        static void Main(string[] args)
        {
            char[,] testMap =
          {
        { '.', '.', '.', '.', '.', '╔', '═', '═', '═', '═', '═', '╗', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' },
        { '═', '═', '═', '═', '╗', '║', '.', '.', '.', '.', '.', '║', '.', '.', '.', '.', '.', '.', '.', '█', '.', '.', '.', '.' },
        { '.', '╔', '═', '═', '╬', '╝', '.', '.', '╔', '═', '═', '═', '╣', '.', '.', '.', '╗', '.', '.', '║', '.', '.', '.', '.' },
        { '.', '║', '.', '.', '║', '.', '.', '.', '║', '.', '.', '.', '╚', '═', '╦', '═', '╝', '╚', '╦', '╦', '.', '═', '.', '.' },
        { '.', '╚', '╦', '═', '╩', '═', '═', '═', '╣', '.', '.', '.', '.', '.', '║', '.', '.', '.', '║', '.', '.', '.', '.', '.' },
        { '.', '.', '╚', '═', '═', '.', '.', '.', '.', '.', '.', '.', '.', '.', '╚', '═', '═', '═', '╝', '.', '.', '.', '.', '.' }
    };

            Console.WriteLine("Termek száma:");
            Console.WriteLine(GetRoomNumber(testMap));

            Console.WriteLine();

            Console.WriteLine("Kijáratok száma: ");
            Console.WriteLine(GetSuitableEntrance(testMap));

            Console.WriteLine();
            Console.WriteLine("Szabálytalan karakter?  ");
            Console.WriteLine(IsInvalidElement(testMap));

            Console.WriteLine();

            List<string> bad = GetUnavailableElements(testMap);

            Console.WriteLine("Elérhetetlen elemek:");

            foreach (string s in bad)
            {
                Console.WriteLine(s);
            }
        }
    }
}
