using System;

namespace StegoSystem.SudokuMethodImplementation.Matrix
{
    public static class ConverterFactory
    {
        public static IConverterFromInt<T> GetConverter<T>()
        {
            if (typeof(T) == typeof(byte))
            {
                return new ConverterFromIntToByte() as IConverterFromInt<T>;
            }

            throw new NotImplementedException();
        }
    }
}
