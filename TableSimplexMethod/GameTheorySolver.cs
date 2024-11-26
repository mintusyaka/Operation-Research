using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableSimplexMethod
{
	public class GameTheorySolver
	{
		public GameTheoryMinimizerTool minimizerTool;

        public GameTheorySolver(GameTheoryMinimizerTool minimizerTool)
        {
            this.minimizerTool = minimizerTool;
        }

        public double[,] Solve()
        {
            bool minimizeRow = true;
            bool minimizeCol = true;

            while (minimizeRow || minimizeCol)
            {
                double[] row = minimizerTool.getDominantedRow();
                if (row != null)
                {
                    minimizerTool.removeRow(row);
                    minimizeRow = true;

                }
                else
                    minimizeRow = false;

                double[] col = minimizerTool.getDominantCol();
                if (col != null)
                {
                    minimizerTool.removeCol(col);
                    minimizeCol = true;
                }
                minimizeCol = false;
            }

            return minimizerTool.getMatrix();
        }

        public double[,] GetClearMatrix()
        {
			int clearRows = CountNotNullRows();
            int clearCols = CountNotNullCols();

            int rows = minimizerTool.getMatrix().GetLength(0);
            int cols = minimizerTool.getMatrix().GetLength(1);


			double[,] clearMatrix = new double[clearRows, clearCols];
            double[,] matrix = minimizerTool.getMatrix();


			int clearIdx = 0;
            int clearJdx = 0;

            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    if (matrix[i,j] != -1)
                    {
                        clearMatrix[clearIdx, clearJdx++] = matrix[i,j];
                        if(clearJdx == clearCols)
                        {
                            clearIdx++;
                            clearJdx = 0;
                        }
                        if(clearIdx == clearRows)
                        {
                            break;
                        }
                    }
                }
            }

            return clearMatrix;
        }

        private int CountNotNullRows()
        {
            int rows = minimizerTool.getMatrix().GetLength(0);
            int notNullRows = 0;
            
            for(int i = 0; i < rows; ++i)
            {
                if (!IsNullRow(GameTheoryMinimizerTool.GetRow(
                    minimizerTool.getMatrix(), i)))
                {
                    notNullRows++;
                }
            }

            return notNullRows;
        }

		private int CountNotNullCols()
		{
			int cols = minimizerTool.getMatrix().GetLength(1);
			int notNullCols = 0;

			for (int i = 0; i < cols; ++i)
			{
				if (!IsNullRow(GameTheoryMinimizerTool.GetCol(
					minimizerTool.getMatrix(), i)))
				{
					notNullCols++;
				}
			}

			return notNullCols;
		}

		private bool IsNullRow(double[] row)
        {
            for(int i = 0; i < row.Length ; ++i)
            {
                if (row[i] != -1)
                    return false;
            }
            return true;
        }
    }
}
