using SudkuStegoSystem.Logic.Models;

namespace SudkuStegoSystem.Logic
{
    public interface INearestCoordinatesFinder
    {
        SudokoCoordinates Find(int valueToFind, SudokoCoordinates initialCoordinates, byte[,] sudokuMatrix);
    }
}
