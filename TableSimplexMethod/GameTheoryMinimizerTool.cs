using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TableSimplexMethod
{
	public class GameTheoryMinimizerTool
	{
		double[,] matrix;
		public GameTheoryMinimizerTool(double[,] matrix) {
			this.matrix = matrix;
		}

		public double[,] getMatrix() { return matrix; }

		public double[,] removeRow(int idx)
		{
			for(int i = 0; i < matrix.GetLength(1); i++)
			{
				matrix[idx, i] = -1;
			}
			return matrix;
		}

		public double[,] removeRow(double[] row)
		{
			int idx = GetRowIdx(row);
			return removeRow(idx);
		}

		private int GetRowIdx(double[] row)
		{
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				if (IsSameRows(row, GetRow(matrix, i)))
					return i;
			}
			return -1;
		}

		private bool IsSameRows(double[] row1, double[] row2)
		{
			for(int i = 0; i < row1.Length; ++i)
			{
				if (row1[i] != row2[i])
					return false;
			}
			return true;
		}

		public double[] getDominantedRow()
		{
			double[] dominantRow = getDominantRow();
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				if(isDominate(dominantRow, GetRow(matrix, i)))
				{
					return GetRow(matrix, i);
				}
			}
			return null;
		}

		private bool isDominate(double[] dominantRow, double[] row)
		{
			if (dominantRow == null || row == null)
				return false;

			bool isTheSameRows = true;
			for (int i = 0; i < row.GetLength(0); i++)
			{
				if (row[i] < 0)
					continue;
				if (dominantRow[i] == row[i] && isTheSameRows)
				{
					isTheSameRows = true;
					continue;
				}
				else if (dominantRow[i] < row[i])
				{
					isTheSameRows = false;
					return false;
				}
				else
				{
					isTheSameRows = false;
				}
			}
			return true && !isTheSameRows;
		}

		private double[] getDominantRow()
		{
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				double[] row = GetRow(matrix, i);

				if(isDominantRow(row))
				{
					return row;
				}
			}
			return null;
		}

		private bool isDominantRow(double[] row)
		{
			bool isDominantRow = false;
			bool isTheSameRows = false;

			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				isTheSameRows = true;
				isDominantRow = false;
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					if (matrix[i, j] < 0)
						continue;
					if (matrix[i, j] == row[j] && isTheSameRows)
					{
						isTheSameRows = true;
						continue;
					}
					else if (row[j] < matrix[i, j])
					{
						isDominantRow = false;
						break;
					}
					else
					{
						isTheSameRows = false;
						isDominantRow = true;
					}
				}
				if (isDominantRow)
					return isDominantRow;
			}

			return isDominantRow;
		}




		public double[,] removeCol(int idx)
		{
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				matrix[i, idx] = -1;
			}
			return matrix;
		}

		public double[,] removeCol(double[] row)
		{
			int idx = GetColIdx(row);
			return removeCol(idx);
		}

		private int GetColIdx(double[] col)
		{
			for (int i = 0; i < matrix.GetLength(1); i++)
			{
				if (IsSameRows(col, GetCol(matrix, i)))
					return i;
			}
			return -1;
		}

		public double[] getDominantCol()
		{
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				double[] col = GetCol(matrix, i);

				if (isDominantCol(col))
				{
					return col;
				}
			}
			return null;
		}

		private bool isDominantCol(double[] col)
		{
			bool isDominantCol = false;
			bool isTheSameCols = false;

			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				isTheSameCols = true;
				isDominantCol = false;
				for (int j = 0; j < matrix.GetLength(1); j++)
				{
					if (matrix[j, i] < 0)
						continue;
					if (matrix[j, i] == col[j] && isTheSameCols)
					{
						isTheSameCols = true;
						continue;
					}
					else if (col[j] < matrix[j, i])
					{
						isDominantCol = false;
						break;
					}
					else
					{
						isTheSameCols = false;
						isDominantCol = true;
					}
				}
				if (isDominantCol)
					return isDominantCol;
			}

			return isDominantCol;
		}


		public static double[] GetCol(double[,] matrix, int colIndex)
		{
			int rows = matrix.GetLength(0);
			double[] col = new double[rows];
			for(int i = 0; i < rows; i++)
			{
				col[i] = matrix[i, colIndex];
			}
			return col;
		}
		public static double[] GetRow(double[,] matrix, int rowIndex)
		{
			int cols = matrix.GetLength(1); // Number of columns
			double[] row = new double[cols];
			for (int i = 0; i < cols; i++)
			{
				row[i] = matrix[rowIndex, i];
			}
			return row;
		}


	}
}
