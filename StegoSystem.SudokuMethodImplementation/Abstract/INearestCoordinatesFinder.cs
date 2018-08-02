using StegoSystem.SudokuMethodImplementation.Matrix;

namespace StegoSystem.SudokuMethodImplementation.Abstract
{
    public interface INearestCoordinatesFinder
    {
        SudokuCoordinates Find(int valueToFind, SudokuCoordinates initialCoordinates, byte[,] sudokuMatrix);
    }
}
