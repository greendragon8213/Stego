using StegoSystem.SudokuMethodImplementation.Abstract;
using System;

namespace StegoSystem.SudokuMethodImplementation.Matrix
{
    public class SwapVerticalRegionsInvariantTransforation<T> : IInvariantTransforation<T>
    {
        public int IndexesLength => 2;

        /// <summary>
        /// 1 - horizpontal region 1 index
        /// 2 - horizpontal region 2 index
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="indexes"></param>
        public void Transform(ref SudokuMatrix<T> matrix, params int[] indexes)
        {
            if (matrix == null || indexes.Length != IndexesLength)
                throw new ArgumentException();

            int index1 = indexes[0] < matrix.RegionsCount ? indexes[0] : indexes[0] % matrix.RegionsCount;
            int index2 = indexes[1] < matrix.RegionsCount ? indexes[1] : indexes[1] % matrix.RegionsCount;

            int block1LineOffet = index1 * matrix.BlockSize;
            int block2LineOffet = index2 * matrix.BlockSize;

            for (int i = 0; i < matrix.BlockSize; i++)
            {
                matrix.SwapColumns(block1LineOffet + i, block2LineOffet + i);
            }
        }
    }
}
