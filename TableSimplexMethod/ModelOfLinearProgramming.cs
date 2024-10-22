using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableSimplexMethod
{
    internal class ModelOfLinearProgramming
    {
        private int _rows, _cols;

        public int Rows { get { return _rows; } }
        public int Columns { get { return _cols; } }

        private double[] _objectiveCoeffs; // коефіцієнти ф-ії мети
        private double[,] _matrixCoeffs; // коефіцієнти функцій-обмежень
        private double[] _freeMembersCoeffs; // коефіцієнти вільних членів

        public void AddRow()
        {
            _rows++;
        }
        public void AddColumn()
        {
            _cols++;
        }

        public double[] ObjectiveCoeffs
        {
            get
            {
                return _objectiveCoeffs;
            }
            set
            {
                _objectiveCoeffs = value;
            }
        }

        public double[,] MatrixCoeffs
        {
            get
            {
                return _matrixCoeffs;
            }
            set
            {
                _matrixCoeffs = value;
            }
        }

        public double[] FreeMembersCoeffs
        {
            get
            {
                return _freeMembersCoeffs;
            }
            set
            {
                _freeMembersCoeffs = value;
            }
        }

        public ModelOfLinearProgramming(int rows, int cols, double[] objectiveCoeffs, double[,] matrixCoeffs, double[] freeMembersCoeffs)
        {
            _rows = rows;
            _cols = cols;
            _objectiveCoeffs = objectiveCoeffs;
            _matrixCoeffs = matrixCoeffs;
            _freeMembersCoeffs = freeMembersCoeffs;
        }

        public ModelOfLinearProgramming(ModelOfLinearProgramming model)
        {
            _rows = model._rows;
            _cols = model._cols;

            _objectiveCoeffs = new double[model.ObjectiveCoeffs.Length];
            _matrixCoeffs = new double[model.MatrixCoeffs.Length / _cols, model.MatrixCoeffs.Length / _rows];
            _freeMembersCoeffs = new double[model.FreeMembersCoeffs.Length];

            Array.Copy(model.ObjectiveCoeffs, _objectiveCoeffs, model.ObjectiveCoeffs.Length);
            Array.Copy(model.MatrixCoeffs, _matrixCoeffs, model.MatrixCoeffs.Length);
            Array.Copy(model.FreeMembersCoeffs, _freeMembersCoeffs, model.FreeMembersCoeffs.Length);
        }

    }
}

