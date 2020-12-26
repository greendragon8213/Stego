using StegoSystem.SudokuMethodImplementation.Matrix;

namespace StegoSystem.SudokuMethodImplementation.Abstract
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
