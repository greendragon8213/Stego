using StegoSystem.Models;
using StegoSystem.Sudoku.Matrix.CoordinateFinders;
using StegoSystem.Sudoku.Matrix.Generators;

namespace StegoSystem.Sudoku.Matrix
{

    public class SudokuByPasswordMatrixFactory<T> : ISudokuMatrixFactory<T, string>
    {
        public SudokuMatrix<T> Create(int matrixSize, IKey<string> password)
        {
            INearestCoordinatesFinder<T> nearestCoordinatesFinder = new NearestCoordinatesFinder<T>();

            var matrix = new SudokuMatrix<T>(nearestCoordinatesFinder, matrixSize);

            var matrixInitializer = new SudokuMatrixInitializerByPassword<T>();
            matrixInitializer.Initialize(ref matrix, password.Payload);

#if DEBUG
            var result = Test.MatrixHelpers
                .CalculateMatrixesDifference(new SudokuMatrix<T>(nearestCoordinatesFinder, matrixSize), matrix);

            System.Console.WriteLine($"Matrix by password is similar to default by {result.Item1}%");
            System.Console.WriteLine($"Identical items count = {result.Item2}");
            System.Console.WriteLine();
#endif

            return matrix;
        }
    }
}
