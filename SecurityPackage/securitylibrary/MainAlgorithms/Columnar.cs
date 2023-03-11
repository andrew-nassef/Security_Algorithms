using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public string Encrypt(string plainText, List<int> key)
        {
            plainText = plainText.ToUpper();
            string cipherText = "";
            int N = plainText.Length % key.Count == 0 ? plainText.Length / key.Count : plainText.Length / key.Count + 1;

            for (int c = 0; c < key.Count; c++)
            {
                int numIndex = key.IndexOf(c + 1);
                for (int r = 0; r < N; r++)
                {
                    //cipherText += PT_matrix[r, numIndex];
                    if(r * key.Count + numIndex < plainText.Length)
                    cipherText += plainText[r * key.Count + numIndex];
                }

            }
            return cipherText;
            //throw new NotImplementedException();
        }
        char[,] createCipherMatrix(string cipherText, List<int> key)
        {
            char[,] matrix = new char[(cipherText.Length / key.Count), key.Count];
            int startIndx = 0;
            int numIndx ;
            for(int c= 0; c < key.Count; c++)
            {
                numIndx = key.IndexOf(c + 1); 
                for (int r = 0; r < (cipherText.Length/key.Count); r++)
                {
                    matrix[r, numIndx] = cipherText[startIndx]; 
                    startIndx++;
                }
            }
            return matrix;
        }

        public string Decrypt(string cipherText, List<int> key)
        {
            char[,] CT_matrix = createCipherMatrix(cipherText, key);
            string plainText = "";
            for(int r = 0; r < CT_matrix.GetLength(0); r++)
            {
                for(int c = 0; c < CT_matrix.GetLength(1); c++)
                {
                    plainText += CT_matrix[r, c];
                }
            }
            
            return plainText;
           // throw new NotImplementedException();
        }
        // Generating permutation using Heap Algorithm
        List<List<int>> result;
        void backtrack(List<int> s, int size)
        {
            if (s.Count == size)
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = i + 1; j < size; j++)
                    {
                        if (s[i] == s[j]) return;
                    }
                }
                result.Add(s.ToList());
                return;
            }
            for (int i = 1; i <= size; i++)
            {
                s.Add(i);
                backtrack(s, size);
                s.RemoveAt(s.Count - 1);
            }
        }

        public List<int> Analyse(string plainText, string cipherText)
        {
            cipherText = cipherText.ToUpper();
            for (int i = 3; i < 10; i++)
            {
                result = new List<List<int>>();
                backtrack(new List<int>(), i);
                for(int j = 0; j < result.Count; j++)
                {
                    if (Encrypt(plainText, result[j]) == cipherText)
                        return result[j];
                }
            }
            return null;
        }

    }
}
