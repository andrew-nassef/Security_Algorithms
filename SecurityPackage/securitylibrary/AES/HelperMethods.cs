using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    public class HelperMethods
    {

        // convert given fixed size string to a 4 * 4 matrix
        public static string[,] toMatrix(string str)
        {
            string[,] matrix = new string[4, 4];
            int indexer = 2;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[j, i] += str[indexer];
                    matrix[j, i] += str[indexer + 1];
                    indexer += 2;
                }
            }
            return matrix;
        }
        // convert string matrix to int matrix
        public static int[,] matrixToInt(string[,] matrix)
        {
            int[,] result = new int[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[i, j] = Convert.ToInt32(matrix[i, j], 16);
                }
            }
            return result;
        }

        public static string toHexa(int n) => n.ToString("X");
        public static string toHexa(string n) => Convert.ToInt32(n, 2).ToString("X");

        public static string HexaToBinary(string s) => Convert.ToString(Convert.ToInt64(s, 16), 2);
        public static string BinaryToHexa(string s) => Convert.ToInt32(s, 2).ToString("X");


        public static string toString(string[,] matrix)
        {
            string str = "0x";
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[j, i].Length == 1)
                        str += "0" + matrix[j, i];
                    else
                        str += matrix[j, i];
                }
            }
            return str;
        }

    }
}
