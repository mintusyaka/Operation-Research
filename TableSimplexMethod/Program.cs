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
        double[][] FunctionsCoefs = { [1, 1, 0, 0, 0] };
        double[] P = [1, 1, 1];
        double[,] LimitCoefs = {
                    {9, 0, 1, 0, 0},
                    {5, 6, 0, 1, 0},
                    {6, 1, 0, 0, 1 }
                };

        *//*int Rows = 2, Columns = 4;
        double[][] FunctionsCoefs = { [8, 6, 0, 0] };
        double[] P = [19, 16];
        double[,] LimitCoefs = {
                      {2, 5, 1, 0},
                      {4, 1, 0, 1}
                  };

        int Rows = 3, Columns = 5;
        double[][] FunctionsCoefs = { [1, 1, 2, 0, 0] };
        double[] P = [8, -4, -6];
        double[,] LimitCoefs = {
                    {1, 1, 1, 0, 0},
                    {-1, 1, 0, 1, 0},
                    {-1, -1, 0, 0, 1}

                };

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
        }*//*

        ModelOfLinearProgramming mp = new ModelOfLinearProgramming(Rows, Columns, FunctionsCoefs[0], LimitCoefs, P);

        OptimalAlgorithms.SimplexMethodAlgorithm(mp);*/

        double[,] matrix = {
			{ 1,1,2,5,5,4 },
            { 9,0,4,3,6,5 },
			{ 2,8,0,4,7,2 },
            { 2,1,9,2,3,0 },
            { 8,7,5,6,7,6 },
            { 7,6,6,7,7,1 }
		};

        GameTheorySolver gts = new GameTheorySolver(new GameTheoryMinimizerTool(matrix));

        double[,] m = gts.Solve();

		for (int i = 0; i < m.GetLength(0); i++)
		{
			for (int j = 0; j < m.GetLength(1); j++)
			{
				Console.Write(m[i, j] + "\t");
			}
			Console.WriteLine();
		}
		Console.WriteLine();

        m = gts.GetClearMatrix();
		for (int i = 0; i < m.GetLength(0); i++)
		{
			for (int j = 0; j < m.GetLength(1); j++)
			{
				Console.Write(m[i, j] + "\t");
			}
			Console.WriteLine();
		}
		Console.WriteLine();

		int RowsX = m.GetLength(0), ColumnsX = m.GetLength(1);
		double[][] FunctionsCoefsX = new double[1][];

        double[] fCoefs = new double[RowsX + ColumnsX];
        for (int i = 0; i < RowsX + ColumnsX; i++)
        {
            if (i < ColumnsX)
                fCoefs[i] = 1;
            else
                fCoefs[i] = 0;
        }
        FunctionsCoefsX[0] = fCoefs;

		double[] PX = new double[RowsX];
        for(int i = 0; i < RowsX; i++)
        {
            PX[i] = 1;
        }

		/*double[,] LimitCoefs = {
			{9, 0, 1, 0, 0},
			{5, 6, 0, 1, 0},
			{6, 1, 0, 0, 1 }
		};*/

		double[,] LimitCoefsX = new double[RowsX, RowsX + ColumnsX];
        for(int i = 0; i < RowsX; i++)
        {
            for(int j = 0; j < ColumnsX; ++j)
            {
                LimitCoefsX[i, j] = m[i,j];
            }
            LimitCoefsX[i, i + ColumnsX] = 1;
        }

		ModelOfLinearProgramming mp = new ModelOfLinearProgramming(RowsX, ColumnsX + RowsX, FunctionsCoefsX[0], LimitCoefsX, PX);

        OptimalAlgorithms.SimplexMethodAlgorithm(mp);

        double[] q = new double[ColumnsX];
        double Q = 0;
        Console.WriteLine();
        Console.Write("qi: ");
        for (int i = 0; i < ColumnsX; i++)
        {
            q[i] = mp.FreeMembersCoeffs[i];
            Q += q[i];
            Console.Write(Math.Round(q[i], 3) + ", ");
        }
        Console.WriteLine("Q* = " + Math.Round(Q, 3));

		double[] p = new double[RowsX];
		double P = 0;
		Console.Write("pi: ");
		for (int i = 0; i < RowsX; i++)
		{
            for(int j = 0; j < RowsX; j++)
            {
                p[i] += mp.ObjectiveCoeffs[j] * mp.MatrixCoeffs[j, ColumnsX + i];

			}
			P += p[i];
			Console.Write(Math.Round(p[i], 3) + ", ");
		}
		Console.WriteLine("P* = " + Math.Round(P, 3));

        double V = 1 / Q;
		Console.WriteLine("Game Value = " + Math.Round(V, 3));

		Console.Write("Optimal B player strategy: (qi * V) = (");
        for(int i = 0; i < ColumnsX; i++)
        {
            Console.Write(q[i] * V);
            if (i != ColumnsX - 1)
                Console.Write("; ");
        }
        Console.WriteLine(")");
		Console.Write("Optimal A player strategy: (pi * V) = (");
		for (int i = 0; i < RowsX; i++)
		{
			Console.Write(p[i] * V);
			if (i != RowsX - 1)
				Console.Write("; ");
		}
		Console.WriteLine(")");
	}
}