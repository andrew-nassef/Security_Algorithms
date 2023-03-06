using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher :  ICryptographicTechnique<List<int>, List<int>>
    {
        enum MatrixType
        {
            Key,
            Plain
        };
        private int[,] toMatrix(List<int> list, int m,MatrixType matrixType)
        {
            int[,] matrix;
            int n = list.Count / m;

            if (matrixType == MatrixType.Key)
                matrix = new int[m, m];
            else
                matrix = new int[m, n];

            for(int i = 0; i < m; i++)
            {
                int index = i;
                for(int j = 0; j < n; j++)
                {
                    if (matrixType == MatrixType.Key)
                        matrix[i, j] = list[i * n + j];
                    else
                    {
                        matrix[i, j] = list[index];
                        index += m;
                    }
                }
            }
            return matrix;
        }
        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            throw new NotImplementedException();
        }


        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            throw new NotImplementedException();
        }
        private static int[,] MatrixMult(int[,] matrix1, int[,] matrix2)
        {
            int[,] result = new int[matrix1.GetLength(0), matrix2.GetLength(1)];
            for (int i = 0; i < result.GetLength(0); i++)
                for (int j = 0; j < result.GetLength(1); j++)
                    result[i, j] = 0;
            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix1.GetLength(1); j++)
                {
                    for (int k = 0; k < matrix2.GetLength(1); k++)
                    {
                        result[i, k] += matrix1[i, j] * matrix2[j, k];
                    }
                }
            }
            return result;
        }

        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            int[,] keyMatrix = toMatrix(key, (int)Math.Sqrt(key.Count), MatrixType.Key);
            int[,] plainTextMatrix = toMatrix(plainText, keyMatrix.GetLength(1), MatrixType.Plain);
            int[,] res = MatrixMult(keyMatrix, plainTextMatrix);

            List<int> result = new List<int>();
            for (int i = 0; i < res.GetLength(1); i++)
            {
                for(int j = 0;j<res.GetLength(0); j++)
                {
                    result.Add(res[j, i] % 26);
                }
            }
            return result;
        }


        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            throw new NotImplementedException();
        }

    }
}
