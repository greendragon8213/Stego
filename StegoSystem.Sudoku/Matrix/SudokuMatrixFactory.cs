using StegoSystem.Sudoku.Matrix.CoordinateFinders;
using StegoSystem.Sudoku.Matrix.Generators;

namespace StegoSystem.Sudoku.Matrix
{
    public class SudokuMatrixFactory<T>
    {
        public SudokuMatrix<T> GetByPassword(int matrixSize, string password)
        {
            INearestCoordinatesFinder<T> nearestCoordinatesFinder = new NearestCoordinatesFinder<T>();

            var matrix = new SudokuMatrix<T>(nearestCoordinatesFinder, matrixSize);

            var matrixInitializer = new SudokuMatrixInitializerByPassword<T>();
            matrixInitializer.Initialize(ref matrix, password);

            return matrix;
        }
    }
}
