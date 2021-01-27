using StegoSystem.Common.Extensions;
using StegoSystem.Sudoku.Matrix.CoordinateFinders;
using StegoSystem.Sudoku.Matrix.Generators;
using System;
using System.Collections.Generic;

namespace StegoSystem.Sudoku.Matrix
{
    /// <summary>
    /// Does actions related to sudoku matrix
    /// </summary>
    public class SudokuMatrix<T>
    {
        private readonly T[,] _sudokuMatrix;
        private readonly INearestCoordinatesFinder<T> _nearestCoordinatesFinder;

        public int SudokuSize { get; }
        public int BlockSize { get; }
        public int RegionsCount { get; }

        public SudokuMatrix(INearestCoordinatesFinder<T> nearestCoordinatesFinder, int matrixSize)
        {
            if (!IsMatrixSizeValid(matrixSize))
            {
                throw new ArgumentException("Invalid matrix size", nameof(matrixSize));
            }

            _nearestCoordinatesFinder = nearestCoordinatesFinder;

            ISudokuMatrixGenerator<T> defaultSudokuMatrixGenerator = new SudokuMatrixGeneratorInitial<T>();
            _sudokuMatrix = defaultSudokuMatrixGenerator.Generate(matrixSize);

            SudokuSize = matrixSize;
            BlockSize = (int)Math.Sqrt(SudokuSize);
            RegionsCount = SudokuSize / BlockSize;
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
            for (int i = 0; i < _sudokuMatrix.GetLength(0); i++)
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

        public virtual bool IsMatrixValid =>
           IsMatrixSquare() && IsMatrixSizeValid(_sudokuMatrix.GetLength(0)) && IsMatrixFilledAccordingToSudokuRules();

        private bool IsMatrixSquare() => _sudokuMatrix.GetLength(0) == _sudokuMatrix.GetLength(1);

        private bool IsMatrixSizeValid(int size) => IsMatrixSizeDividebleToSudokuRegions(size);

        private bool IsMatrixSizeDividebleToSudokuRegions(int size) => Math.Sqrt(size) % 1 == 0; //that's enough checking because of math properties

        private bool IsMatrixFilledAccordingToSudokuRules()
        {
            //let's say 1 row defines alphabet
            HashSet<T> alphabet = _sudokuMatrix.GetRowAsHashSet(0);

            if (alphabet.Count != SudokuSize)
            {
                return false;
            }

            return AreRowsFilledWithUniqueValues(alphabet) && AreColumnsFilledWithUniqueValues(alphabet) && AreRegionsFilledWithUniqueValues(alphabet);
        }

        private bool AreRowsFilledWithUniqueValues(HashSet<T> alphabet)
        {
            for (int i = 0; i < SudokuSize; i++)
            {
                HashSet<T> currentRow = _sudokuMatrix.GetRowAsHashSet(i);

                if (!currentRow.SetEquals(alphabet))
                {
                    return false;
                }
            }

            return true;
        }

        private bool AreColumnsFilledWithUniqueValues(HashSet<T> alphabet)
        {
            for (int i = 0; i < SudokuSize; i++)
            {
                HashSet<T> currentColumn = _sudokuMatrix.GetColumnAsHashSet(i);

                if (!currentColumn.SetEquals(alphabet))
                {
                    return false;
                }
            }

            return true;
        }

        private bool AreRegionsFilledWithUniqueValues(HashSet<T> alphabet)
        {
            for (int i = 0; i < RegionsCount; i++)
            {
                for (int j = 0; j < RegionsCount; j++)
                {
                    HashSet<T> currentRegion = _sudokuMatrix.GetSquareRegionAsHashSet(i, j, BlockSize);

                    if (!currentRegion.SetEquals(alphabet))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
