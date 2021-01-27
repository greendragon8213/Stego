using System.Collections.Generic;
using System.Linq;

namespace StegoSystem.Common.Extensions
{
    public static class ArrayExtension
    {
        public static HashSet<T> GetColumnAsHashSet<T>(this T[,] matrix, int columnNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(0))
                    .Select(x => matrix[x, columnNumber])
                    .ToHashSet();
        }

        public static HashSet<T> GetRowAsHashSet<T>(this T[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToHashSet();
        }

        public static HashSet<T> GetSquareRegionAsHashSet<T>(this T[,] matrix, int squareHorizontalIndex, int squareVerticalIndex, int squareSize)
        {
            int rowOffset = squareHorizontalIndex * squareSize;
            int columnOffset = squareVerticalIndex * squareSize;

            HashSet<T> result = new HashSet<T>();

            for (int i = 0; i < squareSize; i++)
            {
                result.UnionWith(Enumerable.Range(0, squareSize)
                        .Select(x => matrix[rowOffset + i, columnOffset + x])
                        .ToHashSet());
            }

            return result;
        }
    }
}
