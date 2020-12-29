using StegoSystem.Sudoku.Matrix.CoordinateFinders;
using StegoSystem.Sudoku.Matrix.Generators;
using System;

namespace StegoSystem.Sudoku.Matrix
{
    /// <summary>
    /// Does actions related to sudoku matrix
    /// </summary>
    public class SudokuMatrix<T>
    {
        private readonly T[,] _sudokuMatrix;
        private readonly INearestCoordinatesFinder<T> _nearestCoordinatesFinder;
        public int SudokuSize => _sudokuMatrix.GetLength(0);
        public int BlockSize => (int)Math.Sqrt(SudokuSize);
        public int RegionsCount => SudokuSize / BlockSize;

        public SudokuMatrix(INearestCoordinatesFinder<T> nearestCoordinatesFinder, int matrixSize)
        {
            _nearestCoordinatesFinder = nearestCoordinatesFinder;

            ISudokuMatrixGenerator<T> defaultSudokuMatrixGenerator = new SudokuMatrixGeneratorInitial<T>();
            _sudokuMatrix = defaultSudokuMatrixGenerator.Generate(matrixSize);
        }

        //or by Coordinates
        public T this[int x, int y] => _sudokuMatrix[x, y];

        public SudokuCoordinates FindNearestCoordinates(T valueToFind, SudokuCoordinates initialCoordinates)
        {
            return _nearestCoordinatesFinder.Find(valueToFind, initialCoordinates, _sudokuMatrix);
        }
        
        /// <summary>
        /// Just swaps any two rows
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        public void SwapRows(int index1, int index2)
        {
            if (index1 == index2)
                return;

            T temp;
            for(int i=0; i < _sudokuMatrix.GetLength(0); i++)
            {
                temp = _sudokuMatrix[index1, i];
                _sudokuMatrix[index1, i] = _sudokuMatrix[index2, i];
                _sudokuMatrix[index2, i] = temp;
            }
        }

        /// <summary>
        /// Just swaps any two columns
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        public void SwapColumns(int index1, int index2)
        {
            if (index1 == index2)
                return;

            T temp;
            for (int i = 0; i < _sudokuMatrix.GetLength(1); i++)
            {
                temp = _sudokuMatrix[i, index1];
                _sudokuMatrix[i, index1] = _sudokuMatrix[i, index2];
                _sudokuMatrix[i, index2] = temp;
            }
        }
    }
}
