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
        /*
         * 1. Key Expansion: The same key used for encryption is used for decryption, so the first step is to expand the key. This involves generating a series of round keys from the original key using a key schedule.

            2. Add Round Key: In this step, the final round key is XORed with the last block of ciphertext to obtain the first block of plaintext.

            3. Inverse Shift Rows: The rows of the block are shifted to the right. The number of shifts is the inverse of the number of shifts used in the shift rows step during encryption.

            4. Inverse SubBytes: The inverse S-box is applied to each byte of the block.

            5. Add Round Key: The round key generated in step 1 is XORed with the block.

            6. Inverse MixColumns: This step involves applying an inverse matrix operation to each column of the block.

            7. Repeat steps 3 to 6: The inverse shift rows, inverse subbytes, inverse mixcolumns, and add round key steps are repeated for each round of decryption.

            8. Final Round: In the final round, the inverse shift rows, inverse subbytes, and add round key steps are performed as in the previous rounds. However, the inverse mixcolumns step is omitted.

            Output: The result of the final round is the decrypted plaintext.
         * 
         * 
         * */

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

            return toString(matrix);
        }

        private string fixLength(string s, int length) => s.PadLeft(length, '0');

        private string XOR(string v1, string v2)
        {
            v1 = HelperMethods.HexaToBinary(v1);
            v2 = HelperMethods.HexaToBinary(v2);

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
        private string[,] generateRoundKey(string[,] prevKey, int round) {
            string[,] key = new string[prevKey.GetLength(0), prevKey.GetLength(1)];
            for (int i = 0;i <key.GetLength(0);i++) 
            {
                for(int j= 0;j < key.GetLength(1); j++)
                {
                    if(i > 0)
                    {
                        key[j, i] = XOR(key[j, i - 1], prevKey[j, i]);
                        key[j, i] = HelperMethods.toHexa(key[j, i]);
                    }
                    else
                    {
                        // 1. shift row
                        key[j, i] = prevKey[(j + 1)% key.GetLength(1), prevKey.GetLength(0) - 1];
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
