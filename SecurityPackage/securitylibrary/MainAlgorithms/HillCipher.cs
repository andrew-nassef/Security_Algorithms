using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher :  ICryptographicTechnique<List<int>, List<int>>
    {
        enum MatrixType
        {
            Key,
            Plain
        };
        // calculate modules
        private int mod(int det, int mod)
        {
            if (det >= 0) return det % mod;
            else return mod - (det * -1 % mod);
        }

        //Convert a 1d list to a 2d matrix
        private float[,] toMatrix(List<int> list, int m,MatrixType matrixType)
        {
            float[,] matrix;
            int n = list.Count / m;

            if (matrixType == MatrixType.Key)
                matrix = new float[m, m];
            else
                matrix = new float[m, n];

            for(int i = 0; i < m; i++)
            {
                int index = i;
                for(int j = 0; j < n; j++)
                {
                    if (matrixType == MatrixType.Key)
                        matrix[i, j] = list[i * n + j];
                    else
                    {
                        matrix[i, j] = list[index];
                        index += m;
                    }
                }
            }
            return matrix;
        }
        private List<int> toList(float[,] matrix)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    result.Add(mod((int)matrix[j, i]%26 , 26));
                }
            }
            return result;
        }
        private float[,] MatrixMult(float[,] matrix1, float[,] matrix2)
        {
            float[,] result = new float[matrix1.GetLength(0), matrix2.GetLength(1)];
            for (int i = 0; i < result.GetLength(0); i++)
                for (int j = 0; j < result.GetLength(1); j++)
                    result[i, j] = 0;

            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix1.GetLength(1); j++)
                {
                    for (int k = 0; k < matrix2.GetLength(1); k++)
                    {
                        result[i, k] += matrix1[i, j] * matrix2[j, k];
                    }
                }
            }
            return result;
        }
        //convert 2d matrix to 1d list
        
        //---------------------------------------------
        //calculate determinants
        private int det(float[,] matrix)
        {
            if (matrix.GetLength(0) == 2)
                return det2by2Matrix(matrix);
            else
                return det3by3Matrix(matrix);
        }
        private int det3by3Matrix(float[,] matrix)
        {
        return (int)(matrix[0, 0] * det2by2Matrix(new float[2, 2] { { matrix[1, 1], matrix[1, 2] }, { matrix[2, 1], matrix[2, 2] } })
                - matrix[0, 1] * det2by2Matrix(new float[2, 2] { { matrix[1, 0], matrix[1, 2] }, { matrix[2, 0], matrix[2, 2] } })
                + matrix[0, 2] * det2by2Matrix(new float[2, 2] { { matrix[1, 0], matrix[1, 1] }, { matrix[2, 0], matrix[2, 1] } }));
        }
        private int det2by2Matrix(float[,] matrix) => (int)(matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0]);
        private int det_neg_1(int dt)
        {
            for (int i = 0; i <= 26; i++)
            {
                if (dt * i % 26 == 1) return i;
            }
            //if there is no possible solution
            throw new SystemException();
        }
        //-------------------------Determinant section end-----------------------
        //---------------------------------------------
        //Matrices Inverse
        private float[,] InverseMatrix(float[,] matrix)
        {
            if (matrix.GetLength(0) == 2)
            {
                if (isInvertable(matrix))
                    return InverseMatrix2x2(matrix);
            }
            else
                return InverseMatrix3x3(matrix);

            throw new Exception();
        }
        
        private float[,] InverseMatrix3x3(float[,] matrix)
        {
            int dt = mod(det(matrix), 26);
            dt = det_neg_1(dt);
            float[,] cofactorsMatrix = new float[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int a = 0, b = 0;
                    float[,] tmp = new float[2, 2];
                    for (int k = 0; k < 3; k++)
                    {
                        for (int z = 0; z < 3; z++)
                        {
                            if (i != k && j != z)
                            {
                                tmp[a, b] = matrix[k, z];
                                if (b < 1) b++;
                                else
                                {
                                    a++;
                                    b = 0;
                                }
                            }
                        }
                    }
                    cofactorsMatrix[i, j] = det(tmp) * dt;
                    if ((i * 3 + j) % 2 != 0) cofactorsMatrix[i, j] *= -1;
                    cofactorsMatrix[i, j] = mod((int)cofactorsMatrix[i, j], 26);
                }
            }
            float[,] transposedMatrix = MatrixTranspose(cofactorsMatrix);
            return transposedMatrix;
        }

        private float[,] MatrixTranspose(float[,] matrix)
        {
            float[,] tmp = new float[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    tmp[j, i] = matrix[i, j];

            return tmp;
        }

        private float[,] InverseMatrix2x2(float[,] matrix)
        {
            int dt = mod(det(matrix), 26);
            dt = det_neg_1(dt);
            float tmp = matrix[0, 0];
            matrix[0, 0] = mod((int)(matrix[1, 1] * dt), 26);
            matrix[1, 1] = mod((int)(tmp * dt),26);
            matrix[0, 1] = mod((int)(matrix[0, 1] * - 1 * dt), 26);
            matrix[1, 0] = mod((int)(matrix[1, 0] * - 1 * dt), 26);
            return matrix;
        }
        private bool isInvertable(float[,] matrix)
        {
            if (det(matrix) == 0) throw new SystemException();
            
            if(matrix.GetLength(0) == 3)
            {
                if(det(matrix) == 0) throw new Exception();
            }
            return true;
        }
        //-------------------------Inverses section end-----------------------

        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            // convert key to MxM matrix
            float[,] keyMatrix = toMatrix(key, (int)Math.Sqrt(key.Count), MatrixType.Key);
            // convert plain text to MxN matrix
            float[,] plainTextMatrix = toMatrix(plainText, keyMatrix.GetLength(1), MatrixType.Plain);
            // K * P.T
            float[,] result = MatrixMult(keyMatrix, plainTextMatrix);
            return toList(result);
        }
        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            float[,] keyMatrix = toMatrix(key, (int)Math.Sqrt(key.Count), MatrixType.Key);
            float[,] cipherTextMatrix = toMatrix(cipherText, keyMatrix.GetLength(1), MatrixType.Plain);
            keyMatrix = InverseMatrix(keyMatrix);

            float[,] res = MatrixMult(keyMatrix, cipherTextMatrix);

            return toList(res);
        }
        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    for (int k = 0; k < 26; k++)
                    {
                        for (int l = 0; l < 26; l++)
                        {
                            List<int> Key = new List<int>{ i, j, k, l };
                            if (Encrypt(plainText, Key).SequenceEqual(cipherText))
                                return Key;
                        }
                    }
                }
            }
            // if key not found
            throw new InvalidAnlysisException();
        }

        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            float[,] plainMatrix = toMatrix(plainText, 3, MatrixType.Plain);
            float[,] cipherMatrix = toMatrix(cipherText, cipherText.Count / 3, MatrixType.Key);
            plainMatrix = InverseMatrix(plainMatrix);
            plainMatrix = MatrixTranspose(plainMatrix);

            float[,] res = MatrixMult(plainMatrix, cipherMatrix);
            List<int> key = toList(res);

            return key;
        }

    }
}
