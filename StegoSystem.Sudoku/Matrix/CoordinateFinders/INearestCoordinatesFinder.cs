namespace StegoSystem.Sudoku.Matrix.CoordinateFinders
{
    public interface INearestCoordinatesFinder<T>
    {
        SudokuCoordinates Find(T valueToFind, SudokuCoordinates initialCoordinates, T[,] sudokuMatrix);
    }
}
