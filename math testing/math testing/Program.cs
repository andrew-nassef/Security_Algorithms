internal class Program
{

    private static void Main(string[] args)
    {
        int[,] mat1 = new int[3, 4] { {1, 2,3, 4 },{5,6,7,8 },{9,10,11,12} };
        int[,] mat2 = new int[4, 3] { {1,2,3 },{4,5,6 },{7,8,9 },{10,11,12 } };
        int[,] mat = MatrixMult(mat1, mat2);
        foreach (int i in mat)Console.WriteLine(i);

        Console.WriteLine("----------------------------------------------------------");
        int[,] mat3 = new int[2, 2] { { 1, 2 }, { 3, 4 } };
        int[,] mat4 = new int[2, 2] { { 1, 2 }, { 3, 4 } };
        int[,] mat0 = MatrixMult(mat3, mat4);
        foreach (int i in mat0)Console.WriteLine(i);

        int[,] x22 = new int[2, 2] { {5, 6 }, {8, 9 } };
        int[,] x33 = new int[3, 3] { {8,5,3 }, {4,6,7 },{1,-1,20} };
        int det1 = det(x22);
        int det2 = det(x33);
        Console.WriteLine(det2);
    }
        private static int[,] MatrixMult(int[,] matrix1, int[,] matrix2)
    {
        int[,] result = new int[matrix1.GetLength(0), matrix2.GetLength(1)];
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
    private static int det(int[,] matrix)
    {
        if (matrix.GetLength(0) == 2)
            return det2by2Matrix(matrix);
        else
            return det3by3Matrix(matrix);
    }

    private static int det3by3Matrix(int[,] matrix)
    {
        return matrix[0, 0] * det2by2Matrix(new int[2, 2] { { matrix[1, 1], matrix[1, 2] },{ matrix[2, 1], matrix[2, 2] } })
            - matrix[0, 1] * det2by2Matrix(new int[2, 2] { { matrix[1, 0], matrix[1, 2] }, { matrix[2, 0], matrix[2, 2] } })
            + matrix[0, 2] * det2by2Matrix(new int[2, 2] { { matrix[1, 0], matrix[1, 1] }, { matrix[2, 0], matrix[2, 1] } });
    }

    private static int det2by2Matrix(int[,] matrix)
    {
        return matrix[0,0] * matrix[1, 1] - matrix[0,1] * matrix[1, 0];
    }
}