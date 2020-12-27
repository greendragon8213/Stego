using System;

namespace StegoSystem.Sudoku.Matrix.InvariantTransformations
{
    public class SwapColumnsInsideVerticalRegionInvariantTransforation<T> : IInvariantTransforation<T>
    {
        public int IndexesLength => 3;

        /// <summary>
        /// 1 - vertical region index
        /// 2 - column index 1 inside vertical region
        /// 3 - column index 2 inside vertical region
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="indexes"></param>
        public void Transform(ref SudokuMatrix<T> matrix, params int[] indexes)
        {
            if (matrix == null || indexes.Length != IndexesLength)
                throw new ArgumentException();

            int regionIndex = indexes[0] < matrix.RegionsCount ? indexes[0] : indexes[0] % matrix.RegionsCount;
            int regionLineOffset = regionIndex * matrix.BlockSize;

            int index1 = indexes[1] < matrix.BlockSize ? indexes[1] : indexes[1] % matrix.BlockSize;
            int index2 = indexes[2] < matrix.BlockSize ? indexes[2] : indexes[2] % matrix.BlockSize;
            
            matrix.SwapColumns(index1 + regionLineOffset, index2 + regionLineOffset);
        }
    }
}
