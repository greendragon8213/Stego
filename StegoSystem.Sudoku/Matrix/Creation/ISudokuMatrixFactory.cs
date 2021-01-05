using StegoSystem.Models;

namespace StegoSystem.Sudoku.Matrix
{
    public interface ISudokuMatrixFactory<T, TKey>
    {
        SudokuMatrix<T> Create(int matrixSize, IKey<TKey> key);
    }
}
