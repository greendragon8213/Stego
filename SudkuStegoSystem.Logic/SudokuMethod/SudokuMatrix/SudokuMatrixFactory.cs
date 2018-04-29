namespace SudkuStegoSystem.Logic.SudokuMethod.SudokuMatrix
{
    public class SudokuMatrixFactory
    {
        public Logic.SudokuMatrix GetByPassword(int matrixSize, string password)
        {
            ISudokuMatrixGenerator matrixGenerator = new SudokuMatrixGeneratorByPassword(matrixSize, password);
            INearestCoordinatesFinder nearestCoordinatesFinder = new NearestCoordinatesFinder();

            return new Logic.SudokuMatrix(matrixGenerator, nearestCoordinatesFinder);
        }
    }
}
