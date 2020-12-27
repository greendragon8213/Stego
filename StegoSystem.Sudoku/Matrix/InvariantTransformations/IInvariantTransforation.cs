namespace StegoSystem.Sudoku.Matrix.InvariantTransformations
{
    /// <summary>
    /// Represents invariant sudoku matrix transforation
    /// </summary>
    public interface IInvariantTransforation<T>
    {
        int IndexesLength { get; }
        void Transform(ref SudokuMatrix<T> matrix, params int[] indexes);
    }
}
