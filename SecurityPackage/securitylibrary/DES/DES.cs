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
        //...................1. Common methods................................
        private string fixLength(string s, int length) => s.PadLeft(length, '0');
        //XOR two binaries
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
        // apply given permutation on binary values
        private string encode(string key, List<int> permutation)
        {
            string result = "";
            for (int i = 0; i < permutation.Count; i++)
                result += key[permutation[i] - 1];

            return result;
        }
        //generates sub key
        private string generateSubKey(string s, int round)
        {
            if (round == 1 || round == 2 || round == 9 || round == 16)
            {
                s += s[0];
                return s.Substring(1);
            }
            else
            {
                s += s[0];
                s += s[1];
                return s.Substring(2);
            }
        }
        // f function to calculate thr right value
        private string f(string r, string key)
        {
            //1. encode with extender E bit selection permuatation to increase size from 32 tp 48 bits
            r = encode(r, Matrices.E_BitSelection);
            //2. XOR the resulted value with Ki
            string XORed_value = XOR(r, key);
            //3. replace values with s box values
            string replacedValues = Matrices.replaceFromSBox(XORed_value);
            //4. encode with p permutation to reduce size to 32 bits again
            return encode(replacedValues, Matrices.P);
        }

        // divides string into two halves and return desired half
        private string divide(string s, string returnedHalf)
            => (returnedHalf == "LEFT") ? s.Substring(0, s.Length / 2) : s.Substring(s.Length / 2);
        //----------------------------- End of common methods section -----------------------

        //................ 2. Decryption section.......................
        private string decode(string s, List<int> permutation)
        {
            char[] result = new char[permutation.Max()];
            for(int i = 0;i < s.Length; i++)
            {
                result[permutation[i]-1] = s[i];
            }

            return HelperMethods.ListToString(result);
        }
        private List<string> getKeys(string k0)
        {
            List<string> keys = new List<string>() {k0 };
            string c = divide(k0, "LEFT");
            string d = divide(k0, "RIGHT");

            for (int i = 1; i <= 16; i++)
            {
                c = generateSubKey(c, i);
                d = generateSubKey(d, i);
                string k = encode(string.Concat(c, d), Matrices.PC_2);
                keys.Add(k);
            }
            return keys;
        }
        public override string Decrypt(string cipherText, string key)
        {
            key = HelperMethods.HexaToBinary(key);
            string key_plus = encode(key, Matrices.PC_1);
            List<string> keys = getKeys(key_plus);

            cipherText = HelperMethods.HexaToBinary(cipherText);
            cipherText = decode(cipherText, Matrices.IP_neg1);
            string left = divide(cipherText, "LEFT");
            string right = divide(cipherText, "RIGHT");

            for(int i = 16; i >= 1; i--)
            {
                string tmpL = right;
                string tmpR = XOR(left, f(right, keys[i]));
                left = tmpL; right = tmpR;

            }
            string plainText = decode(right + left, Matrices.IP);
            
            return "0x" + HelperMethods.BinaryToHexa(plainText).PadLeft(16, '0');

        }
        //--------------------------- End of decrytption section--------------------

        //........... 3.) Encryption section...........................
        private string finalStep(string left, string right) =>
            "0x" + Convert.ToInt64(encode(string.Concat(right, left), Matrices.IP_neg1), 2).ToString("X");


        public override string Encrypt(string plainText, string key)
        {
            plainText = HelperMethods.HexaToBinary(plainText);
            string encodedPlain = encode(plainText, Matrices.IP);
            string l = divide(encodedPlain, "LEFT");
            string r = divide(encodedPlain, "RIGHT");

            key = HelperMethods.HexaToBinary(key);
            string key_plus = encode(key, Matrices.PC_1);
            string c = divide(key_plus, "LEFT");
            string d = divide(key_plus, "RIGHT");

            for (int i = 1; i <= 16; i++)
            {
                c = generateSubKey(c, i);
                d = generateSubKey(d, i);
                string k = encode(string.Concat(c , d), Matrices.PC_2);
                string newL = r;
                string newR = XOR(l, f(r, k));
                l = newL;
                r = newR;
            }

            return finalStep(l, r);
        }
    }
}
