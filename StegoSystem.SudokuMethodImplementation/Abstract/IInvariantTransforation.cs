using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StegoSystem.SudokuMethodImplementation.Matrix;

namespace StegoSystem.SudokuMethodImplementation.Abstract
{
    /// <summary>
    /// Represents invariant sudoku matrix transforation
    /// </summary>
    public interface IInvariantTransforation
    {
        int IndexesLength { get; }
        void Transform(ref SudokuMatrix matrix, params int[] indexes);
    }
}
