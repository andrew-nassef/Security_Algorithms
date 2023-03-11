using System.Numerics;

internal class Program
{
    static List<List<int>> result = new List<List<int>>();
    static void backtrack(List<int> s, int size)
    {
        if (s.Count == size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    if (s[i] == s[j]) return;
                }
            }
            result.Add(s);
            return;
        }
        for (int i = 1; i <= size; i++)
        {
            s.Add(i);
            backtrack(s, size);
            s.RemoveAt(s.Count - 1);
        }
    }
    private static void Main(string[] args)
    {

        backtrack(new List<int>(),5);
        int c = result.Count;

        Console.WriteLine((float)(1 / 3 % 26));
       /* float[,] mat1 = new float[3, 4] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } };
        float[,] mat2 = new float[4, 3] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 }, { 10, 11, 12 } };
        float[,] mat = MatrixMult(mat1, mat2);
        foreach (int i in mat)Console.WriteLine(i);

        Console.WriteLine("----------------------------------------------------------");
        float[,] mat3 = new float[2, 2] { { 1, 2 }, { 3, 4 } };
        float[,] mat4 = new float[2, 2] { { 1, 2 }, { 3, 4 } };
        float[,] mat0 = MatrixMult(mat3, mat4);
        foreach (int i in mat0)Console.WriteLine(i);
        Console.WriteLine("----------------------------------------------------------");

        float[,] x22 = new float[2, 2] { {5.0f, 6.0f }, {8.0f, 9.0f } };
        float[,] x33 = new float[3, 3] { {17f, 17f, 5f }, {21f,18f,21f },{2f,2f,19f} };
        float det1 = det(x22);
        float det2 = det(x33);
        Console.WriteLine(det2);
        Console.WriteLine("----------------------------------------------------------");
        float[,] xdash2x2 = InverseMatrix(x22);
        float[,] xdash3x3 = InverseMatrix(x33);
        foreach (float i in xdash3x3) Console.WriteLine(i);
        Console.WriteLine("----------------------------------------------------------");*/
    }
    private static int sign(int n) => n < 0 ? -1 : 1;

    private static float[,] InverseMatrix(float[,] matrix)
    {
        if(matrix.GetLength(0) == 2)
            return InverseMatrix2x2(matrix);
        else 
            return InverseMatrix3x3(matrix);
    }


    private static float[,] InverseMatrix3x3(float[,] matrix)
    {
        float dt = det(matrix);
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
                cofactorsMatrix[i, j] = det(tmp) / dt;
                if((i * 3 + j) % 2 != 0) cofactorsMatrix[i, j] *= -1;
            }
        }
        float[,] transposedMatrix = MatrixTranspose(cofactorsMatrix);
        return transposedMatrix;
    }

    private static float[,] MatrixTranspose(float[,] matrix)
    {
        float[,] tmp = new float[matrix.GetLength(0), matrix.GetLength(1)];
        for(int i = 0;i< matrix.GetLength(0);i++)
            for(int j = 0;j< matrix.GetLength(1);j++)
                tmp[j, i] = matrix[i,j];

        return tmp;
    }

    private static float[,] InverseMatrix2x2(float[,] matrix)
    {
        float dt = det(matrix); 
        float tmp = matrix[0, 0];
        matrix[0, 0] = matrix[1, 1] / dt;
        matrix[1, 1] = tmp / dt;
        matrix[0, 1] *= -1 / dt;
        matrix[1, 0] *= -1 / dt;
        return matrix;
    }

    private static float[,] MatrixMult(float[,] matrix1, float[,] matrix2)
    {
        float[,] result = new float[matrix1.GetLength(0), matrix2.GetLength(1)];
        for(int i = 0;i<result.GetLength(0);i++)
            for(int j = 0;j<result.GetLength(1);j++)
                result[i, j] = 0;
        for(int i = 0; i <matrix1.GetLength(0); i++)
        {
            for(int j = 0; j <matrix1.GetLength(1); j++)
            {
                for(int k = 0; k < matrix2.GetLength(1); k++)
                {
                        result[i,k] += matrix1[i,j] * matrix2[j, k];
                }
            }
        }
        return result;
    }
    private static float det(float[,] matrix)
    {
        if (matrix.GetLength(0) == 2)
            return det2by2Matrix(matrix);
        else
            return det3by3Matrix(matrix);
    }

    private static float det3by3Matrix(float[,] matrix)
    {
        return matrix[0, 0] * det2by2Matrix(new float[2, 2] { { matrix[1, 1], matrix[1, 2] },{ matrix[2, 1], matrix[2, 2] } })
            - matrix[0, 1] * det2by2Matrix(new float[2, 2] { { matrix[1, 0], matrix[1, 2] }, { matrix[2, 0], matrix[2, 2] } })
            + matrix[0, 2] * det2by2Matrix(new float[2, 2] { { matrix[1, 0], matrix[1, 1] }, { matrix[2, 0], matrix[2, 1] } });
    }

    private static float det2by2Matrix(float[,] matrix)
    {
        return matrix[0,0] * matrix[1, 1] - matrix[0,1] * matrix[1, 0];
    }
}