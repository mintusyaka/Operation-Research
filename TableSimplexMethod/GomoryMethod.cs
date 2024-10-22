using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TableSimplexMethod
{
    internal class GomoryMethod : Table
    {
        private int MainVariablesCount = 0;

        private SimplexMethodTable _simplexSM;
        private DualSimplexMethodTable _dualSM;

        public GomoryMethod(ModelOfLinearProgramming model) : base(model)
        {
            MainVariablesCount = Math.Abs(model.Columns - model.Rows);
            _simplexSM = new SimplexMethodTable(model);
            _dualSM = new DualSimplexMethodTable(model);
        }

        public new void BuildTable()
        {
            _simplexSM.BuildTable();
            _model = _simplexSM.Model;
            _basis = _simplexSM.Basis;

            while (!isValidSolution())
            {
                RecalculateTable();
            }
        }

        protected override bool isValidSolution()
        {
            for(int i = 0; i < MainVariablesCount; i++)
            {
                if (Fraction(_model.FreeMembersCoeffs[i]) >= Constants.EPS)
                    return false;
            }
            return true;
        }

        protected override void RecalculateTable()
        {

            AddNewLimitation(GetIndexOfRowWithBiggestFraction());
            
            Console.WriteLine("\n--- AFTER ADDING NEW LIMITATION ---\n");

            _dualSM = new DualSimplexMethodTable(_model, _basis);
            _dualSM.BuildTable();

            _model = _dualSM.Model;
            _basis = _dualSM.Basis;
        }

        private void AddNewLimitation(int idx)
        {
            int newRows = _model.Rows + 1;
            int newCols = _model.Columns + 1;
            double[,] newMatrixCoeffs = new double[newRows, newCols];
            double[] newFreeMembersCoeffs = new double[newRows];
            double[] newObjectiveCoeffs = new double[newCols];
            int[] newBasis = new int[newRows];

            for(int i = 0; i < newCols - 1; ++i)
            {
                newObjectiveCoeffs[i] = _model.ObjectiveCoeffs[i];
            }
            newObjectiveCoeffs[newCols - 1] = 0;

            for(int i = 0; i < newRows - 1; ++i)
            {
                newFreeMembersCoeffs[i] = _model.FreeMembersCoeffs[i];
                newBasis[i] = _basis[i];
            }

            newBasis[newRows - 1] = newCols - 1;
            double baseFraction = Fraction(_model.FreeMembersCoeffs[idx]);

            //copy matrix
            for (int i = 0; i < newRows - 1; i++)
            {
                for (int j = 0; j < newCols; j++)
                {
                    if(j == newCols - 1)
                    {
                        newMatrixCoeffs[i, j] = 0;
                        break;
                    }
                    newMatrixCoeffs[i, j] = _model.MatrixCoeffs[i, j];
                }
            }

            for(int i = 0; i < newCols - 1; i++)
            {
                double fraction = Fraction(_model.MatrixCoeffs[idx, i]);

                if(i < MainVariablesCount)
                {
                    if(fraction <= baseFraction)
                    {
                        newMatrixCoeffs[newRows - 1, i] = -fraction;
                    }
                    else
                    {
                        newMatrixCoeffs[newRows - 1, i] = baseFraction / (1.0 - baseFraction) * (1 - fraction);
                    }
                }
                else
                {
                    if (_model.MatrixCoeffs[idx, i] >= 0)
                    {
                        newMatrixCoeffs[newRows - 1, i] = -_model.MatrixCoeffs[idx, i];
                    }
                    else
                    {
                        newMatrixCoeffs[newRows - 1, i] = baseFraction / (1.0 - baseFraction) * Math.Abs(_model.MatrixCoeffs[idx, i]);
                    }
                }
            }

            newMatrixCoeffs[newRows - 1, newCols - 1] = 1;

            newFreeMembersCoeffs[newRows - 1] = -baseFraction;

            _model = new ModelOfLinearProgramming(newRows, newCols, newObjectiveCoeffs, newMatrixCoeffs, newFreeMembersCoeffs);
            _basis = newBasis;
        }

        private double Fraction(double value)
        {
            if (value < 0)
                return 1.0 - Math.Abs(value - Math.Floor(value));
            else
                return Math.Abs(value - Math.Floor(value));
        }

        private int GetIndexOfRowWithBiggestFraction()
        {
            int biggestFractionIdx = 0;
            for (int i = 0; i < MainVariablesCount; i++)
            {
                if (Fraction(_model.FreeMembersCoeffs[i]) > Fraction(_model.FreeMembersCoeffs[biggestFractionIdx]))
                {
                    biggestFractionIdx = i;
                }
            }
            return biggestFractionIdx;
        }
    }
}
