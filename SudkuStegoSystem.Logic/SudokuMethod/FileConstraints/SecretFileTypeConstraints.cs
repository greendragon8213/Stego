using SudkuStegoSystem.Logic.Abstract;

namespace SudkuStegoSystem.Logic.SudokuMethod.FileConstraints
{
    public class SecretFileTypeConstraints : FileTypeConstraints
    {
        public override FileTypes FileType => FileTypes.AnyFiles;
        public override string[] AllowedExtensions => new string[0];
    }
}
