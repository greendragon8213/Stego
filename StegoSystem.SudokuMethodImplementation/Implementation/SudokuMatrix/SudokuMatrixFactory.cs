using StegoSystem.SudokuMethodImplementation.Abstract;

namespace StegoSystem.SudokuMethodImplementation.Matrix
{
    public class SudokuMatrixFactory
    {
        public SudokuMatrix GetByPassword(int matrixSize, string password)
        {
            ISudokuMatrixGenerator matrixGenerator = new SudokuMatrixGeneratorByPassword(matrixSize, password);
            INearestCoordinatesFinder nearestCoordinatesFinder = new NearestCoordinatesFinder();

            return new SudokuMatrix(matrixGenerator, nearestCoordinatesFinder);
        }
    }
}
