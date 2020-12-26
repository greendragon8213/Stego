namespace StegoSystem.SudokuMethodImplementation.Matrix
{
    public class ConverterFromIntToByte : IConverterFromInt<byte>
    {
        public byte Convert(int number)
        {
            return (byte)number;
        }
    }
}
