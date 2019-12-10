using StegoSystem.SudokuMethodImplementation.Abstract;
using StegoSystem.SudokuMethodImplementation.Matrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegoSystem.SudokuMethodImplementation.Matrix
{
    public class SwapHorizontalRegionsInvariantTransforation : IInvariantTransforation
    {
        public int IndexesLength => 2;

        /// <summary>
        /// 1 - horizpontal region 1 index
        /// 2 - horizpontal region 2 index
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="indexes"></param>
        public void Transform(ref SudokuMatrix matrix, params int[] indexes)
        {
            if (matrix == null || indexes.Length != IndexesLength)
                throw new ArgumentException();

            int index1 = indexes[0] < matrix.RegionsCount ? indexes[0]: indexes[0] % matrix.RegionsCount;
            int index2 = indexes[1] < matrix.RegionsCount ? indexes[1]: indexes[1] % matrix.RegionsCount;

            int block1LineOffet = index1 * matrix.BlockSize;
            int block2LineOffet = index2 * matrix.BlockSize;

            for (int i=0; i < matrix.BlockSize; i++)
            {
                matrix.SwapRows(block1LineOffet + i, block2LineOffet + i);
            }
        }
    }
}
