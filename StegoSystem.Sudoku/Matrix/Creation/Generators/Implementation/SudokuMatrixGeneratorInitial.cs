using StegoSystem.Sudoku.Matrix.Generators.Helpers;
using System;

namespace StegoSystem.Sudoku.Matrix.Generators
{
    /// <summary>
    /// Creates new default (simples) sudoky matrix 
    /// </summary>
    /// <typeparam name="T">matrix element type</typeparam>
    public class SudokuMatrixGeneratorInitial<T> : ISudokuMatrixGenerator<T>
    {
        private IConverterFromInt<T> _converter = ConverterFactory.CreateConverter<T>();

        public virtual T[,] Generate(int size)
        {
            T[,] sudokuMatrix = new T[size, size];
            int smallBlockSize = (int)Math.Sqrt(size);

            int[] initial = new int[size];
            for (int j = 0; j < size; j++)
            {
                initial[j] = j;
            }

            //generate initial horizontal region
            for (int i = 0; i < smallBlockSize; i++)
            {
                int offset = i * smallBlockSize;

                for (int j = 0; j < size; j++)
                {
                    if (j + offset < size)
                    {
                        sudokuMatrix[i, j] = _converter.Convert(offset + j);
                    }
                    else
                    {
                        sudokuMatrix[i, j] = _converter.Convert(-size + (offset + j));
                    }
                }
            }

            //fill by offset
            for (int k = 1; k < smallBlockSize; k++)
            {
                for (int i = 0; i < size; i++)
                {
                    int offset = k * smallBlockSize;

                    for (int j = 0; j < smallBlockSize; j++)
                    {
                        if (j + offset < size)
                        {
                            if (i + 1 < size)
                            {
                                sudokuMatrix[j + offset, i] = sudokuMatrix[j + offset - smallBlockSize, i + 1];
                            }
                            else
                            {
                                sudokuMatrix[j + offset, i] = sudokuMatrix[j + offset - smallBlockSize, 0];
                            }
                        }
                    }
                }
            }

            return sudokuMatrix;
        }
    }
}
