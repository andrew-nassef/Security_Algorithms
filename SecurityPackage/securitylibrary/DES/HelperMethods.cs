using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DES
{
    internal class HelperMethods
    {
        public static string HexaToBinary(string hexa)
            => Convert.ToString(Convert.ToInt64(hexa, 16), 2).PadLeft(64, '0');

        public static string BinaryToHexa(string binary)
            => Convert.ToInt64(binary, 2).ToString("X");

        public static string ListToString(char[] list)
        {
            string result = "";
            for(int i = 0;i<list.Length;i++)
                result += list[i];
            return result;
        }
    }
}
