namespace StegoSystem.Sudoku.Matrix.Generators
{
    public interface ISudokuMatrixGenerator<T>
    {
        T[,] Generate(int size);
    }
}
