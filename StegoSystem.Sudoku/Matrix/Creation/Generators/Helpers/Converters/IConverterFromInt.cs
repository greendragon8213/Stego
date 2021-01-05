namespace StegoSystem.Sudoku.Matrix.Generators.Helpers
{
    internal interface IConverterFromInt<T>
    {
        T Convert(int number);
    }
}
