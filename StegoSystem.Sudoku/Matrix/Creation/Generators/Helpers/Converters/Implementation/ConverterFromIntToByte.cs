namespace StegoSystem.Sudoku.Matrix.Generators.Helpers
{
    internal class ConverterFromIntToByte : IConverterFromInt<byte>
    {
        public byte Convert(int number)
        {
            return (byte)number;
        }
    }
}
