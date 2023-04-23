using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RC4
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class RC4 : CryptographicTechnique
    {
        private void Swap(ref int a, ref int b) => (b, a) = (a, b);
        
        private void InitializeArrays(ref int[] S, ref int[] T,string key, int length)
        {
            int ctr = 0;
            for (int i = 0; i < length; i++)
            {
                S[i] = i;
                T[i] = key[ctr];
                ctr = (ctr + 1) % key.Length;
            }
        }
        private int[] ApplyKSA(int[] S, int[] T, int length)
        {
            int j = 0;
            for (int i = 0; i < length; i++)
            {
                j = (j + S[i] + T[i]) % length;
                Swap(ref S[i], ref S[j]);
            }
            return S;
        }
        private int ApplyPRGA(ref int[] S, ref int ind_1, ref int ind_2, int length)
        {
            ind_1 = (ind_1 + 1) % length;
            ind_2 = (ind_2 + S[ind_1]) % length;
            Swap(ref S[ind_1], ref S[ind_2]);
            int key_idx = (S[ind_1] + S[ind_2]) % length;
            int key_stream_byte = S[key_idx];
            return key_stream_byte;
        }
        private string ToString(string hexaText)
        {
            int fixedLength = hexaText.Length % 2 == 0 ? hexaText.Length / 2 : hexaText.Length / 2 + 1;
            byte[] pla = new byte[fixedLength];
            string result = "";
            for (int i = 0; i < hexaText.Length; i += 2)
            {
                string s = "";
                s += hexaText[i];
                s += hexaText[i + 1];
                pla[i / 2] = Convert.ToByte(s, 16);
                result += (char)pla[i / 2];
            }
            return result;
        }
        private string toHex(string str)
        {
            byte[] ba = Encoding.Default.GetBytes(str);
            string hexString = BitConverter.ToString(ba);
            string modifiedHex = "0x";
            for(int i = 0;i < hexString.Length;i ++) 
            {
                if (hexString[i] == '-')
                    continue;
                modifiedHex += hexString[i];
            }
            return modifiedHex;
        }
        private string DecimalToBinary(string s) => Convert.ToString(Convert.ToInt64(s, 10), 2);
        private string fixLength(string s, int length) => s.PadLeft(length, '0');

        private string XOR(string v1, string v2)
        {
            v1 = DecimalToBinary(v1);
            v2 = DecimalToBinary(v2);

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
            string plainText = "";
            bool isHexa = false;
            if (cipherText.StartsWith("0x"))
            {
                key = ToString(key.Substring(2));
                cipherText = ToString(cipherText.Substring(2));
                isHexa = true;
            }

            const int length = 256;
            int[] s_arr = new int[length];
            int[] t_arr = new int[length];
                
            //initializing S and T
            InitializeArrays(ref s_arr, ref t_arr, key, length);
                
            //Key-scheduling algorithm (KSA)
            s_arr = ApplyKSA(s_arr, t_arr, length);

            //Pseudo-random generation algorithm (PRGA)
            int ind_1 = 0, ind_2 = 0;
            for (int i = 0; i < cipherText.Length; i++)
            {
                int key_stream_byte = ApplyPRGA(ref s_arr, ref ind_1, ref ind_2, length);
                string ans = XOR(Convert.ToString(key_stream_byte), Convert.ToString((int)cipherText[i]));
                plainText += (char)Convert.ToInt32(ans, 2);
            }

            return isHexa ? toHex(plainText) : plainText;
        }

        public override  string Encrypt(string plainText, string key)
        {
            string cipherText = "";

            bool isHexa = false;
            if (plainText.StartsWith("0x")) {

                key = ToString(key.Substring(2));
                plainText = ToString(plainText.Substring(2));
                isHexa = true;
            }
                
            const int length = 256;
            int[] s_arr = new int[length];
            int[] t_arr = new int[length];

            //initializing S and T
            InitializeArrays(ref s_arr, ref t_arr, key, length);

            //Key-scheduling algorithm (KSA)
            s_arr = ApplyKSA(s_arr, t_arr, length);

            //Pseudo-random generation algorithm (PRGA)
            int ind_1 = 0, ind_2 = 0;
            for (int i = 0; i < plainText.Length; i++)
            {
                int key_stream_byte = ApplyPRGA(ref s_arr, ref ind_1, ref ind_2, length);
                string ans = XOR(Convert.ToString(key_stream_byte), Convert.ToString((int)plainText[i]));
                cipherText += (char)Convert.ToInt32(ans, 2);
            }
            
            return isHexa?toHex(cipherText) : cipherText;
        }
    }
}
 ;