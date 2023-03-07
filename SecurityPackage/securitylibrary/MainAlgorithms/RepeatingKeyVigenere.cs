using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {

            throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, string key)
        {
            key = key.ToUpper();
            int index = 0;
            int shift = 0;
            string plainText = "";
           for(int c = 0; c < cipherText.Length; c++)
            {
                index = cipherText[c] - 65;
                shift = index - (key[c] - 65);
                plainText += (char)(shift + 65);

            }
            return plainText;
            //throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {
            plainText = plainText.ToUpper();
            key = key.ToUpper();
            int shift = 0;
            string cipherText = "";



            if (key.Length < plainText.Length)
            {
                int remain = plainText.Length - key.Length;
                for(int i =  0;i < remain;i++)
                {
                    key += key[i];
                }
            }

            for(int p = 0; p < plainText.Length; p++)
            {
                shift = plainText[p] - 65;
                cipherText += (char)(((key[p] - 65 + shift)% 26) + 65);
            }
            return cipherText;
               
                
            //throw new NotImplementedException();
        }
    }
}