using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        char[,] createPlainMatrix(string plainText, List<int> key)
        {
            int N = plainText.Length % key.Count == 0 ? plainText.Length / key.Count : plainText.Length / key.Count + 1;
            char[,] matrix = new char[N, key.Count];
            int letterIndex = 0;
            int i = 0;
            for (int r = 0; r < matrix.GetLength(0); r++)
            {
                for (int c = 0; c < key.Count; c++)
                {
                    if (letterIndex < plainText.Length)
                    {
                        matrix[r, c] = plainText[letterIndex];
                        letterIndex++;
                    }
                    else
                    {
                        matrix[r, c] = 'X';
                    } 
                }
            }
            return matrix;
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

        public List<int> Analyse(string plainText, string cipherText)
        {
            
            throw new NotImplementedException();
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

        public string Encrypt(string plainText, List<int> key)
        {
            plainText = plainText.ToUpper();
            string cipherText = "";
            char[,] PT_matrix = createPlainMatrix(plainText, key);

            for (int c = 0; c < key.Count; c++)
            {
                int numIndex = key.IndexOf(c +1);
                for (int r = 0; r < PT_matrix.GetLength(0); r++)
                {

                    cipherText += PT_matrix[r, numIndex];
                }

            }
            return cipherText;
            //throw new NotImplementedException();
        }
    }
}
