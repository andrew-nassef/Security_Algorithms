using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DES
{
    public class Matrices
    {
        public static List<int> PC_1 = new List<int>()
        {
            57, 49, 41, 33, 25, 17, 09,
            01, 58, 50, 42, 34, 26, 18,
            10, 02, 59, 51, 43, 35, 27,
            19, 11, 03, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15,
            07, 62, 54, 46, 38, 30, 22,
            14, 06, 61, 53, 45, 37, 29,
            21, 13, 05, 28, 20, 12, 04
        };

        public static List<int> PC_2 = new List<int>
        {
            14, 17, 11, 24, 01, 05,
            03, 28, 15, 06, 21, 10,
            23, 19, 12, 04, 26, 08,
            16, 07, 27, 20, 13, 02,
            41, 52, 31, 37, 47, 55,
            30, 40, 51, 45, 33, 48,
            44, 49, 39, 56, 34, 53,
            46, 42, 50, 36, 29, 32
        };
    }
}
