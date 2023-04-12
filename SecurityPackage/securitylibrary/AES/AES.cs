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
        // 1. common functions-------------------------------
        //start 
        private string fixLength(string s, int length) => s.PadLeft(length, '0');

        private string XOR(string v1, string v2)
        {
            v1 = HelperMethods.HexaToBinary(v1);
            v2 = HelperMethods.HexaToBinary(v2);

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
        private string[,] generateRoundKey(string[,] prevKey, int round)
        {
            string[,] key = new string[prevKey.GetLength(0), prevKey.GetLength(1)];
            for (int i = 0; i < key.GetLength(0); i++)
            {
                for (int j = 0; j < key.GetLength(1); j++)
                {
                    if (i > 0)
                    {
                        key[j, i] = XOR(key[j, i - 1], prevKey[j, i]);
                        key[j, i] = HelperMethods.toHexa(key[j, i]);
                    }
                    else
                    {
                        // 1. shift row
                        key[j, i] = prevKey[(j + 1) % key.GetLength(1), prevKey.GetLength(0) - 1];
                        // 2. sub bytes
                        key[j, i] = Matrices.replaceFromSBox(key[j, i]);
                        // 3. XOR with value from previous key
                        string x1 = XOR(key[j, i], prevKey[j, i]);
                        x1 = HelperMethods.toHexa(x1);
                        // 4. XOR with RC table
                        key[j, i] = XOR(x1, Matrices.RCtable[round - 1, j].ToString("X"));
                        key[j, i] = HelperMethods.toHexa(key[j, i]);
                        //xor
                    }
                }
            }
            return key;
        }
        // end of common functions section-------------------------------------------

        //-------------------- 2.) decrypt section ------------------------------------------------------------------
        private string[,] inverseShiftRows(string[,] matrix) 
        {
            string[,] result = new string[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[i, j] = matrix[i, (j - i + matrix.GetLength(1)) % matrix.GetLength(1)];
                }
            }
            return result;
        }   
        private string[,] inverseSubBytes(string[,] matrix)
        {
            string[,] result = new string[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[i, j] = Matrices.replaceFromReversedSBox(matrix[i, j]);
                }
            }
                    return result;
        }
        private string[,] inverseMixColumn(string[,] matrix)
        {
            return Matrices.MatrixMult(Matrices.RijndaelInverseMixColumnsMatrix,HelperMethods.matrixToInt(matrix));
        }

        public override string Decrypt(string cipherText, string key)
        {
            string[,] keyMatrix = HelperMethods.toMatrix(key);
            string[,] cipherMatrix = HelperMethods.toMatrix(cipherText);
            Dictionary<int, string[,]> keys = new Dictionary<int, string[,]>();
            for(int i = 1; i <= 10; i++)
            {
                if(i == 1)
                    keys[i] = generateRoundKey(keyMatrix, i);
                else
                    keys[i] = generateRoundKey(keys[i - 1], i);
            }

            string[,] matrix = new string[cipherMatrix.GetLength(0), cipherMatrix.GetLength(1)];

            for (int i = 10; i >=1; i--)
            {
                if(i == 10)
                    matrix = addRoundKey(keys[i], cipherMatrix);
                else
                    matrix = addRoundKey(keys[i], matrix);
                if(i != 10)
                    matrix = inverseMixColumn(matrix);
                matrix = inverseShiftRows(matrix);
                matrix = inverseSubBytes(matrix);
            }
            //final round (round: 0)
            matrix = addRoundKey(matrix, keyMatrix);

            return HelperMethods.toString(matrix);
        }
        //---------------------------------------- End Decrypt-------------------------


        //----------------------------------------- 3.) Encrypt section ----------------------------
        private string[,] addRoundKey(string[,] matrix, string[,] keyMatrix)
        {
            string[,] result = new string[matrix.GetLength(0), matrix.GetLength(1)];
            for(int i = 0;i < matrix.GetLength(0); i++)
            {
                for(int j =0;j < matrix.GetLength(1); j++)
                {
                    string answer = XOR(matrix[i, j], keyMatrix[i, j]);
                    result[i, j] = HelperMethods.BinaryToHexa(answer);
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
            return Matrices.MatrixMult(Matrices.RijndaelMixColumnsMatrix, HelperMethods.matrixToInt(matrix));
        }
        // make initial round (round 0)
        public string[,] initiateState(string plainText, string key) {
            string[,] plainMatrix = HelperMethods.toMatrix(plainText);
            string[,] keyMatrix = HelperMethods.toMatrix(key);
            return addRoundKey(plainMatrix, keyMatrix);
        }
        public override string Encrypt(string plainText, string key)
        {
            //initial state
            string[,] matrix = initiateState(plainText, key);
            string[,] keyMatrix = HelperMethods.toMatrix(key);
            //apply for 10 rounds 
            for (int i = 1; i <= 10; i++)
            {
                keyMatrix = generateRoundKey(keyMatrix ,i);
                matrix = subBytes(matrix);
                matrix = shiftRows(matrix);
                // mix columns for only first 9 rounds
                if(i != 10)
                    matrix = mixColumns(matrix);
                matrix = addRoundKey(matrix, keyMatrix);
            }
            return HelperMethods.toString(matrix);
        }
        //----------------------------------------- end Encrypt----------------------------

    }
}
