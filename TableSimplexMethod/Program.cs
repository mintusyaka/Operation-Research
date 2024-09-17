using System;
using System.Collections;
using System.Linq;
using TableSimplexMethod;
class Program
{
    public static void Main(string[] args)
    {
        Console.SetWindowSize(Console.WindowWidth * 2, Console.WindowHeight);

        /*int Rows = 3, Columns = 5;
        double[][] FunctionsCoefs = { [-1, -1, 0, 0, 0] };
        double[] P = [-4, -4, 10];
        double[,] LimitCoefs = {
            {-2, -1, 1, 0, 0},
            { 2, -2, 0, 1, 0 },
            { 1, 1, 0, 0, 1}
        };
*/
        int Rows, Columns;
        double[][] FunctionsCoefs;
        double[] P;
        double[,] LimitCoefs;

                try
                {
                    Console.WriteLine("Enter rows count: ");
                    string? sRowsCount = Console.ReadLine();
                    Console.WriteLine("Enter columns count: ");
                    string? sColumnsCount = Console.ReadLine();

                    if (!(sRowsCount != null && sColumnsCount != null))
                        throw new Exception("Enter correct rows and column nums!");

                    Rows = int.Parse(sRowsCount);
                    Columns = int.Parse(sColumnsCount);

                    FunctionsCoefs = new double[Rows + 1][];
                    string? sObjFunctionCoefs = "";
                    string[] sArr;

                    for (int i = 0; i < Rows + 1; i++)
                    {
                        if (i == 0)
                        {
                            Console.WriteLine("Enter Objective Function's Coefs: ");
                        }
                        else
                        {
                            Console.WriteLine($"Enter {i} Limit Function's Coefs: ");
                        }

                        sObjFunctionCoefs = Console.ReadLine();
                        if (sObjFunctionCoefs == null)
                            throw new Exception("Enter correct coefs nums!");

                        sArr = sObjFunctionCoefs.Split(' ');
                        if (sArr.Length > Columns)
                            throw new Exception("Enter correct coefs count!");

                        FunctionsCoefs[i] = sArr.Select(double.Parse).ToArray();
                    }

                    Console.WriteLine("Enter Free Members Coefs: ");
                    sObjFunctionCoefs = Console.ReadLine();
                    if (sObjFunctionCoefs == null)
                        throw new Exception("Enter correct coefs nums!");

                    sArr = sObjFunctionCoefs.Split(' ');
                    if (sArr.Length > Rows)
                        throw new Exception("Enter correct coefs count!");

                    P = sArr.Select(double.Parse).ToArray();
                    LimitCoefs = new double[Rows, Columns];

                    for (int i = 0; i < Rows; ++i)
                    {
                        for (int j = 0; j < Columns; ++j)
                        {
                            LimitCoefs[i, j] = FunctionsCoefs[i + 1][j];
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
        
        ModelOfLinearProgramming mp = new ModelOfLinearProgramming(Rows, Columns, FunctionsCoefs[0], LimitCoefs, P);
        OptimalAlgorithms.SimplexMethodAlgorithm(mp);
    }
}