using System;

namespace StegoSystem.Sudoku.Matrix.Test
{
    static class MatrixHelpers
    {
        public static Tuple<double, int> CalculateMatrixesDifference<T>(SudokuMatrix<T> m1, SudokuMatrix<T> m2)
        {
            int identicalElementsCount = 0;

            if (m1.SudokuSize != m2.SudokuSize || m1.SudokuSize == 0)
                throw new ArgumentException();

            for (int i = 0; i < m1.SudokuSize; i++)
            {
                for (int j = 0; j < m1.SudokuSize; j++)
                {
                    if (m1[i, j].Equals(m2[i, j]))
                    {
                        identicalElementsCount++;
                    }
                }
            }

            var similarityPercentage = 100.0 * identicalElementsCount / (m1.SudokuSize * m1.SudokuSize);

            return new Tuple<double, int>(similarityPercentage, identicalElementsCount);
        }
    }
}
