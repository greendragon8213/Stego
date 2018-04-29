using SudkuStegoSystem.Logic.Models;
using System;

namespace SudkuStegoSystem.Logic
{
    /// <summary>
    /// Does actions related to sudoku matrix
    /// </summary>
    public class SudokuMatrix
    {
        private readonly INearestCoordinatesFinder _nearestCoordinatesFinder;
        private byte[,] _sudokyMatrix;
        public int SudokuSize { get; }

        public SudokuMatrix(INearestCoordinatesFinder nearestCoordinatesFinder, string key, int size)
        {
            _nearestCoordinatesFinder = nearestCoordinatesFinder;            
            _sudokyMatrix = GenerateSudokuMatrixByKey(key, size);
            SudokuSize = size;
        }
        
        //or by Coordinates
        public byte this[int x, int y]
        {
            get
            {
                return _sudokyMatrix[x, y];
            }
        }

        public SudokoCoordinates FindNearestCoordinates(int valueToFind, SudokoCoordinates initialCoordinates)
        {
            return _nearestCoordinatesFinder.Find(valueToFind, initialCoordinates, _sudokyMatrix);
        }

        private byte[,] GenerateSudokuMatrixByKey(string key, int size)
        {
            byte[,] sudokoBoard = new byte[size, size];
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
                        //initial[j] = initial[j + offset];
                        sudokoBoard[i, j] = (byte)(offset + j);
                    }
                    else
                    {
                        sudokoBoard[i, j] = (byte)(-size + (offset + j));
                    }

                    //sudokoBoard[i, j] = initial[j];                    
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
                            //initial[j] = initial[j + offset];
                            if (i + 1 < size)
                            {
                                sudokoBoard[j + offset, i] = sudokoBoard[j + offset - smallBlockSize, i + 1];
                            }
                            else
                            {
                                sudokoBoard[j + offset, i] = sudokoBoard[j + offset - smallBlockSize, 0];
                            }
                        }
                        else
                        {
                        }
                    }
                }
            }

            //Assert allright

            return sudokoBoard;
        }
    }
}
