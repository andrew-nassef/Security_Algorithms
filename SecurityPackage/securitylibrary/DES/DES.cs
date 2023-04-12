using SecurityLibrary.AES;
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
        private string fixLength(string s, int length) => s.PadLeft(length, '0');

        private string XOR(string v1, string v2)
        {

            int fixedLength = Math.Max(v1.Length, v2.Length);

            if (v1.Length < fixedLength)
                v1 = fixLength(v1, fixedLength);

            else if (v2.Length < fixedLength)
                v2 = fixLength(v2, fixedLength);

            string XOR_value = "";
            for (int i = 0; i < fixedLength; i++)
            {
                if (v1[i] != v2[i])
                    XOR_value += "1";
                else
                    XOR_value += "0";
            }

            return XOR_value;

        }

        public override string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }

        private string encode(string key, List<int> permutation)
        {
            // 0 - 7 / 8 - 15 / 16 - 23 / 24 - 31 / 32 - 39 / 40 - 47 /  48 - 55 / 56 - 63
            // 1010101
            string result = "";
            for (int i = 0; i < permutation.Count; i++)
                result += key[permutation[i] - 1];

            return result;
        }
        private string generateSubKeys(string s, int round)
        {
            if (round == 1 || round == 2 || round == 9 || round == 16) 
            {
            s += s[0];
            return s.Substring(1);
            }
            else {
                s += s[0];
                s+= s[1];
                return s.Substring(2); 
            }
        } 
        private string f(string r, string key)
        {
            r = encode(r, Matrices.E_BitSelection);
            return XOR(r, key);
        }
        public override string Encrypt(string plainText, string key)
        {
            plainText = Convert.ToString(Convert.ToInt64(plainText.Substring(2), 16), 2).PadLeft(64, '0');
            string encodedPlain = encode(plainText, Matrices.IP);
            string l = encodedPlain.Substring(0, 32);
            string r = encodedPlain.Substring(32, 32);

            key = Convert.ToString(Convert.ToInt64(key.Substring(2), 16), 2).PadLeft(64, '0');
            string key_plus = encode(key, Matrices.PC_1);
            string c = key_plus.Substring(0, 28);
            string d = key_plus.Substring(28, 28);
            string k;
            for (int i = 1; i <= 16; i++)
            {
                c = generateSubKeys(c, i);
                d = generateSubKeys(d, i);
                k = encode(String.Concat(c , d), Matrices.PC_2);
                string newL = r;
                string newR = XOR(l, f(r, k)).Substring(16);
            }

            return "";
        }
    }
}
