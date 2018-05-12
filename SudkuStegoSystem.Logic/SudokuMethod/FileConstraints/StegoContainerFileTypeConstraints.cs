using SudkuStegoSystem.Logic.Abstract;

namespace SudkuStegoSystem.Logic.SudokuMethod.FileConstraints
{
    public class StegoContainerFileTypeConstraints : FileTypeConstraints
    {
        public override FileTypes FileType => FileTypes.Images;
        public override string[] AllowedExtensions => new string[] { "bmp" };
    }
}
