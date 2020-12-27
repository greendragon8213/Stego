using System;

namespace StegoSystem.Sudoku.Matrix.CoordinateFinders
{
    public class NearestCoordinatesFinder<T> : INearestCoordinatesFinder<T>
    {
        public SudokuCoordinates Find(T valueToFind, SudokuCoordinates initialCoordinates, T[,] sudokuMatrix)
        {
            int sudokoSize = sudokuMatrix.GetLength(0);

            if (!(//valueToFind >= 0 && valueToFind < sudokoSize &&
                 initialCoordinates.X >= 0 && initialCoordinates.X < sudokoSize
                 && initialCoordinates.Y >= 0 && initialCoordinates.Y < sudokoSize))
            {
                throw new ArgumentException("Wrong initialCoordinates or valueToFind");
            }

            int sudokoBlockSize = (int)Math.Sqrt(sudokoSize);

            //depth 0
            if(CheckValue(initialCoordinates.X, initialCoordinates.Y, valueToFind, sudokoSize, sudokuMatrix))
            {
                return initialCoordinates;
            }

            for (int depth = 1; depth <= sudokoBlockSize * 2 + 2; depth++)
            {
                for (int i = 0; i < depth + 1; i++)
                {
                    int offset = (depth - i);

                    int x1 = initialCoordinates.X + i;
                    int y1 = initialCoordinates.Y + offset;
                    if (CheckValue(x1, y1, valueToFind, sudokoSize, sudokuMatrix))
                    {
                        return new SudokuCoordinates((byte)x1, (byte)y1);
                    }

                    int x2 = initialCoordinates.X + i;
                    int y2 = initialCoordinates.Y - offset;
                    if (CheckValue(x2, y2, valueToFind, sudokoSize, sudokuMatrix))
                    {
                        return new SudokuCoordinates((byte)x2, (byte)y2);
                    }

                    int x3 = initialCoordinates.X - i;
                    int y3 = initialCoordinates.Y + offset;
                    if (CheckValue(x3, y3, valueToFind, sudokoSize, sudokuMatrix))
                    {
                        return new SudokuCoordinates((byte)x3, (byte)y3);
                    }

                    int x4 = initialCoordinates.X - i;
                    int y4 = initialCoordinates.Y - offset;
                    if (CheckValue(x4, y4, valueToFind, sudokoSize, sudokuMatrix))
                    {
                        return new SudokuCoordinates((byte)x4, (byte)y4);
                    }
                }
            }
            
            throw new Exception($"Cannot find requested value: {valueToFind} in the sudoku matrix.");
        }

        private bool CheckValue(int x, int y, T valueToFind, int sudokoSize, T[,] sudokuMatrix)
        {
            if (x < 0 || x >= sudokoSize || y < 0 || y >= sudokoSize)
            {
                return false;
            }
            
            if (sudokuMatrix[x, y].Equals(valueToFind))//ToDo check if works!
            {
                return true;
            }

            return false;
        }
    }
}
