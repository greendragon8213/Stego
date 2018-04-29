using SudkuStegoSystem.Logic.Models;

namespace SudkuStegoSystem.Logic
{
    public interface INearestCoordinatesFinder
    {
        SudokuCoordinates Find(int valueToFind, SudokuCoordinates initialCoordinates, byte[,] sudokuMatrix);
    }
}
