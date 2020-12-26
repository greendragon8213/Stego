using StegoSystem.SudokuMethodImplementation.Matrix;

namespace StegoSystem.SudokuMethodImplementation.Abstract
{
    public interface INearestCoordinatesFinder<T>
    {
        SudokuCoordinates Find(T valueToFind, SudokuCoordinates initialCoordinates, T[,] sudokuMatrix);
    }
}
