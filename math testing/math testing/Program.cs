internal class Program
{

    private static void Main(string[] args)
    {
        int[,] mat1 = new int[3, 4] { {1, 2,3, 4 },{5,6,7,8 },{9,10,11,12} };
        int[,] mat2 = new int[4, 3] { {1,2,3 },{4,5,6 },{7,8,9 },{10,11,12 } };
        int[,] mat = MatrixMult3D(mat1, mat2);
        Console.WriteLine(mat.GetLength(0));
    }

    private static int[,] MatrixMult3D(int[,] matrix1, int[,] matrix2)
    {
        int[,] result = new int[matrix1.GetLength(0), matrix2.GetLength(1)];
        for (int a = 0; a < result.GetLength(0); a++)
        {
            for (int b = 0; b < result.GetLength(1); b++)
            {
                    int res = 0;
                for (int c = 0; c < matrix1.GetLength(0); c++)
                {
                    result[a, b] = res;
                }
            }
        }
        return result;
    }
}