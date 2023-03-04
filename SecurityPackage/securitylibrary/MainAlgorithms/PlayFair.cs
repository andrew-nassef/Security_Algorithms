using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        private char[,] createMatrix(List<char> key)
        {
            string alphapet = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
            List<char> alphapets = alphapet.ToList();
            char[,] matrix = new char[5, 5];
            int index = 0;
            int index2 = 0;
            for(int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    if(index < key.Count)
                    {
                    matrix[i, j] = char.ToUpper(key[index++]);
                    alphapets.Remove(matrix[i, j]);
                    }
                    else
                    {
                        matrix[i, j] = alphapets[index2++]; 
                    }
                }
            }
            return matrix;
        }

        private List<string> makePairs(string plainText)
        {
            List<string> pairs = new List<string>();
            string tmp = "";
            tmp += plainText[0];
            tmp += plainText[1];
            for(int i = 2; i < plainText.Length; i++)
            {
                if (tmp.Length == 2)
                {
                    pairs.Add(tmp);
                    tmp = "";
                    tmp += plainText[i];
                }
                else
                {
                    if (tmp[0] == plainText[i])
                    {
                        tmp += 'X';
                        i--;
                    }
                    else 
                        tmp += plainText[i];
                }
                if (i == plainText.Length - 1)
                {
                    if (tmp.Length == 1)
                        tmp += 'X';
                    pairs.Add(tmp);
                }
            }
            return pairs;
        }

        private int[,] findPositions(string pair, char[,] matrix)
        {
            int[,] positions = new int[2, 2];

            for(int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    if (matrix[i, j] == pair[0])
                    {
                        positions[0, 0] = i;
                        positions[0, 1] = j;
                    }
                    else if(matrix[i, j] == pair[1])
                    {
                        positions[1, 0] = i;
                        positions[1, 1] = j;
                    }
                }
            }

            return positions;
        }

        public string Encrypt(string plainText, string key)
        {
            string answer = "";
            //1- Build 5*5 matrix
            char[,] matrix = createMatrix(key.Distinct().ToList());

            //2- Divide the plain text into pairs
            List<string> pairs = makePairs(plainText.ToUpper());

            for (int i = 0; i < pairs.Count; i++)
            {
                int[,] positions = findPositions(pairs[i], matrix);
                // same row
                if (positions[0, 0] == positions[1, 0])
                {
                    answer += matrix[positions[0, 0], (positions[0, 1] + 1) % 5];
                    answer += matrix[positions[1, 0], (positions[1, 1] + 1) % 5];
                }
                // same column
                else if (positions[0, 1] == positions[1, 1])
                {
                    answer += matrix[(positions[0, 0] + 1) % 5, positions[0, 1]];
                    answer += matrix[(positions[1, 0] + 1) % 5, positions[1, 1]];
                }
                // neither
                else
                {
                    answer += matrix[positions[0, 0], positions[1, 1]];
                    answer += matrix[positions[1, 0], positions[0, 1]];
                }
            }

            return answer;
        }
        
        public string Decrypt(string cipherText, string key)
        {
            string answer = "";

            return answer;
        }

    }
}
