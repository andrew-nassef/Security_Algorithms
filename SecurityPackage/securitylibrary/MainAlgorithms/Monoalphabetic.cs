using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        //Convert list of characters to lower case string
        private string toString(List<char> list) {
            string tmp = "";
            foreach (var i in list) tmp += i;
            return tmp.ToLower();
        }

        public string Analyse(string plainText, string cipherText)
        {
            string alphapet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            //Convert all lists to upper case letters
            plainText = plainText.ToUpper();
            cipherText = cipherText.ToUpper();

            //Get key from given plain text and cipher text
            List<char> answer = alphapet.ToList();
            for(int i = 0;i < plainText.Length; i++)
                answer[(int)plainText[i] - 65] = cipherText[i];

            //find unused letters in this cipher
            string emptyIndices = "";
            string unMappedChars = "";
            for (int i = 0; i < alphapet.Length; i++)
            {
                if (!plainText.Contains(alphapet[i]))
                    emptyIndices += alphapet[i];
                if (!cipherText.Contains(alphapet[i]))
                    unMappedChars += alphapet[i];
            }

            //fill un mapped letters we previously searched for
            for (int i = 0;i < emptyIndices.Length; i++)
                answer[emptyIndices[i] - 65] = unMappedChars[i];

            //return the final answer
            return toString(answer);
        }

        public string Decrypt(string cipherText, string key)
        {
            string answer = "";

            cipherText = cipherText.ToUpper();
            key = key.ToUpper();
            
            foreach (char c in cipherText)
            {
                answer += (char)(key.IndexOf(c) + 65);
            }

            return answer;
        }

        public string Encrypt(string plainText, string key)
        {
            string answer = "";

            plainText = plainText.ToUpper();

            foreach (char c in plainText)
            {
                answer += key[(int)c - 65];
            }

            return answer;
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            throw new NotImplementedException();
        }
    }
}
