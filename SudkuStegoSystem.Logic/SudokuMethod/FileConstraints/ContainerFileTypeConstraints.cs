using SudkuStegoSystem.Logic.Abstract;

namespace SudkuStegoSystem.Logic.SudokuMethod.FileConstraints
{
    public class ContainerFileTypeConstraints : FileTypeConstraints
    {
        public override FileTypes FileType => FileTypes.Images;
        public override string[] AllowedExtensions => new string[] { "bmp", "jpeg", "jpg", "png"};
    }
}