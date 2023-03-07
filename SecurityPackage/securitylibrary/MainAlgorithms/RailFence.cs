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
            int depth1 = plainText.IndexOf(cipherText[1]) - plainText.IndexOf(cipherText[0]);
            int depth2 = plainText.IndexOf(cipherText[3]) - plainText.IndexOf(cipherText[2]);
            if(depth1 == depth2)
            {
                return depth1;
            }
            else
            {
                int depth3 = plainText.IndexOf(cipherText[5]) - plainText.IndexOf(cipherText[4]);
                return depth3;
            }
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
            //throw new NotImplementedException();
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
            

            //throw new NotImplementedException();
        }
    }
}
