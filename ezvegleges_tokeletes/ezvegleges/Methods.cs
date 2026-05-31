using System;
using System.Collections.Generic;

namespace LabirintusMethods
{
    public class labirintus
    {
        // az elfogadott jarat karakterek
        static readonly char[] pathChars =
        {
            '╬','═','╦','╩','║','╣','╠','╗','╝','╚','╔'
        };

        /// <summary>
        /// megadja, hogy hany termet tartalmaz a terkep
        /// </summary>
        /// /// <param name="map">Labirintus mátrixa</param>
        /// <returns>Termek száma</returns>
        public static int GetRoomNumber(char[,] map)
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
        /// megszamolja a kijaratokat
        /// </summary>
        /// <param name="map">Labirintus mátrixa</param>
        /// <returns>Az alkalmas kijáratok száma</returns>
        public static int GetSuitableEntrance(char[,] map)
        {
            int count = 0;

            int rows = map.GetLength(0);
            int cols = map.GetLength(1);

            // Felső és alsó sor
            for (int j = 0; j < cols; j++)
            {
                if (IsPath(map[0, j])) count++;
                if (IsPath(map[rows - 1, j])) count++;
            }

            // Bal és jobb oldal
            for (int i = 1; i < rows - 1; i++)
            {
                if (IsPath(map[i, 0])) count++;
                if (IsPath(map[i, cols - 1])) count++;
            }

            return count;
        }

        /// <summary>
        /// van-e szabálytalan karakter?
        /// </summary>
        /// /// <param name="map">Labirintus mátrixa</param>
        /// <returns>true - A térkép tartalmaz szabálytalan karaktert, false - nincs benne ilyen</returns>
        public static bool IsInvalidElement(char[,] map)
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
        /// elerhetetlen jaratelemek listája
        /// </summary>
        /// /// <param name="map">Labirintus mátrixa</param>
        /// <returns>A pozíciók "sor_index:oszlop_index" formátumban szerepelnek a lista elemeiként</returns>
        public static List<string> GetUnavailableElements(char[,] map)
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

                        // eszak
                        if (i > 0 && IsPath(map[i - 1, j]))
                            connected = true; 

                        // del
                        if (i < rows - 1 && IsPath(map[i + 1, j]))
                            connected = true;

                        // nyugat
                        if (j > 0 && IsPath(map[i, j - 1]))
                            connected = true;

                        // kelet
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
        /// labirintus generalasa poziciolista alapjan
        /// </summary>
        /// <param name="positionsList">"sor_index:oszlop_index" formátumban az egymáshoz kapcsolódó járatok pozícióit tartalmazó lista </param>
        /// <returns>A létrehozott labirintus térképe</returns>
        public static char[,] GenerateLabyrinth(List<string> positionsList)
        {
            int maxRow = 0;
            int maxCol = 0;

            // meret meghatarozasa
            foreach (string pos in positionsList)
            {
                string[] parts = pos.Split(':');

                int row = int.Parse(parts[0]);
                int col = int.Parse(parts[1]);

                if (row > maxRow) maxRow = row;
                if (col > maxCol) maxCol = col;
            }

            char[,] map = new char[maxRow + 1, maxCol + 1];

            // feltoltes ponttal
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = '.';
                }
            }

            // jaratok elhelyezése
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
        static bool IsPath(char c)
        {
            foreach (char p in pathChars)
            {
                if (c == p)
                    return true;
            }

            return false;
        }
    }
}
