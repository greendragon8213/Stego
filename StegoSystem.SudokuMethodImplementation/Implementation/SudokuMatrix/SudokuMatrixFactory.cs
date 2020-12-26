using StegoSystem.SudokuMethodImplementation.Abstract;

namespace StegoSystem.SudokuMethodImplementation.Matrix
{
    public class SudokuMatrixFactory<T>
    {
        public SudokuMatrix<T> GetByPassword(int matrixSize, string password)
        {
            //ISudokuMatrixGenerator matrixGenerator = new SudokuMatrixGeneratorByPassword(matrixSize, password);
            INearestCoordinatesFinder<T> nearestCoordinatesFinder = new NearestCoordinatesFinder<T>();

            var matrix = new SudokuMatrix<T>(nearestCoordinatesFinder, matrixSize);

            var matrixInitializer = new SudokuMatrixInitializerByPassword<T>();
            matrixInitializer.Initialize(ref matrix, password);

            return matrix;
        }
    }
}
