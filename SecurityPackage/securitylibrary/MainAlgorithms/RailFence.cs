using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToUpper();

            for(int i = 1; i < plainText.Length; i++)
            {
                if(Encrypt(plainText, i) == cipherText)
                    return i;
            }
                return -1;
        }

        public string Decrypt(string cipherText, int key)
        {
            string plainText = "";
            int z = cipherText.Length % key == 0 ? cipherText.Length / key : cipherText.Length / key + 1;

             for (int k = 0; k < z; k++)
                {
                for (int p = 0; p < key; p++)
                    {
                        if ((p * z + k) < cipherText.Length)
                        {
                            plainText += cipherText[p * z + k];
                        }
                    }
                }
            return plainText;
        }

        public string Encrypt(string plainText, int key)
        {
            string cipherText = "";
            
            for(int p = 0; p < key; p++)
            {
                for (int k = 0; k < (plainText.Length % key == 0 ? plainText.Length / key : plainText .Length/ key + 1); k++)
                {
                    if ((p + k * key) < plainText.Length)
                    {
                        cipherText += plainText[p + k * key];
                    }
                }
            }
            return cipherText;
        }
    }
}
