using System;

namespace SudkuStegoSystem.Logic.Models
{
    public class SudokoCoordinates : IEquatable<SudokoCoordinates>
    {
        public byte X { get; set; }
        public byte Y { get; set; }

        public SudokoCoordinates(byte x, byte y)
        {
            X = x;
            Y = y;
        }

        public static bool operator == (SudokoCoordinates first, SudokoCoordinates second)
        {
            return (first.X == second.X && first.Y == second.Y);
        }

        public static bool operator != (SudokoCoordinates first, SudokoCoordinates second)
        {
            return (first.X != second.X || first.Y != second.Y);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SudokoCoordinates);
        }

        public bool Equals(SudokoCoordinates other)
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
