namespace StegoSystem.SudokuMethodImplementation.Abstract
{
    public interface ISudokuMatrixGenerator<T>
    {
        T[,] Generate(int size);
    }
}
