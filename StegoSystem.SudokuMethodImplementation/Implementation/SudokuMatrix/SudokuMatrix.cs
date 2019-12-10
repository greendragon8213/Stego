using StegoSystem.SudokuMethodImplementation.Abstract;
using System;

namespace StegoSystem.SudokuMethodImplementation.Matrix
{
    /// <summary>
    /// Does actions related to sudoku matrix
    /// </summary>
    public class SudokuMatrix
    {
        //ToDo optimize!
        private readonly byte[,] _sudokyMatrix;
        private readonly INearestCoordinatesFinder _nearestCoordinatesFinder;
        public int SudokuSize => _sudokyMatrix.GetLength(1);
        public int BlockSize => (int)Math.Sqrt(SudokuSize);
        public int RegionsCount => SudokuSize / BlockSize;

        public SudokuMatrix(ISudokuMatrixGenerator sudokuMatrixGenerator, INearestCoordinatesFinder nearestCoordinatesFinder)
        {
            _sudokyMatrix = sudokuMatrixGenerator.Generate();
            _nearestCoordinatesFinder = nearestCoordinatesFinder;
        }

        //or by Coordinates
        public byte this[int x, int y] => _sudokyMatrix[x, y];

        public SudokuCoordinates FindNearestCoordinates(int valueToFind, SudokuCoordinates initialCoordinates)
        {
            return _nearestCoordinatesFinder.Find(valueToFind, initialCoordinates, _sudokyMatrix);
        }
        
        /// <summary>
        /// Just swaps any two rows
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        public void SwapRows(int index1, int index2)
        {
            byte temp;
            for(int i=0; i < _sudokyMatrix.GetLength(1); i++)
            {
                temp = _sudokyMatrix[index1, i];
                _sudokyMatrix[index1, i] = _sudokyMatrix[index2, i];
                _sudokyMatrix[index2, i] = temp;
            }
        }

        /// <summary>
        /// Just swaps any two columns
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        public void SwapColumns(int index1, int index2)
        {
            byte temp;
            for (int i = 0; i < _sudokyMatrix.GetLength(2); i++)
            {
                temp = _sudokyMatrix[i, index1];
                _sudokyMatrix[i, index1] = _sudokyMatrix[i, index2];
                _sudokyMatrix[i, index2] = temp;
            }
        }
    }
}
