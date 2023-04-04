using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class AES : CryptographicTechnique
    {
      
        public override string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }

        private string[,] toMatrix(string str)
        {
            string[,] matrix = new string[4, 4];
            int indexer = 2;
            for(int i = 0; i < matrix.GetLength(0); i++)
            {
                for(int j = 0;j<matrix.GetLength(1); j++)
                {
                    matrix[j, i] += str[indexer];
                    matrix[j, i] += str[indexer + 1];
                    indexer += 2;
                }
            }
            return matrix;
            
        }

        private string fixLength(string s, int length) {
            string tmp = "";
            for (int i = 0; i < length - s.Length; i++)
            {
                tmp += "0";
            }
            return tmp + s;
        }
        
        private string XOR(string v1, string v2)
        {
            v1 = Convert.ToString(Convert.ToInt32(v1, 16), 2);
            v2 = Convert.ToString(Convert.ToInt32(v2, 16), 2);
            int fixedLength = Math.Max(v1.Length, v2.Length);

            if(v1.Length < fixedLength)
                v1 = fixLength(v1, fixedLength);
           
            else if(v2.Length < fixedLength)
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

        private string[,] addRoundKey(string[,] plainMatrix, string[,] keyMatrix)
        {
            string[,] result = new string[plainMatrix.GetLength(0), plainMatrix.GetLength(1)];
            for(int i = 0;i < plainMatrix.GetLength(0); i++)
            {
                for(int j =0;j < plainMatrix.GetLength(1); j++)
                {
                    string answer = XOR(plainMatrix[i, j], keyMatrix[i, j]);
                    int a = Convert.ToInt32(answer, 2);
                    result[i, j] = Convert.ToInt32(a).ToString("X");
                }
            }

            return result;
        }
        //replace each string with it's S-box value
        private string[,] subBytes(string[,] matrix)
        {
            string[,] subbedMatrix = new string[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    subbedMatrix[i, j] = Matrices.replaceFromSBox(matrix[i, j]);
                }
            }
            return subbedMatrix;
        }

        private string[,] shiftRows(string[,] matrix)
        {
            string[,] result = new string[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[i, j] = matrix[i, (j + i) % matrix.GetLength(1)];
                }
            }

            return result;
        }
        private string[,] mixColumns(string[,] matrix) {
            return Matrices.MatrixMult(Matrices.RijndaelMixColumnsMatrix, matrix); ;
        }
        private string[,] generateRoundKey(string[,] prevKey, int round) {
            string[,] key = new string[prevKey.GetLength(0), prevKey.GetLength(1)];
            for (int i = 0;i <key.GetLength(0);i++) 
            {
                for(int j= 0;j < key.GetLength(1); j++)
                {
                    if(i > 0)
                    {
                        key[j, i] = XOR(key[j, i - 1], prevKey[j, i]);
                        key[j, i] = Convert.ToInt32(key[j, i], 2).ToString("X");
                    }
                    else
                    {
                        key[j, i] = prevKey[(j + 1)% key.GetLength(1), prevKey.GetLength(0) - 1];
                        key[j, i] = Matrices.replaceFromSBox(key[j, i]);
                        string x1 = XOR(key[j, i], prevKey[j, i]);
                        x1 = Convert.ToInt32(x1, 2).ToString("X");
                        string val = Matrices.RCtable[round - 1, j].ToString("X");
                        key[j, i] = XOR(x1, Matrices.RCtable[round - 1, j].ToString("X"));
                        key[j, i] = Convert.ToInt32(key[j, i], 2).ToString("X");
                        //xor
                    }
                }
            }
            return key;
        }
        public string[,] initiateState(string plainText, string key) {
            string[,] plainMatrix = toMatrix(plainText);
            string[,] keyMatrix = toMatrix(key);
            return addRoundKey(plainMatrix, keyMatrix);
        }
        public override string Encrypt(string plainText, string key)
        {
            //initial state
            string[,] matrix = initiateState(plainText, key);
            string[,] keyMatrix = toMatrix(key);
            for (int i = 1; i <= 10; i++)
            {
                keyMatrix = generateRoundKey(keyMatrix ,i);
                matrix = subBytes(matrix);
                matrix = shiftRows(matrix);
                if(i != 10)
                    matrix = mixColumns(matrix);
                matrix = addRoundKey(matrix, keyMatrix);
            }
            return toString(matrix);
        }

        public string toString(string[,] matrix) {
            string str = "0x";
            for(int i = 0;i < matrix.GetLength(0);i++)
            {
                for(int j = 0;j < matrix.GetLength(1); j++)
                {
                    if (matrix[j, i].Length == 1)
                        str += "0" + matrix[j, i];
                    else
                        str += matrix[j, i];
                }
            }
            return str;
        }

    }
}
