using System;
using System.Collections.Generic;

namespace LabirintusMethods
{
    internal class Program
    {
        // az elfogadott jarat karakterek
        static readonly char[] pathChars =
        {
            '╬','═',
            '╦','╩','║',
            '╣','╠','╗',
            '╝'
                ,'╚','╔'
        };

        /// <summary>
        /// megadja, hogy hany termet tartalmaz a terkep
        /// </summary>
        static int GetRoomNumber(char[,] map)
        {
        }

        /// <summary>
        /// megszamolja a kijaratokat
        /// </summary>
        static int GetSuitableEntrance(char[,] map)
        {

        }

        /// <summary>
        /// van-e szabálytalan karakter?
        /// </summary>
        static bool IsInvalidElement(char[,] map)
        {
            char[] valamik =
            {
                '.', '█',
                '╬','═','╦','╩','║','╣','╠','╗','╝','╚','╔'
            };


            /// <summary>
            /// elerhetetlen jaratelemek listája
            /// </summary>
            static List<string> GetUnavailableElements(char[,] map)
            {
                List<string> asd = new List<string>();
            }

            /// <summary>
            /// labirintus generalasa poziciolista alapjan
            /// </summary>
            static char[,] GenerateLabyrinth(List<string> positionsList)
            {
                int maxRow = 0;
                int maxCol = 0;

            }



            static void Main(string[] args)
            {  //a minta alapjan
                char[,] testMap =
              {
        { '.', '.', '.', '.', '.', '╔', '═', '═', '═', '═', '═', '╗', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.' },
        { '═', '═', '═', '═', '╗', '║', '.', '.', '.', '.', '.', '║', '.', '.', '.', '.', '.', '.', '█', '.', '.', '.', '.', '.' },
        { '.', '╔', '═', '═', '╬', '╝', '█', '.', '╔', '═', '═', '═', '╣', '.', '.', '.', '╗', '.', '║', '║', '.', '.', '.', '.' },
        { '.', '║', '.', '.', '║', '.', '.', '.', '║', '.', '.', '.', '╚', '═', '╦', '═', '╝', '╚', '╦', '╝', '.', '.', '.', '.' },
        { '.', '╚', '╦', '═', '╩', '═', '═', '═', '╣', '.', '.', '.', '.', '.', '║', '.', '.', '.', '║', '.', '.', '.', '.', '.' },
        { '.', '.', '╚', '═', '═', '.', '.', '.', '║', '.', '.', '.', '.', '.', '╚', '═', '═', '═', '╝', '.', '.', '.', '.', '.' }
    };




            }
        }
    }
}