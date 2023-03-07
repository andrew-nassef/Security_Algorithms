using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        private int mod(int det, int mod)
        {
            if (det >= 0) return det % mod;
            else return mod - (det * -1 % mod);
        }
        public string Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToUpper();

            string key = "";
            for (int i = 0; i < plainText.Length; i++)
            {
                int shift = plainText[i] - 65;
                int ans = mod(cipherText[i] - shift - 65, 26);
                key += (char)(ans + 65);
            }

            string tmpKey = "";
            
            int l = 0, matchCount = 0, stopIndex = key.Length;
            for (int k = 1; k <  key.Length; k++)
            {
                if (matchCount > 1) break;
                if (key[k] == key[l])
                {
                    if (matchCount == 0)
                        stopIndex = k;
                    l++;
                    matchCount++;
                }
                else
                {
                    matchCount = 0;
                    l = 0;
                }
                
            }
            for(int i = 0;i<stopIndex;i++)
                tmpKey += key[i];
            return tmpKey;
        }

        public string Decrypt(string cipherText, string key)
        {
            key = key.ToUpper();
            int index = 0;
            int shift = 0;
            string plainText = "";
            if (key.Length < cipherText.Length)
            {
                int remain = cipherText.Length - key.Length;
                for (int i = 0; i < remain; i++)
                {
                    key += key[i];
                }
            }
            for (int c = 0; c < cipherText.Length; c++)
            {
                index = cipherText[c] - 65;
                shift = mod(index - (key[c] - 65) , 26);
                plainText += (char)(shift + 65);

            }
            return plainText;
        }

        public string Encrypt(string plainText, string key)
        {
            plainText = plainText.ToUpper();
            key = key.ToUpper();

            if (key.Length < plainText.Length)
            {
                int remain = plainText.Length - key.Length;
                for(int i =  0;i < remain;i++)
                {
                    key += key[i];
                }
            }

            string cipherText = "";
            for (int p = 0; p < plainText.Length; p++)
            {
                int shift = plainText[p] - 65;
                cipherText += (char)(((key[p] - 65 + shift)% 26) + 65);
            }
            return cipherText;
        }
    }
}