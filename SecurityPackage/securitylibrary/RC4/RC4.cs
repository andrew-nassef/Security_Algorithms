using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RC4
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class RC4 : CryptographicTechnique
    {
        private void swap(ref int a, ref int b)
        {
            int tmp = a;
            a = b;
            b = tmp;
        }
         string HexaToBinary(string s) => Convert.ToString(Convert.ToInt64(s, 10), 2);
         string HexaToDecimal(string s) => Convert.ToString(Convert.ToInt64(s, 16), 10);
         string BinaryToDecimal (string s) => Convert.ToString(Convert.ToInt64(s, 2), 10);

        string toHexa(string n) => Convert.ToInt64(n, 2).ToString("X");
        private string fixLength(string s, int length) => s.PadLeft(length, '0');

        private string XOR(string v1, string v2)
        {
            v1 = HexaToBinary(v1);
            v2 = HexaToBinary(v2);

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

        public override  string Encrypt(string plainText, string key)
        {
            const int length = 256;
            string dec_key = key.Substring(2);
            plainText = plainText.Substring(2);
            int[] s_arr = new int[length];
            int[] t_arr = new int[length];
            string result = "";

            int j = 0, tmp, ctr = 0,  ind_1 = 0, ind_2 = 0, key_idx, key_stream_byte;
            for (int i = 0; i < length; i++)
            {
                s_arr[i] = i;
                t_arr[i] = dec_key[ctr % dec_key.Length] - 48;
                ctr++;
            }
            for (int i = 0; i < length; i++)
            {
                j = (j + s_arr[i] + t_arr[i]) % length;
                tmp = s_arr[i];
                s_arr[i] = s_arr[j];
                s_arr[j] = tmp;
            }
            int c = 0;
            for (int i = 0; i < plainText.Length; i++)
            {
                ind_1 = (ind_1 + 1) % length;
                ind_2 = (ind_2 + s_arr[ind_1]) % length;
                tmp = s_arr[ind_1];
                s_arr[ind_1] = s_arr[ind_2];
                s_arr[ind_2] = tmp;
                key_idx = (s_arr[ind_1] + s_arr[ind_2]) % length;
                key_stream_byte = s_arr[key_idx];
                string ans = XOR(Convert.ToString(key_stream_byte), Convert.ToString(plainText[i]));
                result += ans;
            }
            return toHexa(result);
        }
    }
}
 ;