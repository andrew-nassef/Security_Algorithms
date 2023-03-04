using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        /*Dictionary<int, char> map = new Dictionary<int, char>(){ 
            { 0, 'A' }, { 1, 'B' }, { 2, 'C' }, { 3, 'D' }, { 4, 'E' }, { 5, 'F' }, { 6, 'G' }, { 7, 'H' }, { 8, 'I' },
            { 9, 'J' }, { 10, 'K' }, { 11, 'L' }, { 12, 'M' }, { 13, 'N' }, { 14, 'O' }, { 15, 'P' }, { 16, 'Q' }, { 17, 'R' },
            { 18, 'S' }, { 19, 'T' }, { 20, 'U' }, { 21, 'V' }, { 22, 'W' }, { 23, 'X' }, { 24, 'Y' }, { 25, 'Z' }};*/
        public string Encrypt(string plainText, int key)
        {
            string answer = "";

            foreach(char c in plainText.ToUpper())
                answer += (char)(((c + key - 65) % 26) + 65);
            
            return answer;
        }

        public string Decrypt(string cipherText, int key)
        {
            string answer = "";

            foreach (char c in cipherText.ToUpper())
                answer += (char)(((c - 65 - key + 26) % 26) + 65);

            return answer;
        }

        public int Analyse(string plainText, string cipherText)
        {
            return ((char.ToUpper(cipherText[0]) - 65) - (char.ToUpper(plainText[0]) - 65) + 26) % 26;
        }
    }
}
