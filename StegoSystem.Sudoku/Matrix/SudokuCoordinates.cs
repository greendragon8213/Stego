using System;

namespace StegoSystem.Sudoku.Matrix
{
    public class SudokuCoordinates : IEquatable<SudokuCoordinates>
    {
        public byte X { get; set; }
        public byte Y { get; set; }

        public SudokuCoordinates(byte x, byte y)
        {
            X = x;
            Y = y;
        }

        public static bool operator == (SudokuCoordinates first, SudokuCoordinates second)
        {
            return (first.X == second.X && first.Y == second.Y);
        }

        public static bool operator != (SudokuCoordinates first, SudokuCoordinates second)
        {
            return (first.X != second.X || first.Y != second.Y);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SudokuCoordinates);
        }

        public bool Equals(SudokuCoordinates other)
        {
            return other != null &&
                   X == other.X &&
                   Y == other.Y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}
