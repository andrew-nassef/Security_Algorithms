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
            throw new NotImplementedException();
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
                cipherText += (char)(((key[p] + shift)% 26) + 65);


            }
            return cipherText;
               
                
            //throw new NotImplementedException();
        }
    }
}