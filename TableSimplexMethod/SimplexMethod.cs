using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TableSimplexMethod
{
    internal static class OptimalAlgorithms
    {
        public static void SimplexMethodAlgorithm(ModelOfLinearProgramming mlp)
        {
            ITable smtable = new SimplexMethodTable(mlp);
            smtable.BuildTable();
        }

        public static void DualSimplexMethodAlgorithm(ModelOfLinearProgramming mlp)
        {
            ITable smtable = new DualSimplexMethodTable(mlp);
            smtable.BuildTable();
        }

        public static void GomoryMethodAlgorithm(ModelOfLinearProgramming mlp)
        {
            GomoryMethod gomory = new GomoryMethod(mlp);
            gomory.BuildTable();
        }
    }

    // клас який викликає метод, який буде давати нові межі.
    // ці межі треба підставити у модель програмування
    // тоді пройтись двоїстим симплекс методом
    // пеервірити умови Методом Гоморі. Чи цілочисельний розв'язок?


    interface ITable
    {
        public void BuildTable();

    }
    abstract class Table : ITable
    {
        protected ModelOfLinearProgramming _model;
        protected ModelOfLinearProgramming _tempModel;
        protected int[] _basis;

        public int[] Basis { get { return _basis; } }
        public ModelOfLinearProgramming Model { get { return _model; } }


        protected double[] _Q;
        public double[] Q { get { return _Q; } }
        public Table(ModelOfLinearProgramming model)
        {
            _tempModel = new ModelOfLinearProgramming(model);
            _model = model;
            _Q = new double[_model.Columns];
            FindBasis();
        }
        protected char symbol;

        public void BuildTable()
        {
            PrintTable();

            for (int i = 0; i < _model.Rows; i++)
            {
                PrintRow(_basis[i], _model.ObjectiveCoeffs[_basis[i]], _model.FreeMembersCoeffs[i], i);
            }

            PrintLastRow();

            if (!isValidSolution())
            {
                RecalculateTable();
            }
        }
        abstract protected void RecalculateTable();
        protected void RectangleMethod(int leadRow, int leadColumn)
        {
            for (int i = 0; i < _model.Rows; ++i)
            {
                for (int j = 0; j < _model.Columns; ++j)
                {
                    if (i == leadRow)
                    {
                        if (j == leadColumn)
                        {
                            _tempModel.MatrixCoeffs[i, j] = 1.0;
                        }
                        else
                        {
                            _tempModel.MatrixCoeffs[i, j] = _model.MatrixCoeffs[i, j] / _model.MatrixCoeffs[leadRow, leadColumn];
                        }
                    }
                    else
                    {
                        if (j == leadColumn)
                        {
                            _tempModel.MatrixCoeffs[i, j] = 0.0;
                        }
                        else
                        {
                            _tempModel.MatrixCoeffs[i, j] = _model.MatrixCoeffs[i, j] - (_model.MatrixCoeffs[leadRow, j] * _model.MatrixCoeffs[i, leadColumn] / _model.MatrixCoeffs[leadRow, leadColumn]);
                        }
                    }
                }

                if (i == leadRow)
                {
                    _tempModel.FreeMembersCoeffs[i] = _model.FreeMembersCoeffs[i] / _model.MatrixCoeffs[leadRow, leadColumn];
                }
                else
                {
                    _tempModel.FreeMembersCoeffs[i] = _model.FreeMembersCoeffs[i] - _model.FreeMembersCoeffs[leadRow] * _model.MatrixCoeffs[i, leadColumn] / _model.MatrixCoeffs[leadRow, leadColumn];
                }
            }

            ModelOfLinearProgramming temp = _model;
            _model = _tempModel;
            _tempModel = temp;
        }
        abstract protected bool isValidSolution();
        protected void PrintTable()
        {
            Console.Write($"\t{symbol}b\t|\tcb\t|\tP0\t");
            for (int i = 0; i < _model.Columns; ++i)
            {
                Console.Write($"|\tP{i + 1}\t");
            }
            Console.Write("\n");

            PrintDividerLine('=');

        }
        protected void PrintRow(int xb, double cb, double P0, int i)
        {
            Console.Write($"\t{symbol}{xb + 1}\t|\t{Math.Round(cb, 3)}\t|\t{Math.Round(P0, 3)}\t");
            for (int j = 0; j < _model.Columns; ++j)
            {
                Console.Write($"|\t{Math.Round(_model.MatrixCoeffs[i, j], 3)}\t");
            }
            Console.Write('\n');

            PrintDividerLine('-');
        }
        protected void PrintLastRow()
        {
            double Qc = 0;

            for (int i = 0; i < _model.Rows; i++)
            {
                Qc += _model.ObjectiveCoeffs[_basis[i]] * _model.FreeMembersCoeffs[i];
            }

            Console.Write($"\tQ\t|\t \t|\t{Math.Round(Qc, 3)}\t");

            for (int i = 0; i < _model.Columns; ++i)
            {
                double tempQ = 0;

                for (int j = 0; j < _model.Rows; j++)
                {
                    tempQ += _model.ObjectiveCoeffs[_basis[j]] * _model.MatrixCoeffs[j, i];
                }

                tempQ -= _model.ObjectiveCoeffs[i];
                _Q[i] = tempQ;

                Console.Write($"|\t{Math.Round(_Q[i], 3)}\t");
            }
            Console.Write('\n');

            PrintDividerLine('-');
        }
        protected void PrintDividerLine(char ch)
        {
            for (int i = 0; i < 16 * (_model.Columns + 3); ++i)
            {
                Console.Write(ch);
            }
            Console.Write("\n");
        }
        private void FindBasis()
        {
            _basis = new int[_model.Rows];
            int counter = 0;

            for (int i = 0; i < _model.Columns; ++i)
            {
                List<double> coefs = new List<double>();
                for (int j = 0; j < _model.Rows; ++j)
                {
                    if (_model.MatrixCoeffs[j, i] != 0)
                        coefs.Add(_model.MatrixCoeffs[j, i]);
                }
                if (coefs.Count == 1 && coefs[0] == 1)
                    _basis[counter++] = i;
            }
        }
    }

    // Розв'язання симплекс-методом
    class SimplexMethodTable : Table
    {
        public SimplexMethodTable(ModelOfLinearProgramming model) : base(model)
        {
            symbol = 'x';
        }

        public SimplexMethodTable(ModelOfLinearProgramming model, int[] basis) : base(model)
        {
            symbol = 'y';
            _basis = new int[model.Rows];
            _basis = basis;
        }

        private double CalculateZ(int leadColumn, int leadRow)
        {
            return _model.FreeMembersCoeffs[leadRow] / _model.MatrixCoeffs[leadRow, leadColumn];
        }
        override protected void RecalculateTable()
        {
            int leadColumn = Array.IndexOf(_Q, Enumerable.Min(_Q));

            if (!isCorrectColumn(leadColumn))
            {
                throw new Exception("Lead Column consists of only negative numbers");
            }

            int leadRow = 0;

            for (int i = 0; i < _model.Rows; i++)
            {
                if (CalculateZ(leadColumn, i) < CalculateZ(leadColumn, leadRow))
                    leadRow = i;
            }

            _basis[leadRow] = leadColumn;

            RectangleMethod(leadRow, leadColumn);

            Console.WriteLine();
            Console.WriteLine();


            BuildTable();
        }
        override protected bool isValidSolution()
        {
            if (Enumerable.Min(_Q) < 0)
                return false;
            return true;
        }
        private bool isCorrectColumn(int leadColumn)
        {
            for (int i = 0; i < _model.Rows; ++i)
            {
                if (_model.MatrixCoeffs[i, leadColumn] > 0)
                    return true;
            }
            return false;
        }
        private void FindBasis()
        {
            _basis = new int[_model.Rows];
            int counter = 0;

            for (int i = 0; i < _model.Columns; ++i)
            {
                List<double> coefs = new List<double>();
                for (int j = 0; j < _model.Rows; ++j)
                {
                    if (_model.MatrixCoeffs[j, i] != 0)
                        coefs.Add(_model.MatrixCoeffs[j, i]);
                }
                if (coefs.Count == 1 && coefs[0] == 1)
                    _basis[counter++] = i;
            }
        }

        public bool CanBeSolved()
        {
            foreach (var value in _model.FreeMembersCoeffs)
            {
                if (value < 0)
                    return false;
            }
            return true;
        }

    }

    class DualSimplexMethodTable : Table
    {
        public DualSimplexMethodTable(ModelOfLinearProgramming model) : base(model)
        {
            symbol = 'y';
        }

        public DualSimplexMethodTable(ModelOfLinearProgramming model, int[] basis) : base(model)
        {
            symbol = 'y';
            _basis = new int[model.Rows];
            _basis = basis;
        }


        protected override void RecalculateTable()
        {
            int leadRow = Array.IndexOf(_model.FreeMembersCoeffs, _model.FreeMembersCoeffs.Min());

            int leadColumn = 0;

            for (int i = 0; i < _model.Columns; i++)
            {
                if ((_Q[i] == 0 && i == leadColumn))
                {
                    leadColumn++;
                    continue;
                }
                if (_basis.Contains(i))
                    continue;

                if (_Q[i] != 0 && _model.MatrixCoeffs[leadRow, i] < 0)
                {
                    if (-_Q[i] / _model.MatrixCoeffs[leadRow, i] < -_Q[leadColumn] / _model.MatrixCoeffs[leadRow, leadColumn])
                    {
                        leadColumn = i;
                    }
                    /*if (Math.Abs(_model.MatrixCoeffs[leadRow, i]) > Math.Abs(_model.MatrixCoeffs[leadRow, leadColumn]))
                    {
                        leadColumn = i;
                    }*/
                }
            }

            _basis[leadRow] = leadColumn;

            RectangleMethod(leadRow, leadColumn);

            Console.WriteLine();
            Console.WriteLine();


            BuildTable();

        }
        override protected bool isValidSolution()
        {
            if (Enumerable.Min(_model.FreeMembersCoeffs) < 0)
                return false;
            return true;
        }
    }

    // Модель задачі лінійного програмування
}