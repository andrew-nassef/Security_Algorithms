using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class DES : CryptographicTechnique
    {
        public override string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }

        private string generateKPlus(string key)
        {
            // 0 - 7 / 8 - 15 / 16 - 23 / 24 - 31 / 32 - 39 / 40 - 47 /  48 - 55 / 56 - 63
            // 1010101
            string result = "";
            for (int i = 0; i < Matrices.PC_1.Count; i++)
                result += key[Matrices.PC_1[i] - 1];

            return result;
        }
        public override string Encrypt(string plainText, string key)
        {
            plainText = Convert.ToString(Convert.ToInt64(plainText.Substring(2), 16), 2).PadLeft(64, '0');
            key = Convert.ToString(Convert.ToInt64(key.Substring(2), 16), 2).PadLeft(64, '0');
            string key_plus = generateKPlus(key);
            string c = key_plus.Substring(0, 28);
            string d = key_plus.Substring(28, 28);
            

            return "";
        }
    }
}
