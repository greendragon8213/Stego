using System;

namespace SudkuStegoSystem.Logic.SudokuMethod.SudokuMatrix
{
    public class SudokuMatrixGeneratorInitial : ISudokuMatrixGenerator
    {
        public int MatrixSize { get; }

        public SudokuMatrixGeneratorInitial(int size)
        {
            MatrixSize = size;
        }

        public virtual byte[,] Generate()
        {
            byte[,] sudokuMatrix = new byte[MatrixSize, MatrixSize];
            int smallBlockSize = (int)Math.Sqrt(MatrixSize);

            int[] initial = new int[MatrixSize];
            for (int j = 0; j < MatrixSize; j++)
            {
                initial[j] = j;
            }

            //generate initial horizontal region
            for (int i = 0; i < smallBlockSize; i++)
            {
                int offset = i * smallBlockSize;

                for (int j = 0; j < MatrixSize; j++)
                {
                    if (j + offset < MatrixSize)
                    {
                        sudokuMatrix[i, j] = (byte)(offset + j);
                    }
                    else
                    {
                        sudokuMatrix[i, j] = (byte)(-MatrixSize + (offset + j));
                    }
                }
            }

            //fill by offset
            for (int k = 1; k < smallBlockSize; k++)
            {
                for (int i = 0; i < MatrixSize; i++)
                {
                    int offset = k * smallBlockSize;

                    for (int j = 0; j < smallBlockSize; j++)
                    {
                        if (j + offset < MatrixSize)
                        {
                            if (i + 1 < MatrixSize)
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

            //ToDo assert that matrix is correct

            return sudokuMatrix;
        }
    }
}
