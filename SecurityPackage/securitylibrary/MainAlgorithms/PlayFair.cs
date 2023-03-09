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

        private List<string> makePairsDec(string cipherText)
        {
            List<string> pairs = new List<string>();
            for(int i = 0;i< cipherText.Length; i += 2)
            {
                string tmp = "";
                tmp += cipherText[i];
                tmp += cipherText[i + 1];
                pairs.Add(tmp);
            }

            return pairs;
        }

        private int[,] findPositions(string pair, char[,] matrix)
        {
            int[,] positions = new int[2, 2];
            if (pair[0] == 'J') {
                string newPair = "";
                newPair += 'I';
                newPair += pair[1];
                pair = newPair;
                Console.WriteLine(pair);
            }
            else if (pair[1] == 'J')
            {
                string newPair = "";
                newPair += pair[0];
                newPair += 'I';
                pair = newPair;
                Console.WriteLine(pair);
            }

            for (int i = 0; i < 5; i++)
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
            //1- Build 5*5 matrix
            char[,] matrix = createMatrix(key.Distinct().ToList());

            //2- Divide the plain text into pairs
            List<string> pairs = makePairsDec(cipherText.ToUpper());

            string plainText = "";
            for (int i = 0; i < pairs.Count; i++)
            {
                bool isTrueX = true;
                string tmp = getDecrypt(pairs[i], matrix);
                
                if (tmp[1] == 'X')
                {
                    if(i < pairs.Count - 1)
                    {
                        string tmp2 = getDecrypt(pairs[i + 1], matrix);
                        if (tmp2[0] == tmp[0])
                            isTrueX = false;
                    }
                    else 
                        isTrueX = false;
                }

                if (isTrueX)
                    plainText += tmp;
                else 
                    plainText += tmp[0];
            }
            
           return plainText.ToLower();
        }

        private string getDecrypt(string pair, char[,] matrix)
        {
            int[,] positions = findPositions(pair, matrix);
            string tmp = "";
            // same row
            if (positions[0, 0] == positions[1, 0])
            {
                tmp += matrix[positions[0, 0], (positions[0, 1] - 1 + 5) % 5];
                tmp += matrix[positions[1, 0], (positions[1, 1] - 1 + 5) % 5];
            }
            // same column
            else if (positions[0, 1] == positions[1, 1])
            {
                tmp += matrix[(positions[0, 0] - 1 + 5) % 5, positions[0, 1]];
                tmp += matrix[(positions[1, 0] - 1 + 5) % 5, positions[1, 1]];
            }
            // neither
            else
            {
                tmp += matrix[positions[0, 0], positions[1, 1]];
                tmp += matrix[positions[1, 0], positions[0, 1]];
            }
        return tmp;
        }
    }
}
