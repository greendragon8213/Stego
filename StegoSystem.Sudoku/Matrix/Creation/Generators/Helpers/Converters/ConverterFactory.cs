using System;

namespace StegoSystem.Sudoku.Matrix.Generators.Helpers
{
    internal static class ConverterFactory
    {
        internal static IConverterFromInt<T> CreateConverter<T>()
        {
            if (typeof(T) == typeof(byte))
            {
                return new ConverterFromIntToByte() as IConverterFromInt<T>;
            }

            throw new NotImplementedException();
        }
    }
}
