using StegoSystem.SudokuMethodImplementation.Abstract;

namespace StegoSystem.SudokuMethodImplementation.Matrix
{
    /// <summary>
    /// Does actions related to sudoku matrix
    /// </summary>
    public class SudokuMatrix
    {
        private readonly byte[,] _sudokyMatrix;
        private readonly INearestCoordinatesFinder _nearestCoordinatesFinder;
        public int SudokuSize => _sudokyMatrix.GetLength(1);

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
    }
}
